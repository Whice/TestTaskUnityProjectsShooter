using System;
using UnityEngine;

/// <summary>
/// Модель врага в игре.
/// </summary>
public class SimpleEnemyModel : EnemyModel
{
    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);
        switch (propertyName)
        {
            case nameof(countOfKilledSimpleEnemy):
                {

                    if (this.countOfKilledSimpleEnemy % DEAD_SIMPLE_ENEMY_FOR_CREATE_BOSS == 0)
                    {
                        ArenaModel.instance.CreateNewBoss();
                    }
                }
                break;
        }        
    }

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
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

        //Убить снеговика, который упал слишком низко.
        KillEnemyWithoutArena();

        //Повернуться к игроку
        TurnToPlayer();
    }

    #region Активация/деактивация

    protected override void GetTrophy()
    {
        BoxModel boxModel = ArenaModel.instance.GetLastNotActiveBoxModel();
        boxModel.Activate();
        boxModel.transform.position = this.transform.position;
    }

    /// <summary>
    /// Уровень врага. Увеличиватся при смерти босса.
    /// </summary>
    public static UInt16 level = 1;
    protected override void SetHealth()
    {
        this.healthPoints = 10 * level;
    }

    /// <summary>
    /// Количество простых врагов нужное, чтобы появился босс.
    /// </summary>
    private const UInt64 DEAD_SIMPLE_ENEMY_FOR_CREATE_BOSS = 50;
    /// <summary>
    /// Колчиество простых убитых врагов.
    /// </summary>
    private static UInt64 countOfKilledSimpleEnemyPrivate = 0;
    /// <summary>
    /// Колчиество простых убитых врагов.
    /// </summary>
    protected UInt64 countOfKilledSimpleEnemy
    {
        get
        {
            return SimpleEnemyModel.countOfKilledSimpleEnemyPrivate;
        }
        set
        {
            SetValueProperty(nameof(countOfKilledSimpleEnemy), ref SimpleEnemyModel.countOfKilledSimpleEnemyPrivate, value);
        }
    }
    public override void Deactivate()
    {
        this.arenaModel.aliveEnemies.Remove(this);
        this.arenaModel.deadEnemies.Add(this);
        this.countOfKilledSimpleEnemy++;
        base.Deactivate();
    }

    protected override void SetBeginPosition()
    {
        base.SetBeginPosition();
        this.SetRandomPositionWithoutCameraVision();
        this.yHeight = 5f;
    }
    public override void Activate()
    {
        base.Activate();
        SetHealth();
        SetRandomSpeed();
    }

    #endregion

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
    }

    #endregion

    #region Установка случайного местоположения.

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
    public override void SetRandomPositionWithoutCameraVision()
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
                else if (InViewportCamera(position))
                {
                    position = new Vector3(position.x, position.y, -position.z);
                }

                //Если ни одно положение вокруг центра не подошло, то сдивнуть подальше от него.
                if (InViewportCamera(position))
                {
                    shift = 5;
                    Single X;
                    if(position.x<0)
                    {
                        X = position.x - shift;
                    }
                    else
                    {
                        X = position.x + shift;
                    }
                    Single Y;
                    if(position.y<0)
                    {
                        Y = position.y - shift;
                    }
                    else
                    {
                        Y = position.y + shift;
                    }


                    position = new Vector3(X, position.y, Y);
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
    public AudioSource soundPlayerKick = null;
    /// <summary>
    /// Критический урон.
    /// </summary>
    private Int32 criticalDamage = 9;
    /// <summary>
    /// Сила отталкивания врага после нанесения удара игроку.
    /// </summary>
    const Single IMPULSE_FORCE = 20f;
    /// <summary>
    /// Шанс критического удара.
    /// </summary>
    const Int32 CRITICAL_CHANCE = 15;
    /// <summary>
    /// Отойти назад.
    /// </summary>
    private void StepBack()
    {
        this.rigidbody.AddForce
            (
            new Vector3
                (
                -this.transform.forward.x * IMPULSE_FORCE * this.rigidbody.mass,
                this.transform.position.y,
                -this.transform.forward.z * IMPULSE_FORCE * this.rigidbody.mass
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

        //Возможен критический удар.
        if (randChance < CRITICAL_CHANCE)
        {
            PlayerModel.instance.ApplyDamage(this.criticalDamage);
        }
        else
        {
            PlayerModel.instance.ApplyDamage(this.damage);
        }

        StepBack();
    }

    #endregion

    #region Данные о местоположении врага или игрока.

    /// <summary>
    /// Враг находится около игрока?
    /// </summary>
    public Boolean IsNearWithPlayer
    {
        get => Math.Abs(this.transform.position.x - targetPosition.x) + Math.Abs(this.transform.position.z - targetPosition.z) < 2.5f;
    }

    #endregion
}
