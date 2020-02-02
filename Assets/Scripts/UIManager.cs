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
    [SerializeField] GameObject clock;

    private void Start()
    {
        gameStarted = true;
        GameSystemManager.onRoundOver += ResetTimer;

    }

    private void OnDestroy()
    {
        GameSystemManager.onRoundOver -= ResetTimer;
    }

    void ResetTimer()
    {
        clock.SetActive(false);
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
}
