using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Handle callbacks from InputActions InputActionsAsset > Player Input component for Explore Mode.
/// </summary>
public class ExploreModeInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool select;
    public bool changeVision;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput(ctx.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (cursorInputForLook)
        {
            LookInput(ctx.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        JumpInput(ctx.performed);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        SprintInput(ctx.performed);
    }

    public void OnSelect(InputAction.CallbackContext ctx)
    {
        SelectInput(ctx.performed);
    }

    public void OnChangeVision(InputAction.CallbackContext ctx)
    {
        ChangeVisionInput(ctx.performed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void SelectInput(bool newSelectDirection)
    {
        select = newSelectDirection;
    }

    void ChangeVisionInput(bool newChangeVision)
    {
        changeVision = newChangeVision;
    }
}
