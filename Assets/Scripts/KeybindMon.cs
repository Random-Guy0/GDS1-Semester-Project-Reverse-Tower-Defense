using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class KeybindMon : MonoBehaviour
{
    public Button Normal;
    public Button Speedy;
    public Button Tanky;
    public Button Path;
    public Button ManaCollector;
    public Button Shield;

    public InputActionAsset inputActions;

    private InputActionMap monsterSpawnActionMap;
    private InputAction normalAction;
    private InputAction speedyAction;
    private InputAction tankyAction;
    private InputAction pathAction;
    private InputAction manaCollectorAction;
    private InputAction shieldAction;
    private IEnumerator PathActionCoroutine;

    private bool isSpaceKeyDown;

    private void Awake()
    {
        // Get the MonsterSpawn action map
        monsterSpawnActionMap = inputActions.FindActionMap("MonsterSpawn");

        // Get the actions
        normalAction = monsterSpawnActionMap.FindAction("Normal");
        speedyAction = monsterSpawnActionMap.FindAction("Speedy");
        tankyAction = monsterSpawnActionMap.FindAction("Tanky");
        pathAction = monsterSpawnActionMap.FindAction("Path");
        shieldAction = monsterSpawnActionMap.FindAction("Shield");

        manaCollectorAction = monsterSpawnActionMap.FindAction("ManaCollector");

        // Register the callback methods
        normalAction.performed += OnNormal;
        speedyAction.performed += OnSpeedy;
        tankyAction.performed += OnTanky;
        pathAction.performed += OnPathStarted;
        pathAction.canceled += OnPathCanceled;
        manaCollectorAction.performed += OnManaCollector;
        shieldAction.performed += OnShield;
    }

    private void OnDestroy()
    {
        if (PathActionCoroutine != null)
        {
            StopCoroutine(PathActionCoroutine);
        }
        
        normalAction.performed -= OnNormal;
        speedyAction.performed -= OnSpeedy;
        tankyAction.performed -= OnTanky;
        pathAction.performed -= OnPathStarted;
        pathAction.canceled -= OnPathCanceled;
        manaCollectorAction.performed -= OnManaCollector;
        shieldAction.performed -= OnShield;
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

    private void OnPathStarted(InputAction.CallbackContext context)
    {
        if (PathActionCoroutine == null && !isSpaceKeyDown)
        {
            isSpaceKeyDown = true;
            FindObjectOfType<PathBeacon>().KeyDown();
            PathActionCoroutine = PerformPathAction();
            StartCoroutine(PathActionCoroutine);
        }
    }

    private void OnPathCanceled(InputAction.CallbackContext context)
    {
        if (PathActionCoroutine != null)
        {
            PathManager pathManager = FindObjectOfType<PathManager>();

            if (pathManager != null)
            {
                pathManager.KeyUp();
            }

            isSpaceKeyDown = false;
            StopCoroutine(PathActionCoroutine);
            PathActionCoroutine = null;
        }
    }

    private IEnumerator PerformPathAction()
    {
        while (isSpaceKeyDown)
        {
            Debug.Log("Path action was performed");
            ExecuteEvents.Execute(Path.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            yield return new WaitForSeconds(0.01f); // Customize the delay between repeated executions
        }
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

    private void OnManaCollector(InputAction.CallbackContext context)
    {
        Debug.Log("Mana Collector was performed");
        ExecuteEvents.Execute(ManaCollector.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }
    private void OnShield(InputAction.CallbackContext context)
    {
        Debug.Log("Shield action was performed");
        ExecuteEvents.Execute(Shield.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }
}