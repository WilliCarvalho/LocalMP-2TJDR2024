using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public InputManager InputManager { get; private set; }

    private PlayerInputManager playerInputManager;

    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> playerSpawn;
    [SerializeField] private List<LayerMask> playerLayers;


    private void Awake()
    {
        Instance = this;
        InputManager = new InputManager();
        playerInputManager = GetComponent<PlayerInputManager>();        
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        
        Transform playerParent = player.transform.parent;
        print("Spawn position: " + playerSpawn[players.Count - 1].position);
        player.transform.position = playerSpawn[players.Count - 1].position;

        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        print("layer to add " + layerToAdd);

        playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        //playerParent.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("Look");
    }
}
