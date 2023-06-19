using UnityEngine;

public class MeshMorpher : MonoBehaviour
{
    public Mesh sourceMesh;
    public Mesh targetMesh;
    [Range(0, 100)]
    public float morphValue;

    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component not found on the object.");
            return;
        }

        if (sourceMesh == null || targetMesh == null)
        {
            Debug.LogError("Source or target mesh is missing.");
            return;
        }

        if (sourceMesh.vertexCount != targetMesh.vertexCount)
        {
            Debug.LogError("Source and target meshes have different vertex counts.");
            return;
        }

        meshFilter.mesh = Instantiate(sourceMesh);
    }

    private void Update()
    {
        if (meshFilter == null)
            return;

        // Ensure the mesh has been instantiated
        if (meshFilter.sharedMesh == null)
            return;

        // Get the vertex arrays
        Vector3[] sourceVertices = sourceMesh.vertices;
        Vector3[] targetVertices = targetMesh.vertices;
        Vector3[] meshVertices = meshFilter.sharedMesh.vertices;

        // Move the vertices based on the morph value
        for (int i = 0; i < meshVertices.Length; i++)
        {
            meshVertices[i] = Vector3.Lerp(sourceVertices[i], targetVertices[i], morphValue / 100f);
        }

        // Update the mesh with the modified vertices
        meshFilter.sharedMesh.vertices = meshVertices;
        meshFilter.sharedMesh.RecalculateNormals();
        meshFilter.sharedMesh.RecalculateBounds();
    }
}