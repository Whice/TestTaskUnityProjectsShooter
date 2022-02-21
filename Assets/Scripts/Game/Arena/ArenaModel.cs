using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Арена.
/// </summary>
public class ArenaModel : ItemModel
{
    public ArenaView arenaView
    {
        get => this.view as ArenaView;
    }

    private Boolean pauseField = false;
    private Boolean pause
    {
        get => this.pauseField;
        set => SetValueProperty(nameof(this.pause), ref this.pauseField, value);
    }
    public delegate void ChangeEnableObjectHandler(Boolean isActive);
    public event ChangeEnableObjectHandler OnChangeEnableObject;
    public void ActivateAllDinamicObjectsOnArena(Boolean isActive)
    {
        this.pause = !isActive;
        this.OnChangeEnableObject?.Invoke(isActive);
    }

    #region Босс.

    /// <summary>
    /// Объект босса.
    /// </summary>
    public GameObject boss = null;
    /// <summary>
    /// Модель босса.
    /// </summary>
    private BossModel bossModel = null;
    /// <summary>
    /// Создать на арене нового босса.
    /// </summary>
    /// <returns></returns>
    public BossModel CreateNewBoss()
    {
        if(this.bossModel==null)
        {
            this.bossModel = Instantiate(this.boss).GetComponent<BossModel>();
        }
        if(!this.bossModel.isActive)
        {
            this.bossModel.Activate();
        }
        return this.bossModel;
    }

    #endregion

    #region Стены и пол арены.

    /// <summary>
    /// Ссылка на пол арены.
    /// </summary>
    public GameObject[] walls
    {
        get => this.arenaView.walls;
    }
    /// <summary>
    /// Ссылка на пол арены.
    /// </summary>
    public GameObject arenaFloor
    {
        get
        {
            return this.arenaView.arenaFloor;
        }
    }
    /// <summary>
    /// Размер пола аррены.
    /// </summary>
    public Single arenaFloorSize
    {
        get => this.arenaFloor.transform.localScale.x * 50f;
    }

    #endregion

