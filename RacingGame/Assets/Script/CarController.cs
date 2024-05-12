using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //������ ����
    public Rigidbody theRB;
    private float speedInput, turnInput;
    //���� ��ġ
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f, dragOnGround = 3f ;
    //�ٴڿ� ���� �ʾ��� �� ������ ����
    private bool grounded;
    //�ٴڿ� ��°� �Ǵ�
    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;
    //�� ���� ������ ������ ����
    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;
    //�޸� �� ���� ������ �� ����
    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f;
    private float emissionRate;
    //�Ҹ� ����
    private SoundManager sm;



    private void Start()
    {
        //��ü ����
        theRB.transform.parent = null;
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        sm.idlePlay();
    }
    private void Update()
    {
        //��ü ���󰡱�
        transform.position = theRB.transform.position;

        //�ӵ�, �Ҹ� ����
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
        //���� ��� ���� ���� ȸ�� ����
        if(grounded)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime* Input.GetAxis("Vertical"), 0f));
        //�� ���� ����
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);



    }

    private void FixedUpdate()
    {
        //�ٴڿ� ��°� �Ǵ�
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        emissionRate = 0;
        //�޸��� ��, ������ ���� ����
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
        //�޸� �� ���� ����
        foreach (ParticleSystem part in dustTrail)
        {
            var emissionModule = part.emission;
            emissionModule.rateOverTime = emissionRate;

        }


    }

}
