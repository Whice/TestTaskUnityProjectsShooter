using UnityEngine;

public class BossView : EnemyView
{
    /// <summary>
    /// ��������, ������� ����������� �� ������.
    /// </summary>
    public GameObject heartForHealth
    {
        get => ArenaModel.instance.arenaView.heartForHealthTrophy;
    }
}
