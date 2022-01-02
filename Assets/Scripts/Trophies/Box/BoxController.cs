using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : TrophyItemController
{
    public BoxView boxView
    {
        get => this.view as BoxView;
    }
    public BoxModel boxModel
    {
        get => this.model as BoxModel;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            this.boxView.suckThisBox.Play();
            PlayerModel.instance.AddColoredCube(this.boxView.color);
            this.boxModel.Deactivate();
        }
    }
}
