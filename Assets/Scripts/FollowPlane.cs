using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlane : MonoBehaviour
{   
    
    private Rigidbody rigid;

    public Transform targetAircraft = null;

    public float thrust = 100f;
    public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    public float forceMult = 10f;

    public float sensitivity = 5f;
    public float aggressiveTurnAngle = 10f;

    private float pitch = 0f;
    private float yaw = 0f;
    private float roll = 0f;


    // Missle Logic
    public float angleLimit = 180f;
    public bool lockedOnTarget = true;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        if (targetAircraft != null) lockedOnTarget = true;
        if (lockedOnTarget)
            if (targetAircraft.name == "Player")
                GameManager.gameManager.playerTargeting += 1.0f;
    }

    private void Update()
    {
        // Calculate the autopilot stick inputs.
        // float autoYaw = 0f;
        // float autoPitch = 0f;
        // float autoRoll = 0f;

        yaw = 0f;
        pitch = 0f;
        roll = 0f;

        if(lockedOnTarget)
        {
            Vector3 targetDirection = targetAircraft.position;
            RunAutopilot(targetDirection, out yaw, out pitch, out roll);
        }    

        // yaw = autoYaw;
        // pitch = autoPitch;
        // roll = autoRoll;
    }

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        var localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

        if (angleOffTarget > angleLimit)
        {
            lockedOnTarget = false;
            yaw = pitch = roll = 0;
            GameManager.gameManager.playerTargeting -= 1.0f;
            return;
        }

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

    private void TimeDestroyed()
    {

    }
}

