using UnityEngine;
using UnityEngine.UI;

public class BossView : EnemyView
{
    /// <summary>
    /// ��������, ������� ����������� �� ������.
    /// </summary>
    public GameObject heartForHealth
    {
        get => ArenaModel.instance.arenaView.heartForHealthTrophy;
    }

    #region ������� HP �����.

    /// <summary>
    /// ��������, ������� ������ �����������
    /// ����������� ��� ��������� ����� �����.
    /// </summary>
    public Image dynamicFrontHP
    {
        get => CameraModel.instance.cameraView.dynamicFrontHP;
    }
    /// <summary>
    /// ��� ������� �� �����.
    /// </summary>
    public GameObject bossHPBar
    {
        get => CameraModel.instance.cameraView.bossHPBar;
    }

    #endregion ������� HP �����.
}
