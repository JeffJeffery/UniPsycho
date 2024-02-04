using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicycleMovment : MonoBehaviour
{

    public GameObject Unicycle_Body;
    public GameObject Unicycle_Wheel;
    public float maxMotorSpeed = 500;
    public float motorInterpolate;
    public float leanTorque;
    public float jumpForce;

    private float curMotorSpeed = 0;
    private HingeJoint2D hinge;
    private JointMotor2D motorTemp;
    private Rigidbody2D bodyRigidBody;

    private float distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        hinge = Unicycle_Wheel.GetComponent<HingeJoint2D>();
        bodyRigidBody = Unicycle_Body.GetComponent<Rigidbody2D>();

        distanceToGround = Unicycle_Wheel.GetComponent<CircleCollider2D>().radius;

        hinge.useMotor = true;
    }

    // Update is called once per frame
    void Update()
    {
        wheelMovment();
        rotateBody();
        jump();
    }


    void wheelMovment()
    {
        //move left, make motor speed negative
        if (Input.GetKey(KeyCode.A))
        {
            curMotorSpeed = Mathf.Lerp(-maxMotorSpeed, curMotorSpeed, motorInterpolate);
        }
        //move right, make motor speed postive
        else if (Input.GetKey(KeyCode.D))
        { 
            curMotorSpeed = Mathf.Lerp(maxMotorSpeed, curMotorSpeed, motorInterpolate);
        }

        else
        {
            curMotorSpeed = Mathf.Lerp(0, curMotorSpeed, motorInterpolate);
        }

        motorTemp = hinge.motor;
        motorTemp.motorSpeed = curMotorSpeed;
        hinge.motor = motorTemp;
    }

    void rotateBody()
    {
        //move left, make motor speed negative
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bodyRigidBody.AddTorque(leanTorque);
        }
        //move right, make motor speed postive
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            bodyRigidBody.AddTorque(-leanTorque);
        }

        else
        {
            bodyRigidBody.AddTorque(0);
        }
    }

    void jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Vector2 jumpVector = bodyRigidBody.transform.up;
            bodyRigidBody.AddForce(jumpVector * jumpForce);
        }
    }
  
    bool isGrounded()
    {
        Debug.Log("Is Grounded run: position:" + Unicycle_Wheel.transform.position);
        Debug.DrawRay(new Vector3(0, 0, 0), -Vector3.up, Color.green);
        return Physics.Raycast(Unicycle_Wheel.transform.position, -Vector3.up, distanceToGround + 0.1f);
    }


}
