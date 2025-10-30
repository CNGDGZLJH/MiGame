using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Mouse Look Settings")]
    // 保留灵敏度参数但默认设为1（表示1:1映射，不额外缩放）
    [SerializeField] private float mouseSensitivity = 1f;
    // 优先使用此摄像机的 Y 旋转作为角色朝向，若为空则回退到基于鼠标输入的逻辑
    [SerializeField] private Transform playerCamera;

    [Header("Paper Settings")]
    [SerializeField] private float paperSpeed = 15f;
    [SerializeField] private float paperDestroyDelay = 2f;
    [SerializeField] private float fixedYHeight = 0.5f; // 固定发射高度（相对地面）
    [SerializeField] private float angularDrag = 5f; // 极高旋转阻力
    [SerializeField] private float spawnOffset = 0.2f;

    [Header("Interaction")]
    public GameObject interactHint; // 拖入你的UI或3D提示物体

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isResonating = false;
    private Vector3 _horizontalForward; // 纯水平发射方向
    private NPCController nearbyNPC;

    private float yRotation; // 角色Y轴旋转角度（直接响应摄像机/鼠标输入）

    public Vector3 startPosition = new Vector3(0, 0, 0);
    private Vector3 _initialPosition;//初始化角色位置
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = transform.position;
        if (_controller == null)
        {
            Debug.LogError("角色缺少CharacterController组件！");
        }

        // 若未在 Inspector 指定摄像机，尝试使用主摄像机
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // 锁定鼠标，确保输入不受窗口影响
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 初始化旋转为角色初始朝向
        yRotation = transform.eulerAngles.y;
    }


    void Update()
    {
        // 计算纯水平方向（消除Y轴分量）
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // 角色移动逻辑
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -0.5f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Resonate();
        }

        // 检测交互输入（E键）
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
        {
            nearbyNPC.Interact();
        }

        // 1. 鼠标/摄像机旋转（保证角色Y轴与视角一致）
        UpdateRotationWithMouseOrCamera();

        // 2. 计算水平前方
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // 3. 角色移动
        HandleMovement();

        // 4. 重力与跳跃
        HandleGravityAndJump();

        // 5. 纸片发射
        if (Input.GetKeyDown(KeyCode.R))
        {
            Resonate();
        }

        // 解锁鼠标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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

    // 优先读取摄像机的 Y 角度，确保角色方向与视角完全一致。
    // 若未提供摄像机引用则回退到基于鼠标输入的旋转（保持兼容性）。
    private void UpdateRotationWithMouseOrCamera()
    {
        if (playerCamera != null)
        {
            // 直接使用摄像机的世界 Y 角度（避免插值或缩放），使角色与视角完全同步
            yRotation = playerCamera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            return;
        }

        // 回退：使用鼠标输入进行旋转（原有逻辑）
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

    // 纸片发射逻辑（保持不变）
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
}


