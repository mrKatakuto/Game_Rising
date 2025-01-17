using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking {get; private set; }

    public bool IsBlocking {get; private set; }

    public bool IsSitting;

    public Vector2 MovementValue {  get; private set; }

    // Events for the actions everyone who listens will react
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    private Controls controls;

    private InGameMenuManager menuManager;

    private void Awake()
    {
        controls = new Controls();
    }

    void Start()
    {
        // reference to the actual input methods (action maps)
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        // reference to InGameMenu
        menuManager = FindObjectOfType<InGameMenuManager>();
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }

    

    public void OnJump(InputAction.CallbackContext context)
    {
        // trigger event
        if (!context.performed) { return; }
        {
            JumpEvent?.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        {
            DodgeEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        {
            TargetEvent?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (menuManager.IsPaused) return; // Ignoriere Angriff, wenn das Menü aktiv ist

        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnMenu(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            
        if (menuManager.IsPaused)
        {
                menuManager.ResumeGameESC();
            }
            else
            {
                menuManager.PauseGame();
            }
        }
    }

    public void OnStandUp(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            IsSitting  = false;
        }
    }
}