using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : ItemModel
{

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
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion


}
