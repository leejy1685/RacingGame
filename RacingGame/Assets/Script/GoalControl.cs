using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalControl : MonoBehaviour
{
    playManager playMng;
    // Start is called before the first frame update
    void Start()
    {
        playMng = GameObject.Find("playManager").GetComponent<playManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            playMng.goal();
        }
    }
}
