using UnityEngine;

public class CurtainInteraction : MonoBehaviour
{
    public MeshFilter meshFilter;
    public int topVertexCount = 10; // ���������� ������� �����, ������� ����� ���������

    private Vector3[] originalVertices;
    private bool[] fixedVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalVertices = meshFilter.mesh.vertices;
        fixedVertices = new bool[originalVertices.Length];

        // ��������� ������� �����
        for (int i = 0; i < topVertexCount; i++)
        {
            fixedVertices[i] = true;
        }
    }

    void Update()
    {
        UpdateVertices();
    }

    void UpdateVertices()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (fixedVertices[i])
            {
                vertices[i] = originalVertices[i];
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
