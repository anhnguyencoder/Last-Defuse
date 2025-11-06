using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Cài Đặt Di Chuyển")]
    [Tooltip("CharacterController component của player")]
    public CharacterController charCon;
    [Tooltip("Tốc độ di chuyển bình thường")]
    public float moveSpeed;

    [Header("Input Actions")]
    [Tooltip("Input action cho di chuyển")]
    public InputActionReference moveAction;

    private Vector3 currentMovement;

    [Tooltip("Input action cho nhìn/xoay camera")]
    public InputActionReference lookAction;
    private Vector2 rotStore;
    [Tooltip("Tốc độ xoay camera/look sensitivity")]
    public float lookSpeed;

    [Header("Cài Đặt Camera")]
    [Tooltip("Camera của player")]
    public Camera theCam;

    [Tooltip("Góc nhìn tối thiểu (hướng xuống)")]
    public float minViewAngle;
    [Tooltip("Góc nhìn tối đa (hướng lên)")]
    public float maxViewAngle;

    [Header("Cài Đặt Nhảy")]
    [Tooltip("Input action cho nhảy")]
    public InputActionReference jumpAction;
    [Tooltip("Lực nhảy")]
    public float jumpPower;
    [Tooltip("Hệ số điều chỉnh trọng lực")]
    public float gravityModifier = 4f;

    [Header("Cài Đặt Chạy")]
    [Tooltip("Tốc độ khi chạy (sprint)")]
    public float runSpeed;
    [Tooltip("Input action cho chạy")]
    public InputActionReference sprintAction;

    [Header("Cài Đặt Zoom Camera")]
    [Tooltip("FOV bình thường của camera")]
    public float camZoomNormal;
    [Tooltip("FOV khi chạy (zoom out)")]
    public float camZoomOut;
    [Tooltip("Tốc độ chuyển đổi FOV")]
    public float camZoomSpeed;

    [Header("Cài Đặt Vũ Khí")]
    [Tooltip("WeaponsController component")]
    public WeaponsController weaponCon;
    [Tooltip("Input action cho bắn")]
    public InputActionReference shootAction;

    [Tooltip("Input action cho nạp đạn")]
    public InputActionReference reloadAction;

    [Header("Trạng Thái")]
    [Tooltip("Player đã chết chưa?")]
    public bool isDead;

    [Header("Chuyển Đổi Vũ Khí")]
    [Tooltip("Input action cho vũ khí tiếp theo")]
    public InputActionReference nextWeapon;
    [Tooltip("Input action cho vũ khí trước đó")]
    public InputActionReference prevWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead == true)
        {
            return;
        }

        if (Time.timeScale == 0f)
        {
            return;
        }

        float yStore = currentMovement.y;

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        // Debug.Log(moveInput);

        //currentMovement = new Vector3(moveInput.x * moveSpeed, 0f, moveInput.y * moveSpeed);

        Vector3 moveForward = transform.forward * moveInput.y;
        Vector3 moveSideways = transform.right * moveInput.x;

        //handle sprinting
        if (sprintAction.action.IsPressed())
        {
            currentMovement = (moveForward + moveSideways) * runSpeed;

            if (currentMovement != Vector3.zero)
            {
                theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, camZoomOut, camZoomSpeed * Time.deltaTime);
            }
        }
        else
        {
            currentMovement = (moveForward + moveSideways) * moveSpeed;

            theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, camZoomNormal, camZoomSpeed * Time.deltaTime);
        }

        //handlge gravity
        if(charCon.isGrounded == true)
        {
            yStore = 0f;
        }

        currentMovement.y = yStore + (Physics.gravity.y * Time.deltaTime * gravityModifier);

        //handle jumping
        if(jumpAction.action.WasPressedThisFrame() && charCon.isGrounded == true)
        {
            currentMovement.y = jumpPower;

            AudioManager.instance.PlaySFX(8);
        }

        charCon.Move(currentMovement * Time.deltaTime);


        //handle looking
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        lookInput.y = -lookInput.y;

        rotStore = rotStore + (lookInput * lookSpeed * Time.deltaTime);

        rotStore.y = Mathf.Clamp(rotStore.y, minViewAngle, maxViewAngle);

        transform.rotation = Quaternion.Euler(0f, rotStore.x, 0f);
        theCam.transform.localRotation = Quaternion.Euler(rotStore.y, 0f, 0f);


        //handle shooting
        if(shootAction.action.WasPressedThisFrame())
        {
            weaponCon.Shoot();
        }

        if(shootAction.action.IsPressed())
        {
            weaponCon.ShootHeld();
        }

        if(reloadAction.action.WasPressedThisFrame())
        {
            weaponCon.Reload();
        }

        if(nextWeapon.action.WasPressedThisFrame())
        {
            weaponCon.NextWeapon();
        }

        if(prevWeapon.action.WasPressedThisFrame())
        {
            weaponCon.PreviousWeapon();
        }
    }
}
