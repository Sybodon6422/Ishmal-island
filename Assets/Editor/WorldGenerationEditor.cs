using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WorldGeneration))]
public class WorldGenerationEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		WorldGeneration mapPreview = (WorldGeneration)target;

		if (GUILayout.Button("Generate"))
		{
			mapPreview.GenerateWorld();
		}
	}
}
