using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public InputManager InputManager { get; private set; }

    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private Camera SceneCamera;

    private PlayerInputManager playerInputManager;


    private void Awake()
    {
        Instance = this;
        InputManager = new InputManager();
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += TurnCameraOff;
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void TurnCameraOff(PlayerInput obj)
    {
        SceneCamera.gameObject.SetActive(false);
    }

    private void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        //Spawna o player num transform diferente do primeiro
        Transform playerParent = player.transform.parent;
        playerParent.position = startingPoints[players.Count - 1].position;

        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        
        print("layer to add " + layerToAdd);

        playerParent.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        playerParent.GetComponentInChildren<InputHandler>().horizontal =  player.actions.FindAction("Look");
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }
}
