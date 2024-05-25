using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startGame;
    public Button EscButton;
    public Button QuitButton;
    public Button EnterButton;
    public GameObject startMenu;
    public Button carRight;
    public Button carLeft;
    public Image carImg;
    public Sprite[] carSp;

    private int car;
    public GameObject[] player;


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
        Button btn5 = carRight.GetComponent<Button>();
        btn5.onClick.AddListener(carR);
        Button btn6 = carLeft.GetComponent<Button>();
        btn6.onClick.AddListener(carL);

        car = 0;
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
        player[car].name = "player";
        Instantiate(player[car]);

        SceneManager.LoadScene("map1");
    }
    public void carR()
    {
        car++;
        if (car > 3)
            car = 0;
        carImg.sprite = carSp[car];

    }
    public void carL()
    {
        car--;
        if (car < 0)
            car = 3;
        carImg.sprite = carSp[car];
    }

    public int getCar() { return car; }
}
