using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    /// <summary>
    /// ��������� ������� ����� � ������.
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Arena");
    }
    /// <summary>
    /// ����� �� ����.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
