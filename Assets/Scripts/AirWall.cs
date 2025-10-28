using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AirWall : MonoBehaviour
{
    [Header("基础设置")]
    [Tooltip("当为 false 时，AirWall 使用非触发器的 Collider（推荐）。CharacterController 会自动被阻挡。")]
    public bool useTriggerMode = false;

    [Tooltip("触发器模式下推动玩家的力度（单位：米，每帧）。太大可能产生抖动，建议 0.01 ~ 0.2")]
    public float pushOutPerFrame = 0.05f;

    [Tooltip("如果非空，则只对具有此 Tag 的对象生效；否则对所有带 CharacterController 的对象生效")]
    public string targetTag = "Player";

    [Header("调试")]
    public bool drawGizmos = true;
    public Color gizmoColor = new Color(0f, 0.6f, 1f, 0.25f);

    private Collider _coll;

    void Reset()
    {
        // 方便快速创建：若没有 MeshRenderer，则默认不可见
        _coll = GetComponent<Collider>();
    }

    void Awake()
    {
        _coll = GetComponent<Collider>();
        ApplyModeToCollider();
    }

    void OnValidate()
    {
        _coll = GetComponent<Collider>();
        ApplyModeToCollider();
    }

    private void ApplyModeToCollider()
    {
        if (_coll == null) return;

        // 推荐使用非触发器（CharacterController 会自动阻挡）
        _coll.isTrigger = useTriggerMode;
    }

    // 触发器模式：当玩家在触发器内部或试图穿过时，按一个小量把玩家推出去
    // 优点：可以保留 Trigger 事件逻辑（用于其他交互）。缺点：可能需要调参以防抖动。
    private void OnTriggerStay(Collider other)
    {
        if (!useTriggerMode) return;

        if (!string.IsNullOrEmpty(targetTag))
        {
            if (!other.CompareTag(targetTag)) return;
        }

        var cc = other.GetComponent<CharacterController>();
        if (cc == null) return;

        PushOutCharacterController(cc, other.transform);
    }

    private void PushOutCharacterController(CharacterController cc, Transform playerTransform)
    {
        // 计算 player 与墙体最近点
        Vector3 playerPos = playerTransform.position;
        Vector3 closest = _coll.ClosestPoint(playerPos);

        // 如果 ClosestPoint 与 playerPos 相等，表示 player 在碰撞体内部（或非常接近）。需要计算推开方向
        Vector3 pushDir;
        float dist = Vector3.Distance(playerPos, closest);

        if (dist <= 0.001f)
        {
            // 使用墙体表面到玩家的大致方向（水平面），如果不可用则使用玩家朝向的反方向
            pushDir = (playerPos - transform.position);
            pushDir.y = 0f;
            if (pushDir.sqrMagnitude <= 0.0001f)
            {
                // 兜底：沿玩家朝向取反方向
                pushDir = -playerTransform.forward;
                pushDir.y = 0f;
            }
            pushDir.Normalize();
        }
        else
        {
            // 玩家在外部，但距离最近点有正距离：从最近点指向玩家，沿该方向推开（水平分量优先）
            pushDir = (playerPos - closest);
            pushDir.y = 0f;
            if (pushDir.sqrMagnitude <= 0.0001f)
            {
                // 若仅在垂直方向上接触，尝试使用世界向外方向
                pushDir = (playerPos - transform.position);
                pushDir.y = 0f;
            }
            pushDir.Normalize();
        }

        // 只在水平面上推（避免影响跳跃/下落）
        Vector3 move = new Vector3(pushDir.x, 0f, pushDir.z) * pushOutPerFrame;

        // 使用 CharacterController.Move 来移动玩家，保证仍通过 CharacterController 处理碰撞
        cc.Move(move);
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos || _coll == null) return;
        Gizmos.color = gizmoColor;
        // 尝试绘制简单的包围盒（若是 BoxCollider，绘制精确盒子）
        var box = _coll as BoxCollider;
        if (box != null)
        {
            Matrix4x4 old = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
            Gizmos.matrix = old;
            return;
        }

        // 兜底：绘制 bounds
        Gizmos.DrawCube(_coll.bounds.center, _coll.bounds.size);
    }
}