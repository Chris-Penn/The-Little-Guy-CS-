using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float sprintBoost = 4.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletMaxTime = 25f;
    [SerializeField]
    public Transform lookAtTarget;
    [SerializeField]
    private Transform swordPoint;


    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private bool aiming = false;
    private bool slashing = false;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction slashAction;
    private InputAction sprintAction;
    


    //initialize character settings
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];
        slashAction = playerInput.actions["Slash"];
        sprintAction = playerInput.actions["Sprint"];

        Cursor.lockState = CursorLockMode.Locked; //hides mouse while in play mode
    }


    //handle actions
    private void OnEnable()
    {
        slashAction.performed += _ => Slash();
        shootAction.performed += _ => ShootGun();
        sprintAction.performed += _ => StartSprint();
        sprintAction.canceled += _ => CancelSprint();
    }

    private void OnDisable()
    {
        slashAction.performed -= _ => Slash();
        shootAction.performed -= _ => ShootGun();
        sprintAction.performed -= _ => StartSprint();
        sprintAction.canceled -= _ => CancelSprint();
    }

    //use sword with LMB while not aiming
    private void Slash()
    {
        if (!aiming)
        {
            if (GameObject.Find("Sword") == null)
            {
                Vector3 dash = new Vector3(0f,0f,0f);
                dash = 1 * transform.right.normalized + 60 * transform.forward.normalized;
                controller.Move(dash * Time.deltaTime);
                Quaternion swordRotation = Quaternion.Euler(-100, transform.eulerAngles.y, 180);
                GameObject sword = GameObject.Instantiate(swordPrefab, transform.position, swordRotation, swordPoint);
            }

        }
        
    }

    //use gun with LMB if aiming
    private void ShootGun()
    {
        if (aiming)
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletMaxTime;
                bulletController.hit = false;
            }
        }
        
    }

    //hold shift to sprint
    private void StartSprint()
    {     
            playerSpeed += sprintBoost;
    }
    private void CancelSprint()
    {
        playerSpeed -= sprintBoost;
    }

        void Update()
        {
        groundedPlayer = controller.isGrounded;

        //set y velocity to 0 if standing on ground
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //move using "wasd" or arrow keys + camera values
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        aimAction.performed += _ => aiming = true;
        aimAction.canceled += _ => aiming = false;
     
        //disable automatic rotation (strafing) while not ADS
        if (aiming)
        {
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (move != Vector3.zero)
        {
            lookAtTarget.position = transform.position + 2 * move;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtTarget.transform.position - transform.position), rotationSpeed * Time.deltaTime);

        }

    }
}