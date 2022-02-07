using UnityEngine;

/// <summary>
/// Контроллер врага в игре.
/// </summary>
public class SimpleEnemyController : EnemyController
{
    private SimpleEnemyModel enemyModel
    {
        get => this.model as SimpleEnemyModel;
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        //При столкновении с игроком ему сразу наноситься один удар.
        if (other.gameObject.name == "PlayerFront")
        {
            this.enemyModel.KickPlayer();
        }
        //При столкновении с игроком ему сразу наноситься один удар.
        else if (other.gameObject.name.Contains("Wall"))
        {
            this.transform.position = this.transform.position + this.transform.forward;
        }

    }
}
