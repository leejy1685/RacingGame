using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public Button startGame;
    public Button EscButton;
    public Button QuitButton;
    public Button EnterButton;
    public GameObject startMenu;


    private void Start()
    {
        Button btn = startGame.GetComponent<Button>();
        btn.onClick.AddListener(startButton);
        Button btn2 = EscButton.GetComponent<Button>();
        btn2.onClick.AddListener(ESC);
        Button btn3 = QuitButton.GetComponent<Button>();
        btn3.onClick.AddListener(GameQuit);
        Button btn4 = EnterButton.GetComponent<Button>();
        btn4.onClick.AddListener(EnterGame);
    }

    public void startButton()
    {
        startMenu.SetActive(true);
    }
    public void ESC()
    {
        startMenu.SetActive(false);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void EnterGame()
    {
        SceneManager.LoadScene("map1");
    }

}
