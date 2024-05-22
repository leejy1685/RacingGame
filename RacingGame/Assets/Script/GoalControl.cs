using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalControl : MonoBehaviour
{
    playManager playMng;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        playMng = GameObject.Find("playManager").GetComponent<playManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            playMng.goal();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "OutOfGround")
            rb.mass = 120f;
        if (collision.gameObject.tag == "Ground")
            rb.mass = 70f;
        
    }

}
