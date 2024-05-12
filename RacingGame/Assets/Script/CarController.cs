using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //움직임 제어
    public Rigidbody theRB;
    private float speedInput, turnInput;
    //조작 수치
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f, dragOnGround = 3f ;
    //바닥에 닿지 않았을 때 마찰력 제어
    private bool grounded;
    //바닥에 닿는거 판단
    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;
    //앞 바퀴 돌리는 디테일 구현
    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;
    //달릴 때 먼지 날리는 거 구현
    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f;
    private float emissionRate;
    //소리 구현
    private SoundManager sm;



    private void Start()
    {
        //구체 독립
        theRB.transform.parent = null;
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sm.idlePlay();
    }
    private void Update()
    {
        //구체 따라가기
        transform.position = theRB.transform.position;

        //속도, 소리 조작
        speedInput = 0;
        if (Input.GetKey("w"))
        {
            speedInput = forwardAccel * 1000f;
            sm.idleStop();
            sm.runningPlay();
        }
        else if (Input.GetKey("s"))
        {
            speedInput =  reverseAccel * 1000f;
            sm.idleStop();
            sm.runningPlay();
        }
        else
        {
            sm.idlePlay();
            sm.runningStop();

        }


        turnInput = Input.GetAxis("Horizontal");
        //땅에 닿아 있을 때만 회전 가능
        if(grounded)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime* Input.GetAxis("Vertical"), 0f));
        //앞 바퀴 조절
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);



    }

    private void FixedUpdate()
    {
        //바닥에 닿는거 판단
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        emissionRate = 0;
        //달리는 거, 마찰력 조절 구현
        if (grounded)
        {
            theRB.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                theRB.AddForce(transform.forward * speedInput);
                emissionRate = maxEmission;
            }

        }
        else
        {
            theRB.drag = 0.1f;
            theRB.AddForce(Vector3.up * -gravityForce * 100f);
        }
        //달릴 때 먼지 구현
        foreach (ParticleSystem part in dustTrail)
        {
            var emissionModule = part.emission;
            emissionModule.rateOverTime = emissionRate;

        }


    }

}
