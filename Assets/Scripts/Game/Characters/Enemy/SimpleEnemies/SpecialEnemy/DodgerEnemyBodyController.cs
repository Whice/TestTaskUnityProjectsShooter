using UnityEngine;

public class DodgerEnemyBodyController : MonoBehaviour
{
    /// <summary>
    /// ћодель врага, которому принадлежит тело.
    /// </summary>
    [SerializeField]
    private DodgerEnemyModel enemyModel = null;

    private void OnTriggerEnter(Collider other)
    {
        //ѕри попадании пули в голову, сразу убить игрока.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            this.enemyModel.Dodge();
        }
    }
}
