using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Extension;
using UnityEngine;

namespace InterativaSystem.Views.Meshes
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshCuting : GenericView
    {
        private MeshFilter meshRenderer;
        private Mesh mesh;
        public Transform pointA, pointB;

        protected override void OnStart()
        {
            base.OnStart();
            meshRenderer = GetComponent<MeshFilter>();
            mesh = meshRenderer.mesh;

            Cut();
        }

        public class Triangle
        {
            public int pointA;
            public int pointB;
            public int pointC;
        }

        int[] DeserializeTriangles(List<Triangle> tris)
        {
            var triangles = new int[tris.Count*3];

            for (int i = 0; i < tris.Count; i++)
            {
                triangles[i*3] = tris[i].pointA;
                triangles[i*3 + 1] = tris[i].pointB;
                triangles[i*3 + 2] = tris[i].pointC;
            }

            return triangles;
        }
        List<Triangle> FillList(int[] tris)
        {
            var triangles = new List<Triangle>();

            var length = tris.Length;

            for (int i = 0; i < length; i += 3)
            {
                triangles.Add(new Triangle
                {
                    pointA = tris[i],
                    pointB = tris[i + 1],
                    pointC = tris[i + 2]
                });
            }

            return triangles;
        }


        void Cut()
        {
            //var facecount = mesh.vertexCount/4;

            var norm = Vector3.Cross(Camera.main.transform.position, pointB.position - pointA.position);
            var plane = new Plane(norm.normalized, pointA.position);

            var vertices = mesh.vertices.ToList();
            var trianglesStr = FillList(mesh.triangles);

            var nomUsableVertices = new List<Vector3>();

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                if (plane.GetSide(vertices[i] + transform.position))
                {

                }
                else
                {
                    nomUsableVertices.Add(vertices[i]);
                }
            }
            Debug.Log("nomUsableVertices.Count " + nomUsableVertices.Count);
            for (int i = 0; i < nomUsableVertices.Count; i++)
            {
                vertices[vertices.FindIndex(x => x == nomUsableVertices[i])] = Vector3.up;
            }

            var mx = new Mesh();
            mx.vertices = vertices.ToArray();
            mx.triangles = DeserializeTriangles(trianglesStr);

            mx.RecalculateBounds();
            mx.RecalculateNormals();

            meshRenderer.mesh = mx;
        }

        public void OnDrawGizmos()
        {
            if (pointA != null && pointB != null)
            {
                Debug.DrawLine(pointA.position, pointB.position);

                var norm = Vector3.Cross(Camera.main.transform.position, pointB.position - pointA.position);
                Debug.DrawRay(pointA.position, norm.normalized, Color.red);
            }
        }
    }
}