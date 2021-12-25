using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemController : ItemModelViewContoller
{
    private void Awake()
    {
        //Получить ItemModel и ItemView из this
        this.itemControllerProtected = this;
    }
}
