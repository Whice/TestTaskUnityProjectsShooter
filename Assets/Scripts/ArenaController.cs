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
            //инициализация объект
            var box = new BoxModel();//На самом деле берется из объекта
            box.player = view.player;
            boxes.Add(box);
        }
    }
}
