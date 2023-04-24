using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindMenu : MonoBehaviour
{
    // A reference to the input action asset
    public InputActionAsset inputActions;

    // A reference to the current input action map
    private InputActionMap currentActionMap;

    // A reference to the current input action
    private InputAction currentAction;

    // A flag to indicate if we are rebinding a key
    private bool isRebinding;

    // A method that is called when the user clicks on a keybind button
    public void OnKeybindButtonClicked(InputAction action)
    {
        // Set the current action to the selected action
        currentAction = action;

        // Start rebinding the current action
        StartRebinding();
    }

    // A method that starts rebinding the current action
    private void StartRebinding()
    {
        // Set the flag to true
        isRebinding = true;

        // Disable all actions in the current action map
        currentActionMap.Disable();

        // Perform a rebind operation on the current action
        currentAction.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishRebinding(operation))
            .Start();
    }

    // A method that finishes rebinding the current action
    private void FinishRebinding(InputActionRebindingExtensions.RebindingOperation operation)
    {
        // Set the flag to false
        isRebinding = false;

        // Apply the new binding to the current action
        operation.ApplyBindingOverride(0, "<Keyboard>/# (g)");

        // Enable all actions in the current action map
        currentActionMap.Enable();
    }

    // A method that is called when the user presses Esc key
    [MenuItem("Keybinds/Cancel Rebinding _Escape")]
    private static void CancelRebinding()
    {
        // Check if we are rebinding a key
        if (isRebinding)
        {
            // Cancel the rebind operation on the current action
            currentAction.CancelInteractiveRebinding();

            // Enable all actions in the current action map
            currentActionMap.Enable();
        }
    }

    // A method that is called when the script is enabled
    private void OnEnable()
    {
        // Get the default input action map from the input action asset
        currentActionMap = inputActions.FindActionMap("Default");

        // Enable all actions in the current action map
        currentActionMap.Enable();
    }

}