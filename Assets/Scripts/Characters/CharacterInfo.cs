using System;
using UnityEngine;

/// <summary>
/// Класс с общими настройками и полями для игрока и его противников.
/// </summary>
public class CharacterInfo : MonoBehaviour
{
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
            if(value<0)
            {
                this.healthPointsPrivate = 0;
            }
            else
            {
                this.healthPointsPrivate = value;
            }
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
            return this.healthPoints == 0;
        }
        set
        {
            if(value)
            {
                this.healthPoints = 0;
            }
            else
            {
                this.healthPoints = 10;
            }
        }
    }
}
