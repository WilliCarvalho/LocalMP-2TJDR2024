using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public InputManager InputManager { get; private set; }


    private void Awake()
    {
        Instance = this;
        InputManager = new InputManager();
    }
}
