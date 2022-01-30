using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предмтавление пули в игре.
/// </summary>
public class BulletView : ItemView
{
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
