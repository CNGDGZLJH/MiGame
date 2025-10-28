using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AirWall : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("��Ϊ false ʱ��AirWall ʹ�÷Ǵ������� Collider���Ƽ�����CharacterController ���Զ����赲��")]
    public bool useTriggerMode = false;

    [Tooltip("������ģʽ���ƶ���ҵ����ȣ���λ���ף�ÿ֡����̫����ܲ������������� 0.01 ~ 0.2")]
    public float pushOutPerFrame = 0.05f;

    [Tooltip("����ǿգ���ֻ�Ծ��д� Tag �Ķ�����Ч����������д� CharacterController �Ķ�����Ч")]
    public string targetTag = "Player";

    [Header("����")]
    public bool drawGizmos = true;
    public Color gizmoColor = new Color(0f, 0.6f, 1f, 0.25f);

    private Collider _coll;

    void Reset()
    {
        // ������ٴ�������û�� MeshRenderer����Ĭ�ϲ��ɼ�
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

        // �Ƽ�ʹ�÷Ǵ�������CharacterController ���Զ��赲��
        _coll.isTrigger = useTriggerMode;
    }

    // ������ģʽ��������ڴ������ڲ�����ͼ����ʱ����һ��С��������Ƴ�ȥ
    // �ŵ㣺���Ա��� Trigger �¼��߼�������������������ȱ�㣺������Ҫ�����Է�������
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
        // ���� player ��ǽ�������
        Vector3 playerPos = playerTransform.position;
        Vector3 closest = _coll.ClosestPoint(playerPos);

        // ��� ClosestPoint �� playerPos ��ȣ���ʾ player ����ײ���ڲ�����ǳ��ӽ�������Ҫ�����ƿ�����
        Vector3 pushDir;
        float dist = Vector3.Distance(playerPos, closest);

        if (dist <= 0.001f)
        {
            // ʹ��ǽ����浽��ҵĴ��·���ˮƽ�棩�������������ʹ����ҳ���ķ�����
            pushDir = (playerPos - transform.position);
            pushDir.y = 0f;
            if (pushDir.sqrMagnitude <= 0.0001f)
            {
                // ���ף�����ҳ���ȡ������
                pushDir = -playerTransform.forward;
                pushDir.y = 0f;
            }
            pushDir.Normalize();
        }
        else
        {
            // ������ⲿ��������������������룺�������ָ����ң��ظ÷����ƿ���ˮƽ�������ȣ�
            pushDir = (playerPos - closest);
            pushDir.y = 0f;
            if (pushDir.sqrMagnitude <= 0.0001f)
            {
                // �����ڴ�ֱ�����ϽӴ�������ʹ���������ⷽ��
                pushDir = (playerPos - transform.position);
                pushDir.y = 0f;
            }
            pushDir.Normalize();
        }

        // ֻ��ˮƽ�����ƣ�����Ӱ����Ծ/���䣩
        Vector3 move = new Vector3(pushDir.x, 0f, pushDir.z) * pushOutPerFrame;

        // ʹ�� CharacterController.Move ���ƶ���ң���֤��ͨ�� CharacterController ������ײ
        cc.Move(move);
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos || _coll == null) return;
        Gizmos.color = gizmoColor;
        // ���Ի��Ƽ򵥵İ�Χ�У����� BoxCollider�����ƾ�ȷ���ӣ�
        var box = _coll as BoxCollider;
        if (box != null)
        {
            Matrix4x4 old = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
            Gizmos.matrix = old;
            return;
        }

        // ���ף����� bounds
        Gizmos.DrawCube(_coll.bounds.center, _coll.bounds.size);
    }
}