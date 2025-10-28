<<<<<<< HEAD
=======
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
            _playerVelocity.y = -0.5f; // ÇáÎ¢ÏÂÑ¹Á¦·ÀÖ¹Ðü¿Õ
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


//update by LJH 2025-10-24: Ìí¼ÓÐ­³ÌÖ´ÐÐËø£¬·ÀÖ¹¶à´Î´¥·¢¹²ÕñÐ§¹ûÖØµþ
/*private bool _isResonating = false; // ÐÂÔö£ºÐ­³ÌÖ´ÐÐËø

private void Resonate()
{
    if (!_isResonating) // Ö»ÓÐµ±Ð­³ÌÎ´ÔÚÖ´ÐÐÊ±£¬²Å´¥·¢ÐÂµÄ¹²Õñ
    {
        StartCoroutine(ResonanceVisualEffect());
    }
}

private System.Collections.IEnumerator ResonanceVisualEffect()
{
    _isResonating = true; // ±ê¼ÇÎª¡°ÕýÔÚ¹²Õñ¡±

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

    _isResonating = false; // Ð­³ÌÖ´ÐÐÍê±Ï£¬½âËø
}
}*/



>>>>>>> 546407b (LYT_20251028)
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Mouse Look Settings")]
<<<<<<< HEAD
    // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½È²ï¿½ï¿½ï¿½ï¿½ï¿½Ä¬ï¿½ï¿½ï¿½ï¿½Îª1ï¿½ï¿½ï¿½ï¿½Ê¾1:1Ó³ï¿½ä£¬ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Å£ï¿½
    [SerializeField] private float mouseSensitivity = 1f;
    // ï¿½ï¿½ï¿½ï¿½Ê¹ï¿½Ã´ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Y ï¿½ï¿½×ªï¿½ï¿½Îªï¿½ï¿½É«ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Îªï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ëµï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ß¼ï¿½
=======
    // ±£ÁôÁéÃô¶È²ÎÊýµ«Ä¬ÈÏÉèÎª1£¨±íÊ¾1:1Ó³Éä£¬²»¶îÍâËõ·Å£©
    [SerializeField] private float mouseSensitivity = 1f;
    // ÓÅÏÈÊ¹ÓÃ´ËÉãÏñ»úµÄ Y Ðý×ª×÷Îª½ÇÉ«³¯Ïò£¬ÈôÎª¿ÕÔò»ØÍËµ½»ùÓÚÊó±êÊäÈëµÄÂß¼­
>>>>>>> 546407b (LYT_20251028)
    [SerializeField] private Transform playerCamera;

    [Header("Paper Settings")]
    [SerializeField] private float paperSpeed = 15f;
    [SerializeField] private float paperDestroyDelay = 2f;
<<<<<<< HEAD
    [SerializeField] private float fixedYHeight = 0.5f;
    [SerializeField] private float angularDrag = 5f;
    [SerializeField] private float spawnOffset = 0.2f;

=======
    [SerializeField] private float fixedYHeight = 0.5f; // ¹Ì¶¨·¢Éä¸ß¶È£¨Ïà¶ÔµØÃæ£©
    [SerializeField] private float angularDrag = 5f; // ¼«¸ßÐý×ª×èÁ¦
    [SerializeField] private float spawnOffset = 0.2f;

    [Header("Interaction")]
    public GameObject interactHint; // ÍÏÈëÄãµÄUI»ò3DÌáÊ¾ÎïÌå

>>>>>>> 546407b (LYT_20251028)
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private bool _isResonating = false;
<<<<<<< HEAD
    private Vector3 _horizontalForward;

    private float yRotation; // ï¿½ï¿½É«Yï¿½ï¿½ï¿½ï¿½×ªï¿½Ç¶È£ï¿½Ö±ï¿½ï¿½ï¿½ï¿½Ó¦ï¿½ï¿½ï¿½ï¿½ï¿½/ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ë£©

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("ï¿½ï¿½É«È±ï¿½ï¿½CharacterControllerï¿½ï¿½ï¿½ï¿½ï¿½");
        }

        // ï¿½ï¿½Î´ï¿½ï¿½ Inspector Ö¸ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ê¹ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
=======
    private Vector3 _horizontalForward; // ´¿Ë®Æ½·¢Éä·½Ïò
    private NPCController nearbyNPC;

    private float yRotation; // ½ÇÉ«YÖáÐý×ª½Ç¶È£¨Ö±½ÓÏìÓ¦ÉãÏñ»ú/Êó±êÊäÈë£©

    public Vector3 startPosition = new Vector3(0, 0, 0);
    private Vector3 _initialPosition;//³õÊ¼»¯½ÇÉ«Î»ÖÃ
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = transform.position;
        if (_controller == null)
        {
            Debug.LogError("½ÇÉ«È±ÉÙCharacterController×é¼þ£¡");
        }

        // ÈôÎ´ÔÚ Inspector Ö¸¶¨ÉãÏñ»ú£¬³¢ÊÔÊ¹ÓÃÖ÷ÉãÏñ»ú
>>>>>>> 546407b (LYT_20251028)
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

