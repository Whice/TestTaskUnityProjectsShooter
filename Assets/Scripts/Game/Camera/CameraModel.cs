using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : ItemModel
{
    public CameraView cameraView
    {
        get => this.view as CameraView;
    }

    #region Реализация синглтона

    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    public static CameraModel instance = null;
    private void Awake()
    {
        if (CameraModel.instance == null)
        {
            CameraModel.instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    /// <summary>
    /// Проверить находится ли точка в зоне видимости камеры камеры.
    /// </summary>
    /// <param name="position">Местоположение точки.</param>
    /// <returns></returns>
    public Boolean InViewportCamera(Vector3 position)
    {
        return this.cameraView.InViewportCamera(position);
    }
}
