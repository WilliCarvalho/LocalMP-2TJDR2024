using Cinemachine;
using StarterAssets;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    [SerializeField] private float rotationVelocity = 20f;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private ProjectileBehavior projectilePrefab;
    [SerializeField] private Transform projectileSpawnerPosition;
    
    private Vector3 mouseWorldPosition = Vector3.zero;
    Vector3 worldAimTarget = Vector3.zero;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Camera myCamera;
    private Animator animator;

    private void Awake()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        myCamera = transform.parent.gameObject.GetComponentInChildren<Camera>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAimDirection();

        CheckAimInput();
        CheckShootInput();
    }

    private void HandleAimDirection()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        //Ray ray = myCamera.ScreenPointToRay(screenCenterPoint);
        Ray ray = myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, aimColliderMask))
        {
            mouseWorldPosition = rayHit.point;
        }
    }

    private void CheckAimInput()
    {
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetBRotateOnMove(false);
            
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            
            Quaternion currentRotation = transform.rotation;
            worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            Quaternion directionToAim = Quaternion.LookRotation(aimDirection);

            transform.rotation = Quaternion.Slerp(currentRotation, directionToAim, rotationVelocity * Time.deltaTime);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetBRotateOnMove(true);
            thirdPersonController.SetSensitivity(normalSensitivity);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }

    private void CheckShootInput()
    {
        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDirection = (mouseWorldPosition - projectileSpawnerPosition.position).normalized;
            Instantiate(projectilePrefab, projectileSpawnerPosition.position,
                Quaternion.LookRotation(aimDirection, Vector3.up));
            starterAssetsInputs.shoot = false;
        }
    }
}
