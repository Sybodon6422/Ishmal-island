using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeCreator))]
public class ForestBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TreeCreator treeCreator = (TreeCreator)target;

        if (GUILayout.Button("Create Trees"))
        {
            treeCreator.CreateTrees();
        }
    }
}
