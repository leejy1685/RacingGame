using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //버튼에 기능을 넣기 위한 버튼 클래스 생성
    public Button startGame;
    public Button EscButton;
    public Button QuitButton;
    public Button EnterButton;
    public GameObject startMenu;
    public Button carRight;
    public Button carLeft;
    public Image carImg;
    public Sprite[] carSp;

    //자동차 선택 기능을 위한 게임오브젝트 클래스
    private int car;
    public GameObject[] player;

    
    private void Start()    //스크립트 생성 시 실행되는 메소드
    {
        //버튼에 기능을 넣는 과정
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
        //오브젝트 활성화
        startMenu.SetActive(true);
    }
    public void ESC()
    {
        //오브젝트 비활성화
        startMenu.SetActive(false);
    }

    public void GameQuit()    //게임 종료
    {
        Application.Quit();
    }
    public void EnterGame() //게임 실행
    {
        //선택된 자동차의 이름을 플레이어로 변경
        player[car].name = "player";
        //생성
        Instantiate(player[car]);
        //씬 이동
        SceneManager.LoadScene("map1");
    }
    public void carR()  //자동차 선택 기능(오른쪽 버튼)
    {
        car++;
        if (car > 3)
            car = 0;
        carImg.sprite = carSp[car];

    }
    public void carL()  //자동차 선택 기능(왼쪽 버튼)
    {
        car--;
        if (car < 0)
            car = 3;
        carImg.sprite = carSp[car];
    }

}
