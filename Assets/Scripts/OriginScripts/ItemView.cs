using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemView : ItemModelViewContoller
{
    private void Awake()
    {
        //Получить ItemModel и itemController из this
        this.itemViewProtected = this;
    }
}

