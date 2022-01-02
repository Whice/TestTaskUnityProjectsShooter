using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : ItemModel
{

    #region Реализация синглтона

    private CameraModel() { }

    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    private static CameraModel instancePrivate = null;
    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    public static CameraModel instance
    {
        get => instancePrivate == null ? new CameraModel() : instancePrivate;
    }

    #endregion

    
}
