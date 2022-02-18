using System;
using System.Collections.Generic;
using UnityEngine;

public class MainApplicationClass : MonoBehaviour
{
    #region Реализация синглтона

    /// <summary>
    /// Объект главного класса приложения.
    /// </summary>
    public static MainApplicationClass instance = null;
    private void Awake()
    {
        if (MainApplicationClass.instance == null)
        {
            MainApplicationClass.instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    public List<ItemController> controllers = new List<ItemController>();
    public List<ItemModel> models = new List<ItemModel>();
    public List<ItemView> views = new List<ItemView>();
    public void AddItemModelViewContoller(ItemModelViewContoller item)
    {
        if (item is ItemController controller)
        {
            this.controllers.Add(controller);
        }
        else if (item is ItemModel model)
        {
            this.models.Add(model);
        }
        else if (item is ItemView view)
        {
            this.views.Add(view);
        }
    }


    #region Включение/отключение нужных объектов при смене сцен.

    /// <summary>
    /// Запасная камера. Нужна, когда отключается основная. 
    /// </summary>
    public GameObject AddiitionalCamera = null;
    /// <summary>
    /// Занавес, чтобы загородить взор камеры.
    /// </summary>
    public GameObject blackBox = null;
    /// <summary>
    /// Деактивация всех одиночек для перехода между сценами.
    /// </summary>
    public void DeactivateAllSingletons()
    {
        SetActiveAllSingletons(false);
    }
    /// <summary>
    /// Активация всех одиночек для перехода между сценами.
    /// </summary>м
    public void ActivateAllSingletons()
    {
        SetActiveAllSingletons(true);
    }
    /// <summary>
    /// Установка активированности для одиночек.
    /// </summary>
    /// <param name="isActive">Включить?</param>
    private void SetActiveAllSingletons(Boolean isActive)
    {
        SetActiveForAddiitionalCamera(isActive);
        CameraModel.instance.gameObject.SetActive(isActive);
        ArenaModel.instance.gameObject.SetActive(isActive);
        PlayerModel.instance.gameObject.SetActive(isActive);
    }
    /// <summary>
    /// Включить/отключить дополнительную камеру.
    /// </summary>
    /// <param name="isActive">Камера должна быть включена.</param>
    private void SetActiveForAddiitionalCamera(Boolean isActive)
    {
        if (this.AddiitionalCamera != null && this.blackBox != null)
        {
            this.AddiitionalCamera.SetActive(!isActive);
            if (!isActive)
            {
                this.AddiitionalCamera.transform.position = CameraModel.instance.transform.position;
                this.AddiitionalCamera.transform.forward = CameraModel.instance.transform.forward;
                this.AddiitionalCamera.GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnLevelWasLoaded()
    {
        //При входе на сцену отключить дополнительную камеру.
        SetActiveForAddiitionalCamera(false);
    }

    #endregion Включение/отключение нужных объектов при смене сцен.
}
