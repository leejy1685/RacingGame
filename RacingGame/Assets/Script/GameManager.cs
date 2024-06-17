using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //초기화 지점
    Transform[] wayPoint;
    //플레이어 위치
    Rigidbody playerPoint;
    GameObject player;
    CarController carCon;
    public Transform startPosition;
    //UI
    public GameObject countDownUI;
    public int countdownTime;
    public TextMeshProUGUI Display;
    public TextMeshProUGUI LapDisplay;
    public TextMeshProUGUI Timer;
    public GameObject GameMenu;

    //조작 활성화
    private bool carActive;
    //규칙을 위한 카운트
    int goalCnt;
    //타이머
    bool timerActive;
    float time;
    int second,minute;
    //UI 꾸미기 용
    public Sprite[] arrowKey;
    public Sprite[] ctrlUpDown;
    public Sprite[] ShiftUpDown;
    public Sprite[] RUpDown;
    public Image arrowKeyUp;
    public Image arrowKeyDown;
    public Image arrowKeyLeft;
    public Image arrowKeyRight;
    public Image CTRL;
    public Image Shift;
    public GameObject[] speed;
    public Image RButton;
    //Sound
    SoundManager sm;
    //Score
    public TextMeshProUGUI best;
    public TextMeshProUGUI newScore;
    public Button reStartButton;
    public Button Exit;
    //GoalControll
    GoalControl goalCon;

    // Start is called before the first frame update
    void Start()
    {
        //위치 초기화 포인트
        wayPoint = new Transform[22];
        for (int i = 1; i < 22; i++)
        {
            string str = "wayPoint" + i;
            wayPoint[i] = GameObject.Find(str).GetComponent<Transform>();
        }

        //현 차의 위치 파악
        player = GameObject.Find("player(Clone)");
        carCon = player.GetComponent<CarController>();
        playerPoint = GameObject.Find("Sphere").GetComponent<Rigidbody>();
        goalCon = GameObject.Find("Sphere").GetComponent<GoalControl>();

        //사운드 매니저
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        //ButtonSetting
        Button btn = reStartButton.GetComponent<Button>();
        btn.onClick.AddListener(reStart);
        Button btn1 = Exit.GetComponent<Button>();
        btn1.onClick.AddListener(exit);
        //start
        Invoke("reStart",0.1f);
        
     
    }

    // Update is called once per frame
    void Update()
    {
        //리셋 버튼
        resetButton();

        //타이머
        timerUI();

        //UI
        UIControll();

        //pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }


    }
    
    //카운트 다운 시스템
    IEnumerator CountdownStart()
    {
        carActive = false;
        countDownUI.SetActive(true);
        while(countdownTime > 0)
        {
            if (countdownTime == 1)
                sm.readyPlay();
            Display.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        sm.goPlay();
        Display.text = "Start!";
        carActive = true;
        yield return new WaitForSeconds(1f);
        countdownTime = 3;
        countDownUI.SetActive(false);

    }

    //결승점 통과 시스템
    IEnumerator goalCount()
    {
        goalCnt++;
        if (goalCnt == 1)
            startTimer();
        if (goalCnt == 3)
        {
            carActive = false;
            stopTimer();
        }

        switch (goalCnt) {
            case 1:
                countDownUI.SetActive(true);
                Display.text = "First Lap";
                LapDisplay.text = "1/2Lap";
                yield return new WaitForSeconds(1f);
                countDownUI.SetActive(false);
                break;
            case 2:
                sm.hurryUpPlay();
                countDownUI.SetActive(true);
                Display.text = "Final Lap";
                LapDisplay.text = "2/2Lap";
                yield return new WaitForSeconds(1f);
                countDownUI.SetActive(false);
                break;
            case 3:
                sm.CongrePlay();
                sm.GoalPlay();
                countDownUI.SetActive(true);
                Display.text = "Ending!";
                yield return new WaitForSeconds(1f);
                countDownUI.SetActive(false);

                //bestTime 기록
                if(!PlayerPrefs.HasKey("best"))
                    PlayerPrefs.SetFloat("best", 999999);
                float thisTime = minute * 60 + second + time;
                if (PlayerPrefs.GetFloat("best") > thisTime)
                    PlayerPrefs.SetFloat("best", thisTime);

                pause();
                break;
        }

    }
    //내부 이용 함수
    //리셋 버튼
    private void resetButton()
    {
        //차 뒤집어 졌을 때 위치 초기화 버튼
        if (Input.GetKeyDown("r"))
        {
            playerPoint.velocity = new Vector3(0, 0, 0);
            float minDis = 999999999;
            int res = 0;
            for (int i = 1; i < 22; i++)
            {
                float dis = Vector3.Distance(wayPoint[i].position, playerPoint.position);
                if (dis < minDis)
                {
                    res = i;
                    minDis = dis;
                }

            }
            playerPoint.MovePosition(wayPoint[res].position);
            player.transform.rotation = wayPoint[res].rotation;
            StartCoroutine(CountdownStart());
        }
    }
    //타이머 시작
    private void startTimer() {timerActive = true;}
    //타이머 종료
    private void stopTimer() { timerActive = false; }
    //타이머 UI 조작
    private void timerUI()
    {
        if (timerActive)
        {
            //타이머
            time += Time.deltaTime;
            if (time > 1)
            {
                second++;
                time--;
            }
            if (second >= 60)
            {
                second -= 60;
                minute++;
            }
            Timer.text = string.Format("{0:D2}", minute) + ":" + string.Format("{0:D2}", second) + ":" + string.Format("{0:D2}", (int)(time * 100));
        }
    }

    //UI컨트롤 함수
    private void UIControll()
    {
        //누르면 색 이미지 변환
        if (Input.GetKey(KeyCode.UpArrow))
            arrowKeyUp.sprite = arrowKey[1];
        else
        {
            arrowKeyUp.sprite = arrowKey[0];
            for (int i = 0; i < 4; i++)
            {
                if (carCon.getForwardAccel() < -4 - i)
                    speed[i].SetActive(true);
                else
                    speed[i].SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
            arrowKeyDown.sprite = arrowKey[1];
        else
            arrowKeyDown.sprite = arrowKey[0];
        if (Input.GetKey(KeyCode.LeftArrow))
            arrowKeyLeft.sprite = arrowKey[1];
        else
            arrowKeyLeft.sprite = arrowKey[0];
        if (Input.GetKey(KeyCode.RightArrow))
            arrowKeyRight.sprite = arrowKey[1];
        else
            arrowKeyRight.sprite = arrowKey[0];
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CTRL.sprite = ctrlUpDown[1];
            for (int i = 0; i < 4; i++)
            {
                if (carCon.getForwardAccel() < -4 - i)
                    speed[i].SetActive(true);
                else
                    speed[i].SetActive(false);
            }

        }
        else
            CTRL.sprite = ctrlUpDown[0];
        if (Input.GetKey(KeyCode.LeftShift))
            Shift.sprite = ShiftUpDown[1];
        else
            Shift.sprite = ShiftUpDown[0];
        if (Input.GetKey(KeyCode.R))
            RButton.sprite = RUpDown[1];
        else
            RButton.sprite = RUpDown[0];
    }
    private void pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            GameMenu.SetActive(true);

            float besttime = PlayerPrefs.GetFloat("best");
            best.text = string.Format("{0:D2}", (int)(besttime/60)) + ":" + string.Format("{0:D2}", (int)(besttime % 60)) + ":" + string.Format("{0:D2}", (int)(besttime % 1 * 100));
            newScore.text = Timer.text;
        }
        else
        {
            Time.timeScale = 1;
            GameMenu.SetActive(false);
        }
    }

    private void reStart()
    {
        //바퀴 수
        LapDisplay.text = "0/2Lap";
        goalCnt = 0;

        
        //위치 이동
        playerPoint.MovePosition(startPosition.position);
        player.transform.rotation = startPosition.rotation;
        StartCoroutine(CountdownStart());

        //게임 메뉴 비활성
        GameMenu.SetActive(false);
        Time.timeScale = 1;

        //타이머 초기화 및 비활성
        stopTimer();
        time = 0;
        second = 0;
        minute = 0;
        Timer.text = "00:00:00";

        //노래 초기화
        sm.BGMStop();
        sm.BGMPlay();

        //속도 초기화
        playerPoint.velocity = new Vector3(0, 0, 0);

        //goal
        goalCon.setPoint(true);

    }
    public void exit()
    {
        Destroy(player);
        Destroy(GameObject.Find("Sphere"));
        pause();
        SceneManager.LoadScene("StartScene");
    }

    //외부 이용 함수
    //조작 활성 유무
    public bool getCarActive() { return carActive; }
    //골 카운트
    public void goal() { StartCoroutine(goalCount()); }
}
