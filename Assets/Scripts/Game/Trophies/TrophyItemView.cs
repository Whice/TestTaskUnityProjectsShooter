using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyItemView : ItemView
{
    #region Звук.

    /// <summary>
    /// Звук высасывания.
    /// </summary>
    public AudioSource suckSound
    {
        get => PlayerModel.instance.playerView.suckTrophy;
    }

    #endregion
}
