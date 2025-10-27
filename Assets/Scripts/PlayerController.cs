using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Mouse Look Settings")]
    // ���������Ȳ�����Ĭ����Ϊ1����ʾ1:1ӳ�䣬���������ţ�
    [SerializeField] private float mouseSensitivity = 1f;
    // ����ʹ�ô�������� Y ��ת��Ϊ��ɫ������Ϊ������˵��������������߼�
    [SerializeField] private Transform playerCamera;

    [Header("Paper Settings")]
    [SerializeField] private float paperSpeed = 15f;
    [SerializeField] private float paperDestroyDelay = 2f;
    [SerializeField] private float fixedYHeight = 0.5f;
    [SerializeField] private float angularDrag = 5f;
    [SerializeField] private float spawnOffset = 0.2f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isResonating = false;
    private Vector3 _horizontalForward;

    private float yRotation; // ��ɫY����ת�Ƕȣ�ֱ����Ӧ�����/������룩

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("��ɫȱ��CharacterController�����");
        }

        // ��δ�� Inspector ָ�������������ʹ���������
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // ������꣬ȷ�����벻�ܴ���Ӱ��
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ��ʼ����תΪ��ɫ��ʼ����
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        // 1. ���/�������ת����֤��ɫY�����ӽ�һ�£�
        UpdateRotationWithMouseOrCamera();

        // 2. ����ˮƽǰ��
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // 3. ��ɫ�ƶ�
        HandleMovement();

        // 4. ��������Ծ
        HandleGravityAndJump();

        // 5. ֽƬ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            Resonate();
        }

        // �������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ���ȶ�ȡ������� Y �Ƕȣ�ȷ����ɫ�������ӽ���ȫһ�¡�
    // ��δ�ṩ�������������˵���������������ת�����ּ����ԣ���
    private void UpdateRotationWithMouseOrCamera()
    {
        if (playerCamera != null)
        {
            // ֱ��ʹ������������� Y �Ƕȣ������ֵ�����ţ���ʹ��ɫ���ӽ���ȫͬ��
            yRotation = playerCamera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            return;
        }

        // ���ˣ�ʹ��������������ת��ԭ���߼���
        float mouseX = Input.GetAxis("Mouse X");
        yRotation += mouseX * mouseSensitivity * 100f * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDir = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            _controller.Move(moveDir * _moveSpeed * Time.deltaTime);
        }
    }

    private void HandleGravityAndJump()
    {
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    // ֽƬ�����߼������ֲ��䣩
    private void Resonate()
    {
        if (!_isResonating)
        {
            StartCoroutine(ResonanceVisualEffect());
        }
    }

    private IEnumerator ResonanceVisualEffect()
    {
        _isResonating = true;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
        {
            Material mat = renderer.material;
            Color originalEmission = mat.GetColor("_EmissionColor");
            Color brightEmission = originalEmission * 5;
            mat.SetColor("_EmissionColor", brightEmission);

            yield return new WaitForSeconds(0.3f);
            mat.SetColor("_EmissionColor", originalEmission);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        GameObject paper = GameObject.CreatePrimitive(PrimitiveType.Quad);
        float groundY = transform.position.y - _controller.height / 2f;
        Vector3 spawnPos = new Vector3(
            transform.position.x + _horizontalForward.x * 0.3f + Random.Range(-spawnOffset, spawnOffset),
            groundY + fixedYHeight,
            transform.position.z + _horizontalForward.z * 0.3f + Random.Range(-spawnOffset, spawnOffset)
        );
        paper.transform.position = spawnPos;
        paper.transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);

        MeshCollider meshCollider = paper.GetComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rb = paper.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = angularDrag;
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ;
        rb.velocity = _horizontalForward * paperSpeed;

        Destroy(paper, paperDestroyDelay);

        _isResonating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            interactHint.SetActive(true);
        }
        if (other.CompareTag("NPC"))
        {
            nearbyNPC = other.GetComponent<NPCController>();
        }
        if (other.CompareTag("NPC"))
        {
            if (interactHint != null) interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            interactHint.SetActive(false);
        }
        if (other.CompareTag("NPC") && nearbyNPC == other.GetComponent<NPCController>())
        {
            nearbyNPC = null;
        }
        if (other.CompareTag("NPC"))
        {
            if (interactHint != null) interactHint.SetActive(false);
        }
    }

}
