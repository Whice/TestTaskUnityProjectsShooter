using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraView : ItemView
{
    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    public Camera mainCamera = null;

    #region полоска HP босса.

    [Header("Boss HP bar")]
    /// <summary>
    /// Картинка, которая должна динамически
    /// уменьшаться при нанесении урона боссу.
    /// </summary>
    public Image dynamicFrontHP = null;
    /// <summary>
    /// Вся полоска ХП босса.
    /// </summary>
    public GameObject bossHPBar = null;

    #endregion полоска HP босса.
}