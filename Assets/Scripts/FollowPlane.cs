using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlane : MonoBehaviour
{   
    
    private Rigidbody rigid;

    private Transform aircraft;
    public Transform targetAircraft;

    public float thrust = 100f;
    public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    public float forceMult = 10f;

    public float sensitivity = 5f;
    public float aggressiveTurnAngle = 10f;

    private float pitch = 0f;
    private float yaw = 0f;
    private float roll = 0f;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Calculate the autopilot stick inputs.
        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;

        // Vector3 targetDirection = (targetAircraft.position - transform.position).normalized;
        Vector3 targetDirection = targetAircraft.position;
        RunAutopilot(targetDirection, out autoYaw, out autoPitch, out autoRoll);

        yaw = autoYaw;
        pitch = autoPitch;
        roll = autoRoll;
    }

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        
        var localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);

        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        var wingsLevelRoll = transform.right.y;

        var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, 
            -turnTorque.z * roll) * forceMult, ForceMode.Force);
    }
}

