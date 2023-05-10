using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    public GameObject slow;
    public InputActionAsset inputActions;
    private InputAction slowAction;
    private InputAction healAction;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private PlayerHealth PlayerHealth;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        slowAction = inputActions.FindActionMap("Player").FindAction("SlowSkill");
        healAction = inputActions.FindActionMap("Player").FindAction("HealSkill");
    }

    private void OnEnable()
    {
        slowAction.performed += OnSlowActionPerformed;
        slowAction.canceled += OnSlowActionCanceled;
        healAction.performed += OnHealActionPerformed;
        healAction.canceled += OnHealActionCanceled;
        slowAction.Enable();
        healAction.Enable();
    }

    private void OnDisable()
    {
        slowAction.performed -= OnSlowActionPerformed;
        slowAction.canceled -= OnSlowActionCanceled;
        healAction.performed -= OnHealActionPerformed;
        healAction.canceled -= OnHealActionCanceled;
        slowAction.Disable();
        healAction.Disable();
    }

    private void OnSlowActionPerformed(InputAction.CallbackContext context)
    {
        StopAbility();
    }

    private void OnSlowActionCanceled(InputAction.CallbackContext context)
    {
        animator.ResetTrigger("Ability");
    }

    private void OnHealActionPerformed(InputAction.CallbackContext context)
    {
        HealAbility();
    }

    private void OnHealActionCanceled(InputAction.CallbackContext context)
    {
        animator.ResetTrigger("Ability");
    }
    public void HealAbility()
    {
        if (manaManager.ableToCost(100))
        {
            manaManager.costMana(100);
            PlayerHealth.health = PlayerHealth.maxHealth;
            animator.SetTrigger("Ability");
        }
    }

    public void StopAbility()
    {
        if (manaManager.ableToCost(50))
        {
            manaManager.costMana(50);
            Instantiate(slow, gameObject.transform.position, gameObject.transform.rotation);
            animator.SetTrigger("Ability");
        }
    }
}
