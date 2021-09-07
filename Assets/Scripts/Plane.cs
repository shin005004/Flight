using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Plane : MonoBehaviour
{
    public float thrust = 100f;
    public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    public float forceMult = 10f;

    [SerializeField] [Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField] [Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField] [Range(-1f, 1f)] private float roll = 0f;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }

    public float mouseSensetivity = 1.0f;
    public float rollDeadSpace = 0.0f;
    public float pitchDeadSpace = .25f;
    public float yawDeadSpace = 0.0f;

    public bool invertPitch = false;

    private bool rollActive = false;
    private bool pitchActive = false;
    private bool yawActive = false;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rollActive = false;
        pitchActive = false;

        float keyboardRoll = Input.GetAxis("Horizontal");
        float mouseRoll = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;

        if (Mathf.Abs(mouseRoll) > rollDeadSpace)
        {
            rollActive = true;
        }

        // float keyboardPitch = Input.GetAxis("Vertical");
        float keyboardPitch = Input.GetAxis("Pitch");
        if (Mathf.Abs(keyboardPitch) > pitchDeadSpace)
        {
            pitchActive = true;
        }

        float keyboardYaw = Input.GetAxis("Yaw");
        if (Mathf.Abs(keyboardYaw) > yawDeadSpace)
        {
            yawActive = true;
        }

        yaw = (yawActive) ? keyboardYaw : 0f;

        pitch = (pitchActive) ? mouseRoll : 0f;
        roll = (rollActive) ? keyboardRoll : 0f ;
        pitch = (invertPitch) ? -pitch : pitch;
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch, turnTorque.y * yaw, 
            -turnTorque.z * roll) * forceMult, ForceMode.Force);
    }
}
