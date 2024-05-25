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
    //�� ��Ʈ�� ���� �Ұ���
    GameManager playMng;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //��ü ����
        theRB.transform.parent = null;
    }
    private void Update()
    {
        if(sm == null)
            sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        if(playMng == null)
            playMng = GameObject.Find("playManager").GetComponent<GameManager>();

        //��ü ���󰡱�
        transform.position = theRB.transform.position;

        //�ӵ� ����
        speedInput = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speedInput = forwardAccel * 1000f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            speedInput =  reverseAccel * 1000f;
        }
        
        //�Ҹ� ����
        if(theRB.velocity != new Vector3(0, 0, 0))
        {
            sm.idleStop();
            sm.runningPlay();
        }
        else
        {
            sm.runningStop();
            sm.idlePlay();
        }



        turnInput = Input.GetAxis("Horizontal");
        //���� ��� ���� ���� ȸ�� ����
        if(grounded)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime* Input.GetAxis("Vertical"), 0f));
        //�� ���� ����
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

        //����Ʈ ��Ʈ�ѷ� �ӵ� ����
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            forwardAccel--;
            StartCoroutine(sm.BoostPlay());
        }
        if (forwardAccel < -8 || Input.GetKeyUp(KeyCode.UpArrow))
            forwardAccel = -4;




        //�帮��Ʈ ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            theRB.mass = 120;
            theRB.drag = 300;
            turnStrength = 270;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
                sm.DriftPlay();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            theRB.mass = 70;
            theRB.drag = 3;
            turnStrength = 180;
            sm.DriftStop();
        }

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
            if (Mathf.Abs(speedInput) > 0 && playMng.getCarActive())
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

    public float getForwardAccel() { return forwardAccel; }

}
