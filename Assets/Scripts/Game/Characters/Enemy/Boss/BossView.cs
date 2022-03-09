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

    #region полоска HP босса.

    /// <summary>
    /// Картинка, которая должна динамически
    /// уменьшаться при нанесении урона боссу.
    /// </summary>
    public Image dynamicFrontHP
    {
        get => CameraModel.instance.cameraView.dynamicFrontHP;
    }
    /// <summary>
    /// Вся полоска ХП босса.
    /// </summary>
    public GameObject bossHPBar
    {
        get => CameraModel.instance.cameraView.bossHPBar;
    }

    #endregion полоска HP босса.
}
