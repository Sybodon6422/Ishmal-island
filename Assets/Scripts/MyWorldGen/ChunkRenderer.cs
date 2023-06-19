using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : MonoBehaviour
{
    const int chunkSize = 40;
    Vector3[] vertices;
    int[] triangles;
    [SerializeField]
    public WorldGeneration master;

    Cell[] cellsInChunk;

    public void Setup(WorldGeneration _master)
    {
        master = _master;
        GenerateChunkMesh();
        SetupTiles();
    }

    void SetupTiles()
    {
        for (int i = 0; i < cellsInChunk.Length; i++)
        {
            cellsInChunk[i].cornerVertices = new float[4];
            for (int v = 0; v < 4; v++)
            {
                cellsInChunk[i].cornerVertices[0] = CheckCornerMatch(new Vector3(cellsInChunk[i].xPosition, 0, cellsInChunk[i].zPosition));
                cellsInChunk[i].cornerVertices[1] = CheckCornerMatch(new Vector3(cellsInChunk[i].xPosition*4, 0, cellsInChunk[i].zPosition));
                cellsInChunk[i].cornerVertices[2] = CheckCornerMatch(new Vector3(cellsInChunk[i].xPosition, 0, cellsInChunk[i].zPosition*4));
                cellsInChunk[i].cornerVertices[3] = CheckCornerMatch(new Vector3(cellsInChunk[i].xPosition*4, 0, cellsInChunk[i].zPosition*4));
            }

        }
    }

    float CheckCornerMatch(Vector3 pos)
    {

        for (int v = 0; v < vertices.Length; v++)
        {
            if(vertices[v].x == pos.x && vertices[v].z == pos.z)
            {
                return vertices[v].y;
            }
        }
        return 0;
    }
    private ChunkData chunkData;
    public void GenerateChunkMesh()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        MeshCollider collision = gameObject.GetComponent<MeshCollider>();

        chunkData = master.GetChunkData(transform.position);
        vertices = chunkData.heightMap;


        int vert = 0;
        int tris = 0;

        triangles = new int[chunkSize*chunkSize*6];
        cellsInChunk = new Cell[chunkSize * chunkSize];

        for (int z = 0,i = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                cellsInChunk[i] = new Cell(x, z, Cell.CellTypes.Grass, false);

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = vert + chunkSize + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + chunkSize + 2;
                triangles[tris + 5] = vert + chunkSize + 1;
                vert++;
                tris += 6;
                i++;
            }
            vert++;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        collision.sharedMesh = mesh;
        filter.mesh = mesh;
    }
}

[System.Serializable]
public class Cell
{
    public float[] cornerVertices;
    public int xPosition;
    public int zPosition;

    public CellTypes cellType;

    public bool UnderWater;

    public Cell(int xPosition, int zPosition, CellTypes cellType, bool underWater)
    {
        this.xPosition = xPosition;
        this.zPosition = zPosition;

        this.cellType = cellType;
        UnderWater = underWater;
    }

    public enum CellTypes
    {
        Dirt,
        Grass,
        Rock,
        Sand
    }
}
