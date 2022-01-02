using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : ItemController
{
    public void CreateWithoutArenaBoxes()
    {
        var model = this.model as ArenaModel;
        var view = this.view as ArenaView;
        var boxes = model.withoutArenaBoxes;
        for (Int32 number = 0; number < ArenaModel.boxesCountOnStart; number++)
        {
            ArenaModel.instance.CreateNewGameBoxWithoutArena();
        }
        for (Int32 number = 0; number < ArenaModel.countDeadEnemyInStartGame; number++)
        {
            ArenaModel.instance.CreateNewDeadEnemy();
        }
        ArenaModel.instance.AddNotActiveBullets(200);
    }
}
