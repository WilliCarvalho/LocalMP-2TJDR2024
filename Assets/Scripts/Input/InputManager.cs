using UnityEngine;

public class InputManager
{
    private InputControls inputControls;
    
    public Vector2 Movement => inputControls.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => inputControls.Player.Look.ReadValue<Vector2>();

    public InputManager()
    {
        inputControls = new InputControls();
        inputControls.Enable();
    }
}