<<<<<<< HEAD
        // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ê£¬È·ï¿½ï¿½ï¿½ï¿½ï¿½ë²»ï¿½Ü´ï¿½ï¿½ï¿½Ó°ï¿½ï¿½
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ï¿½ï¿½Ê¼ï¿½ï¿½ï¿½ï¿½×ªÎªï¿½ï¿½É«ï¿½ï¿½Ê¼ï¿½ï¿½ï¿½ï¿½
=======
        // Ëø¶¨Êó±ê£¬È·±£ÊäÈë²»ÊÜ´°¿ÚÓ°Ïì
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ³õÊ¼»¯Ðý×ªÎª½ÇÉ«³õÊ¼³¯Ïò
>>>>>>> 546407b (LYT_20251028)
        yRotation = transform.eulerAngles.y;
    }


    void Update()
    {
<<<<<<< HEAD
        // 1. ï¿½ï¿½ï¿½/ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½×ªï¿½ï¿½ï¿½ï¿½Ö¤ï¿½ï¿½É«Yï¿½ï¿½ï¿½ï¿½ï¿½Ó½ï¿½Ò»ï¿½Â£ï¿½
        UpdateRotationWithMouseOrCamera();

        // 2. ï¿½ï¿½ï¿½ï¿½Ë®Æ½Ç°ï¿½ï¿½
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // 3. ï¿½ï¿½É«ï¿½Æ¶ï¿½
        HandleMovement();
=======
        // ¼ÆËã´¿Ë®Æ½·½Ïò£¨Ïû³ýYÖá·ÖÁ¿£©
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // ½ÇÉ«ÒÆ¶¯Âß¼­
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -0.5f;
        }
>>>>>>> 546407b (LYT_20251028)

        // 4. ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ô¾
        HandleGravityAndJump();

        // 5. Ö½Æ¬ï¿½ï¿½ï¿½ï¿½
        if (Input.GetKeyDown(KeyCode.R))
        {
            Resonate();
        }

<<<<<<< HEAD
        // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        if (Input.GetKeyDown(KeyCode.Escape))
=======
        // ¼ì²â½»»¥ÊäÈë£¨E¼ü£©
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
>>>>>>> 546407b (LYT_20251028)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

<<<<<<< HEAD
    // ï¿½ï¿½ï¿½È¶ï¿½È¡ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Y ï¿½Ç¶È£ï¿½È·ï¿½ï¿½ï¿½ï¿½É«ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ó½ï¿½ï¿½ï¿½È«Ò»ï¿½Â¡ï¿½
    // ï¿½ï¿½Î´ï¿½á¹©ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ëµï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½×ªï¿½ï¿½ï¿½ï¿½ï¿½Ö¼ï¿½ï¿½ï¿½ï¿½Ô£ï¿½ï¿½ï¿½
    private void UpdateRotationWithMouseOrCamera()
    {
        if (playerCamera != null)
        {
            // Ö±ï¿½ï¿½Ê¹ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Y ï¿½Ç¶È£ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Öµï¿½ï¿½ï¿½ï¿½ï¿½Å£ï¿½ï¿½ï¿½Ê¹ï¿½ï¿½É«ï¿½ï¿½ï¿½Ó½ï¿½ï¿½ï¿½È«Í¬ï¿½ï¿½
            yRotation = playerCamera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            return;
        }

        // ï¿½ï¿½ï¿½Ë£ï¿½Ê¹ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½×ªï¿½ï¿½Ô­ï¿½ï¿½ï¿½ß¼ï¿½ï¿½ï¿½
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

    // Ö½Æ¬ï¿½ï¿½ï¿½ï¿½ï¿½ß¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ö²ï¿½ï¿½ä£©
    private void Resonate()
    {
        if (!_isResonating)
=======
        // 1. Êó±ê/ÉãÏñ»úÐý×ª£¨±£Ö¤½ÇÉ«YÖáÓëÊÓ½ÇÒ»ÖÂ£©
        UpdateRotationWithMouseOrCamera();

        // 2. ¼ÆËãË®Æ½Ç°·½
        _horizontalForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // 3. ½ÇÉ«ÒÆ¶¯
        HandleMovement();

        // 4. ÖØÁ¦ÓëÌøÔ¾
        HandleGravityAndJump();

        // 5. Ö½Æ¬·¢Éä
        if (Input.GetKeyDown(KeyCode.R))
>>>>>>> 546407b (LYT_20251028)
        {
            Resonate();
        }

<<<<<<< HEAD
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
=======
        // ½âËøÊó±ê
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
>>>>>>> 546407b (LYT_20251028)
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

    // ÓÅÏÈ¶ÁÈ¡ÉãÏñ»úµÄ Y ½Ç¶È£¬È·±£½ÇÉ«·½ÏòÓëÊÓ½ÇÍêÈ«Ò»ÖÂ¡£
    // ÈôÎ´Ìá¹©ÉãÏñ»úÒýÓÃÔò»ØÍËµ½»ùÓÚÊó±êÊäÈëµÄÐý×ª£¨±£³Ö¼æÈÝÐÔ£©¡£
    private void UpdateRotationWithMouseOrCamera()
    {
        if (playerCamera != null)
        {
            // Ö±½ÓÊ¹ÓÃÉãÏñ»úµÄÊÀ½ç Y ½Ç¶È£¨±ÜÃâ²åÖµ»òËõ·Å£©£¬Ê¹½ÇÉ«ÓëÊÓ½ÇÍêÈ«Í¬²½
            yRotation = playerCamera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            return;
        }

        // »ØÍË£ºÊ¹ÓÃÊó±êÊäÈë½øÐÐÐý×ª£¨Ô­ÓÐÂß¼­£©
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

    // Ö½Æ¬·¢ÉäÂß¼­£¨±£³Ö²»±ä£©
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
<<<<<<< HEAD
=======


>>>>>>> 546407b (LYT_20251028)
