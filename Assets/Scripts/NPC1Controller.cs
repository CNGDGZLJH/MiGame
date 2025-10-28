using System.Reflection;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 5f;    // �й䷶Χ
    public float moveSpeed = 1f;       // �ƶ��ٶ�
    private Vector3 startPos;
    private Vector3 targetPos;

    [Header("Interaction")]
    public GameObject speechBubble;    // �Ի�����Ԥ�Ƽ�����ѡ��
    public string[] dialogueLines;     // �Ի��ı�
    public float interactionCooldown = 2f; // ������ȴʱ��
    private bool canInteract = true;
    private GameObject currentBubble;

    // ��ҽ��뷶Χʱָ��� NPC���� Update �а� E ʹ�ã�
    private NPCController nearbyNPC;

    // �����ҵ��� InteractHint3D ��������ܹ��� NPC ����봥������ Player �����Ӷ���
    private Component _cachedInteractHint;

    // �����������
    private Rigidbody _rb;
    private Collider _selfCollider;

    void Start()
    {
        startPos = transform.position;
        SetNewWanderTarget();

        _rb = GetComponent<Rigidbody>();
        _selfCollider = GetComponent<Collider>();

        // ȷ��ʹ�ô����������������Ƽ���
        if (_selfCollider != null)
            _selfCollider.isTrigger = true;

        // ���ø��壨ƽ���ƶ���
        if (_rb != null)
        {
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            // ����������Ӱ�����Ӱ�������������������ﲻǿ�� isKinematic
            // �����ϣ�� NPC ���������ƶ������� Inspector �� isKinematic �򿪲�����ʹ�� MovePosition
        }
    }

    void Update()
    {
        // ���й�AI
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            SetNewWanderTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // ������ڷ�Χ�ڲ��� E�����ȴ��� InteractHint3D �������δ�ҵ���ִ�� Interact��
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
        {
            bool invoked = nearbyNPC.InvokeInteractHint();
            if (!invoked)
            {
                nearbyNPC.Interact();
            }
        }
    }

    void SetNewWanderTarget()
    {
        targetPos = startPos + Random.insideUnitSphere * wanderRadius;
        targetPos.y = startPos.y; // ����Y�᲻��
    }

    // ��ҽ��뽻����Χ ���� ������ʾ "hello"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowSpeechBubble("Press E to interact:");
            nearbyNPC = this; // ���Ϊ�ɽ����� NPC���� Update �а� E ʹ�ã�

            // ������ NPC ���弰���봥��������Ҷ���/���Ӷ����ϲ��� InteractHint3D ���������
            _cachedInteractHint = FindInteractHintIn(transform) ?? FindInteractHintIn(other.transform);
        }
    }

    // ����뿪������Χ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ȫ�������ݲ�ȡ����������
            if (currentBubble != null)
            {
                Destroy(currentBubble);
                currentBubble = null;
            }

            if (nearbyNPC == this) nearbyNPC = null;

            // �뿪ʱ������棨��һ�ν�������²��ң�
            _cachedInteractHint = null;
        }
    }

    // �� NPC ��ָ�����²�����Ϊ InteractHint3D �����������������ʱ������
    private Component FindInteractHintIn(Transform root)
    {
        if (root == null) return null;
        MonoBehaviour[] mbs = root.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var mb in mbs)
        {
            if (mb == null) continue;
            if (mb.GetType().Name == "InteractHint3D")
                return mb;
        }
        return null;
    }

    // ���Ե��û���� InteractHint3D ����Ľ������������ҵ����������Ƿ�ɹ�����
    public bool InvokeInteractHint()
    {
        if (_cachedInteractHint == null)
        {
            // ������Ϊ�գ��ȳ������������
            _cachedInteractHint = FindInteractHintIn(transform);
            if (_cachedInteractHint == null)
                return false;
        }

        var comp = _cachedInteractHint;
        var type = comp.GetType();

        // �������������ԣ��޲�����
        string[] methodNames = { "Trigger", "Show", "Activate", "OnInteract", "Interact", "Use", "ShowHint", "Play" };
        foreach (var name in methodNames)
        {
            MethodInfo mi = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mi != null && mi.GetParameters().Length == 0)
            {
                mi.Invoke(comp, null);
                return true;
            }
        }

        // ���Ե��ÿ��ܴ����� string �����ķ��������� Show(string))
        foreach (var name in new[] { "Show", "ShowHint", "Trigger", "Interact" })
        {
            MethodInfo mi = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
            if (mi != null)
            {
                mi.Invoke(comp, new object[] { "E" });
                return true;
            }
        }

        // �����ˣ�ʹ�� SendMessage ��������������Ϲ㲥 OnInteract�������쳣��
        try
        {
            comp.gameObject.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            comp.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            return true; // ��Ϊ�ѳ��Ե���
        }
        catch
        {
            Debug.LogWarning($"{name}: �ҵ� InteractHint3D ������޷������䷽�������鷽��������");
            return false;
        }
    }

    // ��ʾ�Ի����ݣ�����ʹ�� speechBubble Ԥ�Ƽ������޻�Ԥ�Ƽ�ȱ�ı��������˵���̬���� TextMesh��
    void ShowSpeechBubble(string text)
    {
        // ��������ݣ�����У�
        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
        }

        // ���������Ԥ�Ƽ�������Ԥ�Ƽ������Լ���ڲ��Ƿ����ı������
        if (speechBubble != null)
        {
            currentBubble = Instantiate(speechBubble, transform.position + Vector3.up * 2f, Quaternion.identity);
            currentBubble.transform.SetParent(transform, true);

            // �������ݼ����Ӷ�������� Collider����ֹ���ݶ���Ҳ�������Ӱ�죨��������ײ��ȥ��
            var colliders = currentBubble.GetComponentsInChildren<Collider>(true);
            foreach (var c in colliders)
            {
                if (c != null) c.enabled = false;
            }

            // ���ȳ��� TextMeshPro
            TextMeshPro tmpComponent = currentBubble.GetComponentInChildren<TextMeshPro>();
            if (tmpComponent != null)
            {
                tmpComponent.text = text;
                tmpComponent.ForceMeshUpdate();
                return;
            }

            // ���˵��ɵ� TextMesh
            TextMesh legacyText = currentBubble.GetComponentInChildren<TextMesh>();
            if (legacyText != null)
            {
                legacyText.text = text;
                return;
            }

            // Ԥ�Ƽ����ڵ�û���ı���� ���� �����׳�����ʽ���󣬸�Ϊ��ͨ��־����ʹ�û����ı���ʾ
            Debug.Log($"{name}: speechBubble Ԥ�Ƽ���δ�ҵ� TextMeshPro �� TextMesh �����ʹ�û����ı���ʾ��");
            Destroy(currentBubble);
            currentBubble = null;
        }

        // ���ˣ���̬����һ���򵥵� 3D TextMesh��������ʾ��ȷ�����ᱨ NullReference��
        GameObject fallback = new GameObject("SpeechBubble_Fallback");
        fallback.transform.SetParent(transform, false);
        fallback.transform.localPosition = Vector3.up * 2f;
        // ��� TextMesh������������ TMP ��Ŀ�ļ򵥽��������
        TextMesh tm = fallback.AddComponent<TextMesh>();
        tm.text = text;
        tm.fontSize = 48;
        tm.characterSize = 0.02f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.white;

        // ��ѡ�����ı�������������������ݣ�
        fallback.AddComponent<FaceCamera>();

        currentBubble = fallback;
    }

    // �ⲿ���õĽ�������
    public void Interact()
    {
        if (!canInteract) return;

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            ShowSpeechBubble("...");
        }
        else
        {
            // ���ѡ��һ��Ի�
            string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];
            ShowSpeechBubble(randomLine);
        }

        // �����¼���ʾ�������Ŷ�����
        // ��ȫ���� Animator���ȳ��Ի�ȡ����ٵ��ã����� MissingComponentException
        Animator animator;
        if (TryGetComponent<Animator>(out animator) && animator != null)
        {
            animator.SetTrigger("Wave");
        }

        // ��ȴʱ��
        canInteract = false;
        Invoke(nameof(ResetInteraction), interactionCooldown);
    }

    void ResetInteraction()
    {
        canInteract = true;
        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
        }
    }
}

// �������ʹ���˵� 3D �ı�ʼ�������������UI-like��
public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}