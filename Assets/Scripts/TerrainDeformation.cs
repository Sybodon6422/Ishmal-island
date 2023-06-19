using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDeformation : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices, modifiedVerts;

    private MeshCollider collision;
    bool setup = false;
    void First()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        collision = GetComponent<MeshCollider>();

        var filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        setup = true;
    }

    private void UpdateMethod()
    {
        if (!setup) { First(); }

        vertices = mesh.vertices;
        modifiedVerts = mesh.vertices;

    }

    void RecalculateMesh()
    {
        mesh.vertices = modifiedVerts;
        collision.sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    public void ChangeHeight(Vector3 _position, float strength, bool removing)
    {
        UpdateMethod();
        Vector3 position = transform.InverseTransformPoint(_position);
        int posInArray = 1;
        float oldDistance = 500;
        for (int v = 0; v < modifiedVerts.Length; v++)
        {
            if (Vector3.Distance(position, modifiedVerts[v]) < oldDistance)
            {
                oldDistance = Vector3.Distance(position, modifiedVerts[v]);
                posInArray = v;
            }
        }
        float modPosition = Mathf.Round(modifiedVerts[posInArray].y * 10) / 10;

        if (removing)
        {
            modifiedVerts[posInArray].y = modPosition - strength;
            GameHUD.I.consoleBox.PrintToConsole("Moved: " + modifiedVerts[posInArray].y + " by " + (modPosition - strength));
        }
        else
        {
            modifiedVerts[posInArray].y = modPosition + strength;
            GameHUD.I.consoleBox.PrintToConsole("Moved: " + modifiedVerts[posInArray].y + " by " + (modPosition + strength));
        }

        RecalculateMesh();
    }
    /*    public void MoveTowardsSetHeight(Vector3 _position, float strength, Vector3 desiredPosition)
        {

            Vector3 position = transform.InverseTransformPoint(_position);
            Vector3 fixedDsiredPosition = transform.InverseTransformPoint(desiredPosition);

            float desiredYPosition = Mathf.Round(fixedDsiredPosition.y*10)/10;


            int posInArray = 1;
            float oldDistance = 500;
            for (int v = 0; v < modifiedVerts.Length; v++)
            {
                if (Vector3.Distance(position, modifiedVerts[v]) < oldDistance)
                {
                    oldDistance = Vector3.Distance(position, modifiedVerts[v]);
                    posInArray = v;
                }
            }
            float moveTowardsAmmount = 0;
            if (desiredYPosition < modifiedVerts[posInArray].y)
            {
                moveTowardsAmmount = Mathf.Clamp(moveTowardsAmmount + strength, 0, desiredYPosition);
            }
            else
            {
                moveTowardsAmmount = Mathf.Clamp(moveTowardsAmmount - strength, desiredYPosition, 60);
            }
            GameHUD.I.consoleBox.PrintToConsole("Flattened: " + modifiedVerts[posInArray].y + " by " + (moveTowardsAmmount));
            float modPosition = Mathf.Round(moveTowardsAmmount * 10) / 10;
            modifiedVerts[posInArray].y = modPosition;
            RecalculateMesh();
        }*/
    public void MoveTowardsSetHeight(Vector3 _position, float strength, Vector3 desiredPosition)
    {
        UpdateMethod();
        Vector3 fixedDigPosition = transform.InverseTransformPoint(_position);
        float desiredYPosition = Mathf.Round(transform.InverseTransformPoint(desiredPosition).y * 10) / 10;

        int posInArray = 1;
        float oldDistance = 500;
        for (int v = 0; v < modifiedVerts.Length; v++)
        {
            if (Vector3.Distance(fixedDigPosition, modifiedVerts[v]) < oldDistance)
            {
                oldDistance = Vector3.Distance(fixedDigPosition, modifiedVerts[v]);
                posInArray = v;
            }
        }
        float difference = desiredYPosition - modifiedVerts[posInArray].y;
        if(difference == 0 || difference < strength && difference > 0 || difference > -strength && difference < 0)
        {
            modifiedVerts[posInArray].y = desiredYPosition;
            RecalculateMesh();
            return;
        }
        else if(difference > strength)
        {
            modifiedVerts[posInArray].y += strength;
            RecalculateMesh();
            return;
        }
        else if (difference < strength)
        {
            modifiedVerts[posInArray].y -= strength;
            RecalculateMesh();
            return;
        }
    }

}
