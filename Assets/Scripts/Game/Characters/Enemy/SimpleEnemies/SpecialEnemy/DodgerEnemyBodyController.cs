using UnityEngine;

public class DodgerEnemyBodyController : MonoBehaviour
{
    /// <summary>
    /// ������ �����, �������� ����������� ����.
    /// </summary>
    [SerializeField]
    private DodgerEnemyModel enemyModel = null;

    private void OnTriggerEnter(Collider other)
    {
        //��� ��������� ���� � ������, ����� ����� ������.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            this.enemyModel.Dodge();
        }
    }
}
