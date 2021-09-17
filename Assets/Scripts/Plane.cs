using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Plane : MonoBehaviour
{
    // Camera
    CameraMovement cameraMovement;

    // Plane Property
    public float thrust = 400f;
    public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    public float forceMult = 10f;

    // Plane movement
    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField] [Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField] [Range(-1f, 1f)] private float roll = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }


    // Control Settings
    public float mouseSensetivity = 1.0f;
    public float rollDeadSpace = 0.0f;
    public float pitchDeadSpace = .25f;
    public float yawDeadSpace = 0.0f;

    // Personal Settings
    public bool invertPitch = false;


    // Logig
    private bool rollActive = false;
    private bool pitchActive = false;
    private bool yawActive = false;
    // private bool thrustAtive = false;

    // Thrust movement
    private float thrustVelocity = 0.0f;
    private float thrustSmooth = 3.0f;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        cameraMovement = GameManager.gameManager.player.GetComponent<CameraMovement>();
    }

    private void Update()
    {
        rollActive = false;
        pitchActive = false;

        float keyboardRoll = Input.GetAxis("Horizontal");
        float mouseRoll = Input.GetAxis("Mouse X");

        if (Mathf.Abs(mouseRoll) > rollDeadSpace)
        {
            cameraMovement.horizontal = (mouseRoll > 0) ? 0.2f : -0.2f;
            rollActive = true;
        }

        // float keyboardPitch = Input.GetAxis("Vertical");
        float keyboardPitch = Input.GetAxis("Pitch");
        if (Mathf.Abs(keyboardPitch) > pitchDeadSpace)
        {
            pitchActive = true;
            cameraMovement.height = (keyboardPitch > 0) ? 0.5f : -0.5f;
        }
        else cameraMovement.height = 0f;

        float keyboardYaw = Input.GetAxis("Yaw");
        if (Mathf.Abs(keyboardYaw) > yawDeadSpace)
        {
            yawActive = true;
            cameraMovement.horizontal = (keyboardYaw > 0) ? 0.2f : -0.2f;
        }
        else cameraMovement.horizontal = 0f;

        // if (Mathf.Abs(keyboardThrust) > 0) { }

        yaw = (yawActive) ? keyboardYaw : 0f;

        pitch = (pitchActive) ? keyboardPitch : 0f;
        roll = (rollActive) ? mouseRoll : 0f ;
        pitch = (invertPitch) ? -pitch : pitch;

        float keyboardThrust = Input.GetAxis("Thrust");
        ThrustUpdate(keyboardThrust);
    }

    private void ThrustUpdate(float keyboardThrust)
    {
        if (thrust > 400f)
        {
            thrustSmooth = 6.0f;
            thrust = Mathf.SmoothDamp(thrust, 300, ref thrustVelocity, thrustSmooth);
            cameraMovement.zett = 0f;
        }
        if (keyboardThrust > 0)
        {
            thrustSmooth = 3.0f;
            thrust = Mathf.SmoothDamp(thrust, 600, ref thrustVelocity, thrustSmooth);
            cameraMovement.zett = -1f;
        }
        if (keyboardThrust < 0)
        {
            thrustSmooth = 3.0f;
            thrust = Mathf.SmoothDamp(thrust, 0, ref thrustVelocity, thrustSmooth);
            cameraMovement.zett = 1f;
        }
    }

    private void CameraUpdate(float height, float zett)
    {
        if (Mathf.Abs(height) > 0)
            cameraMovement.height = height;
        if (Mathf.Abs(zett) > 0)
            cameraMovement.zett = zett;
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, 
            -turnTorque.z * roll) * forceMult, ForceMode.Force);
    }
}
