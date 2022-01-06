using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер пули в игре.
/// </summary>
public class BulletController : ItemController
{
    /// <summary>
    /// Количество пуль в самом начале игры.
    /// </summary>
    public const Int32 countOfBulletInStart = 99;
    private void OnCollisionEnter(Collision collision)
    {
        String nameCollision = collision.transform.gameObject.name;
        if (
            name.Contains("Enemy") ||
            name.Contains("Floor") ||
            name.Contains("Wall")
            )
        {
            BulletModel model = this.model as BulletModel;
            model.Deactivate();
        }
    }
}
