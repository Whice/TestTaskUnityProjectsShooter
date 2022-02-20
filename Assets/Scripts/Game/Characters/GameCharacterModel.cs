using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель персонажа в игре.
/// </summary>
public class GameCharacterModel : ItemModel
{
    /// <summary>
    /// Максимальное количество очков здоровья.
    /// </summary>
    public Int32 maxHealthPoint = 100;
    /// <summary>
    /// Очки жизни.
    /// </summary>
    private Int32 healthPointsPrivate = 10;
    /// <summary>
    /// Очки жизни.
    /// </summary>
    public Int32 healthPoints
    {
        get
        {
            return this.healthPointsPrivate;
        }
        set
        {
            Int32 newValue = value;
            if (value < 0)
            {
                newValue = 0;
            }
            else if (value > this.maxHealthPoint)
            {
                newValue = this.maxHealthPoint;
            }

            SetValueProperty(nameof(this.healthPoints), ref this.healthPointsPrivate, newValue);
        }
    }

    /// <summary>
    /// Урон.
    /// </summary>
    public Int32 damage = 5;
    /// <summary>
    /// Если true, то персонаж мертв.
    /// </summary>
    public Boolean isDead
    {
        get
        {
            return this.healthPoints < 1;
        }
        set
        {
            if (value)
            {
                this.healthPoints = 0;
            }
            else
            {
                this.healthPoints = 10;
            }
        }
    }

    /// <summary>
    /// Нанести урон этому персонажу.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void ApplyDamage(Int32 damage)
    {
        this.healthPoints -= damage;
    }

    private void Update()
    {
        Update(true);
    }
    protected virtual void Update(Boolean execute) { }
}
