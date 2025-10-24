using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public float cellSize = 5f;

    // ����һ����ά���飬�洢ÿ�����ӵ�����
    public GridType[,] grid;

    // ����������ͣ���Ӧ��ͬ����
    public enum GridType
    {
        Path, // ·��
        Stage, // ��̨
        DrumTemple, // �ĵ�����
        PianoGarden, // �ټ���԰
        NeonMountain, // �޺�ɽ
        EchoCave // ������Ѩ
    }

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new GridType[gridSizeX, gridSizeZ];

        // ��ʼ�����и���ΪĬ��·��
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                grid[x, z] = GridType.Path;
            }
        }

        // �ֶ�������������
        SetGridArea(4, 4, 5, 5, GridType.Stage); // ������̨
        SetGridArea(0, 0, 2, 2, GridType.DrumTemple);
        SetGridArea(7, 0, 9, 2, GridType.PianoGarden);
        SetGridArea(0, 7, 2, 9, GridType.NeonMountain);
        SetGridArea(4, 0, 4, 0, GridType.EchoCave); // �������ӵ���������
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

    // �������������ȡ���ڵĸ�������
    public GridType GetGridTypeAtPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize + gridSizeX / 2);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize + gridSizeZ / 2);

        if (x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ)
        {
            return grid[x, z];
        }
        return GridType.Path; // ��������Χ�򷵻�Ĭ������
    }
}