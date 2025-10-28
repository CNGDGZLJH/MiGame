using System.Reflection;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float wanderRadius = 5f;    // 闲逛范围
    public float moveSpeed = 1f;       // 移动速度
    private Vector3 startPos;
    private Vector3 targetPos;

    [Header("Interaction")]
    public GameObject speechBubble;    // 对话气泡预制件（可选）
    public string[] dialogueLines;     // 对话文本
    public float interactionCooldown = 2f; // 交互冷却时间
    private bool canInteract = true;
    private GameObject currentBubble;

    // 玩家进入范围时指向该 NPC（供 Update 中按 E 使用）
    private NPCController nearbyNPC;

    // 缓存找到的 InteractHint3D 组件（可能挂在 NPC 或进入触发器的 Player 或其子对象）
    private Component _cachedInteractHint;

    // 物理组件缓存
    private Rigidbody _rb;
    private Collider _selfCollider;

    void Start()
    {
        startPos = transform.position;
        SetNewWanderTarget();

        _rb = GetComponent<Rigidbody>();
        _selfCollider = GetComponent<Collider>();

        // 确保使用触发器（避免物理推挤）
        if (_selfCollider != null)
            _selfCollider.isTrigger = true;

        // 配置刚体（平滑移动）
        if (_rb != null)
        {
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            // 保持受物理影响或不受影响由你的需求决定；这里不强制 isKinematic
            // 如果你希望 NPC 不被外力推动，可在 Inspector 把 isKinematic 打开并继续使用 MovePosition
        }
    }

    void Update()
    {
        // 简单闲逛AI
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            SetNewWanderTarget();
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 当玩家在范围内并按 E，优先触发 InteractHint3D 组件（若未找到再执行 Interact）
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
        targetPos.y = startPos.y; // 保持Y轴不变
    }

    // 玩家进入交互范围 —— 弹出提示 "hello"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowSpeechBubble("Press E to interact:");
            nearbyNPC = this; // 标记为可交互的 NPC（供 Update 中按 E 使用）

            // 尝试在 NPC 本体及进入触发器的玩家对象/其子对象上查找 InteractHint3D 组件并缓存
            _cachedInteractHint = FindInteractHintIn(transform) ?? FindInteractHintIn(other.transform);
        }
    }

    // 玩家离开交互范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 安全销毁气泡并取消交互引用
            if (currentBubble != null)
            {
                Destroy(currentBubble);
                currentBubble = null;
            }

            if (nearbyNPC == this) nearbyNPC = null;

            // 离开时清除缓存（下一次进入会重新查找）
            _cachedInteractHint = null;
        }
    }

    // 在 NPC 或指定根下查找名为 InteractHint3D 的组件（不产生编译时依赖）
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

    // 尝试调用缓存的 InteractHint3D 组件的交互方法（若找到），返回是否成功调用
    public bool InvokeInteractHint()
    {
        if (_cachedInteractHint == null)
        {
            // 若缓存为空，先尝试在自身查找
            _cachedInteractHint = FindInteractHintIn(transform);
            if (_cachedInteractHint == null)
                return false;
        }

        var comp = _cachedInteractHint;
        var type = comp.GetType();

        // 常见方法名尝试（无参数）
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

        // 尝试调用可能带单个 string 参数的方法（比如 Show(string))
        foreach (var name in new[] { "Show", "ShowHint", "Trigger", "Interact" })
        {
            MethodInfo mi = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
            if (mi != null)
            {
                mi.Invoke(comp, new object[] { "E" });
                return true;
            }
        }

        // 最后回退：使用 SendMessage 在组件所属对象上广播 OnInteract（不抛异常）
        try
        {
            comp.gameObject.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            comp.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            return true; // 认为已尝试调用
        }
        catch
        {
            Debug.LogWarning($"{name}: 找到 InteractHint3D 组件但无法调用其方法（请检查方法名）。");
            return false;
        }
    }

    // 显示对话气泡（优先使用 speechBubble 预制件，若无或预制件缺文本组件则回退到动态创建 TextMesh）
    void ShowSpeechBubble(string text)
    {
        // 清除旧气泡（如果有）
        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
        }

        // 如果设置了预制件，先用预制件（但仍检查内部是否有文本组件）
        if (speechBubble != null)
        {
            currentBubble = Instantiate(speechBubble, transform.position + Vector3.up * 2f, Quaternion.identity);
            currentBubble.transform.SetParent(transform, true);

            // 禁用气泡及其子对象的所有 Collider，防止气泡对玩家产生物理影响（避免把玩家撞出去）
            var colliders = currentBubble.GetComponentsInChildren<Collider>(true);
            foreach (var c in colliders)
            {
                if (c != null) c.enabled = false;
            }

            // 优先尝试 TextMeshPro
            TextMeshPro tmpComponent = currentBubble.GetComponentInChildren<TextMeshPro>();
            if (tmpComponent != null)
            {
                tmpComponent.text = text;
                tmpComponent.ForceMeshUpdate();
                return;
            }

            // 回退到旧的 TextMesh
            TextMesh legacyText = currentBubble.GetComponentInChildren<TextMesh>();
            if (legacyText != null)
            {
                legacyText.text = text;
                return;
            }

            // 预制件存在但没有文本组件 —— 不再抛出警告式错误，改为普通日志，并使用回退文本显示
            Debug.Log($"{name}: speechBubble 预制件中未找到 TextMeshPro 或 TextMesh 组件，使用回退文本显示。");
            Destroy(currentBubble);
            currentBubble = null;
        }

        // 回退：动态创建一个简单的 3D TextMesh，用于提示（确保不会报 NullReference）
        GameObject fallback = new GameObject("SpeechBubble_Fallback");
        fallback.transform.SetParent(transform, false);
        fallback.transform.localPosition = Vector3.up * 2f;
        // 添加 TextMesh（兼容所有无 TMP 项目的简单解决方案）
        TextMesh tm = fallback.AddComponent<TextMesh>();
        tm.text = text;
        tm.fontSize = 48;
        tm.characterSize = 0.02f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.white;

        // 可选：让文本面向主相机（更像气泡）
        fallback.AddComponent<FaceCamera>();

        currentBubble = fallback;
    }

    // 外部调用的交互方法
    public void Interact()
    {
        if (!canInteract) return;

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            ShowSpeechBubble("...");
        }
        else
        {
            // 随机选择一句对话
            string randomLine = dialogueLines[Random.Range(0, dialogueLines.Length)];
            ShowSpeechBubble(randomLine);
        }

        // 触发事件（示例：播放动画）
        // 安全调用 Animator：先尝试获取组件再调用，避免 MissingComponentException
        Animator animator;
        if (TryGetComponent<Animator>(out animator) && animator != null)
        {
            animator.SetTrigger("Wave");
        }

        // 冷却时间
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

// 简单组件：使回退的 3D 文本始终面向摄像机（UI-like）
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