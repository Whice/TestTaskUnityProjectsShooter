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
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "PlayerFront" ||
            other.gameObject.name == "MFPController" ||
            other.gameObject.name == "Canvas" ||
            other.gameObject.name == "Player"
            )
        {
            this.boxView.suckSound.Play();
            PlayerModel.instance.AddColoredCube(this.boxView.color);
            this.boxModel.Deactivate();
        }
    }
}
