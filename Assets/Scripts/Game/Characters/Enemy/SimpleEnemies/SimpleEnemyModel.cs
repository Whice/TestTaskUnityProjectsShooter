using System;
using UnityEngine;

/// <summary>
/// Модель врага в игре.
/// </summary>
public class SimpleEnemyModel : EnemyModel
{
    public SimpleEnemyView simpleEnemyView
    {
        get => this.view as SimpleEnemyView;
    }

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        OnUpdate();
    }

    /// <summary>
    /// Действия при обновлении кадра.
    /// </summary>
    protected virtual void OnUpdate()
    {
        //Если этот враг жив, то он может стремиться к убийству игрока.
        MoveToOrKickPlayer();

        //Убить снеговика, который упал слишком низко.
        KillEnemyWithoutArena();

        //Повернуться к игроку
        TurnToPlayer();
    }

    #region Активация/деактивация

    /// <summary>
    /// Убить этого врага.
    /// </summary>
    public virtual void KillEnemy()
    {
        ApplyDamage(this.healthPoints);
    }
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

    public override void Deactivate()
    {
        this.arenaModel.aliveEnemies.Remove(this);
        this.arenaModel.deadEnemies.Add(this);
        ArenaModel.instance.countOfKilledSimpleEnemy++;
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
    protected Boolean InViewportCamera(Vector3 position)
    {
        return CameraModel.instance.InViewportCamera(position);
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
                                            /*Y*/ this.yHeight,
                                            /*Z*/ UnityEngine.Random.Range(playerPosition.z - shift, playerPosition.z + shift)
                                            );
            //Если враг слишком близко к игроку, подвинуть врага подальше.
            const Single distance = 15F;
            if (position.x * position.x + position.z * position.z < distance * distance)
            {
                position = new Vector3(-position.x + distance, position.y, position.z + distance);
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
                    if (position.x < 0)
                    {
                        X = position.x - shift;
                    }
                    else
                    {
                        X = position.x + shift;
                    }
                    Single Y;
                    if (position.y < 0)
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

            this.transform.position = position;
        }
        //Если игрок упирается в стену.
        else
        {
            Vector3 playerPosition = PlayerModel.instance.transform.position;
            var yzPosition = playerPosition - CameraModel.instance.transform.forward * 0.5f;
            //Поставить сразу за спиной игрока.
            this.transform.position = new Vector3(yzPosition.x, this.yHeight, yzPosition.z);
        }
    }

    #endregion

    #region Урон.

    /// <summary>
    /// Звук удара по игроку.
    /// </summary>
    private AudioSource soundPlayerKickPrivate = null;
    /// <summary>
    /// Звук удара по игроку.
    /// </summary>
    public AudioSource soundPlayerKick
    {
        get
        {
            if (this.soundPlayerKickPrivate == null)
            {
                this.soundPlayerKickPrivate = this.simpleEnemyView.enemiesAudioSource;
            }

            AudioClip kickClip = this.simpleEnemyView.EnemyKick;
            if (this.soundPlayerKickPrivate.clip.name != kickClip.name)
            {
                this.soundPlayerKickPrivate.clip = kickClip;
            }

            return this.soundPlayerKickPrivate;
        }
    }
    /// <summary>
    /// Критический урон.
    /// </summary>
    protected Int32 criticalDamage
    {
        get => (Int32)(this.damage * 1.8);
    }
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
    /// <summary>
    /// Если этот враг жив, то он может стремиться к убийству игрока. 
    /// Либо атаковать раз в секунду
    /// </summary>
    protected void MoveToOrKickPlayer()
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
            if (this.timerAtack > 1)
            {
                KickPlayer();
                this.timerAtack = 0;
            }
        }
        this.timerAtack += Time.deltaTime;
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
