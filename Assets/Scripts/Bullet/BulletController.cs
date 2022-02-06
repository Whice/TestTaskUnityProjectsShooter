using System;
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
            nameCollision.Contains("Enemy") ||
            nameCollision.Contains("BulletBorders")
            )
        {
            BulletModel model = this.model as BulletModel;
            model.Deactivate();
        }
    }
}
