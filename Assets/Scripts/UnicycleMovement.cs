using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicycleMovment : MonoBehaviour
{

    public GameObject Unicycle_Body;
    public GameObject Unicycle_Wheel;
    public SpringJoint2D Crunch_Joint;
    public float maxMotorSpeed = 500;
    public float motorInterpolate;
    public float leanTorque;
    public float correctionTorque;
    private float jumpForce;
    public float minJumpForce;
    public float maxJumpForce;
    public float jumpIncrement;
    private bool holdingJump = false;
    public float bodyMoveScaler;
    public float crunchNumber;

    private float curMotorSpeed = 0;
    private HingeJoint2D hinge;
    private JointMotor2D motorTemp;
    private Rigidbody2D bodyRigidBody;
    private Rigidbody2D wheelRigitBody;
    private Collider2D wheelCollider;
    private Camera cam;

    //private float distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        hinge = Unicycle_Wheel.GetComponent<HingeJoint2D>();
        bodyRigidBody = Unicycle_Body.GetComponent<Rigidbody2D>();
        wheelRigitBody = Unicycle_Wheel.GetComponent<Rigidbody2D>();
        wheelCollider = Unicycle_Wheel.GetComponent<CircleCollider2D>();
        Application.targetFrameRate = 120;
        cam = Camera.main;
    }


    private void Update()
    {
        Jump();
        WheelMovement();
        RotateBody();
        Jump();
        SelfCorrect();
    }


    void WheelMovement()
    {
        //move left, make motor speed negative
        if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.D))
        {

            curMotorSpeed = 0;
            hinge.useMotor = false;

        }
        else if (Input.GetKey(KeyCode.A))
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

        if (Input.GetKey(KeyCode.S))
        {
            hinge.useMotor = true;
            motorTemp = hinge.motor;
            motorTemp.motorSpeed = 0;
            hinge.motor = motorTemp;
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
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            Crunch_Joint.frequency = crunchNumber;
            if (holdingJump == false)
            {
                jumpForce = minJumpForce;
                holdingJump = true;
            }
            else
            {
                jumpForce = Mathf.Min(jumpForce + jumpIncrement, maxJumpForce);
            }          
        }
        else
        {
            Crunch_Joint.frequency = 1;
            holdingJump = false;
        }

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) && IsGrounded())
        {
            Vector2 jumpVector = bodyRigidBody.transform.up;
            bodyRigidBody.AddForce(jumpVector * jumpForce);
            Debug.Log(jumpForce);
        }
    }
  
    RaycastHit2D IsGrounded()
    {
        float radius = wheelCollider.bounds.extents.y;
        //return Physics2D.Raycast((Vector2) wheelCollider.bounds.center + Vector2.down*(radius + .01f), Vector2.down, .01f);
        return Physics2D.BoxCast((Vector2)wheelCollider.bounds.center, wheelCollider.bounds.size/2, 0f, -bodyRigidBody.transform.up, wheelCollider.bounds.extents.y + .01f);
    }

    Vector2 GetCorrectionUp()
    {
        Vector2 correction = (Vector2) cam.ScreenToWorldPoint(Input.mousePosition) - wheelRigitBody.position;
        return correction.normalized;
    }

    void SelfCorrect()
    {
        if (Input.GetKey(KeyCode.LeftArrow) | (Input.GetKey(KeyCode.RightArrow)))
        {
            return;
        }
        Vector2 CorrectionUp = GetCorrectionUp();

        //Debug.DrawRay(wheelRigitBody.position, CorrectionUp * 3, Color.green);

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
