using _Scripts.InputHandler.InputReader;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private PlayerInputReader inputReader;

    [SerializeField] private Transform fpsPivot;
    [SerializeField] private Transform tpsPivot;
    [SerializeField] private CinemachineCamera fpsCam;
    [SerializeField] private CinemachineCamera tpsCam;

    [Header("Settings")] [SerializeField] private float fpsSensitivity = 0.5f;
    [SerializeField] private float tpsSensitivity = 1f;
    [SerializeField] private float pitchLimit = 60f;

    private float _yaw;
    private float _pitch;
    private bool _isFPS;
    private Vector2 _lookInput;

    private void OnEnable()
    {
        if (inputReader == null) return;
        inputReader.LookEvent += OnLook;
        inputReader.SwitchCameraEvent += ToggleView;
        // inputReader.OnMouseEvent += OnMouseEvent;
    }

    private void OnDisable()
    {
        if (inputReader == null) return;
        inputReader.LookEvent -= OnLook;
        inputReader.SwitchCameraEvent -= ToggleView;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isFPS = true;
    }

    

    private void LateUpdate()
    {
        Vector2 curInput = _lookInput;
        
        float sensitivity = _isFPS ? fpsSensitivity : tpsSensitivity;

        _yaw += curInput.x * sensitivity ;
        _pitch -= curInput.y * sensitivity;
        
        _pitch = Mathf.Clamp(_pitch, -pitchLimit, pitchLimit);
        
        transform.parent.rotation = Quaternion.Euler(_pitch, _yaw, 0);
        
        if(_isFPS && fpsPivot != null)
            fpsPivot.localRotation = Quaternion.Euler(_pitch, 0, 0);
        else if (tpsPivot != null)
            tpsPivot.localRotation = Quaternion.Euler(_pitch, 0, 0);
            

    }

    private void OnLook(Vector2 input)
    {
        _lookInput = input;
    }

    private void ToggleView()
    {
        _isFPS = !_isFPS;
        fpsCam.Priority = _isFPS ? 20 : 10;
        tpsCam.Priority = _isFPS ? 10 : 20;
    }

    private void OnMouseEvent()
    {
        Cursor.visible = true;
    }
}