using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 5f;    // 闲逛范围
    public float moveSpeed = 1f;       // 移动速度
    private Vector3 startPos;
    private Vector3 targetPos;
    private NPCController nearbyNPC;

    [Header("Interaction")]
    public GameObject speechBubble;    // 对话气泡预制件
    public string[] dialogueLines;     // 对话文本
    public float interactionCooldown = 2f; // 交互冷却时间
    private bool canInteract = true;
    private GameObject currentBubble;

    void Start()
    {
        startPos = transform.position;
        SetNewWanderTarget();
    }

    void Update()
    {
        // 简单闲逛AI
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            SetNewWanderTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        // 检测交互输入（E键）
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
        {
            nearbyNPC.Interact();
        }
    }
    void SetNewWanderTarget()
        {
            targetPos = startPos + Random.insideUnitSphere * wanderRadius;
            targetPos.y = startPos.y; // 保持Y轴不变
        }

    // 玩家进入交互范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowSpeechBubble("!");
        }
    }

    // 玩家离开交互范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(currentBubble);
        }
    }

    // 显示对话气泡
    void ShowSpeechBubble(string text)
    {
        if (speechBubble == null) return;

        Destroy(currentBubble); // 清除旧气泡
        currentBubble = Instantiate(speechBubble, transform.position + Vector3.up * 2, Quaternion.identity);
        currentBubble.GetComponentInChildren<TextMesh>().text = text;
        currentBubble.transform.SetParent(transform);
        
        
        // 实例化气泡

        // 获取TMP组件
        TextMeshPro tmpComponent = currentBubble.GetComponentInChildren<TextMeshPro>();
        tmpComponent.text = text;

        // 强制TMP更新网格以计算正确尺寸
        tmpComponent.ForceMeshUpdate();

        // 获取渲染完文本后的实际宽高
            Vector2 textSize = tmpComponent.GetRenderedValues(true);

        // 动态调整背景大小（假设背景是SpriteRenderer）
        SpriteRenderer background = currentBubble.GetComponentInChildren<SpriteRenderer>();
        if (background != null)
            {
                // 设置背景大小，比文本大一圈（增加一些边距）
                float padding = 0.1f;
                background.size = new Vector2(textSize.x + padding, textSize.y + padding);
            }

        // 或者，如果背景是Quad，调整其Transform Scale
        // Transform backgroundTransform = ...;
        // backgroundTransform.localScale = new Vector3(textSize.x + padding, textSize.y + padding, 1);
        
    }

    // 外部调用的交互方法
    public void Interact()
    {
        if (!canInteract) return;

        // 随机选择一句对话
        string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];
        ShowSpeechBubble(randomLine);

        // 触发事件（示例：播放动画）
        GetComponent<Animator>()?.SetTrigger("Wave");

        // 冷却时间
        canInteract = false;
        Invoke(nameof(ResetInteraction), interactionCooldown);
    }

    void ResetInteraction()
    {
        canInteract = true;
        Destroy(currentBubble); // 可选：交互结束后清除气泡
    }
}



