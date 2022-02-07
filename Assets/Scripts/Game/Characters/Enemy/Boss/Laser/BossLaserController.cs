using UnityEngine;

public class BossLaserController : ItemController
{
    public BossModel bossModel = null;
    private void OnTriggerEnter(Collider other)
    {
        //При столкновении нанести сказать боссу нанести урон персонажу.
        if (other.gameObject.name == "SimpleEnemy(Clone)")
        {
            this.bossModel.KickOtherEnemy(other.transform.GetComponent<EnemyModel>());
        }
        else if (other.gameObject.name == "PlayerFront")
        {
            this.bossModel.KickPlayer();
        }

    }
}
