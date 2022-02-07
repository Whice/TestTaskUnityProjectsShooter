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
        if (other.gameObject.name == "PlayerFront" ||
            other.gameObject.name == "MFPController" ||
            other.gameObject.name == "Canvas" ||
            other.gameObject.name == "Player"
            )
        {
            this.boxModel.Deactivate();
        }
    }
}
