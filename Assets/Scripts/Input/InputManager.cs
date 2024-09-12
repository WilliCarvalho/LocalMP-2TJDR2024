using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    private InputControls inputControls;
    
    public Vector2 Movement => inputControls.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => inputControls.Player.Look.ReadValue<Vector2>();
    
    public event Action OnJump;
    public event Action OnAttack;

    public InputManager()
    {
        inputControls = new InputControls();
        inputControls.Enable();

        inputControls.Player.Jump.performed += OnJumpPerformed;
        inputControls.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        OnAttack?.Invoke();
    }
}
