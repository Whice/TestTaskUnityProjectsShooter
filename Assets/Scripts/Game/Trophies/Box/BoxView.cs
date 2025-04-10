﻿using UnityEngine;

public class BoxView : TrophyItemView
{
    #region  Цвет.

    /// <summary>
    /// Материал ящика.
    /// </summary>
    private Material materialPrivate;
    /// <summary>
    /// Материал ящика.
    /// </summary>
    public Material material
    {
        get
        {
            if(this.materialPrivate==null)
            {
                this.materialPrivate = this.transform.GetChild(0).GetComponent<Renderer>().material;
            }
            return this.materialPrivate;
        }
    }

    /// <summary>
    /// Цвет ящика.
    /// </summary>
    protected Color colorPrivate;
    /// <summary>
    /// Цвет ящика.
    /// </summary>
    public Color color
    {
        get => this.colorPrivate;
    }

    /// <summary>
    /// Установить случайный цвет ящика.
    /// </summary>
    public void SetRandomColor()
    {
        int rValue = Random.Range(11, 39)/10;
        switch (rValue)
        {
            case 1:
                {
                    this.colorPrivate = Color.red;
                }
                break;
            case 2:
                {
                    this.colorPrivate = Color.yellow;
                }
                break;
            case 3:
                {
                    this.colorPrivate = Color.green;
                }
                break;
            default:
                {
                    this.colorPrivate = Color.green;
                }
                break;
        }
        this.material.color = this.colorPrivate;
    }

    #endregion
}
