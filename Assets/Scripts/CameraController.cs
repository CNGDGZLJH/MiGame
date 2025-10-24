using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _distanceFromPlayer = 5f;
    [SerializeField] private LayerMask _obstructionMask;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseInput(); // 现在方法已存在
        UpdateCameraPosition();
        transform.position = _playerBody.position + new Vector3(1.45f, 2.0f, -7.0f);
    }

    void LateUpdate()
    {
        
    }

    // === 新增的方法定义 ===
    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _yRotation -= mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }

    private void UpdateCameraPosition()
    {
        Vector3 idealPos = _playerBody.position - transform.forward * _distanceFromPlayer;

        if (Physics.Linecast(_playerBody.position, idealPos, out RaycastHit hit, _obstructionMask))
        {
            transform.position = hit.point + transform.forward * 0.5f;
        }
        else
        {
            transform.position = idealPos;
        }
    }
}