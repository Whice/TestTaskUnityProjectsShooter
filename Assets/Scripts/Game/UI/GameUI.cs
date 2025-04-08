using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Space;
    public GameObject pausePanel;
    public GameObject gameUIPanel;
    public Button resumeButton;
    public Button mainMenuButton;



    private bool isPause = false;
    public bool IsPause() => isPause;
    public event Action<bool> pauseChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeButton.onClick.AddListener(() => SetPause(false));
        SetPause(false);
    }
    private void SetPause(bool isPause)
    {
        this.isPause = isPause;
        pausePanel.SetActive(isPause);
        gameUIPanel.SetActive(!isPause);
        pauseChanged?.Invoke(isPause);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(pauseKey) && !isPause)
        {
            SetPause(true);
        }
    }
}
