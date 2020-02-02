using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    bool mainmenuClosed;
    bool gameStarted;

    int shotsLeft;
    int pigsalive;
    int pigCount;

    string playbtmtext_1 = "Play";
    string playbtntext_2 = "continue";

    [SerializeField]  TMP_Text shootsLeft_Text;
    [SerializeField] TMP_Text pigsAlive_Text;
    [SerializeField] TMP_Text pigCount_Text;
    [SerializeField] TMP_Text playbuttonText;


    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject gamePanels;
    [SerializeField] GameObject LevelComplettePanel;
    [SerializeField] GameObject clock;


    bool isFirstSession = true;

    private void Start()
    {
        gameStarted = true;
        GameSystemManager.onRoundOver += ResetTimer;
        GameSystemManager.onBuildtimeOver += hideClock;
        GameSystemManager.onLevelComplette += ShowLevelComplete;
        GameSystemManager.onGameOver += ShowGameOver;

    }

    private void OnDestroy()
    {
        GameSystemManager.onRoundOver -= ResetTimer;
        GameSystemManager.onBuildtimeOver -= hideClock;
        GameSystemManager.onLevelComplette -= ShowLevelComplete;
        GameSystemManager.onGameOver -= ShowGameOver;
    }

    void ResetTimer()
    {
        clock.SetActive(true);
        Camera.main.GetComponent<CameraController>().ZoomIn();
    }
    void hideClock()
    {
        clock.SetActive(false);
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gamePanels.SetActive(false);
        clock.SetActive(false);
    }

    void ShowLevelComplete()
    {
        LevelComplettePanel.SetActive(true);
        gamePanels.SetActive(false);
    }


    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && mainmenuClosed)
            {
                mainMenuPanel.SetActive(true);
                gamePanels.SetActive(false);
                mainmenuClosed = false;
                playbuttonText.text = playbtntext_2;
            }
        }

    }

    public void PlayPressed()
    {
        gameStarted = false;
        mainmenuClosed = true;
        mainMenuPanel.SetActive(false);
        gamePanels.SetActive(true);
        clock.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
