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
}
