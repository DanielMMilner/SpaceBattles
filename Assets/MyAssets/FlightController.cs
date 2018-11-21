﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public UIHandler uIHandler;

    public float forwardThrust = 10.0f;
    public float horizontalThrust = 10.0f;
    public float verticalThrust = 10.0f;

    public float yawSpeed = 0.03f;
    public float rollSpeed = 0.03f;
    public float pitchSpeed = 0.03f;

    private Rigidbody ShipRigidbody;
    private Transform ShipTransform;

    private float moveForward;
    private float moveHorizontal;
    private float moveVertical;

    private float yaw;
    private float pitch;
    private float roll;

    private bool rotationalStabiliersActive = false;
    private bool movementStabiliersActive = false;

    private ThrusterParticlesController thrusterParticlesController;

    void Start()
    {
        ShipRigidbody = gameObject.GetComponent<Rigidbody>();
        ShipTransform = gameObject.GetComponent<Transform>();

        thrusterParticlesController = gameObject.GetComponent<ThrusterParticlesController>();
    }

    void FixedUpdate()
    {
        moveForward = Input.GetAxis("Move Forward") - Input.GetAxis("Move Backward");
        moveHorizontal = Input.GetAxis("Move Right") - Input.GetAxis("Move Left");
        moveVertical = Input.GetAxis("Move Up") - Input.GetAxis("Move Down");
        pitch = Input.GetAxis("Pitch Down") - Input.GetAxis("Pitch Up");
        yaw = Input.GetAxis("Yaw Right") - Input.GetAxis("Yaw Left");
        roll = Input.GetAxis("Roll Left") - Input.GetAxis("Roll Right");

        ThrusterMovement();
        ThrusterRotation();
        thrusterParticlesController.ActivateThrusters(ShipRigidbody, movementStabiliersActive, rotationalStabiliersActive);

        if (rotationalStabiliersActive)
        {
            StabiliseRotation();
        }

        if (movementStabiliersActive)
        {
            StabiliseMovement();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("RotationalStabilisers"))
        {
            rotationalStabiliersActive = !rotationalStabiliersActive;
            uIHandler.ToggleRotationalStabiliers(rotationalStabiliersActive);
        }


        if (Input.GetButtonDown("MovementStabilisers"))
        {
            movementStabiliersActive = !movementStabiliersActive;
            uIHandler.ToggleMovementStabiliers(movementStabiliersActive);
        }

        uIHandler.UpdateUI(ShipRigidbody);
    }

    private void StabiliseRotation()
    {
        if (pitch == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x * 0.95f, ShipRigidbody.angularVelocity.y, ShipRigidbody.angularVelocity.z);
        }
        if (yaw == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x, ShipRigidbody.angularVelocity.y * 0.95f, ShipRigidbody.angularVelocity.z);
        }
        if (roll == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x, ShipRigidbody.angularVelocity.y, ShipRigidbody.angularVelocity.z * 0.95f);
        }
    }

    private void StabiliseMovement()
    {

        if (moveHorizontal == 0)
        {            
            if (ShipRigidbody.velocity.x < 0.02 && ShipRigidbody.velocity.x > -0.02)
            {
                ShipRigidbody.velocity = new Vector3(0, ShipRigidbody.velocity.y, ShipRigidbody.velocity.z);
            }            

            float xVel = transform.InverseTransformDirection(ShipRigidbody.velocity).x;

            if (xVel > 0.01)
            {
                ShipRigidbody.AddForce(-ShipTransform.right * horizontalThrust);                
            }
            else if(xVel < -0.01)
            {
                ShipRigidbody.AddForce(ShipTransform.right * horizontalThrust);
            }
        }
        if (moveVertical == 0)
        {            
            if (ShipRigidbody.velocity.y < 0.02 && ShipRigidbody.velocity.y > -0.02)
            {
                ShipRigidbody.velocity = new Vector3(ShipRigidbody.velocity.x, 0, ShipRigidbody.velocity.z);
            }
            
            float yVel = transform.InverseTransformDirection(ShipRigidbody.velocity).y;

            if (yVel > 0.01)
            {
                ShipRigidbody.AddForce(-ShipTransform.up * forwardThrust);
            }
            else if (yVel < -0.01)
            {
                ShipRigidbody.AddForce(ShipTransform.up * forwardThrust);
            }
        }
        if (moveForward == 0)
        {            
            if (ShipRigidbody.velocity.z < 0.02 && ShipRigidbody.velocity.z > -0.02)
            {
                ShipRigidbody.velocity = new Vector3(ShipRigidbody.velocity.x, ShipRigidbody.velocity.y, 0);
            }            

            float zVel = transform.InverseTransformDirection(ShipRigidbody.velocity).z;

            if (zVel > 0.01)
            {
                ShipRigidbody.AddForce(-ShipTransform.forward * verticalThrust);
            }
            else if (zVel < -0.01)
            {
                ShipRigidbody.AddForce(ShipTransform.forward * verticalThrust);
            }
        }
    }

    private void ThrusterMovement()
    {
        ShipRigidbody.AddForce(ShipTransform.forward * forwardThrust * moveForward);
        ShipRigidbody.AddForce(ShipTransform.right * horizontalThrust * moveHorizontal);
        ShipRigidbody.AddForce(ShipTransform.up * verticalThrust * moveVertical);
    }

    private void ThrusterRotation()
    {
        // Rotate about Y principal axis
        //Vector3 desiredAngularVelInY = Vector3.up * yawSpeed * yaw;
        //Vector3 torque = ShipRigidbody.inertiaTensorRotation * Vector3.Scale(ShipRigidbody.inertiaTensor, desiredAngularVelInY);
        //ShipRigidbody.AddRelativeTorque(torque, ForceMode.Impulse);

        ShipRigidbody.AddTorque(ShipTransform.right * pitchSpeed * pitch);
        ShipRigidbody.AddTorque(ShipTransform.up * yawSpeed * yaw);
        ShipRigidbody.AddTorque(ShipTransform.forward * rollSpeed * roll);
    }    
}