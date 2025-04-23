using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player
    public Transform player;
    // Offset from player
    public Vector3 offset = new Vector3(0, 2f, -4f);

    public float rotationSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 0f;
    public float pitchMin = -40f;
    public float pitchMax = 80f;

    void Start()
    {
        //Lock the cursor for mouse look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        //Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        //Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        //Apply position and rotation
        transform.position = player.position + rotation * offset;
        transform.rotation = rotation;
    }
}
