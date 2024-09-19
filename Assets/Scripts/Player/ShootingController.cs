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

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Camera myCamera;

    private void Awake()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        myCamera = transform.parent.gameObject.GetComponentInChildren<Camera>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = myCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, aimColliderMask))
        {
            mouseWorldPosition = rayHit.point;
        }
        
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetBRotateOnMove(false);
            
            Quaternion currentRotation = transform.rotation;
            Vector3 worldAimTarget = mouseWorldPosition;
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
        }

        
    }
}
