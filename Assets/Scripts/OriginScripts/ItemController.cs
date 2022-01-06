using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemController : ItemModelViewContoller
{
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    public ItemModelViewContoller view
    {
        get
        {
            if (this.itemViewProtected == null)
            {
                this.itemViewProtected = this.transform.GetComponent<ItemView>();
            }
            return this.itemViewProtected;
        }
    }
    /// <summary>
    /// Объект модели.
    /// </summary>
    public ItemModelViewContoller model
    {
        get
        {
            if (this.itemModelProtected == null)
            {
                this.itemModelProtected = this.transform.GetComponent<ItemModel>();
            }
            return this.itemModelProtected;
        }
    }
    private void Awake()
    {
        //Получить ItemModel и ItemView из this
        this.itemControllerProtected = this;
    }
}
