/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainToMesh : MonoBehaviour
{
    ChunkData GenerateMeshData(int chunkSize, Vector3Int position)
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData myTerrainData = terrain.terrainData;

        Vector3[] heightMap = new Vector3[chunkSize * chunkSize];
        Cell[,] cells = new Cell[41,41];

        for (int x = 0,i = 0; x < chunkSize*4; x+=4)
        {
            for (int z = 0; z < chunkSize*4; z+=4)
            {
                Debug.Log("Called");
                float terrainYHeight = terrain.SampleHeight(new Vector3(x + position.x, 0, z + position.z));
                Vector3 heightMapWorldPosition = new Vector3(x, terrainYHeight, z);
                heightMapWorldPosition.y = Mathf.RoundToInt((terrainYHeight * 10) / 10);
                heightMap[i] = heightMapWorldPosition;
                cells[x/4, z/4] = new Cell(x, z, Cell.CellTypes.Grass, false);

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

*/