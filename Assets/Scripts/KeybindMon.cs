using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeybindMon : MonoBehaviour
{
    public Button Normal;
    public Button Speedy;
    public Button Tanky;
    public Button Path;
    public Button ManaCollector;

    public InputActionAsset inputActions;

    private InputActionMap monsterSpawnActionMap;
    private InputAction normalAction;
    private InputAction speedyAction;
    private InputAction tankyAction;
    private InputAction pathAction;
    private InputAction manaCollectorAction;

    private void Awake()
    {
        // Get the MonsterSpawn action map
        monsterSpawnActionMap = inputActions.FindActionMap("MonsterSpawn");

        // Get the actions
        normalAction = monsterSpawnActionMap.FindAction("Normal");
        speedyAction = monsterSpawnActionMap.FindAction("Speedy");
        tankyAction = monsterSpawnActionMap.FindAction("Tanky");
        pathAction = monsterSpawnActionMap.FindAction("Path");
        manaCollectorAction = monsterSpawnActionMap.FindAction("ManaCollector");

        // Register the callback methods
        normalAction.performed += OnNormal;
        speedyAction.performed += OnSpeedy;
        tankyAction.performed += OnTanky;
        pathAction.performed += OnPath;
        manaCollectorAction.performed += OnManaCollector;
    }

    private void OnEnable()
    {
        // Enable the action map
        monsterSpawnActionMap.Enable();
    }

    private void OnDisable()
    {
        // Disable the action map
        monsterSpawnActionMap.Disable();
    }

    private void OnNormal(InputAction.CallbackContext context)
    {
        Debug.Log("Normal action was performed");
        ExecuteEvents.Execute(Normal.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private void OnSpeedy(InputAction.CallbackContext context)
    {
        Debug.Log("Speedy action was performed");
        ExecuteEvents.Execute(Speedy.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private void OnTanky(InputAction.CallbackContext context)
    {
        Debug.Log("Tanky action was performed");
        ExecuteEvents.Execute(Tanky.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private void OnPath(InputAction.CallbackContext context)
    {
        Debug.Log("Path action was performed");
        ExecuteEvents.Execute(Path.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    private void OnManaCollector(InputAction.CallbackContext context)
    {
        Debug.Log("Mana Collector was performed");
        ExecuteEvents.Execute(ManaCollector.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }
}