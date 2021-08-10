using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    public Material mat;

    public void CreateMesh(Transform[] squares)
    {
        List<Vector3> verticesL = new List<Vector3>();
        List<int> trianglesL = new List<int>();

        Vector3 s = new Vector3(1, 3, 1);

        int i = 0;
        foreach (Transform t in squares)
        {
            Vector3 c = t.localPosition;

            Vector3[] vertices = {
                new Vector3 (-0.5f * s.x + c.x, -0.5f * s.y + c.y, -0.5f * s.z + c.z),
                new Vector3 (0.5f * s.x + c.x, -0.5f * s.y + c.y, -0.5f * s.z + c.z),
                new Vector3 (0.5f * s.x + c.x, 0.5f * s.y + c.y, -0.5f * s.z + c.z),
                new Vector3 (-0.5f * s.x + c.x, 0.5f * s.y + c.y, -0.5f * s.z + c.z),
                new Vector3 (-0.5f * s.x + c.x, 0.5f * s.y + c.y, 0.5f * s.z + c.z),
                new Vector3 (0.5f * s.x + c.x, 0.5f * s.y + c.y, 0.5f * s.z + c.z),
                new Vector3 (0.5f * s.x + c.x, -0.5f * s.y + c.y, 0.5f * s.z + c.z),
                new Vector3 (-0.5f * s.x + c.x, -0.5f * s.y + c.y, 0.5f * s.z + c.z),
            };

            verticesL.AddRange(vertices);

            int[] triangles = {
                0 + 8 * i, 2 + 8 * i, 1 + 8 * i, //face front
			    0 + 8 * i, 3 + 8 * i, 2 + 8 * i,
                2 + 8 * i, 3 + 8 * i, 4 + 8 * i, //face top
			    2 + 8 * i, 4 + 8 * i, 5 + 8 * i,
                1 + 8 * i, 2 + 8 * i, 5 + 8 * i, //face right
			    1 + 8 * i, 5 + 8 * i, 6 + 8 * i,
                0 + 8 * i, 7 + 8 * i, 4 + 8 * i, //face left
			    0 + 8 * i, 4 + 8 * i, 3 + 8 * i,
                5 + 8 * i, 4 + 8 * i, 7 + 8 * i, //face back
			    5 + 8 * i, 7 + 8 * i, 6 + 8 * i,
                0 + 8 * i, 6 + 8 * i, 7 + 8 * i, //face bottom
			    0 + 8 * i, 1 + 8 * i, 6 + 8 * i
            };

            trianglesL.AddRange(triangles);

            i++;
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.Clear();
        mesh.vertices = verticesL.ToArray();
        mesh.triangles = trianglesL.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;

        GetComponent<MeshRenderer>().material = mat;

        //gameObject.AddComponent<Rigidbody>();
    }

    void CreateCube(Vector3 c, Vector3 s)
    {
        Vector3[] vertices = {
            new Vector3 (-0.5f * s.x + c.x, -0.5f * s.y + c.y, -0.5f * s.z + c.z),
            new Vector3 (0.5f * s.x + c.x, -0.5f * s.y + c.y, -0.5f * s.z + c.z),
            new Vector3 (0.5f * s.x + c.x, 0.5f * s.y + c.y, -0.5f * s.z + c.z),
            new Vector3 (-0.5f * s.x + c.x, 0.5f * s.y + c.y, -0.5f * s.z + c.z),
            new Vector3 (-0.5f * s.x + c.x, 0.5f * s.y + c.y, 0.5f * s.z + c.z),
            new Vector3 (0.5f * s.x + c.x, 0.5f * s.y + c.y, 0.5f * s.z + c.z),
            new Vector3 (0.5f * s.x + c.x, -0.5f * s.y + c.y, 0.5f * s.z + c.z),
            new Vector3 (-0.5f * s.x + c.x, -0.5f * s.y + c.y, 0.5f * s.z + c.z),
        };

        int[] triangles = {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
            0, 6, 7, //face bottom
			0, 1, 6
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }

    public Animator anim;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("Walk", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("Walk", false);
        }
    }
}