using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playManager : MonoBehaviour
{
    Transform[] wayPoint;
    Rigidbody playerPoint;
    GameObject player;
    public GameObject countDownUI;
    public int countdownTime;
    public TextMeshProUGUI Display;
    public TextMeshProUGUI LapDisplay;
    private bool carActive;
    int goalCnt;

    // Start is called before the first frame update
    void Start()
    {
        LapDisplay.text = "0/2Lap";
        goalCnt = 0;
        wayPoint = new Transform[22];
        for(int i = 1; i < 22; i++)
        {
            string str = "wayPoint" + i;
            wayPoint[i] = GameObject.Find(str).GetComponent<Transform>();
        }
        player = GameObject.Find("raceCarRed");
        playerPoint = GameObject.Find("Sphere").GetComponent<Rigidbody>();
        StartCoroutine(CountdownStart());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            playerPoint.velocity = new Vector3(0, 0, 0);
            float minDis = 999999999;
            int res = 0;
            for(int i = 1; i < 22; i++)
            {
                float dis = Vector3.Distance(wayPoint[i].position, playerPoint.position);
                if (dis < minDis) {
                    res = i;
                    minDis = dis;
                }
                 
            }
            playerPoint.MovePosition(wayPoint[res].position);
            player.transform.rotation = wayPoint[res].rotation;
            StartCoroutine(CountdownStart());
        }
    }

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

    IEnumerator goalCount()
    {
        goalCnt++;
        if (goalCnt == 3)
            carActive = false;
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

    public bool getCarActive() { return carActive; }
    public void goal() { StartCoroutine(goalCount()); }
}
