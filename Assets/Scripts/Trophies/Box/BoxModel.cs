using System;

public class BoxModel: TrophyItemView
{
    public BoxView boxView
    {
        get => this.view as BoxView;
    }
    /// <summary>
    /// Положение в списке активных или неактивных.
    /// </summary>
    private Int32 numberInListPrivate = -1;
    /// <summary>
    /// Положение в списке активных или неактивных.
    /// Задать можно только один раз.
    /// Номер должен быть больше 0.
    /// </summary>
    public Int32 numberInList
    {
        get => this.numberInListPrivate;
        set
        {
            if (this.numberInListPrivate == -1 && value > -1)
            {
                this.numberInListPrivate = value;
            }
        }
    }
    /// <summary>
    /// Сделать ящик активным, 
    /// чтобы его можно было положить на арену.
    /// </summary>
    public void Activate()
    {
        ArenaModel arenaModel = ArenaModel.instance;

        arenaModel.withoutArenaBoxes.RemoveAt(this.numberInListPrivate);
        arenaModel.onArenaBoxes.Add(this);
        this.numberInListPrivate = arenaModel.onArenaBoxes.Count - 1;
        this.gameObject.SetActive(true);
        this.boxView.SetRandomColor();
    }
    /// <summary>
    /// Убрать ящик с арены.
    /// </summary>
    public void Deactivate()
    {
        ArenaModel arenaModel = ArenaModel.instance;

        arenaModel.onArenaBoxes.RemoveAt(this.numberInListPrivate);
        arenaModel.withoutArenaBoxes.Add(this);
        this.numberInListPrivate = arenaModel.withoutArenaBoxes.Count - 1;
        this.gameObject.SetActive(false);
    }
}
