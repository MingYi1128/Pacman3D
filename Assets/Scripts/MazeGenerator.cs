using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    
    [Header("Parameters")]
    public int width = 25;
    public int depth = 25;
    public float wallSize = 1.0f;
    [Range(0f, 1f)]
    public float loopChance = 0.1f;
    
    public NavMeshSurface navMeshSurface;
    public Transform mazeParent;
    public Transform pelletParent;

    [Header("Prefab")]
    public GameObject wallPrefab; 
    public EntityPellet pelletPrefab;
    
    private int[,] map; 
    
    public MazeGeneration GenerateMaze()
    {
        if (mazeParent != null)
        {
            foreach (Transform child in mazeParent)
            {
                Destroy(child.gameObject);
            }
        }

        if (pelletParent != null)
        {
            foreach (Transform child in pelletParent)
            {
                Destroy(child.gameObject);
            }
        }
        
        if (width % 2 == 0) width++;
        if (depth % 2 == 0) depth++;

        map = new int[width, depth];
        
        GeneratePath(1, 1);
        CreateLoops();
        CreateCenterRoom();
        var playerSpawnPoint = CreateCornerSpawnRoom();
        var pellets = BuildMap();

        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }


        return new MazeGeneration(map, playerSpawnPoint, pellets);
    }
    
    Vector3 CreateCornerSpawnRoom()
    {
        int roomSize = 4;

        int cornerIndex = Random.Range(0, 4);

        int startX = 1;
        int startZ = 1;
        
        switch (cornerIndex)
        {
            case 0:
                startX = 1;
                startZ = 1;
                break;
            case 1:
                startX = width - 1 - roomSize;
                startZ = 1;
                break;
            case 2: 
                startX = 1;
                startZ = depth - 1 - roomSize;
                break;
            case 3: 
                startX = width - 1 - roomSize;
                startZ = depth - 1 - roomSize;
                break;
        }

        for (int x = startX; x < startX + roomSize; x++)
        {
            for (int z = startZ; z < startZ + roomSize; z++)
            {
                map[x, z] = 1;
            }
        }
        
        float centerX = (startX + roomSize / 2.0f) * wallSize;
        float centerZ = (startZ + roomSize / 2.0f) * wallSize;
        
        centerX -= wallSize * 0.5f; 
        centerZ -= wallSize * 0.5f;

        return new Vector3(centerX, 1.0f, centerZ);
    }
    
    
    void CreateCenterRoom()
    {
        int centerX = width / 2;
        int centerZ = depth / 2;

        int startX = centerX - 2;
        int startZ = centerZ - 2;
        int endX = centerX + 2;
        int endZ = centerZ + 2;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                if (x > 0 && x < width - 1 && z > 0 && z < depth - 1)
                {
                    map[x, z] = 1;
                }
            }
        }
    }
    
    void GeneratePath(int x, int z)
    {
        map[x, z] = 1; 

        int[] directions = { 0, 1, 2, 3 };
        Shuffle(directions);

        foreach (int dir in directions)
        {
            int nextX = x;
            int nextZ = z;
            
            switch (dir)
            {
                case 0: nextZ += 2; break; // Top
                case 1: nextZ -= 2; break; // Bottom
                case 2: nextX -= 2; break; // Left
                case 3: nextX += 2; break; // Right
            }

            if (IsInBounds(nextX, nextZ) && map[nextX, nextZ] == 0)
            {
                map[(x + nextX) / 2, (z + nextZ) / 2] = 1;
                
                GeneratePath(nextX, nextZ);
            }
        }
    }

    void CreateLoops()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                if (map[x, z] == 0 && Random.value < loopChance)
                {
                    if (CountNeighbors(x, z) <= 2) 
                        map[x, z] = 1;
                }
            }
        }
    }

    List<EntityPellet> BuildMap()
    {
        List<EntityPellet> pellets = new List<EntityPellet>();
            
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (map[x, z] == 0)
                {
                    Vector3 pos = new Vector3(x * wallSize, 0, z * wallSize);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    wall.transform.SetParent(mazeParent);
                    wall.transform.localScale = new Vector3(wallSize, 2.5f, wallSize); 
                }
                else
                {
                    Vector3 pos = new Vector3(x * wallSize, 0.5f, z * wallSize);
                    EntityPellet pellet = Instantiate(pelletPrefab, pos, Quaternion.identity);
                    pellet.transform.SetParent(pelletParent);
                    pellets.Add(pellet);
                }
            }
        }
        
        return pellets;
    }

    bool IsInBounds(int x, int z)
    {
        return x > 0 && x < width - 1 && z > 0 && z < depth - 1;
    }

    int CountNeighbors(int x, int z)
    {
        int count = 0;
        if (map[x + 1, z] == 1) count++;
        if (map[x - 1, z] == 1) count++;
        if (map[x, z + 1] == 1) count++;
        if (map[x, z - 1] == 1) count++;
        return count;
    }

    void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rnd = Random.Range(0, array.Length);
            int temp = array[rnd];
            array[rnd] = array[i];
            array[i] = temp;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
