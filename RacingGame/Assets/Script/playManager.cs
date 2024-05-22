using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playManager : MonoBehaviour
{
    //�ʱ�ȭ ����
    Transform[] wayPoint;
    //�÷��̾� ��ġ
    Rigidbody playerPoint;
    GameObject player;
    //UI
    public GameObject countDownUI;
    public int countdownTime;
    public TextMeshProUGUI Display;
    public TextMeshProUGUI LapDisplay;
    public TextMeshProUGUI Timer;

    //���� Ȱ��ȭ
    private bool carActive;
    //��Ģ�� ���� ī��Ʈ
    int goalCnt;
    //Ÿ�̸�
    bool timerActive;
    float time;
    int second,minute;
    //UI �ٹ̱� ��
    public Sprite[] arrowKey;
    public Image arrowKeyImg;

    // Start is called before the first frame update
    void Start()
    {   
        //���� ��
        LapDisplay.text = "0/2Lap";
        goalCnt = 0;

        //��ġ �ʱ�ȭ ����Ʈ
        wayPoint = new Transform[22];
        for(int i = 1; i < 22; i++)
        {
            string str = "wayPoint" + i;
            wayPoint[i] = GameObject.Find(str).GetComponent<Transform>();
        }

        //�� ���� ��ġ �ľ�
        player = GameObject.Find("raceCarRed");
        playerPoint = GameObject.Find("Sphere").GetComponent<Rigidbody>();

        //���� ī��Ʈ
        StartCoroutine(CountdownStart());

        
    }

    // Update is called once per frame
    void Update()
    {
        //���� ��ư
        resetButton();

        //Ÿ�̸�
        timerUI();

        //UI
        if (Input.GetKey(KeyCode.UpArrow))
            arrowKeyImg.sprite = arrowKey[1];
        else if (Input.GetKey(KeyCode.DownArrow))
            arrowKeyImg.sprite = arrowKey[2];
        else if (Input.GetKey(KeyCode.LeftArrow))
            arrowKeyImg.sprite = arrowKey[3];
        else if (Input.GetKey(KeyCode.RightArrow))
            arrowKeyImg.sprite = arrowKey[4];
        else
            arrowKeyImg.sprite = arrowKey[0];

    }
    
    //ī��Ʈ �ٿ� �ý���
    IEnumerator CountdownStart()
    {
        carActive = false;
        countDownUI.SetActive(true);
        while(countdownTime > 0)
        {
            Display.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        Display.text = "Start!";
        carActive = true;
        yield return new WaitForSeconds(1f);
        countdownTime = 3;
        countDownUI.SetActive(false);

    }

    //����� ��� �ý���
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
                countDownUI.SetActive(true);
                Display.text = "Final Lap";
                LapDisplay.text = "2/2Lap";
                yield return new WaitForSeconds(1f);
                countDownUI.SetActive(false);
                break;
            case 3:
                countDownUI.SetActive(true);
                Display.text = "Ending!";
                yield return new WaitForSeconds(1f);
                countDownUI.SetActive(false);
                break;
        }

    }
    //���� �̿� �Լ�
    //���� ��ư
    private void resetButton()
    {
        //�� ������ ���� �� ��ġ �ʱ�ȭ ��ư
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
    //Ÿ�̸� ����
    private void startTimer() {timerActive = true;}
    //Ÿ�̸� ����
    private void stopTimer() { timerActive = false; }
    //Ÿ�̸� UI ����
    private void timerUI()
    {
        if (timerActive)
        {
            //Ÿ�̸�
            time += Time.deltaTime;
            if (time > 1)
            {
                second++;
                time--;
            }
            if (second > 60)
            {
                second -= 60;
                minute++;
            }
            Timer.text = string.Format("{0:D2}", minute) + ":" + string.Format("{0:D2}", second) + ":" + string.Format("{0:D2}", (int)(time * 100));
        }
    }

    //�ܺ� �̿� �Լ�
    //���� Ȱ�� ����
    public bool getCarActive() { return carActive; }
    //�� ī��Ʈ
    public void goal() { StartCoroutine(goalCount()); }
}
