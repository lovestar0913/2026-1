using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WFCMapGenerator : MonoBehaviour
{
    public const int WIDTH = 50;
    public const int HEIGHT = 50;
    public float cellSize = 1f;

    // ⭐ 地圖起始座標（左下角）
    public Vector3 origin = new Vector3(-25f, -25f, 0f);

    // ================= Tile 定義 =================
    public enum TileType
    {
        Floor0, Floor1, Floor2, Floor3, Floor4, Floor5,
        Floor6, Floor7, Floor8, Floor9, Floor10, Floor11,

        Wall1,
        Wall2_L,
        Wall2_R,

        WallTile1,
        WallTile2,
        WallTile3,
        WallTile4,

        Hole
    }

    // ================= Prefab =================
    [System.Serializable]
    public class TilePrefab
    {
        public TileType type;
        public GameObject prefab;
    }

    public TilePrefab[] tilePrefabs;
    public GameObject lightPrefab;

    Dictionary<TileType, GameObject> prefabDict;

    // ================= Cell =================
    class Cell
    {
        public HashSet<TileType> possible;
        public bool Collapsed => possible.Count == 1;
        public TileType Value => possible.First();

        public Cell(IEnumerable<TileType> init)
        {
            possible = new HashSet<TileType>(init);
        }
    }

    Cell[,] grid = new Cell[WIDTH, HEIGHT];

    // ================= Unity =================
    void Awake()
    {
        prefabDict = new Dictionary<TileType, GameObject>();
        foreach (var t in tilePrefabs)
            prefabDict[t.type] = t.prefab;
    }

    void Start()
    {
        Generate();
    }

    // ================= 主流程 =================
    void Generate()
    {
        foreach (Transform c in transform)
            Destroy(c.gameObject);

        InitCells();
        BuildBorders();
        BuildTopBottomWalls();
        ApplyWFC();
        PlaceHoles(15);
        SpawnTiles();
        PlaceLights();
    }

    // ================= 初始化 =================
    void InitCells()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (x == 0 || x == WIDTH - 1)
                {
                    grid[x, y] = new Cell(new[] { TileType.Wall1 });
                }
                else if (y == HEIGHT - 2)
                {
                    grid[x, y] = new Cell(new[]
                    {
                        TileType.WallTile1,
                        TileType.WallTile2,
                        TileType.WallTile3,
                        TileType.WallTile4
                    });
                }
                else
                {
                    grid[x, y] = new Cell(AllFloors());
                }
            }
        }
    }

    IEnumerable<TileType> AllFloors()
    {
        return new[]
        {
            TileType.Floor0, TileType.Floor1, TileType.Floor2,
            TileType.Floor3, TileType.Floor4, TileType.Floor5,
            TileType.Floor6, TileType.Floor7, TileType.Floor8,
            TileType.Floor9, TileType.Floor10, TileType.Floor11
        };
    }

    // ================= 固定牆 =================
    void BuildBorders()
    {
        for (int y = 0; y < HEIGHT; y++)
        {
            grid[0, y].possible.Clear();
            grid[0, y].possible.Add(TileType.Wall1);

            grid[WIDTH - 1, y].possible.Clear();
            grid[WIDTH - 1, y].possible.Add(TileType.Wall1);
        }
    }

    void BuildTopBottomWalls()
    {
        BuildWallRow(HEIGHT - 1);
        BuildWallRow(0);
    }

    void BuildWallRow(int y)
    {
        int x = 1;
        while (x < WIDTH - 1)
        {
            if (x < WIDTH - 2 && Random.value < 0.4f)
            {
                grid[x, y].possible.Clear();
                grid[x, y].possible.Add(TileType.Wall2_L);

                grid[x + 1, y].possible.Clear();
                grid[x + 1, y].possible.Add(TileType.Wall2_R);

                x += 2;
            }
            else
            {
                grid[x, y].possible.Clear();
                grid[x, y].possible.Add(TileType.Wall1);
                x++;
            }
        }
    }

    // ================= WFC（Floor only） =================
    void ApplyWFC()
    {
        while (true)
        {
            Vector2Int pos = FindLowestEntropy();
            if (pos.x < 0) break;

            Cell cell = grid[pos.x, pos.y];
            TileType chosen = cell.possible.OrderBy(_ => Random.value).First();

            cell.possible.Clear();
            cell.possible.Add(chosen);

            Propagate(pos);
        }
    }

    Vector2Int FindLowestEntropy()
    {
        int min = int.MaxValue;
        Vector2Int result = new Vector2Int(-1, -1);

        for (int x = 1; x < WIDTH - 1; x++)
        {
            for (int y = 1; y < HEIGHT - 2; y++)
            {
                Cell c = grid[x, y];
                if (!c.Collapsed && c.possible.Count < min)
                {
                    min = c.possible.Count;
                    result = new Vector2Int(x, y);
                }
            }
        }
        return result;
    }

    void Propagate(Vector2Int start)
    {
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(start);

        while (q.Count > 0)
        {
            Vector2Int p = q.Dequeue();
            Cell center = grid[p.x, p.y];

            foreach (Vector2Int n in Neighbors(p))
            {
                Cell neighbor = grid[n.x, n.y];
                if (neighbor.Collapsed) continue;

                int before = neighbor.possible.Count;

                neighbor.possible.RemoveWhere(
                    t => !IsValid(center.Value, t)
                );

                if (neighbor.possible.Count < before)
                    q.Enqueue(n);
            }
        }
    }

    bool IsValid(TileType center, TileType neighbor)
    {
        if (center.ToString().StartsWith("Floor"))
            return neighbor.ToString().StartsWith("Floor") || neighbor == TileType.Hole;

        return true;
    }

    IEnumerable<Vector2Int> Neighbors(Vector2Int p)
    {
        if (p.x > 1) yield return new Vector2Int(p.x - 1, p.y);
        if (p.x < WIDTH - 2) yield return new Vector2Int(p.x + 1, p.y);
        if (p.y > 1) yield return new Vector2Int(p.x, p.y - 1);
        if (p.y < HEIGHT - 3) yield return new Vector2Int(p.x, p.y + 1);
    }

    // ================= Hole =================
    void PlaceHoles(int count)
    {
        List<Vector2Int> floors = new List<Vector2Int>();

        for (int x = 1; x < WIDTH - 1; x++)
            for (int y = 1; y < HEIGHT - 2; y++)
                if (grid[x, y].Value.ToString().StartsWith("Floor"))
                    floors.Add(new Vector2Int(x, y));

        for (int i = 0; i < count && floors.Count > 0; i++)
        {
            int idx = Random.Range(0, floors.Count);
            Vector2Int p = floors[idx];

            grid[p.x, p.y].possible.Clear();
            grid[p.x, p.y].possible.Add(TileType.Hole);

            floors.RemoveAt(idx);
        }
    }

    // ================= 生 Tile =================
    void SpawnTiles()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileType type = grid[x, y].Value;
                if (!prefabDict.ContainsKey(type)) continue;

                // ⭐ 關鍵：套用 origin
                Vector3 pos = origin + new Vector3(
                    x * cellSize,
                    y * cellSize,
                    0
                );

                Instantiate(prefabDict[type], pos, Quaternion.identity, transform);
            }
        }
    }

    // ================= Light =================
    void PlaceLights()
    {
        int y = HEIGHT - 2;
        int gap = Random.Range(5, 11);
        int count = 0;

        for (int x = 1; x < WIDTH - 1; x++)
        {
            count++;
            if (count >= gap)
            {
                Vector3 pos = origin + new Vector3(
                    x * cellSize,
                    (y + 0.5f) * cellSize,
                    -1f
                );

                Instantiate(lightPrefab, pos, Quaternion.identity, transform);

                count = 0;
                gap = Random.Range(5, 11);
            }
        }
    }
}