    #region Реализация синглтона

    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    public static ArenaModel instance = null;
    private void Awake()
    {
        //организация синглтона
        if (ArenaModel.instance == null)
        {
            ArenaModel.instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        //создание пуль
        this.notActiveBullets = new List<BulletModel>(BulletController.countOfBulletInStart);
        this.activeBullets = new List<BulletModel>(BulletController.countOfBulletInStart);
        this.AddNotActiveBullets(BulletController.countOfBulletInStart);
    }

    #endregion

    private void Update()
    {
        if (!pause)
        {
            CreateNewEnemy();
        }
    }

    #region Враги

    /// <summary>
    /// Количество простых врагов нужное, чтобы появился босс.
    /// </summary>
    private const UInt64 DEAD_SIMPLE_ENEMY_FOR_CREATE_BOSS = 50;
    /// <summary>
    /// Количество простых убитых врагов.
    /// </summary>
    private static UInt64 countOfKilledSimpleEnemyPrivate = 0;
    /// <summary>
    /// Количество простых убитых врагов.
    /// </summary>
    public UInt64 countOfKilledSimpleEnemy
    {
        get
        {
            return ArenaModel.countOfKilledSimpleEnemyPrivate;
        }
        set
        {
            SetValueProperty(nameof(countOfKilledSimpleEnemy), ref ArenaModel.countOfKilledSimpleEnemyPrivate, value);
        }
    }
    /// <summary>
    /// Сбросить количество убитых врагов на 0 в обход всех условий.
    /// </summary>
    public void ResetCountOfKilledSimpleEnemy()
    {
        ArenaModel.countOfKilledSimpleEnemyPrivate = 0;
    }

    /// <summary>
    /// Количество мертвых врагов, которое будет создано в началае игры.
    /// </summary>
    public const Int32 COUNT_DEAD_ENEMY_ON_START = 100;
    /// <summary>
    /// Список мертвых врагов.
    /// В списке мертвых врагов хораняться еще или уже не задействованые враги.
    /// Таким образом на их создание и уничтожение не тратиться время.
    /// </summary>
    [HideInInspector]
    public List<EnemyModel> deadEnemies = new List<EnemyModel>(COUNT_DEAD_ENEMY_ON_START);
    /// <summary>
    /// Список живых врагов.
    /// </summary>
    [HideInInspector]
    public List<EnemyModel> aliveEnemies = new List<EnemyModel>(COUNT_DEAD_ENEMY_ON_START);
    /// <summary>
    /// Создать нового мертвого врага.
    /// </summary>
    /// <returns></returns>
    public EnemyModel CreateNewDeadEnemy()
    {
        GameObject enemy = Instantiate(this.arenaView.enemyPrefab);
        EnemyModel enemyModel = enemy.GetComponent<EnemyModel>();
        enemy.SetActive(false);
        this.deadEnemies.Add(enemyModel);
        return enemyModel;
    }

    /// <summary>
    /// Таймер для отсчета времени перед появления нового врага.
    /// </summary>
    private Single timerForSpawnEnemy = 0;
    /// <summary>
    /// Оживить одного врага и поставить его в случайное место, где его не видно.
    /// </summary>
    private void ReviveOneEnemyInRandomPlace()
    {
        //Если враги кончились, создать нового мертвого врага.
        if (this.deadEnemies.Count == 0)
        {
            CreateNewDeadEnemy();
        }

        //добавить врага к живым
        EnemyModel newAliveEnemy = this.deadEnemies[this.deadEnemies.Count - 1];
        this.deadEnemies.RemoveAt(this.deadEnemies.Count - 1);
        this.aliveEnemies.Add(newAliveEnemy);
        newAliveEnemy.Activate();
    }
    /// <summary>
    /// Создать нового врага на арене("оживить его").
    /// </summary>
    private void CreateNewEnemy()
    {
        //Создание нового врага.
        this.timerForSpawnEnemy += Time.deltaTime;
        if (this.timerForSpawnEnemy - Time.deltaTime > 3)
        {
            this.timerForSpawnEnemy = Time.deltaTime;
            ReviveOneEnemyInRandomPlace();
        }
    }

    #endregion

    #region Ящики

    /// <summary>
    /// Количество неактивных ящиков в начале игры.
    /// </summary>
    public const Int32 BOXES_COUNT_ON_START = 50;
    /// <summary>
    /// Список ящиков вне арены.
    /// </summary>
    [HideInInspector]
    public List<BoxModel> withoutArenaBoxes = new List<BoxModel>(BOXES_COUNT_ON_START);
    /// <summary>
    /// Список ящиков на арене.
    /// </summary>
    [HideInInspector]
    public List<BoxModel> onArenaBoxes = new List<BoxModel>(BOXES_COUNT_ON_START);
    /// <summary>
    /// Создать новый ящик вне арены.
    /// </summary>
    public void CreateNewGameBoxWithoutArena()
    {
        GameObject box = Instantiate(this.arenaView.gameBoxPrefab);
        BoxModel boxModel = box.GetComponent<BoxModel>();
        this.withoutArenaBoxes.Add(boxModel);
        box.SetActive(false);
    }
    /// <summary>
    /// Получить последний неактивный ящик.
    /// </summary>
    /// <returns></returns>
    public BoxModel GetLastNotActiveBoxModel()
    {
        Int32 lastNumber = this.withoutArenaBoxes.Count - 1;
        if (lastNumber<0)
        {
            for(Int32 i=0;i<BOXES_COUNT_ON_START;i++)
            {
                CreateNewGameBoxWithoutArena();
            }
        }
        return this.withoutArenaBoxes[this.withoutArenaBoxes.Count - 1];
    }

    #endregion

    #region Пули

    /// <summary>
    /// Количество неактивных пуль в начале игры.
    /// </summary>
    public const Int32 BULLET_COUNT_ON_START = 200;
    /// <summary>
    /// Создать одну новую неактивную пулю.
    /// </summary>
    private void CreateNewNotActiveBullet()
    {
        GameObject newBullet = Instantiate(this.arenaView.bulletPrefab);
        newBullet.SetActive(false);
        BulletModel newBulletModel = newBullet.GetComponent<BulletModel>();
        this.notActiveBullets.Add(newBulletModel);
    }
    /// <summary>
    /// Добавить несколько пуль в список неактивных.
    /// </summary>
    /// <param name="count">Количество пуль.</param>
    public void AddNotActiveBullets(Int32 count)
    {
        for (Int32 i = 0; i < count; i++)
        {
            CreateNewNotActiveBullet();
        }
    }
    /// <summary>
    /// Список неактивных пуль.
    /// </summary>
    [HideInInspector]
    public List<BulletModel> notActiveBullets = new List<BulletModel>();
    /// <summary>
    /// Список активных пуль.
    /// </summary>
    [HideInInspector]
    public List<BulletModel> activeBullets = new List<BulletModel>();
    /// <summary>
    /// Получить последнюю из списка неактивных пуль.
    /// </summary>
    /// <returns></returns>
    public BulletModel GetLastNotActiveBullet()
    {
        Int32 lastNumber = this.notActiveBullets.Count - 1;
        if (lastNumber < 0)
        {
            AddNotActiveBullets(50);
        }

        return this.notActiveBullets[this.notActiveBullets.Count - 1];
    }

    #endregion

    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);

        switch (propertyName)
        {
            case nameof(this.pause):
                {
                    this.arenaView.textPause.SetActive(this.pause);
                    break;
                }
            case nameof(countOfKilledSimpleEnemy):
                {

                    if (this.countOfKilledSimpleEnemy % DEAD_SIMPLE_ENEMY_FOR_CREATE_BOSS == 0)
                    {
                        ArenaModel.instance.CreateNewBoss();
                        PlayerModel.instance.playerView.endBossText.SetActive(true);
                    }
                break;
                }
        }
    }
}
