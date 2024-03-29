﻿using MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class DudeAgent : Agent
{
    public bool doRandomThings;

    public float forceMultiplier = 25f;

    public Transform hip1Transform;
    public Transform hip2Transform;
    public Transform leg1Transform;
    public Transform leg2Transform;

    private HingeJoint2D _hip1Joint;
    private HingeJoint2D _hip2Joint;
    private HingeJoint2D _leg1Joint;
    private HingeJoint2D _leg2Joint;

    private Rigidbody2D _hip1Rigidbody2D;
    private Rigidbody2D _hip2Rigidbody2D;
    private Rigidbody2D _leg1Rigidbody2D;
    private Rigidbody2D _leg2Rigidbody2D;

    public Transform headTransform;
    public Transform target;

    private Vector3 _hip1StartPosition;
    private Vector3 _hip2StartPosition;
    private Vector3 _leg1StartPosition;
    private Vector3 _leg2StartPosition;
    private Vector3 _headInitialPosition;

    private Quaternion _hip1StartRotation;
    private Quaternion _hip2StartRotation;
    private Quaternion _leg1StartRotation;
    private Quaternion _leg2StartRotation;
    private Quaternion _headInitialRotation;

    public float minDistance = 2;

    private JointMotor2D _motor1;
    private JointMotor2D _motor2;
    private JointMotor2D _motor3;
    private JointMotor2D _motor4;

    public Transform floorTransform;

    public void Start()
    {
        _hip1Joint = hip1Transform.GetComponent<HingeJoint2D>();
        _hip2Joint = hip2Transform.GetComponent<HingeJoint2D>();
        _leg1Joint = leg1Transform.GetComponent<HingeJoint2D>();
        _leg2Joint = leg2Transform.GetComponent<HingeJoint2D>();

        _hip1Rigidbody2D = hip1Transform.GetComponent<Rigidbody2D>();
        _hip2Rigidbody2D = hip2Transform.GetComponent<Rigidbody2D>();
        _leg1Rigidbody2D = leg1Transform.GetComponent<Rigidbody2D>();
        _leg2Rigidbody2D = leg2Transform.GetComponent<Rigidbody2D>();

        _hip1StartPosition = hip1Transform.position;
        _hip2StartPosition = hip2Transform.position;
        _leg1StartPosition = leg1Transform.position;
        _leg2StartPosition = leg2Transform.position;
        _headInitialPosition = headTransform.position;

        _hip1StartRotation = hip1Transform.rotation;
        _hip2StartRotation = hip2Transform.rotation;
        _leg1StartRotation = leg1Transform.rotation;
        _leg2StartRotation = leg2Transform.rotation;
        _headInitialRotation = headTransform.rotation;

        _motor1 = _hip1Joint.motor;
        _motor2 = _hip2Joint.motor;
        _motor3 = _leg1Joint.motor;
        _motor4 = _leg2Joint.motor;
    }

    private void ManualReset()
    {
        // Reset RigidBodies

        _hip1Rigidbody2D.velocity = Vector2.zero;
        _hip1Rigidbody2D.angularVelocity = 0;
        _hip2Rigidbody2D.velocity = Vector2.zero;
        _hip2Rigidbody2D.angularVelocity = 0;

        _leg1Rigidbody2D.velocity = Vector2.zero;
        _leg1Rigidbody2D.angularVelocity = 0;
        _leg2Rigidbody2D.velocity = Vector2.zero;
        _leg2Rigidbody2D.angularVelocity = 0;

        // Reset Joints
        _motor1.motorSpeed = 0;
        _hip1Joint.motor = _motor1;
        _motor2.motorSpeed = 0;
        _hip2Joint.motor = _motor2;
        _motor3.motorSpeed = 0;
        _leg1Joint.motor = _motor3;
        _motor4.motorSpeed = 0;
        _leg2Joint.motor = _motor4;

        // Reset Positions
        hip1Transform.position = _hip1StartPosition;
        hip2Transform.position = _hip2StartPosition;
        leg1Transform.position = _leg1StartPosition;
        leg2Transform.position = _leg2StartPosition;

        headTransform.position = _headInitialPosition;

        // Reset Rotation
        hip1Transform.rotation = _hip1StartRotation;
        hip2Transform.rotation = _hip2StartRotation;
        leg1Transform.rotation = _leg1StartRotation;
        leg2Transform.rotation = _leg2StartRotation;

        headTransform.rotation = _headInitialRotation;


        var rx = Random.Range(6, 12);
        var ry = Random.Range(1.5f, 4f);
        if (Random.Range(0, 10) <= 5)
            rx *= -1;



        target.transform.position = new Vector3(_headInitialPosition.x + rx, _headInitialPosition.y + ry, -2);

    }

    public override void AgentReset()
    {
        ManualReset();
    }

    private void Update()
    {
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            ManualReset();
//        }
//
//        if (doRandomThings)
//        {
//            _motor1.motorSpeed = Random.Range(-100f, 100f);
//            _hip1Joint.motor = _motor1;
//            _motor2.motorSpeed = Random.Range(-100f, 100f);
//            _hip2Joint.motor = _motor2;
//
//            _motor3.motorSpeed = Random.Range(-100f, 100f);
//            _leg1Joint.motor = _motor3;
//            _motor4.motorSpeed = Random.Range(-100f, 100f);
//            _leg2Joint.motor = _motor4;
//        }
        
//        Debug.Log(CalculateReward());
    }


    public override void CollectObservations()
    {
        // 42

        // Target Position Vector3
        AddVectorObs(target.position);

        // Position Vector3
        AddVectorObs(hip1Transform.position);
        AddVectorObs(hip2Transform.position);
        AddVectorObs(leg1Transform.position);
        AddVectorObs(leg2Transform.position);
        AddVectorObs(headTransform.position);

        // Rotation Vector4
        AddVectorObs(hip1Transform.rotation);
        AddVectorObs(hip2Transform.rotation);
        AddVectorObs(leg1Transform.rotation);
        AddVectorObs(leg2Transform.rotation);
        AddVectorObs(headTransform.rotation);

        // Motor Speed (float)
        var motor1 = _hip1Joint.motor;
        var motor2 = _hip2Joint.motor;
        var motor3 = _leg1Joint.motor;
        var motor4 = _leg2Joint.motor;

        AddVectorObs(motor1.motorSpeed);
        AddVectorObs(motor2.motorSpeed);
        AddVectorObs(motor3.motorSpeed);
        AddVectorObs(motor4.motorSpeed);

        // What else?!
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (!doRandomThings)
        {
            // Actions, size = 4
            _motor1.motorSpeed = vectorAction[0] * forceMultiplier;
            _hip1Joint.motor = _motor1;
            _motor2.motorSpeed = vectorAction[1] * forceMultiplier;
            _hip2Joint.motor = _motor2;

            _motor3.motorSpeed = vectorAction[2] * forceMultiplier;
            _leg1Joint.motor = _motor3;
            _motor4.motorSpeed = vectorAction[3] * forceMultiplier;
            _leg2Joint.motor = _motor4;

        }
        else
        {
            _motor1.motorSpeed = Random.Range(-100f, 100f);
            _hip1Joint.motor = _motor1;
            _motor2.motorSpeed = Random.Range(-100f, 100f);
            _hip2Joint.motor = _motor2;

            _motor3.motorSpeed = Random.Range(-100f, 100f);
            _leg1Joint.motor = _motor3;
            _motor4.motorSpeed = Random.Range(-100f, 100f);
            _leg2Joint.motor = _motor4;
        }

        // Rewards
        var reward = CalculateReward();

        // Reached target
        if (reward > 1f)
        {
            SetReward(reward);
            Done();
        }
        else
        {
            SetReward(reward);
        }

        // Fell off platform
        if (headTransform.position.y < floorTransform.position.y - 1f)
        {
            Done();
        }
    }

    private float CalculateReward()
    {
        var tt = target.position;
        tt.z = 0;

        var at = headTransform.position;
        at.z = 0;

        var dst = Vector2.Distance(tt, at);

        if (dst < minDistance)
            return minDistance - dst;
        return -dst;

    }

    private void LateUpdate()
    {
        LogPositions();
    }

    private void LogPositions()
    {
        var lData = "";

        lData += _toString(headTransform) + ", " + _toString(hip1Transform)
                 + ", " + _toString(hip2Transform)
                 + ", " + _toString(leg1Transform)
                 + ", " + _toString(leg2Transform);

        DataLogger.instance.DoLogging(lData);
    }

    private static string _toString(Vector3 data)
    {
        return $"{data.x}, {data.y}";
    }

    private static string _toString(Quaternion data)
    {
        return $"{data.eulerAngles.x}, {data.eulerAngles.y}, {data.eulerAngles.z}";
    }

    private static string _toString(Transform data)
    {
        return _toString(data.position) + ", " + _toString(data.rotation);
    }
}