using System;
using UnityEngine;

public class BoxModel: TrophyItemModel
{
    public BoxView boxView
    {
        get
        {
            if (this.view == null)
            {
                this.itemViewProtected = this.gameObject.GetComponent<BoxView>();
            }

            return this.view as BoxView;
        }
    }
    /// <summary>
    /// Сделать ящик активным, 
    /// чтобы его можно было положить на арену.
    /// </summary>
    public void Activate()
    {
        ArenaModel arenaModel = ArenaModel.instance;

        arenaModel.withoutArenaBoxes.Remove(this);
        arenaModel.onArenaBoxes.Add(this);
        this.gameObject.SetActive(true);
        this.boxView.SetRandomColor();
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;

        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0));
    }
    /// <summary>
    /// Убрать ящик с арены.
    /// </summary>
    public void Deactivate()
    {
        this.boxView.suckSound.Play();
        ArenaModel arenaModel = ArenaModel.instance;

        arenaModel.onArenaBoxes.Remove(this);
        arenaModel.withoutArenaBoxes.Add(this);
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
}
