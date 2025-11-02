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

    public CharacterController charCon;
    public float moveSpeed;

    public InputActionReference moveAction;

    private Vector3 currentMovement;

    public InputActionReference lookAction;
    private Vector2 rotStore;
    public float lookSpeed;

    public Camera theCam;

    public float minViewAngle, maxViewAngle;

    public InputActionReference jumpAction;
    public float jumpPower;
    public float gravityModifier = 4f;

    public float runSpeed;
    public InputActionReference sprintAction;

    public float camZoomNormal, camZoomOut, camZoomSpeed;

    public WeaponsController weaponCon;
    public InputActionReference shootAction;

    public InputActionReference reloadAction;

    public bool isDead;

    public InputActionReference nextWeapon, prevWeapon;

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
