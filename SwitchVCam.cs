using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    //initialize secondary (ADS) camera
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
        aimCanvas.enabled = false;
    }

    //switch camera to ADS
    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    //switch back to normal camera
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
    }

}
