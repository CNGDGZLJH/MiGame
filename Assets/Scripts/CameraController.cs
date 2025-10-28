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


    public float xSpeed = 250.0f; // X�ᣨˮƽ����ת�ٶ�
    public float ySpeed = 120.0f; // Y�ᣨ��ֱ����ת�ٶ�
    public float yMinLimit = -20f; // Y����ת��С�Ƕ�
    public float yMaxLimit = 80f; // Y����ת���Ƕ�

    [Header("Distance Settings")]
    public float distance = 5.0f; // �������Ŀ��ĳ�ʼ����
    public float minDistance = 1.0f; // ��С���ž���
    public float maxDistance = 10.0f; // ������ž���
    public float zoomSpeed = 2.0f; // �����ٶ�

    [Header("Smooth Settings")]
    public float rotationSmoothTime = 0.3f; // ��תƽ��ʱ��

    private float mouseX = 0.0f; // ��ǰX����ת�Ƕ�
    private float mouseY = 0.0f; // ��ǰY����ת�Ƕ�
    private Vector3 rotationSmoothVelocity; // ��תƽ���ٶȲο�ֵ
    private Vector3 currentRotation; // ��ǰ��תֵ
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        // ��ʼ����ǰ������Ƕ�
        Vector3 angles = transform.eulerAngles;
        mouseX = angles.y;
        mouseY = angles.x;
        currentRotation = new Vector3(mouseY, mouseX);

    }

    void Update()
    {
        //HandleMouseInput(); // ���ڷ����Ѵ���
        //UpdateCameraPosition();
        transform.position = _playerBody.position + new Vector3(1.45f, 2.0f, -7.0f);
    }

    void LateUpdate()
    {
        // ���ƽǶ���ָ����Χ��
        if (mouseY < -360) mouseY += 360;
        if (mouseY > 360) mouseY -= 360;   
        mouseX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        mouseY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        mouseY = Mathf.Clamp(mouseY, yMinLimit, yMaxLimit);

        // ��������������
        float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);

            // ƽ����ת
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);
            Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);

            // ������λ�ò�Ӧ��
            Vector3 idealPos = _playerBody.position - transform.forward * _distanceFromPlayer;
            transform.rotation = rotation;
            transform.position = idealPos;
        }
    }



// === �����ķ������� ===
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

    
