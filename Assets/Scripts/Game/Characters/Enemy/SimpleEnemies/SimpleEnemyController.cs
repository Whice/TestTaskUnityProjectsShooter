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

        //Если враг в стене, то сдвинть его немного вперед.
        if (other.gameObject.name.Contains("Wall"))
        {
            this.transform.position = this.transform.position + this.transform.forward;
        }

    }
}
