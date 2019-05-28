using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.Grid
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PolygonSquareGridView : GenericView
    {
        private GridData gridInfo;
        private MeshFilter meshFilter;
        //private MeshRenderer meshRenderer;
        private GroupsInfo groupsInfo;
        
        private ConquerController _conquerController;

        protected override void OnStart()
        {
            base.OnStart();

            gridInfo = _bootstrap.GetModel(ModelTypes.Grid) as GridData;
            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;

            _conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
            _conquerController.MapGrid = this;
            
            meshFilter = GetComponent<MeshFilter>();
            //meshRenderer = GetComponent<MeshRenderer>();

            CreatePolygon();

            /*
            var list = gridInfo.grid.ExpandArea(10, new Vector2(15, 15));
            PaintAera(list, 1);
            list = gridInfo.grid.ExpandArea(30, new Vector2(16, 12));
            PaintAera(list, 2);

            var list = gridInfo.grid.GetNeighbours(3, new Vector2(15, 15));
            list.Add(gridInfo.grid.GetGridPoint(new Vector2(15, 15)));
            PaintAera(list, 1);

            list = gridInfo.grid.GetNeighbours(3, new Vector2(16, 12));
            list.Add(gridInfo.grid.GetGridPoint(new Vector2(16, 12)));
            PaintAera(list, 2);
            /**/
        }

        List<Color> colorl;
        List<VerticeGripPointAssoc> assoc;
        private Mesh mesh;
        protected void CreatePolygon()
        {
            mesh = meshFilter.mesh = new Mesh();

            var verticesl = new List<Vector3>();
            var trianglesl = new List<int>();
            assoc = new List<VerticeGripPointAssoc>();
            var uvl = new List<Vector2>();
            colorl = new List<Color>();

            var areas = gridInfo.grid.GetValidGridPoints();
            
            var pointSize = gridInfo.grid.PointSize;

            var count = 0;
            for (int i = 0, n = areas.Count; i < n; i++)
            {
                assoc.Add(new VerticeGripPointAssoc {localPos = areas[i].localPosition});
                assoc.Last().vertices = new List<int>();

                for (int j = 0; j < areas[i].cornerPoints.Length; j++)
                {
                    verticesl.Add(areas[i].cornerPoints[j]);
                    assoc.Last().vertices.Add(count);

                    switch (areas[i].usableCorners)
                    {
                        #region Nothing
                        case 0:
                            break;
                        #endregion

                        #region One Corner
                        case 14:
                            if (j == 0)
                                verticesl[verticesl.Count-1] = new Vector3(
                                    areas[i].worldPosition.x + (pointSize/4),
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z + (pointSize/4)
                                    );
                            break;
                        case 13:
                            if (j == 1)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                areas[i].worldPosition.x + (pointSize / 4),
                                areas[i].worldPosition.y,
                                areas[i].worldPosition.z - (pointSize / 4)
                                );
                            break;
                        case 11:
                            if (j == 2)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                areas[i].worldPosition.x - (pointSize / 4),
                                areas[i].worldPosition.y,
                                areas[i].worldPosition.z - (pointSize / 4)
                                );
                            break;
                        case 7:
                            if (j == 3)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                areas[i].worldPosition.x - (pointSize / 4),
                                areas[i].worldPosition.y,
                                areas[i].worldPosition.z + (pointSize / 4)
                                );
                            break;
                        #endregion

                        #region Two Corners
                        case 3:
                            if (j == 2 || j == 3)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].cornerPoints[j].z
                                    );
                            break;
                        case 6:
                            if (j == 3 || j == 0)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].cornerPoints[j].x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            break;
                        case 9:
                            if (j == 1 || j == 2)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].cornerPoints[j].x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            break;
                        case 12:
                            if (j == 0 || j == 1)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].cornerPoints[j].z
                                    );
                            break;
                        #endregion

                        #region Two Corners Diagonaly
                        case 5:
                            break;
                        case 10:
                            break;
                        #endregion

                        #region Three Corners
                        case 1:
                            if (j == 1)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].cornerPoints[j].x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            else if (j == 2)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            else if (j == 3)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].cornerPoints[j].z
                                    );
                            break;
                        case 2:
                            if (j == 2)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].cornerPoints[j].z
                                    );
                            else if (j == 3)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            else if (j == 0)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                        areas[i].cornerPoints[j].x,
                                        areas[i].worldPosition.y,
                                        areas[i].worldPosition.z
                                        );
                            break;
                        case 4:
                            if (j == 3)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                        areas[i].cornerPoints[j].x,
                                        areas[i].worldPosition.y,
                                        areas[i].worldPosition.z
                                        );
                            else if (j == 0)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            else if (j == 1)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                        areas[i].worldPosition.x,
                                        areas[i].worldPosition.y,
                                        areas[i].cornerPoints[j].z
                                        );
                            break;
                        case 8:
                            if (j == 0)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].cornerPoints[j].z
                                    );
                            else if (j == 1)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                    areas[i].worldPosition.x,
                                    areas[i].worldPosition.y,
                                    areas[i].worldPosition.z
                                    );
                            else if (j == 2)
                                verticesl[verticesl.Count - 1] = new Vector3(
                                        areas[i].cornerPoints[j].x,
                                        areas[i].worldPosition.y,
                                        areas[i].worldPosition.z
                                        );
                            break;
                        #endregion
                    }

                    count++;
                }

                trianglesl.AddRange(AddTriangles(verticesl));
                uvl.AddRange(AddTUVs(verticesl));
            }
            /**/
            for (int i = 0; i < verticesl.Count; i++)
            {
                colorl.Add(new Color(1,1,1,0));
            }

            mesh.vertices = verticesl.ToArray();
            mesh.triangles = trianglesl.ToArray();
            mesh.colors = colorl.ToArray();

            mesh.uv = uvl.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            transform.localPosition += Vector3.up * 0.3f;
        }

        protected List<int> AddTriangles(List<Vector3> vertices)
        {
            var tris = new List<int>();
            var lastvert = vertices.Count - 1;

            tris.Add(lastvert - 3);
            tris.Add(lastvert - 2);
            tris.Add(lastvert - 1);
            tris.Add(lastvert - 1);
            tris.Add(lastvert);
            tris.Add(lastvert - 3);

            return tris;
        }
        protected List<Vector2> AddTUVs(List<Vector3> vertices)
        {
            var uvs = new List<Vector2>();

            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));

            return uvs;
        }

        public void PaintAera(List<GridPoint> area, Group group)
        {
            PaintAera(area, group.Color);
        }
        public void PaintAera(List<GridPoint> area, int groupid)
        {
            if(groupsInfo.Groups.Exists(x => x.Id == groupid))
                PaintAera(area, groupsInfo.Groups.Find(x => x.Id == groupid).Color);
            else
                Debug.LogError("Grupo não encontrado, " + groupid);
        }
        public void PaintAera(List<GridPoint> area, Color color)
        {
            if (area == null) return;

            StartCoroutine(AnimatePaintingArea(area, color, 0.3f));

            /*
            var colors = mesh.colors;
            for (int i = 0, n= area.Count; i < n; i++)
            {
                if (assoc.Exists(x => Vector2.Distance(x.localPos, area[i].localPosition) < 0.1f))
                {
                    var square = assoc.Find(x => Vector2.Distance(x.localPos, area[i].localPosition) < 0.1f);

                    area[i].owned = 1;

                    for (int j = 0; j < square.vertices.Count; j++)
                    {
                        colors[square.vertices[j]] = color;
                        colors[square.vertices[j]].a = 1;
                    }
                }
            }

            mesh.colors = colors;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            /**/
        }

        public IEnumerator AnimatePaintingArea(List<GridPoint> area, Color color, float delay)
        {
            if (area == null) yield break;

            var colors = mesh.colors;

            for (int i = 0, n = area.Count; i < n; i++)
            {
                area[i].owned = 1;
            }

            for (int i = 0, n = area.Count; i < n; i++)
            {
                if (assoc.Exists(x => Vector2.Distance(x.localPos, area[i].localPosition) < 0.1f))
                {
                    var square = assoc.Find(x => Vector2.Distance(x.localPos, area[i].localPosition) < 0.1f);

                    area[i].owned = 1;

                    for (int j = 0; j < square.vertices.Count; j++)
                    {
                        colors[square.vertices[j]] = color;
                        colors[square.vertices[j]].a = 1;
                    }

                    mesh.colors = colors;
                    //sfxController.PlaySound("ConquerSquare");
                    //yield return new WaitForSeconds(delay);
                    yield return null;
                }
            }

            mesh.colors = colors;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }

    public class VerticeGripPointAssoc
    {
        public Vector2 localPos;
        public List<int> vertices;
    }

}