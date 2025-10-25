/*using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;  
    [SerializeField] private float _jumpHeight = 2f;  
    [SerializeField] private float _gravity = -9.81f;

    public float moveSpeed => _moveSpeed;  
    public float jumpHeight => _jumpHeight;
    public float gravity => _gravity;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private Vector3 _originalScale;

    public float jumpScaleFactor = 1.2f; 
    public float scaleReturnSpeed = 5f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _originalScale = transform.localScale;
    }

    void Update()
    {
        
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -0.5f; // 轻微下压力防止悬空
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
    }
 
    //private void Resonate()
    //{
        
    //    StartCoroutine(ResonanceVisualEffect());

        
    //    //AudioSource.PlayClipAtPoint(yourResonanceSound, transform.position);

    //}*/


/*private System.Collections.IEnumerator ResonanceVisualEffect()
{
    Material mat = GetComponent<Renderer>().material;
    Color originalEmission = mat.GetColor("_EmissionColor");
    Color brightEmission = originalEmission * 5; 

    mat.SetColor("_EmissionColor", brightEmission);


    GameObject pulse = GameObject.CreatePrimitive(PrimitiveType.Quad);
    pulse.transform.position = transform.position;
    pulse.transform.rotation = Quaternion.Euler(90, 0, 0); 
    //pulse.GetComponent<Renderer>().material = yourPulseMaterial; 
    Destroy(pulse, 1f); 


    yield return new WaitForSeconds(0.3f);
    mat.SetColor("_EmissionColor", originalEmission);
}*/


//update by LJH 2025-10-24: 添加协程执行锁，防止多次触发共振效果重叠
/*private bool _isResonating = false; // 新增：协程执行锁

private void Resonate()
{
    if (!_isResonating) // 只有当协程未在执行时，才触发新的共振
    {
        StartCoroutine(ResonanceVisualEffect());
    }
}

private System.Collections.IEnumerator ResonanceVisualEffect()
{
    _isResonating = true; // 标记为“正在共振”

    Material mat = GetComponent<Renderer>().material;
    Color originalEmission = mat.GetColor("_EmissionColor");
    Color brightEmission = originalEmission * 5;

    mat.SetColor("_EmissionColor", brightEmission);

    GameObject pulse = GameObject.CreatePrimitive(PrimitiveType.Quad);
    pulse.transform.position = transform.position;
    pulse.transform.rotation = Quaternion.Euler(90, 0, 0);
    Destroy(pulse, 1f);

    yield return new WaitForSeconds(0.3f);
    mat.SetColor("_EmissionColor", originalEmission);

    _isResonating = false; // 协程执行完毕，解锁
}
}*/



using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Paper Settings")]
    [SerializeField] private float paperSpeed = 15f;
    [SerializeField] private float paperDestroyDelay = 2f;
    [SerializeField] private float fixedYHeight = 0.5f; // 固定发射高度（相对地面）
    [SerializeField] private float angularDrag = 5f; // 极高旋转阻力
    [SerializeField] private float spawnOffset = 0.2f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isResonating = false;
    private Vector3 _horizontalForward; // 纯水平发射方向

    public Vector3 startPosition = new Vector3(0, 0, 0);
    private Vector3 _initialPosition;//初始化角色位置
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = transform.position;
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
    }

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

        // 发光效果
        Material mat = GetComponent<Renderer>().material;
        Color originalEmission = mat.GetColor("_EmissionColor");
        Color brightEmission = originalEmission * 5;
        mat.SetColor("_EmissionColor", brightEmission);

        // 生成纸片
        GameObject paper = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // 1. 固定发射高度（基于地面，而非角色位置）
        // 计算角色脚下地面的Y坐标（假设角色 pivot 在脚底，否则需调整）
        float groundY = transform.position.y - _controller.height / 2;
        Vector3 spawnPos = new Vector3(
            transform.position.x + _horizontalForward.x * 0.3f + Random.Range(-spawnOffset, spawnOffset),
            groundY + fixedYHeight, // 严格固定在地面上方fixedYHeight处
            transform.position.z + _horizontalForward.z * 0.3f + Random.Range(-spawnOffset, spawnOffset)
        );
        paper.transform.position = spawnPos;

        // 2. 强制水平姿态（双重锁定）
        paper.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        // 锁定旋转轴，禁止任何方向的旋转
        paper.transform.GetComponent<Transform>().rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);

        // 3. 物理组件：完全冻结姿态和垂直位置
        MeshCollider meshCollider = paper.GetComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rb = paper.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = angularDrag;
        // 冻结Y轴位置和所有旋转，仅允许X、Z轴平移
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ;
        // 沿水平方向设置速度
        rb.velocity = _horizontalForward * paperSpeed;

        Destroy(paper, paperDestroyDelay);

        yield return new WaitForSeconds(0.3f);
        mat.SetColor("_EmissionColor", originalEmission);
        _isResonating = false;
    }
}