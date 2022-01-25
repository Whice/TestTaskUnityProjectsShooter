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
    public void OnTriggerEnter(Collider other)
    {

        //При столкновении с пулей нанести урон врагу.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            this.enemyModel.healthPoints -= PlayerModel.instance.damage;
        }

        //При столкновении с игроком ему сразу наноситься один удар.
        if (other.gameObject.name == "PlayerFront")
        {
            this.enemyModel.KickPlayer();
        }
    }
}
