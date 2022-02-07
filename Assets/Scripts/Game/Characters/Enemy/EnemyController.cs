using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер врага в игре.
/// </summary>
public class EnemyController : GameCharacterController
{
    private EnemyModel enemyModel
    {
        get => this.model as EnemyModel;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        //При столкновении с пулей нанести урон врагу.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            this.enemyModel.ApplyDamage(PlayerModel.instance.damage);
        }
    }
}
