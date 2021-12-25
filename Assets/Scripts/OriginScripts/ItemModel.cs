using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemModel : ItemModelViewContoller
{
    private void Awake()
    {
        //Получить ItemView и itemController из this
        this.itemModelProtected = this;
    }
}
