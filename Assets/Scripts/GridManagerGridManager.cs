using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public float cellSize = 5f;

    // 这是一个二维数组，存储每个格子的类型
    public GridType[,] grid;

    // 定义格子类型，对应不同区域
    public enum GridType
    {
        Path, // 路径
        Stage, // 舞台
        DrumTemple, // 鼓点神庙
        PianoGarden, // 琴键花园
        NeonMountain, // 霓虹山
        EchoCave // 回声洞穴
    }

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new GridType[gridSizeX, gridSizeZ];

        // 初始化所有格子为默认路径
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                grid[x, z] = GridType.Path;
            }
        }

        // 手动设置特殊区域
        SetGridArea(4, 4, 5, 5, GridType.Stage); // 中央舞台
        SetGridArea(0, 0, 2, 2, GridType.DrumTemple);
        SetGridArea(7, 0, 9, 2, GridType.PianoGarden);
        SetGridArea(0, 7, 2, 9, GridType.NeonMountain);
        SetGridArea(4, 0, 4, 0, GridType.EchoCave); // 单个格子的隐藏区域
    }

    void SetGridArea(int startX, int startZ, int endX, int endZ, GridType type)
    {
        for (int x = startX; x <= endX; x++)
        {
            for (int z = startZ; z <= endZ; z++)
            {
                if (x < gridSizeX && z < gridSizeZ)
                {
                    grid[x, z] = type;
                }
            }
        }
    }

    // 根据世界坐标获取所在的格子类型
    public GridType GetGridTypeAtPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize + gridSizeX / 2);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize + gridSizeZ / 2);

        if (x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ)
        {
            return grid[x, z];
        }
        return GridType.Path; // 超出网格范围则返回默认类型
    }
}