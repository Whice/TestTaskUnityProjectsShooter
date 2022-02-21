using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    /// <summary>
    /// Загрузить игровую сцену с ареной.
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Arena");
    }
    /// <summary>
    /// Выйти из игры.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
