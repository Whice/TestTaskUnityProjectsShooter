using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : ItemController
{
    public ArenaModel arenaModel
    {
        get => ArenaModel.instance;
    }

    /// <summary>
    /// Обновить списки пуль.
    /// </summary>
    private void CreateBulletLists()
    {
        this.arenaModel.activeBullets = new List<BulletModel>(ArenaModel.BULLET_COUNT_ON_START);
        this.arenaModel.notActiveBullets = new List<BulletModel>(ArenaModel.BULLET_COUNT_ON_START);
        for (Int32 number = 0; number < ArenaModel.BULLET_COUNT_ON_START; number++)
        {
            this.arenaModel.CreateNewGameBoxWithoutArena();
        }
    }
    /// <summary>
    /// Обновить списки врагов.
    /// </summary>
    private void UpdateEnemyLists()
    {
        this.arenaModel.deadEnemies = new List<EnemyModel>(ArenaModel.COUNT_DEAD_ENEMY_ON_START);
        this.arenaModel.aliveEnemies = new List<EnemyModel>(ArenaModel.COUNT_DEAD_ENEMY_ON_START);
        for (Int32 number = 0; number < ArenaModel.COUNT_DEAD_ENEMY_ON_START; number++)
        {
            ArenaModel.instance.CreateNewDeadEnemy();
        }
    }
    /// <summary>
    /// Обновить списки врагов.
    /// </summary>
    private void UpdateBoxesLists()
    {
        this.arenaModel.withoutArenaBoxes = new List<BoxModel>(ArenaModel.BOXES_COUNT_ON_START);
        this.arenaModel.onArenaBoxes = new List<BoxModel>(ArenaModel.BOXES_COUNT_ON_START);
    }

    /// <summary>
    /// При входе на сцену обновить все списке,
    /// т.к. объекты в них уничтожаются, а списки остаются.
    /// </summary>
    private void OnLevelWasLoaded()
    {
        //Обновить списки пуль и врагов перед началом игры.
        CreateBulletLists();
        UpdateEnemyLists();
        UpdateBoxesLists();
    }    
}
