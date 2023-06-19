using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public Terrain terrain;

    public GameObject chunkFab;

    private GameObject worldHolder;

    public void GenerateWorld()
    {
        if (worldHolder)
        {
            Destroy(worldHolder);
        }
        worldHolder = new GameObject("WorldHolder");
        worldHolder.transform.parent = transform;
        int worldSize = Mathf.CeilToInt(terrain.terrainData.size.x / 160);
        float xPos = 0;
        float zPos = 0;
        for (int i = 0; i < worldSize*worldSize; i++)
        {
            GameObject chunkI = Instantiate(chunkFab,Vector3.zero,Quaternion.identity, worldHolder.transform);
            chunkI.name = "TerrainChunk";
            if(xPos < worldSize-1)
            {
                chunkI.transform.position = new Vector3(xPos * 160, 0, zPos*160);
                xPos++;
            }
            else
            {
                chunkI.transform.position = new Vector3(xPos * 160, 0, zPos*160);
                zPos++;
                xPos = 0;
            }
            chunkI.GetComponent<ChunkRenderer>().Setup(this);
        }
    }

    ChunkData GenerateMeshData(int chunkSize, Vector3Int position)
    {
        TerrainData myTerrainData = terrain.terrainData;

        Vector3[] heightMap = new Vector3[chunkSize * chunkSize];
        Cell[,] cells = new Cell[41, 41];

        for (int x = 0, i = 0; x < chunkSize * 4; x += 4)
        {
            for (int z = 0; z < chunkSize * 4; z += 4)
            {
                float terrainYHeight = terrain.SampleHeight(new Vector3(x + position.x, 0, z + position.z));
                Vector3 heightMapWorldPosition = new Vector3(x, terrainYHeight, z);
                heightMapWorldPosition.y = Mathf.RoundToInt((terrainYHeight * 10) / 10);
                heightMap[i] = heightMapWorldPosition;
                cells[x / 4, z / 4] = new Cell(x, z, Cell.CellTypes.Grass, false);

                i++;
            }
        }

        ChunkData chunkData = new ChunkData(heightMap, cells);
        return chunkData;
    }

    public ChunkData GetChunkData(Vector3 chunkPosition)
    {
        Vector3Int roundedPosition = new Vector3Int(Mathf.FloorToInt(chunkPosition.x), 0, Mathf.FloorToInt(chunkPosition.z));

        ChunkData chunkData = GenerateMeshData(41, roundedPosition);

        return chunkData;

    }
}

public class ChunkData
{
    public Vector3[] heightMap;
    public Cell[,] chunkCells;

    public ChunkData(Vector3[] heightMap, Cell[,] chunkCells)
    {
        this.heightMap = heightMap;
        this.chunkCells = chunkCells;
    }
}
