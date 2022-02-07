using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Arena");
    }
}
