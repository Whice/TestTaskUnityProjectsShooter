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

    void Start()
    {
        this.soundPlayerKick = GetComponent<AudioSource>();
        this.floorHeight = ArenaModel.instance.arenaFloor.transform.position.y;
    }
    void Update()
    {
        //Убить снеговика, который 
        if(this.transform.position.y<-5)
        {
            this.Deactivate();
        }

        //Если этот враг жив, то он может стремиться к убийству игрока.
        //Двигаться к игроку
        if (!IsNearWithPlayer)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, this.speed * Time.deltaTime);
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
        this.transform.LookAt(this.targetPosition);
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
        this.yHeight = 5f;
        this.healthPoints = 10;
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //Задать ему начальную позицию.
        this.SetRandomPositionWithoutCameraVision();
        this.SetRandomSpeed();

        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0));
    }


    #region Скорость.

    /// <summary>
    /// Изанчальная скорость движения врагов.
    /// </summary>
    public const Single ORIGIN_SPEED = 0.6f;
    /// <summary>
    /// Скорость движения врагов.
    /// </summary>
    [HideInInspector]
    public Single speed = 0f;
    /// <summary>
    /// Установить случайную скорость для врага.
    /// </summary>
    public void SetRandomSpeed()
    {
        this.speed = UnityEngine.Random.Range(ORIGIN_SPEED, ORIGIN_SPEED * 3);

        //Скорость врага не может быть больше скорости игрока.
        //Она обязательно должна быть чуть меньше.
       /* if (!(this.speed < PlayerModel.instance.runSpeed))
        {
            this.speed = PlayerModel.instance.runSpeed - ;
        }*/
    }

    #endregion

    #region Установка случайного местоположения.

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
    /// <summary>
    /// Установить случайное местоположение вне зоны видимости камеры.
    /// </summary>
    /// <returns></returns>
    public void SetRandomPositionWithoutCameraVision()
    {
        //Если игрок не упирается в стену.
        if (PlayerModel.instance.GetDistanceToWall() > 0.7)
        {
            Single shift = 30;
            Vector3 playerPosition = PlayerModel.instance.transform.position;
            Vector3 position = new Vector3(
                                            /*X*/ UnityEngine.Random.Range(playerPosition.x - shift, playerPosition.x + shift),
                                            /*Y*/ this.floorHeight,
                                            /*Z*/ UnityEngine.Random.Range(playerPosition.z - shift, playerPosition.z + shift)
                                            );
            //Если враг слишком близко к игроку, подвинуть врага подальше.
            const Single distance = 15F;
            if (position.x * position.x + position.z * position.z < distance * distance)
            {
                position = new Vector3(-position.x + distance, position.y , position.z + distance);
            }

            //Перемещать врага на 90 градусов вокруг игрока,
            //пока он не окажется за спиной игрока.
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

            //Если игрок встал в угол, то поставить врага сразу за спиной игрока.
            if (PlayerModel.instance.GetDistanceToWall() < 2)
            {
                position = playerPosition - CameraModel.instance.transform.forward;
            }

            this.transform.position = position;
        }
        //Если игрок упирается в стену.
        else
        {
            //Поставить сразу за спиной игрока.
            this.transform.position = new Vector3
                (
                -1 * this.mainCamera.transform.forward.x,
                this.yHeight,
                -1 * this.mainCamera.transform.forward.z
                );
        }
    }

    #endregion

    #region Урон.

    /// <summary>
    /// Звук удара по игроку.
    /// </summary>
    private AudioSource soundPlayerKick = null;
    /// <summary>
    /// Критический урон.
    /// </summary>
    private Int32 criticalDamage = 9;
    /// <summary>
    /// Таймер для сдерживания атаки.
    /// </summary>
    private Single timerAtack = 0;
    /// <summary>
    /// Физика этого врага.
    /// </summary>
    private Rigidbody rigidbodyField = null;
    /// <summary>
    /// Физика этого врага.
    /// </summary>
    private Rigidbody rigidbody
    {
        get
        {
            if(this.rigidbodyField==null)
            {
                this.rigidbodyField = GetComponent<Rigidbody>();
            }
            return this.rigidbodyField;
        }
    }
    /// <summary>
    /// Сила отталкивания врага после нанесения удара игроку.
    /// </summary>
    const Single IMPULSE_FORCE = 10f;
    /// <summary>
    /// Отойти назад.
    /// </summary>
    private void StepBack()
    {
        this.rigidbody.AddForce
            (
            new Vector3
                (
                -this.transform.forward.x * IMPULSE_FORCE,
                this.transform.position.y,
                -this.transform.forward.z * IMPULSE_FORCE
                ),
            ForceMode.Impulse
            );
    }
    /// <summary>
    /// Нанести удар по игроку.
    /// </summary>
    public void KickPlayer()
    {
        this.soundPlayerKick.Play();


        Int32 randChance = UnityEngine.Random.Range(1, 100);

        //5% критического удара.
        if (randChance < 5)
        {
            PlayerModel.instance.healthPoints -= this.criticalDamage;
        }
        else
        {
            PlayerModel.instance.healthPoints -= this.damage;
        }

        StepBack();
    }

    #endregion

    #region Данные о местоположении врага или игрока.

    /// <summary>
    /// Значение высоты врага.
    /// </summary>
    private Single yHeightField = 0;
    /// <summary>
    /// Значение высоты врага.
    /// </summary>
    private Single yHeight
    {
        get
        {
            return this.transform.position.y;
        }
        set
        {
            this.yHeightField = value;
            /*
             * Если враг над полом, то пусть падает.
             * Если нет, то убрать физику
             */
            if (this.yHeightField > 0)
            {
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                this.yHeightField = 0;

                this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
    /// <summary>
    /// Цель, к которой стремится снеговик.
    /// </summary>
    private Vector3 targetPosition
    {
        get => new Vector3
            (
            this.mainCamera.transform.position.x,
            this.yHeight,
            this.mainCamera.transform.position.z
            );
    }
    /// <summary>
    /// Враг находится около игрока?
    /// </summary>
    public Boolean IsNearWithPlayer
    {
        get => Math.Abs(this.transform.position.x - targetPosition.x) + Math.Abs(this.transform.position.z - targetPosition.z) < 2.5f;
    }
    /// <summary>
    /// Высота на которой находится пол.
    /// Y координата.
    /// </summary>
    private Single floorHeight;

    #endregion
}
