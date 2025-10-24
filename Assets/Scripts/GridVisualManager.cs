using UnityEngine;

public class GridVisualManager : MonoBehaviour
{
    // 在Inspector中拖拽赋值：6种区域材质
    public Material pathMaterial;
    public Material stageMaterial;
    public Material drumTempleMaterial;
    public Material pianoGardenMaterial;
    public Material neonMountainMaterial;
    public Material echoCaveMaterial;

    // 当前地面的Renderer组件（假设整个岛是一个大地面）
    [Header("必填项")]
    [Tooltip("将场景中的地面物体拖拽到这里")]
    public Renderer groundRenderer;
    private Material currentGroundMaterial;

    // 用于平滑切换颜色的变量
    private Color targetColor;
    private Color targetEmission;
    public float colorChangeSpeed = 2f;


    void Start()
    {
        if (groundRenderer == null)
        {
            // 如果没指定，就自动查找Tag为"Ground"的物体
            GameObject ground = GameObject.FindGameObjectWithTag("Ground");
            if (ground != null) groundRenderer = ground.GetComponent<Renderer>();
        }

        if (groundRenderer != null)
        {
            // 创建一个新的材质实例，避免修改原始材质资产
            currentGroundMaterial = groundRenderer.material;
        }
    }

    void Update()
    {
        // 平滑过渡颜色（可选，增加视觉效果流畅度）
        if (currentGroundMaterial != null)
        {
            currentGroundMaterial.color = Color.Lerp(currentGroundMaterial.color, targetColor, Time.deltaTime * colorChangeSpeed);
            currentGroundMaterial.SetColor("_EmissionColor", Color.Lerp(currentGroundMaterial.GetColor("_EmissionColor"), targetEmission, Time.deltaTime * colorChangeSpeed));
        }
    }

    // 这个方法由PlayerController在进入新区域时调用
    public void OnPlayerEnteredGrid(GridManager.GridType newType)
    {
        Debug.Log("视觉管理器：切换至区域 - " + newType);

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

        // 立即设置材质，如果不需要过渡效果，可以注释掉Update中的Lerp代码，并取消下面两行的注释
        // currentGroundMaterial.color = targetColor;
        // currentGroundMaterial.SetColor("_EmissionColor", targetEmission);
    }
}