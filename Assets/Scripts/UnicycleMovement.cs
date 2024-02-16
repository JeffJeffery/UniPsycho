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
    public float correctionTorque;
    public float jumpForce;
    public float bodyMoveScaler;

    private float curMotorSpeed = 0;
    private HingeJoint2D hinge;
    private JointMotor2D motorTemp;
    private Rigidbody2D bodyRigidBody;
    private Rigidbody2D wheelRigitBody;
    private Collider2D wheelCollider;

    //private float distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        hinge = Unicycle_Wheel.GetComponent<HingeJoint2D>();
        bodyRigidBody = Unicycle_Body.GetComponent<Rigidbody2D>();
        wheelRigitBody = Unicycle_Wheel.GetComponent<Rigidbody2D>();
        wheelCollider = Unicycle_Wheel.GetComponent<CircleCollider2D>();
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        WheelMovement();
        RotateBody();
        Jump();
        SelfCorrect();
    }


    void WheelMovement()
    {
        //move left, make motor speed negative
        if (Input.GetKey(KeyCode.A))
        {
            hinge.useMotor = true;
            curMotorSpeed = Mathf.Lerp(-maxMotorSpeed, curMotorSpeed, motorInterpolate);
            bodyRigidBody.AddTorque(bodyMoveScaler * wheelRigitBody.velocity.magnitude);
            motorTemp = hinge.motor;
            motorTemp.motorSpeed = curMotorSpeed;
            hinge.motor = motorTemp;
        }
        //move right, make motor speed postive
        else if (Input.GetKey(KeyCode.D))
        {
            hinge.useMotor = true;
            curMotorSpeed = Mathf.Lerp(maxMotorSpeed, curMotorSpeed, motorInterpolate);
            bodyRigidBody.AddTorque(-bodyMoveScaler * wheelRigitBody.velocity.magnitude);
            motorTemp = hinge.motor;
            motorTemp.motorSpeed = curMotorSpeed;
            hinge.motor = motorTemp;
        }

        else
        {
            curMotorSpeed = 0;
            hinge.useMotor = false;
        }


    }

    void RotateBody()
    {
        //move left, make motor speed negative
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bodyRigidBody.AddForce(-bodyRigidBody.transform.right * leanTorque);
            //wheelRigitBody.AddForce(Vector2.right * leanTorque);

        }
        //move right, make motor speed postive
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            bodyRigidBody.AddForce(bodyRigidBody.transform.right * leanTorque);
            //wheelRigitBody.AddForce(Vector2.left * leanTorque);
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
  
    RaycastHit2D IsGrounded()
    {
        float radius = wheelCollider.bounds.extents.y;
        //return Physics2D.Raycast((Vector2) wheelCollider.bounds.center + Vector2.down*(radius + .01f), Vector2.down, .01f);
        return Physics2D.BoxCast((Vector2)wheelCollider.bounds.center, wheelCollider.bounds.size/2, 0f, -bodyRigidBody.transform.up, wheelCollider.bounds.extents.y + .01f);
    }


    void SelfCorrect()
    {
        if (Input.GetKey(KeyCode.LeftArrow) | (Input.GetKey(KeyCode.RightArrow)))
        {
            return;
        }
        Vector2 CorrectionUp;
        //If it is grounded, we get the normal
        RaycastHit2D Output = IsGrounded();
        if (Output)
        {
            CorrectionUp = Output.normal;
        }
        else
        {
            CorrectionUp = Vector2.up;
        }
        Vector2 PerpComp = Vector3.Project(bodyRigidBody.transform.up, Vector2.Perpendicular(CorrectionUp));

        //If PerpComp is positive we need to go to the right
        //DOT PRODUCT: if pointing same way cos(0) = 1, if pointing away cos(pi) = -1
        //Perp points left
        if (Vector2.Dot(Vector2.Perpendicular(CorrectionUp), PerpComp) > 0)
        {
            bodyRigidBody.AddForce(bodyRigidBody.transform.right * correctionTorque * PerpComp.magnitude);
           // wheelRigitBody.AddForce(-bodyRigidBody.transform.right * correctionTorque * PerpComp.magnitude);
        }
        //If it is Negative we need to go left
        else if (Vector2.Dot(Vector2.Perpendicular(CorrectionUp), PerpComp) < 0)
        {
            bodyRigidBody.AddForce(-bodyRigidBody.transform.right * correctionTorque * PerpComp.magnitude);
           // wheelRigitBody.AddForce(bodyRigidBody.transform.right * correctionTorque * PerpComp.magnitude);
        }
        //Otherwise we apply no force


    }



}
