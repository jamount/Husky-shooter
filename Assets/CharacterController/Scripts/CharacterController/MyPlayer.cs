using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public Camera Camera;
    public MyCharacterController Character;

    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    float cameraRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        // Tell camera to follow transform
        //OrbitCamera.SetFollowTransform(CameraFollowPoint);

        //// Ignore the character's collider(s) for camera obstruction checks
        //OrbitCamera.IgnoredColliders.Clear();
        //OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        HandleCharacterInput();
    }

    private void LateUpdate()
    {
        HandleCameraInput();
        Character.PostInputUpdate(Time.deltaTime, Camera.transform.forward);
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            cameraRotation -= mouseLookAxisUp * 100f * Time.deltaTime;
            cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);
            Debug.Log(Quaternion.Euler(cameraRotation, 0f, 0f));
            Camera.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
        }

    }

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
        characterInputs.xMouseInput = Input.GetAxisRaw(MouseXInput);
        characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
        characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
        characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);
        characterInputs.ChargingDown = Input.GetKeyDown(KeyCode.Q);

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}