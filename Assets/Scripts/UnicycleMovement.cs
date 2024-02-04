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
    private Collider2D wheelCollider;

    //private float distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        hinge = Unicycle_Wheel.GetComponent<HingeJoint2D>();
        bodyRigidBody = Unicycle_Body.GetComponent<Rigidbody2D>();

        wheelCollider = Unicycle_Wheel.GetComponent<CircleCollider2D>();

        hinge.useMotor = true;
    }

    // Update is called once per frame
    void Update()
    {
        WheelMovement();
        RotateBody();
        Jump();
    }


    void WheelMovement()
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

    void RotateBody()
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

    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector2 jumpVector = bodyRigidBody.transform.up;
            bodyRigidBody.AddForce(jumpVector * jumpForce);
        }
    }
  
    bool IsGrounded()
    {
        float radius = wheelCollider.bounds.extents.y;
        //return Physics2D.Raycast((Vector2) wheelCollider.bounds.center + Vector2.down*(radius + .01f), Vector2.down, .01f);
        return Physics2D.BoxCast((Vector2)wheelCollider.bounds.center, wheelCollider.bounds.size/4, 0f, Vector2.down, wheelCollider.bounds.extents.y + .01f);
    }


}
