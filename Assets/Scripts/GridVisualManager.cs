using UnityEngine;

public class GridVisualManager : MonoBehaviour
{
    // ��Inspector����ק��ֵ��6���������
    public Material pathMaterial;
    public Material stageMaterial;
    public Material drumTempleMaterial;
    public Material pianoGardenMaterial;
    public Material neonMountainMaterial;
    public Material echoCaveMaterial;

    // ��ǰ�����Renderer�����������������һ������棩
    [Header("������")]
    [Tooltip("�������еĵ���������ק������")]
    public Renderer groundRenderer;
    private Material currentGroundMaterial;

    // ����ƽ���л���ɫ�ı���
    private Color targetColor;
    private Color targetEmission;
    public float colorChangeSpeed = 2f;


    void Start()
    {
        if (groundRenderer == null)
        {
            // ���ûָ�������Զ�����TagΪ"Ground"������
            GameObject ground = GameObject.FindGameObjectWithTag("Ground");
            if (ground != null) groundRenderer = ground.GetComponent<Renderer>();
        }

        if (groundRenderer != null)
        {
            // ����һ���µĲ���ʵ���������޸�ԭʼ�����ʲ�
            currentGroundMaterial = groundRenderer.material;
        }
    }

    void Update()
    {
        // ƽ��������ɫ����ѡ�������Ӿ�Ч�������ȣ�
        if (currentGroundMaterial != null)
        {
            currentGroundMaterial.color = Color.Lerp(currentGroundMaterial.color, targetColor, Time.deltaTime * colorChangeSpeed);
            currentGroundMaterial.SetColor("_EmissionColor", Color.Lerp(currentGroundMaterial.GetColor("_EmissionColor"), targetEmission, Time.deltaTime * colorChangeSpeed));
        }
    }

    // ���������PlayerController�ڽ���������ʱ����
    public void OnPlayerEnteredGrid(GridManager.GridType newType)
    {
        Debug.Log("�Ӿ����������л������� - " + newType);

        switch (newType)
        {
            case GridManager.GridType.Path:
                SetGroundMaterial(pathMaterial);
                break;
            case GridManager.GridType.Stage:
                SetGroundMaterial(stageMaterial);
                break;
            case GridManager.GridType.DrumTemple:
                SetGroundMaterial(drumTempleMaterial);
                break;
            case GridManager.GridType.PianoGarden:
                SetGroundMaterial(pianoGardenMaterial);
                break;
            case GridManager.GridType.NeonMountain:
                SetGroundMaterial(neonMountainMaterial);
                break;
            case GridManager.GridType.EchoCave:
                SetGroundMaterial(echoCaveMaterial);
                break;
            default:
                SetGroundMaterial(pathMaterial);
                break;
        }
    }

    private void SetGroundMaterial(Material newMat)
    {
        if (groundRenderer == null) return;

        targetColor = newMat.color;
        targetEmission = newMat.GetColor("_EmissionColor");

        // �������ò��ʣ��������Ҫ����Ч��������ע�͵�Update�е�Lerp���룬��ȡ���������е�ע��
        // currentGroundMaterial.color = targetColor;
        // currentGroundMaterial.SetColor("_EmissionColor", targetEmission);
    }
}