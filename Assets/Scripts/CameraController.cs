using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _distanceFromPlayer = 5f;
    [SerializeField] private LayerMask _obstructionMask;

    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, -5f);
    public float smoothSpeed = 5f;

    [Header("Rotation Settings")]
    public float rotationDamping = 3.0f;


    public float xSpeed = 250.0f; // X轴（水平）旋转速度
    public float ySpeed = 120.0f; // Y轴（垂直）旋转速度
    public float yMinLimit = -20f; // Y轴旋转最小角度
    public float yMaxLimit = 80f; // Y轴旋转最大角度

    [Header("Distance Settings")]
    public float distance = 5.0f; // 摄像机与目标的初始距离
    public float minDistance = 1.0f; // 最小缩放距离
    public float maxDistance = 10.0f; // 最大缩放距离
    public float zoomSpeed = 2.0f; // 缩放速度

    [Header("Smooth Settings")]
    public float rotationSmoothTime = 0.3f; // 旋转平滑时间

    private float mouseX = 0.0f; // 当前X轴旋转角度
    private float mouseY = 0.0f; // 当前Y轴旋转角度
    private Vector3 rotationSmoothVelocity; // 旋转平滑速度参考值
    private Vector3 currentRotation; // 当前旋转值
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        // 初始化当前摄像机角度
        Vector3 angles = transform.eulerAngles;
        mouseX = angles.y;
        mouseY = angles.x;
        currentRotation = new Vector3(mouseY, mouseX);

    }

    void Update()
    {
        //HandleMouseInput(); // 现在方法已存在
        //UpdateCameraPosition();
        transform.position = _playerBody.position + new Vector3(1.45f, 2.0f, -7.0f);
    }

    void LateUpdate()
    {
        // 限制角度在指定范围内
        if (mouseY < -360) mouseY += 360;
        if (mouseY > 360) mouseY -= 360;   
        mouseX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        mouseY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        mouseY = Mathf.Clamp(mouseY, yMinLimit, yMaxLimit);

        // 处理鼠标滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);

            // 平滑旋转
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);
            Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);

            // 计算新位置并应用
            Vector3 idealPos = _playerBody.position - transform.forward * _distanceFromPlayer;
            transform.rotation = rotation;
            transform.position = idealPos;
        }
    }



// === 新增的方法定义 ===
/*private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseX;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _yRotation -= mouseY;
        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }

    private void UpdateCameraPosition()
    {


        if (Physics.Linecast(_playerBody.position, idealPos, out RaycastHit hit, _obstructionMask))
        {
            transform.position = hit.point + transform.forward * 0.5f;
        }
        else
        {
            transform.position = idealPos;
        }
    }
}*/

    
