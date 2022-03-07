using System;
using UnityEngine;

/// <summary>
/// Модель врага, который ускоряется когда игрок его не видит.
/// </summary>
public class InvisibleSpeedBoosterEnemyModel: SimpleEnemyModel
{
    private void Update()
    {
        OnUpdate();
        if(InViewportCamera(this.transform.position))
        {
            this.speed = ORIGIN_SPEED;
        }
        else
        {
            this.speed = ORIGIN_SPEED * 5;
        }
    }
}
