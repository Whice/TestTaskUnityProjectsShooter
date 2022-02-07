using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemModel : ItemModelViewContoller
{
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    public ItemModelViewContoller controller
    {
        get
        {
            if (this.itemControllerProtected == null)
            {
                this.itemControllerProtected = this.gameObject.GetComponent<ItemController>();
            }
            return this.itemControllerProtected;
        }
    }
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    public ItemModelViewContoller view
    {
        get
        {
            if (this.itemViewProtected == null)
            {
                this.itemViewProtected = this.gameObject.GetComponent<ItemView>();
            }
            return this.itemViewProtected;
        }
    }
    private void Awake()
    {
        //Получить ItemView и itemController из this
        this.itemModelProtected = this;
    }
}
