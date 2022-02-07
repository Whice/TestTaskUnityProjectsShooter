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

    public GameObject AddiitionalCamera = null;
    public void DeactivateAllSingletons()
    {

        if(this.AddiitionalCamera!=null)
        {
            this.AddiitionalCamera.SetActive(true);
            this.AddiitionalCamera.transform.position = CameraModel.instance.transform.position;
            this.AddiitionalCamera.transform.forward = CameraModel.instance.transform.forward;
        }
        PlayerModel.instance.gameObject.SetActive(false);
        ArenaModel.instance.gameObject.SetActive(false);
        CameraModel.instance.gameObject.SetActive(false);
    }
    public void ActivateAllSingletons()
    {
        CameraModel.instance.gameObject.SetActive(true);
        ArenaModel.instance.gameObject.SetActive(true);
        PlayerModel.instance.gameObject.SetActive(true);
    }
}
