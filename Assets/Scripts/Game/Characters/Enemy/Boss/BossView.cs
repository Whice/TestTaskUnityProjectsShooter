using UnityEngine;
using UnityEngine.UI;

public class BossView : EnemyView
{
    /// <summary>
    /// Сердчеко, которое увеличивает ХП игроку.
    /// </summary>
    public GameObject heartForHealth
    {
        get => ArenaModel.instance.arenaView.heartForHealthTrophy;
    }
    public Image dynamicFrontHP = null;
}
