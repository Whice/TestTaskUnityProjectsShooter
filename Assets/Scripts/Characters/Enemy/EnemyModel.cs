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
    /// Изанчальная скорость движения врагов.
    /// </summary>
    public const Single speedOrigin = 0.016f;
    /// <summary>
    /// Скорость движения врагов.
    /// </summary>
    public Single speed = 0.16f;
    /// <summary>
    /// Высота на которой находится пол.
    /// Y координата.
    /// </summary>
    private Single floorHeight;
    /// <summary>
    /// Установить случайную скорость для врага.
    /// </summary>
    public void SetRandomSpeed()
    {
        this.speed = UnityEngine.Random.Range(speedOrigin, speedOrigin * 3);
    }
    /// <summary>
    /// Звук удара по игроку.
    /// </summary>
    private AudioSource soundPlayerKick = null;

    void Start()
    {
        this.soundPlayerKick = GetComponent<AudioSource>();
        this.floorHeight = ArenaModel.instance.arenaFloor.transform.position.y;
    }
    void Update()
    {
        //Если этот враг жив, то он может стремиться к убийству игрока.
        Vector3 thisPosition = this.transform.position;
        
        Vector3 targetPosition = new Vector3
            (
            this.mainCamera.transform.position.x,
            this.floorHeight,
            this.mainCamera.transform.position.z
            );

        //Двигаться к игроку
        if (Math.Abs(thisPosition.x - targetPosition.x) + Math.Abs(thisPosition.z - targetPosition.z) > 1.5f)
        {
            this.transform.position = Vector3.MoveTowards(thisPosition, targetPosition, this.speed * Time.deltaTime);
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
            this.mainCamera.transform.position.x,
            this.floorHeight,
            this.mainCamera.transform.position.z
            ));
    }


    /// <summary>
    /// Критический урон.
    /// </summary>
    private Int32 criticalDamage = 9;
    /// <summary>
    /// Таймер для сдерживания атаки.
    /// </summary>
    private Single timerAtack = 0;
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
            this.healthPoints -= PlayerModel.instance.damage;
        }
        
        //При столкновении с игроком ему сразу наноситься один удар.
        if (other.gameObject.name == "PlayerFront")
        {
            KickPlayer();
        }
    }

    private ArenaModel arenaModel
    {
        get => ArenaModel.instance;
    }
    /// <summary>
    /// Сделать неактивным.
    /// </summary>
    private void Deactivate()
    {
        this.arenaModel.aliveEnemies.Remove(this);
        this.arenaModel.deadEnemies.Add(this);
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
    /// <summary>
    /// Добавить врага к живым.
    /// </summary>
    public void Activate()
    {
        this.healthPoints = 10;
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //Задать ему начальную позицию.
        this.SetRandomPositionWithoutCameraVision();
    }

    /// <summary>
    /// Получить случайное местоположение вне зоны видимости камеры.
    /// </summary>
    /// <returns></returns>
    public void SetRandomPositionWithoutCameraVision()
    {
        Single shift = 30;
        Vector3 playerPosition = PlayerModel.instance.transform.position;
        Single floorSize = ArenaModel.instance.arenaFloor.transform.localScale.x * 5;
        Vector3 position = new Vector3(
                                        /*X*/ UnityEngine.Random.Range(playerPosition.x - shift, playerPosition.x + shift),
                                        /*Y*/ ArenaModel.instance.arenaFloor.transform.position.y,
                                        /*Z*/ UnityEngine.Random.Range(playerPosition.z - shift, playerPosition.z + shift)
                                        );

        while (InViewportCamera(position))
        {
            if (InViewportCamera(position))
            {
                position = new Vector3(-position.x, position.y, position.z);
            }
            else if (InViewportCamera(position))
            {
                position = new Vector3(position.x, position.y, -position.z);
            }
            else if (InViewportCamera(position))
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
    private Camera mainCameraPrivate = null;
    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    private Camera mainCamera
    {
        get
        {
            if (this.mainCameraPrivate == null)
            {
                this.mainCameraPrivate = CameraModel.instance.gameObject.GetComponent<Camera>();
            }
            return this.mainCameraPrivate;
        }
    }
    /// <summary>
    /// Проверить находится ли точка в зоне видимости камеры камеры.
    /// </summary>
    /// <param name="position">Местоположение точки.</param>
    /// <returns></returns>
    private Boolean InViewportCamera(Vector3 position)
    {
        Vector3 viewPosition = this.mainCamera.WorldToViewportPoint(position);
        if (viewPosition.x > -0.1 && viewPosition.x < 1.1 && viewPosition.z > 0)
        {
            return true;
        }
        return false;
    }
}
