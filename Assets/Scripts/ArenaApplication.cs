using System.Collections.Generic;
using UnityEngine;

public class ArenaApplication:MonoBehaviour
{
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
