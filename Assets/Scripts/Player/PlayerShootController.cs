using Cinemachine;
using StarterAssets;
using System;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    [SerializeField] private float animTransitionSpeed = 10f;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float rotationVelocity = 10f;

    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private ParticleSystem hitParticle;

    private StarterAssetsInputs assetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;

    private Vector3 mouseWorldPosition = Vector3.zero;
    private Vector3 worldAimTarget = Vector3.zero;

    private Camera myCamera;
    private Transform hittedObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myCamera = transform.parent.GetComponentInChildren<Camera>();
        assetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        aimCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckAiminput();
        HandleAimDirection();
        CheckShootInput();
    }    

    private void CheckAiminput()
    {
        if (assetsInputs.aim)
        {
            aimCamera.gameObject.SetActive(true);
            thirdPersonController.SetLockRotation(true);
            thirdPersonController.SetSensitivity(aimSensitivity);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * animTransitionSpeed));

            Quaternion currentRotation = transform.rotation;
            worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            Quaternion directionToAim = Quaternion.LookRotation(aimDirection);

            transform.rotation = Quaternion.Slerp(currentRotation, directionToAim, rotationVelocity * Time.deltaTime);

        }
        else
        {
            thirdPersonController.SetLockRotation(false);
            aimCamera.gameObject.SetActive(false);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 20f));

            thirdPersonController.SetSensitivity(normalSensitivity);
        }
    }

    private void HandleAimDirection()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray shootRay = myCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(shootRay, out RaycastHit rayHit, 999f, aimColliderMask))
        {
            mouseWorldPosition = rayHit.point;
            hittedObject = rayHit.transform;
        }
    }

    private void CheckShootInput()
    {
        if (assetsInputs.shoot)
        {
            Instantiate(hitParticle, mouseWorldPosition, hittedObject.rotation);
            assetsInputs.shoot = false;
        }
    }
}
