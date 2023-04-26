using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    // Action Map string location
    public string actionMap;

    // A reference to the Text component that displays the current key binding
    public TextMeshProUGUI keybindText;

    private void Awake()
    {
        // Get the MonsterSpawn action map
        currentActionMap = inputActions.FindActionMap(actionMap);

    }


    // A method that is called when the user clicks on a keybind button
    public void OnKeybindTextClicked(string actionName)
    {
        // Update the keybind text
        keybindText.text = "Press a new key...";

        currentAction = currentActionMap.FindAction(actionName);

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
        currentAction.ApplyBindingOverride(operation.selectedControl.path);

        // Update the keybind text
        keybindText.text = currentAction.GetBindingDisplayString();

        // Enable all actions in the current action map
        currentActionMap.Enable();
    }
}