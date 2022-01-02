using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель пули в игре.
/// </summary>
public class BulletModel : ItemModel
{
    /// <summary>
    /// Скорость движения пули.
    /// </summary>
    public Single speed = 100f;
    /// <summary>
    /// Дальность полета. При превышении пуля уничтожается.
    /// </summary>
    private Single rangeOfFlight;
    private void Start()
    {
        this.rangeOfFlight = GameObject.Find("Floor").transform.localScale.x * 5 + 10;
    }
    void Update()
    {
        //Движение пули каждый кадр.
        this.transform.position = this.transform.position + this.transform.forward * this.speed * Time.deltaTime;

        //Уничтожение снаряда, если он улетел далеко.
        if (this.transform.position.y < -1 ||//Снаряд под землей.
            this.transform.position.x > this.rangeOfFlight ||
            this.transform.position.y > this.rangeOfFlight ||
            this.transform.position.z > this.rangeOfFlight
            )
        {
            Deactivate();
        }
    }

    #region Активация и деактивация пули.

    /// <summary>
    /// Положение пули в списке активных или неактивных пуль.
    /// </summary>
    private Int32 numberInListPrivate = -1;
    /// <summary>
    /// Положение пули в списке активных или неактивных пуль.
    /// Задать можно только один раз.
    /// Номер должен быть больше 0.
    /// </summary>
    public Int32 numberInList
    {
        get => this.numberInListPrivate;
        set
        {
            if(this.numberInListPrivate==-1 && value>-1)
            {
                this.numberInListPrivate = value;
            }
        }
    }
    /// <summary>
    /// Сделать пулю неактивной:
    /// убрать пулю с арены и сделать ее неактивной.
    /// </summary>
    public void Deactivate()
    {
        ArenaModel.instance.activeBullets.RemoveAt(numberInListPrivate);

        this.numberInListPrivate = ArenaModel.instance.notActiveBullets.Count;
        ArenaModel.instance.notActiveBullets.Add(this);
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Активация пули:
    /// перемещает ее на арену во время выстрела
    /// и запускает звук выстрела.
    /// </summary>
    public void Activate(in Vector3 position, in Vector3 forward)
    {
        ArenaModel.instance.notActiveBullets.RemoveAt(this.numberInListPrivate);

        this.numberInListPrivate = ArenaModel.instance.activeBullets.Count;
        ArenaModel.instance.activeBullets.Add(this);
        this.gameObject.SetActive(true);

        this.transform.position = position;
        this.transform.forward = forward;

        (this.view as BulletView).shotSound.Play();
    }

    #endregion

    #region Цвет пули.

    /// <summary>
    /// Материал объекта пули.
    /// </summary>
    private Material bulletMaterial = null;
    /// <summary>
    /// Установить случайный цвет ящика.
    /// </summary>
    public void SetColor(Color color)
    {
        if (this.bulletMaterial == null)
        {
            this.bulletMaterial = this.GetComponent<Renderer>().material;
        }
        this.bulletMaterial.color = color;
    }

    #endregion
}
