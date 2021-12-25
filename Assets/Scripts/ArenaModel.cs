using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaModel : ItemModel
{
    public static Int32 boxesCountOnStart = 100;
    public List<BoxModel> withoutArenaBoxes = new List<BoxModel>();
    public List<BoxModel> onArenaBoxes = new List<BoxModel>();

    private PlayerModel playerModelPrivate = null;
    public PlayerModel playerModel
    {
        get
        {
            if (this.playerModelPrivate == null)
            {
                var view = this.view as ArenaView;
                //this.playerModelPrivate = view.Plaer.GetComponent<PlayerModel>();
            }
            return this.playerModelPrivate;
        }
    }
}
