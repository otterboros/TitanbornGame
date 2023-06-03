using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Convert input data into object manipulation: rotation and selection.
/// </summary>
public class RepairableObjController : MonoBehaviour
{
    [Header("RepairableObject")]
    [Tooltip("Rotation speed of the character")]
    [SerializeField] float RotationSpeed;

    private const float _rotationThreshold = 0.005f;

    private PlayerInput _playerInput;
    private RepairModeInputs _input;

    float _objTargetPitch;
    float _objTargetYaw;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    private void Start()
    {
        _input = GetComponent<RepairModeInputs>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        ObjectSelection();
        ObjectRotation();
    }

    // modified from Unity Docs on CameraRays
    // & InfallibleCode video on selecting objects w/ raycasts
    /// <summary>
    /// Handles selection of objects by mouse position
    /// </summary>
    void ObjectSelection()
    {
        if(_input.selectTapped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                if (objectHit.TryGetComponent(out ActivatorBase activatorBase))
                {
                    if (!activatorBase.isActive) { activatorBase.OnActivated(); }
                    else { activatorBase.OnDeactivated(); }
                }
            }
        }
    }

    void ObjectRotation()
    {
        // if there is an input && obj is selected
        if (_input.rotate.sqrMagnitude >= _rotationThreshold && _input.selectHeld)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _objTargetPitch = _input.rotate.y * RotationSpeed * deltaTimeMultiplier;
            _objTargetYaw = -_input.rotate.x * RotationSpeed * deltaTimeMultiplier;

            // Update Cinemachine camera target pitch, then yaw
            transform.Rotate(_objTargetPitch, _objTargetYaw, 0.0f, Space.World);
        }
    }
}
