using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraView : ItemView
{
    #region Main camera.

    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    public Camera mainCamera = null;

    /// <summary>
    /// Проверить находится ли точка в зоне видимости камеры камеры.
    /// </summary>
    /// <param name="position">Местоположение точки.</param>
    /// <returns></returns>
    public Boolean InViewportCamera(Vector3 position)
    {
        Vector3 viewPosition = this.mainCamera.WorldToViewportPoint(position);
        if (viewPosition.x > 0.015f && viewPosition.x < 1.05f && viewPosition.z > 0)
        {
            return true;
        }
        return false;
    }

    #endregion Main camera.

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

    #region Skybox rotate

    /// <summary>
    /// Скорость вращения неба.
    /// </summary>
    private const Single speedRaotateSkybox = 0.08f;
    private void Update()
    {
        //Вращение неба.
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedRaotateSkybox);
    }

    #endregion Skybox rotate
}