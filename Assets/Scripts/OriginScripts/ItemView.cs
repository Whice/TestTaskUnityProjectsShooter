using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemView : ItemModelViewContoller
{
    /// <summary>
    /// Объект модели.
    /// </summary>
    public ItemModelViewContoller model
    {
        get
        {
            if(this.itemModelProtected ==null)
            {
                this.itemModelProtected = this.transform.GetComponent<ItemModel>();
            }
            return this.itemModelProtected;
        }
    }
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    public ItemModelViewContoller controller
    {
        get
        {
            if (this.itemControllerProtected == null)
            {
                this.itemControllerProtected = this.transform.GetComponent<ItemController>();
            }
            return this.itemControllerProtected;
        }
    }
    private void Awake()
    {
        //Получить ItemModel и itemController из this
        this.itemViewProtected = this;
    }
}

