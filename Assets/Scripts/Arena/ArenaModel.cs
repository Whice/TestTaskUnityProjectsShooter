﻿using System;
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
            DontDestroyOnLoad(this);
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
    [HideInInspector]
    public List<EnemyModel> deadEnemies = new List<EnemyModel>(countDeadEnemyInStartGame);
    /// <summary>
    /// Список живых врагов.
    /// </summary>
    [HideInInspector]
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
    public const Int32 boxesCountOnStart = 50;
    /// <summary>
    /// Список ящиков вне арены.
    /// </summary>
    [HideInInspector]
    public List<BoxModel> withoutArenaBoxes = new List<BoxModel>(boxesCountOnStart);
    /// <summary>
    /// Список ящиков на арене.
    /// </summary>
    [HideInInspector]
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
        Int32 lastNumber = this.withoutArenaBoxes.Count - 1;
        if (lastNumber<0)
        {
            for(Int32 i=0;i<boxesCountOnStart;i++)
            {
                CreateNewGameBoxWithoutArena();
            }
        }
        return this.withoutArenaBoxes[this.withoutArenaBoxes.Count - 1];
    }

    #endregion

    #region Пули

    /// <summary>
    /// Создать Одну новую неактивную пулю.
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
}