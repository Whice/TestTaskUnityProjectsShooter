using System;
using UnityEngine;

public class BossModel : EnemyModel
{
    #region Нанести урон

    public void KickPlayer()
    {
        PlayerModel.instance.ApplyDamage(this.damage);
    }
    public void KickOtherEnemy(EnemyModel otherEnemy)
    {
        otherEnemy.ApplyDamage(this.damage);
    }

    #endregion

    #region Стрельба лазерами

    public Animator shootAnimations = null;
    private Single timerDelay = 0f;
    private const Single DELAY_BETWEEN_SHOOTS = 10f;
    private BossAttacks lastAttack = BossAttacks.rightLaserShoot;

    private void ShootLaser(String nameAimation)
    {
        this.shootAnimations.Play(nameAimation);
        //Тут прописать звук выстрела.
    }
    private void ShootLeftLaser()
    {
        ShootLaser("BossShootAnimationLeft");
        this.lastAttack = BossAttacks.leftLaserShoot;
    }
    private void ShootRightLaser()
    {
        ShootLaser("BossShootAnimationRight");
        this.lastAttack = BossAttacks.rightLaserShoot;
    }
    private void ExecuteNextShoot()
    {
        switch (this.lastAttack)
        {
            case BossAttacks.leftLaserShoot:
                {
                    ShootRightLaser();
                    break;
                }
            case BossAttacks.rightLaserShoot:
                {
                    ShootLeftLaser();
                    break;
                }
        }
    }
    private void EnableTurnToPlayer()
    {
        this.isEnableTurnToPlayer = true;
        this.timerDelay = 0;
    }

    #endregion

    #region Действия при создании объекта.

    protected override void SetDamage()
    {
        this.damage = 10 * level;
    }
    protected override void SetHealth()
    {
        this.healthPoints = 100 * level;
    }
    private void Start()
    {
        InitializeEnemy();
    }

    #endregion

    private void Update()
    {
        // Персонаж либо поворачивается к игроку, либо атакует.
        if (this.timerDelay<DELAY_BETWEEN_SHOOTS)
        {
            //Повернуться к игроку
            TurnToPlayer();
            this.timerDelay += Time.deltaTime;
        }
        else if(this.isEnableTurnToPlayer)
        {
            ExecuteNextShoot();
            this.isEnableTurnToPlayer = false;
        }
    }

    #region Активация/деактивация

    protected override void GetTrophy()
    {
        for (UInt16 countBox = 0; countBox < this.level; countBox++)
        {
            for (int i = 0; i < 10; i++)
            {
                BoxModel boxModel = ArenaModel.instance.GetLastNotActiveBoxModel();
                boxModel.Activate();
                boxModel.transform.position = new Vector3
                    (
                     this.transform.position.x + i,
                      this.transform.position.y,
                       this.transform.position.z + level
                    );
            }
        }
    }
    /// <summary>
    /// Есть ли босс на арене.
    /// </summary>
    private Boolean isActivePrivate = false;
    /// <summary>
    /// Есть ли босс на арене.
    /// </summary>
    public Boolean isActive
    {
        get => this.isActivePrivate;
    }
    /// <summary>
    /// Уровень босса. Увеличиватся при его смерти.
    /// </summary>
    private UInt16 level = 1;
    /// <summary>
    /// Поднять уровень босса на 1.
    /// </summary>
    private void LevelUp()
    {
        this.level++;
        SetDamage();
        SetHealth();
        SimpleEnemyModel.level++;
        
    }
    /// <summary>
    /// Сделать неактивным.
    /// </summary>
    public override void Deactivate()
    {
        LevelUp();
        this.isActivePrivate = false;
        base.Deactivate();
    }
    protected override void SetBeginPosition()
    {
        this.transform.position = new Vector3(0, this.yHeight + 1, 0);
    }
    /// <summary>
    /// Добавить к живым.
    /// </summary>
    public override void Activate()
    {
        this.isActivePrivate = true;
        base.Activate();
        this.yHeight = 0;
    }

    #endregion
}
