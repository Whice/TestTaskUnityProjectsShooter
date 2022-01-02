using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaModel : ItemModel
{
    public ArenaView arenaView
    {
        get => this.view as ArenaView;
    }

    /// <summary>
    /// Ссылка на пол арены.
    /// </summary>
    private GameObject arenaFloorPrivate = null;
    /// <summary>
    /// Ссылка на пол арены.
    /// </summary>
    public GameObject arenaFloor
    {
        get
        {
            if (this.arenaFloorPrivate == null)
            {
                this.arenaFloorPrivate = GameObject.Find("Floor");
            }
            return this.arenaFloorPrivate;
        }
    }

    #region Реализация синглтона

    private ArenaModel() { }

    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    private static ArenaModel instancePrivate = null;
    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    public static ArenaModel instance
    {
        get => instancePrivate == null ? new ArenaModel() : instancePrivate;
    }

    #endregion

    private void Update()
    {
        CreateNewEnemy();
    }

    #region Враги

    /// <summary>
    /// Количество мертвых врагов, которое будет создано в началае игры.
    /// </summary>
    public const Int32 countDeadEnemyInStartGame = 100;
    /// <summary>
    /// Список мертвых врагов.
    /// В списке мертвых врагов хораняться еще или уже не задействованые враги.
    /// Таким образом на их создание и уничтожение не тратиться время.
    /// </summary>
    public List<EnemyModel> deadEnemies = new List<EnemyModel>(countDeadEnemyInStartGame);
    /// <summary>
    /// Список живых врагов.
    /// </summary>
    public List<EnemyModel> aliveEnemies = new List<EnemyModel>(countDeadEnemyInStartGame);
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
        EnemyModel newAliveEnemy = this.deadEnemies[deadEnemies.Count - 1];
        this.deadEnemies.RemoveAt(deadEnemies.Count - 1);
        this.aliveEnemies.Add(newAliveEnemy);
        newAliveEnemy.gameObject.SetActive(true);

        //Задать ему начальную позицию.
        newAliveEnemy.SetRandomPositionWithoutCameraVision();
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
    public const Int32 boxesCountOnStart = 100;
    /// <summary>
    /// Список ящиков вне арены.
    /// </summary>
    public List<BoxModel> withoutArenaBoxes = new List<BoxModel>(boxesCountOnStart);
    /// <summary>
    /// Список ящиков на арене.
    /// </summary>
    public List<BoxModel> onArenaBoxes = new List<BoxModel>(boxesCountOnStart);
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
        return this.withoutArenaBoxes[this.withoutArenaBoxes.Count - 1];
    }

    #endregion

    #region Пули

    /// <summary>
    /// Добавить несколько пуль в список неактивных.
    /// </summary>
    /// <param name="count">Количество пуль.</param>
    public void AddNotActiveBullets(Int32 count)
    {
        GameObject newBullet = null;
        for (Int32 i=0;i<count;i++)
        {
            newBullet = Instantiate(this.arenaView.bulletPrefab);
            newBullet.SetActive(false);
            BulletModel newBulletModel = newBullet.GetComponent<BulletModel>();
            newBulletModel.numberInList = this.notActiveBullets.Count;
            this.notActiveBullets.Add(newBulletModel);
        }
    }
    /// <summary>
    /// Список неактивных пуль.
    /// </summary>
    public List<BulletModel> notActiveBullets = new List<BulletModel>();
    /// <summary>
    /// Список активных пуль.
    /// </summary>
    public List<BulletModel> activeBullets = new List<BulletModel>();
    /// <summary>
    /// Получить последнюю из списка неактивных пуль.
    /// </summary>
    /// <returns></returns>
    public BulletModel GetLastNotActiveBullet()
    {
        Int32 lastNumber = this.notActiveBullets.Count - 1;
        if (lastNumber<0)
        {
            AddNotActiveBullets(50);
        }
        return this.notActiveBullets[this.notActiveBullets.Count - 1];
    }

    #endregion
}
