using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalControl : MonoBehaviour
{
    GameManager playMng;
    Rigidbody rb;
    private bool point1, point2, point3;
    // Start is called before the first frame update
    void Start()
    {
        point1 = true;
        point2 = true;
        point3 = true;
       
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(playMng == null)
            playMng = GameObject.Find("playManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal")){
            if (getPoint())
            {
                playMng.goal();
                setPoint(false);
            }
        }
        if (other.CompareTag("point1")) { point1 = true; }
        if (other.CompareTag("point2")) { point2 = true; }
        if (other.CompareTag("point3")) { point3 = true; }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "OutOfGround")
            rb.mass = 120f;
        if (collision.gameObject.tag == "Ground")
            rb.mass = 70f;
        
    }

    //외부 이용 함수
    public bool getPoint()
    {
        if (point1 && point2 && point3)
            return true;
        else
            return false;
    }
    public void setPoint(bool bo)
    {
        point1 = bo;
        point2 = bo;
        point3 = bo;
    }

}
