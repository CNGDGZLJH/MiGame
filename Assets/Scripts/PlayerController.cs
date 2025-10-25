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
            _playerVelocity.y = -0.5f; // ��΢��ѹ����ֹ����
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


//update by LJH 2025-10-24: ���Э��ִ��������ֹ��δ�������Ч���ص�
/*private bool _isResonating = false; // ������Э��ִ����

private void Resonate()
{
    if (!_isResonating) // ֻ�е�Э��δ��ִ��ʱ���Ŵ����µĹ���
    {
        StartCoroutine(ResonanceVisualEffect());
    }
}

private System.Collections.IEnumerator ResonanceVisualEffect()
{
    _isResonating = true; // ���Ϊ�����ڹ���

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

    _isResonating = false; // Э��ִ����ϣ�����
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
    [SerializeField] private float fixedYHeight = 0.5f; // �̶�����߶ȣ���Ե��棩
    [SerializeField] private float angularDrag = 5f; // ������ת����
    [SerializeField] private float spawnOffset = 0.2f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isResonating = false;
    private Vector3 _horizontalForward; // ��ˮƽ���䷽��

    public Vector3 startPosition = new Vector3(0, 0, 0);
    private Vector3 _initialPosition;//��ʼ����ɫλ��
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = transform.position;
    }

    void Update()
    {
        // ���㴿ˮƽ��������Y�������
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // ��ɫ�ƶ��߼�
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

        // ����Ч��
        Material mat = GetComponent<Renderer>().material;
        Color originalEmission = mat.GetColor("_EmissionColor");
        Color brightEmission = originalEmission * 5;
        mat.SetColor("_EmissionColor", brightEmission);

        // ����ֽƬ
        GameObject paper = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // 1. �̶�����߶ȣ����ڵ��棬���ǽ�ɫλ�ã�
        // �����ɫ���µ����Y���꣨�����ɫ pivot �ڽŵף������������
        float groundY = transform.position.y - _controller.height / 2;
        Vector3 spawnPos = new Vector3(
            transform.position.x + _horizontalForward.x * 0.3f + Random.Range(-spawnOffset, spawnOffset),
            groundY + fixedYHeight, // �ϸ�̶��ڵ����Ϸ�fixedYHeight��
            transform.position.z + _horizontalForward.z * 0.3f + Random.Range(-spawnOffset, spawnOffset)
        );
        paper.transform.position = spawnPos;

        // 2. ǿ��ˮƽ��̬��˫��������
        paper.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        // ������ת�ᣬ��ֹ�κη������ת
        paper.transform.GetComponent<Transform>().rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);

        // 3. �����������ȫ������̬�ʹ�ֱλ��
        MeshCollider meshCollider = paper.GetComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rb = paper.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = angularDrag;
        // ����Y��λ�ú�������ת��������X��Z��ƽ��
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ;
        // ��ˮƽ���������ٶ�
        rb.velocity = _horizontalForward * paperSpeed;

        Destroy(paper, paperDestroyDelay);

        yield return new WaitForSeconds(0.3f);
        mat.SetColor("_EmissionColor", originalEmission);
        _isResonating = false;
    }
}