using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ChunkRenderer))]
public class TerrainToMeshEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		ChunkRenderer mapPreview = (ChunkRenderer)target;

		if (GUILayout.Button("Generate"))
		{
			//mapPreview.Setup();
		}
	}
}
