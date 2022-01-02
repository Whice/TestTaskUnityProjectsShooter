using System;
using UnityEngine;

/// <summary>
/// Модель врага в игре.
/// </summary>
public class EnemyModel : GameCharacterModel
{
    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);
        switch(propertyName)
        {
            case nameof(this.healthPoints):
                {
                    if(this.healthPoints<1)
                    {
                        BoxModel boxModel = ArenaModel.instance.GetLastNotActiveBoxModel();
                        boxModel.Activate();
                        boxModel.transform.position = this.transform.position;

                        this.Deactivate();
                    }
                }
                break;
        }
    }
    /// <summary>
    /// Игровой ящик.
    /// </summary>
    public GameObject gameBox = null;
    /// <summary>
    /// Критический урон.
    /// </summary>
    private Int32 criticalDamage = 9;
    /// <summary>
    /// Таймер для сдерживания атаки.
    /// </summary>
    private Single timerAtack = 0;
    /// <summary>
    /// Скорость движения врагов.
    /// </summary>
    public const Single speedOrigin = 0.016f;
    public Single speed = 0.016f;
    /// <summary>
    /// Установить случайную скорость для врага.
    /// </summary>
    public void SetRandomSpeed()
    {
        this.speed = UnityEngine.Random.Range(speed, speed * 3);
    }
    /// <summary>
    /// Высота, на которой ходят живые враги.
    /// </summary>
    public const Single startPositionY = 0.6f;
    /// <summary>
    /// Звук удара по игроку.
    /// </summary>
    private AudioSource soundPlayerKick = null;

    void Start()
    {
        this.gameBox = GameObject.Find("GameBox");
        this.soundPlayerKick = GetComponent<AudioSource>();
    }
    void Update()
    {
        //Если этот враг жив, то он может стремиться к убийству игрока.
        if (!this.isDead)
        {

            Vector3 thisPosition = this.transform.position;
            Vector3 targetPosition = new Vector3
                (
                PlayerModel.instance.transform.position.x,
                startPositionY,
                PlayerModel.instance.transform.position.z
                );

            //Двигаться к игроку
            if (Math.Abs(thisPosition.x - targetPosition.x) + Math.Abs(thisPosition.z - targetPosition.z) > 1.5f)
            {
                this.transform.position = Vector3.MoveTowards(thisPosition, targetPosition, this.speed);
            }
            //Либо атаковать раз в секунду
            else
            {
                this.timerAtack += Time.deltaTime;
                if (this.timerAtack > 1)
                {
                    KickPlayer();
                    this.timerAtack = 0;
                }
            }

            //Повернуться к игроку
            this.transform.LookAt(new Vector3
                (
                CameraModel.instance.transform.position.x,
                startPositionY,
                CameraModel.instance.transform.position.z
                ));
        }
    }

    /// <summary>
    /// Нанести удар по игроку.
    /// </summary>
    private void KickPlayer()
    {
        this.soundPlayerKick.Play();


        Int32 randChance = UnityEngine.Random.Range(1, 100);

        //15% критического удара.
        if (randChance < 15)
        {
            PlayerModel.instance.healthPoints -= this.criticalDamage;
        }
        else
        {
            PlayerModel.instance.healthPoints -= this.damage;
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        //При столкновении с пулей умертвить врага.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            Destroy(other.gameObject);
            this.healthPoints -= PlayerModel.instance.damage;

            
        }
        
        //При столкновении с игроком ему сразу наноситься один удар.
        if (other.gameObject.name == "PlayerFront")
        {
            KickPlayer();
        }
    }

    /// <summary>
    /// Положение в списке активных или неактивных.
    /// </summary>
    private Int32 numberInListPrivate = -1;
    /// <summary>
    /// Положение в списке активных или неактивных.
    /// Задать можно только один раз.
    /// Номер должен быть больше 0.
    /// </summary>
    public Int32 numberInList
    {
        get => this.numberInListPrivate;
        set
        {
            if (this.numberInListPrivate == -1 && value > -1)
            {
                this.numberInListPrivate = value;
            }
        }
    }
    /// <summary>
    /// Сделать неактивным.
    /// </summary>
    private void Deactivate()
    {
        ArenaModel.instance.aliveEnemies.RemoveAt(this.numberInListPrivate);
        ArenaModel.instance.deadEnemies.Add(this);
        this.numberInListPrivate = ArenaModel.instance.deadEnemies.Count - 1;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Получить случайное местоположение вне зоны видимости камеры.
    /// </summary>
    /// <returns></returns>
    public void SetRandomPositionWithoutCameraVision()
    {
        Single floorSize = ArenaModel.instance.arenaFloor.transform.localScale.x * 5;
        Vector3 position = new Vector3(
                                        /*X*/ UnityEngine.Random.Range(0, floorSize + 10),
                                        /*Y*/ ArenaModel.instance.arenaFloor.transform.position.y,
                                        /*Z*/ UnityEngine.Random.Range(0, floorSize + 10)
                                        );

        while (InViewportCamera(position))
        {
            if (InViewportCamera(position))
            {
                position = new Vector3(-position.x, position.y, position.z);
            }
            if (InViewportCamera(position))
            {
                position = new Vector3(position.x, position.y, -position.z);
            }
            if (InViewportCamera(position))
            {
                position = new Vector3(-position.x, position.y, position.z);
            }
            if (InViewportCamera(position))
            {
                position = new Vector3(position.x - 5, position.y, -position.z - 5);
            }
        }

        this.transform.position = position;
    }
    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    private Camera mainCamera = null;
    /// <summary>
    /// Проверить находится ли точка в зоне видимости камеры камеры.
    /// </summary>
    /// <param name="position">Местоположение точки.</param>
    /// <returns></returns>
    private Boolean InViewportCamera(Vector3 position)
    {
        if (this.mainCamera == null)
        {
            this.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        Vector3 viewPosition = this.mainCamera.WorldToViewportPoint(position);
        if (viewPosition.x > -0.1 && viewPosition.x < 1.1 && viewPosition.z > 0)
        {
            return true;
        }
        return false;
    }
}
