using System;
using UnityEngine;

public class BossModel : EnemyModel
{
    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);
        switch(propertyName)
        {
            case nameof(this.healthPoints):
                {
                    this.bossView.dynamicFrontHP.fillAmount = (Single)this.healthPoints / this.maxHealthPoint;
                    break;
                }
        }
    }
    public BossView bossView
    {
        get => this.view as BossView;
    }

    #region ������� ����

    public void KickPlayer()
    {
        PlayerModel.instance.ApplyDamage(this.damage);
    }
    public void KickOtherEnemy(EnemyModel otherEnemy)
    {
        otherEnemy.ApplyDamage(this.damage);
    }

    #endregion

    #region �������� ��������

    public Animator shootAnimations = null;
    private Single timerDelay = 0f;
    private const Single DELAY_BETWEEN_SHOOTS = 10f;
    private BossAttacks lastAttack = BossAttacks.rightLaserShoot;

    private void ShootLaser(String nameAimation)
    {
        this.shootAnimations.Play(nameAimation);
        //��� ��������� ���� ��������.
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

    private void Awake()
    {
        SetHealth();
    }

    #region �������� ��� �������� �������.

    protected override void SetDamage()
    {
        this.damage = 11 * level;
    }
    protected override void SetHealth()
    {
        this.maxHealthPoint = 111 * (level + 1);
        this.healthPoints = this.maxHealthPoint;
    }
    private void Start()
    {
        InitializeEnemy();
    }

    #endregion

    private void Update()
    {
        // �������� ���� �������������� � ������, ���� �������.
        if (this.timerDelay<DELAY_BETWEEN_SHOOTS)
        {
            //����������� � ������
            TurnToPlayer();
            this.timerDelay += Time.deltaTime;
        }
        else if(this.isEnableTurnToPlayer)
        {
            ExecuteNextShoot();
            this.isEnableTurnToPlayer = false;
        }
    }

    #region ���������/�����������

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

        //������������ ��������, ����� ����� ��� ��������� ��������.
        this.bossView.heartForHealth.SetActive(true);
    }
    /// <summary>
    /// ���� �� ���� �� �����.
    /// </summary>
    private Boolean isActivePrivate = false;
    /// <summary>
    /// ���� �� ���� �� �����.
    /// </summary>
    public Boolean isActive
    {
        get => this.isActivePrivate;
    }
    /// <summary>
    /// ������� �����. ������������ ��� ��� ������.
    /// </summary>
    private UInt16 level = 1;
    /// <summary>
    /// ������� ������� ����� �� 1.
    /// </summary>
    private void LevelUp()
    {
        this.level++;
        SetDamage();
        SetHealth();
        SimpleEnemyModel.level++;
        
    }
    /// <summary>
    /// ������� ����������.
    /// </summary>
    public override void Deactivate()
    {
        this.bossView.bossHPBar.SetActive(false);
        LevelUp();
        this.isActivePrivate = false;
        base.Deactivate();
    }
    protected override void SetBeginPosition()
    {
        this.transform.position = new Vector3(0, this.yHeight + 1, 0);
    }
    /// <summary>
    /// �������� � �����.
    /// </summary>
    public override void Activate()
    {
        this.bossView.bossHPBar.SetActive(true);
        this.isActivePrivate = true;
        base.Activate();
        this.yHeight = 0;
    }

    #endregion
}
