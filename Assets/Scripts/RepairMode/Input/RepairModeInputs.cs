using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// Handle callbacks from InputActions InputActionsAsset > Player Input component for Repair Mode.
/// </summary>
public class RepairModeInputs : MonoBehaviour
{
    [HideInInspector] public Vector2 rotate;
    [HideInInspector] public bool selectTapped;
    [HideInInspector] public bool selectHeld;
    [HideInInspector] public bool changeVision;

    public void OnRotate(InputAction.CallbackContext ctx)
    {
        RotateInput(ctx.ReadValue<Vector2>());
    }

    public void OnSelect(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is TapInteraction) { SelectTappedInput(ctx.started); }
        else { SelectHeldInput(ctx.performed); }
    }

    // Operates just like a default button press w/ performed
    public void OnChangeVision(InputAction.CallbackContext ctx)
    {
        ChangeVisionInput(ctx.performed);
    }

    void RotateInput(Vector2 newRotationDirection)
    {
        rotate = newRotationDirection;
    }

    void SelectTappedInput(bool newSelectDirection)
    {
        selectTapped = newSelectDirection;
    }

    void SelectHeldInput(bool newSelectDirection)
    {
        selectHeld = newSelectDirection;
    }

    void ChangeVisionInput(bool newChangeVision)
    {
        changeVision = newChangeVision;
    }
}
