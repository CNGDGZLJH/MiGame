using System.Globalization;
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

    //}


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
    private bool _isResonating = false; // 新增：协程执行锁

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
}