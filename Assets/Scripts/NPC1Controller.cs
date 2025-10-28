using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 5f;    // �й䷶Χ
    public float moveSpeed = 1f;       // �ƶ��ٶ�
    private Vector3 startPos;
    private Vector3 targetPos;
    private NPCController nearbyNPC;

    [Header("Interaction")]
    public GameObject speechBubble;    // �Ի�����Ԥ�Ƽ�
    public string[] dialogueLines;     // �Ի��ı�
    public float interactionCooldown = 2f; // ������ȴʱ��
    private bool canInteract = true;
    private GameObject currentBubble;

    void Start()
    {
        startPos = transform.position;
        SetNewWanderTarget();
    }

    void Update()
    {
        // ���й�AI
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            SetNewWanderTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        // ��⽻�����루E����
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
        {
            nearbyNPC.Interact();
        }
    }
    void SetNewWanderTarget()
        {
            targetPos = startPos + Random.insideUnitSphere * wanderRadius;
            targetPos.y = startPos.y; // ����Y�᲻��
        }

    // ��ҽ��뽻����Χ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowSpeechBubble("!");
        }
    }

    // ����뿪������Χ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(currentBubble);
        }
    }

    // ��ʾ�Ի�����
    void ShowSpeechBubble(string text)
    {
        if (speechBubble == null) return;

        Destroy(currentBubble); // ���������
        currentBubble = Instantiate(speechBubble, transform.position + Vector3.up * 2, Quaternion.identity);
        currentBubble.GetComponentInChildren<TextMesh>().text = text;
        currentBubble.transform.SetParent(transform);
        
        
        // ʵ��������

        // ��ȡTMP���
        TextMeshPro tmpComponent = currentBubble.GetComponentInChildren<TextMeshPro>();
        tmpComponent.text = text;

        // ǿ��TMP���������Լ�����ȷ�ߴ�
        tmpComponent.ForceMeshUpdate();

        // ��ȡ��Ⱦ���ı����ʵ�ʿ��
            Vector2 textSize = tmpComponent.GetRenderedValues(true);

        // ��̬����������С�����豳����SpriteRenderer��
        SpriteRenderer background = currentBubble.GetComponentInChildren<SpriteRenderer>();
        if (background != null)
            {
                // ���ñ�����С�����ı���һȦ������һЩ�߾ࣩ
                float padding = 0.1f;
                background.size = new Vector2(textSize.x + padding, textSize.y + padding);
            }

        // ���ߣ����������Quad��������Transform Scale
        // Transform backgroundTransform = ...;
        // backgroundTransform.localScale = new Vector3(textSize.x + padding, textSize.y + padding, 1);
        
    }

    // �ⲿ���õĽ�������
    public void Interact()
    {
        if (!canInteract) return;

        // ���ѡ��һ��Ի�
        string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];
        ShowSpeechBubble(randomLine);

        // �����¼���ʾ�������Ŷ�����
        GetComponent<Animator>()?.SetTrigger("Wave");

        // ��ȴʱ��
        canInteract = false;
        Invoke(nameof(ResetInteraction), interactionCooldown);
    }

    void ResetInteraction()
    {
        canInteract = true;
        Destroy(currentBubble); // ��ѡ�������������������
    }
}



