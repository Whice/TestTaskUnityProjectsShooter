using System;
using UnityEngine;

public class BoxModel: TrophyItemModel
{
    private void Update()
    {
        // Если ящик ждет деактивации и звук всасывания закончил проигрывание,
        //то отключить ящик.
        if(isDeactivateWait && !this.boxView.suckSound.isPlaying)
        {
            this.isDeactivateWait = false;
            this.gameObject.SetActive(false);
        }
    }

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

        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 200, 0));
    }

    private Boolean isDeactivateWait = false;
    /// <summary>
    /// Убрать ящик с арены.
    /// </summary>
    public void Deactivate()
    {
        this.boxView.suckSound.Play(0);
        PlayerModel.instance.AddColoredCube(this.boxView.color);

        ArenaModel arenaModel = ArenaModel.instance;
        arenaModel.onArenaBoxes.Remove(this);
        arenaModel.withoutArenaBoxes.Add(this);

        //Отправить под пол в ожидании деактивации.
        this.transform.position = new Vector3(0, ArenaModel.instance.arenaFloor.transform.position.y - 10, 0);
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
}
