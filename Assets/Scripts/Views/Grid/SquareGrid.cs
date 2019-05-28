using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InterativaSystem.Views.Grid
{
    public class SquareGrid : Grid
    {
        Vector3[] directions = new Vector3[]
        {
            new Vector3(1,0,0),
            new Vector3(0,0,-1),
            new Vector3(-1,0,0),
            new Vector3(0,0,1),
        };
        Vector2[] localDirections = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(0,-1),
            new Vector2(-1,0),
            new Vector2(0,1),
        };
        Vector3[] quadPoints = new Vector3[]
        {
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),
            new Vector3(-1,0,1),
        };

        #region internal Methods
        protected float GetQuadSize(Vector3 area, int quantity)
        {
            var A = area.x;
            var B = area.z;
            //A -= A % B;

            float size = (Mathf.Sqrt((A * B) / quantity));

            return size;
        }

        protected void PrintQuad(Vector3 center, Color color)
        {
            var points = new Vector3[directions.Length];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Vector3(
                    center.x + quadPoints[i].x * (PointSize/2), 
                    center.y, 
                    center.z + quadPoints[i].z * (PointSize/2)
                    );
            }

            Debug.DrawLine(points[0], points[1], color, 5);
            Debug.DrawLine(points[1], points[2], color, 5);
            Debug.DrawLine(points[2], points[3], color, 5);
            Debug.DrawLine(points[3], points[0], color, 5);
            Debug.DrawLine(points[0], points[2], color, 5);
        }
        protected void PrintQuad(GridPoint center, Color color)
        {
            Debug.DrawLine(center.cornerPoints[0], center.cornerPoints[1], color, 5);
            Debug.DrawLine(center.cornerPoints[1], center.cornerPoints[2], color, 5);
            Debug.DrawLine(center.cornerPoints[2], center.cornerPoints[3], color, 5);
            Debug.DrawLine(center.cornerPoints[3], center.cornerPoints[0], color, 5);
            Debug.DrawLine(center.cornerPoints[0], center.cornerPoints[2], color, 5);
        }

        #endregion

        public override void CreateGrid(Vector3 size, Vector3 position, int quantity)
        {
            CreateGrid(size, position, quantity, false, new LayerMask());
        }
        public override void CreateGrid(Vector3 size, Vector3 position, int quantity, bool useHeight, LayerMask mask)
        {
            base.CreateGrid(size, position, quantity);

            if(quantity<0) return;

            PointSize = GetQuadSize(size, quantity);

            var x = (size.x/ PointSize);
            var y = (size.z/ PointSize);

            Points =  new List<GridPoint>();

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var point = new Vector2(i,j);

                    var height = position.y + (size.y/2);
                    var corners = new Vector3[directions.Length];
                    var isValid = true;
                    var weight = 1;
                    var owned = -1;
                    var usableCorners = 15;

                    var pos = new Vector3(
                        position.x - (size.x / 2) + (PointSize / 2) + (i * PointSize),
                        height,
                        position.z - (size.z / 2) + (PointSize / 2) + (j * PointSize)
                        );

                    #region Use Height
                    if (useHeight)
                    {
                        RaycastHit hit;
                        var ray = new Ray(pos, Vector3.down);
                        if (Physics.Raycast(ray, out hit, size.y, mask))
                        {
                            height = hit.point.y;
                        }
                        else
                        {
                            isValid = false;
                            weight = 1000;
                        }

                        #region Get Corners
                        usableCorners = 0;
                        for (int k = 0; k < corners.Length; k++)
                        {
                            var xPos = pos.x + quadPoints[k].x * (PointSize / 2);
                            var yPos = position.y + (size.y / 2);
                            var zPos = pos.z + quadPoints[k].z * (PointSize / 2);

                            ray = new Ray(new Vector3(xPos, yPos, zPos), Vector3.down);

                            if (Physics.Raycast(ray, out hit, size.y, mask))
                            {
                                yPos = hit.point.y;

                                switch (k)
                                {
                                    case 0:
                                        usableCorners += 1;
                                        break;
                                    case 1:
                                        usableCorners += 2;
                                        break;
                                    case 2:
                                        usableCorners += 4;
                                        break;
                                    case 3:
                                        usableCorners += 8;
                                        break;
                                }
                            }
                            else
                            {
                                yPos = height;
                            }

                            corners[k] = new Vector3(xPos, yPos, zPos);
                        }
                        #endregion
                    }
                    #endregion
                    else
                    {
                        #region Get Corners
                        for (int k = 0; k < corners.Length; k++)
                        {
                            corners[k] = new Vector3(
                                pos.x + quadPoints[k].x * (PointSize / 2),
                                pos.y,
                                pos.z + quadPoints[k].z * (PointSize / 2)
                                );
                        }
                        #endregion
                    }

                        pos.y = height;


                    Points.Add(new GridPoint
                    {
                        localPosition = point,
                        worldPosition = pos,
                        isValid = isValid,
                        weight = weight,
                        cornerPoints = corners,
                        usableCorners = usableCorners,
                        owned = owned
                    });
                }
            }
        }

        #region Print
        public override void PrintGrid()
        {
            PrintGrid(Color.green);
        }
        public override void PrintGrid(Color color)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if(!Points[i].isValid)
                    continue;

                PrintQuad(Points[i], color);
            }
        }
        public override void PrintAreaGrid(List<GridPoint> area)
        {
            if(area == null) return;

            for (int i = 0; i < area.Count; i++)
            {
                PrintQuad(area[i], Color.green);
            }
        }
        #endregion

        #region Neighbour
        public override List<GridPoint> ExpandArea(int area, GridPoint point)
        {
            return ExpandArea(area, point.localPosition);
        }
        public override List<GridPoint> ExpandArea(int area, Vector2 localPoint)
        {
            var list = new List<GridPoint>();

            if (!Points.Exists(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f))
                return null;
            if (!Points.Find(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f).isValid)
                return null;

            list.Add(GetGridPoint(localPoint));

            var count = 0;
            var lastList = 0;
            var loops = 0;
            while (list.Count < area)
            {
                //Check if has no more spaces
                if(CountValid(true) <= list.Count)
                    break;

                //Check if has no more space to grow
                if (loops != 0 && count >= lastList - 1)
                {
                    var closer = Points.FindAll(x=>x.isValid && x.owned<0 && !list.Exists(y=>y==x)).OrderBy(a => Vector2.Distance(a.localPosition, list.Last().localPosition)).ToList();

                    list.Add(closer[0]);
                }

                count = 0;
                lastList = list.Count;

                for (int j = 0; j < lastList; j++)
                {
                    for (int k = 0; k < localDirections.Length; k++)
                    {
                        var targetPos = list[j].localPosition + localDirections[k];

                        if (Vector2.Distance(localPoint, targetPos) < 0.1f)
                        {
                            count++;
                            continue;
                        }

                        if (list.Exists(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f))
                        {
                            count++;
                            continue;
                        }

                        if (Points.Exists(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f))
                        {
                            var point = Points.Find(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f);
                            if (point.isValid && point.owned < 0)
                                list.Add(point);
                            else
                                count++;

                        }
                        else
                        {
                            count ++;
                        }
                    }
                }

                loops++;
            }
            while (list.Count > area)
            {
                list.Remove(list.Last());
            }

            return list;
        }
        public override List<GridPoint> GetNeighbours(GridPoint point)
        {
            return GetNeighbours(point.localPosition);
        }
        public override List<GridPoint> GetNeighbours(Vector2 localPoint)
        {
            var list = new List<GridPoint>();

            if(!Points.Exists(x=>Vector2.Distance(x.localPosition, localPoint) < 0.1f))
                return null;
            if (!Points.Find(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f).isValid)
                return null;

            for (int i = 0; i < localDirections.Length; i++)
            {
                var targetPos = localPoint + localDirections[i];

                if (Points.Exists(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f))
                {
                    var point = Points.Find(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f);
                    if (point.isValid && point.owned < 0)
                        list.Add(point);
                }
            }

            return list;
        }
        public override List<GridPoint> GetNeighbours(int distance, GridPoint localPoint)
        {
            return GetNeighbours(distance, localPoint.localPosition);
        }
        public override List<GridPoint> GetNeighbours(int distance, Vector2 localPoint)
        {
            var list = new List<GridPoint>();

            if (!Points.Exists(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f))
                return null;
            if(!Points.Find(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f).isValid)
                return null;

            list = GetNeighbours(localPoint);
            
            for (int i = 1; i < distance; i++)
            {

                for (int j = 0, n= list.Count; j < n; j++)
                {
                    for (int k = 0; k < localDirections.Length; k++)
                    {
                        var targetPos = list[j].localPosition + localDirections[k];

                        if (Vector2.Distance(localPoint, targetPos) < 0.1f)
                            continue;
                        if (list.Exists(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f))
                            continue;

                        if (Points.Exists(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f))
                        {
                            var point = Points.Find(x => Vector2.Distance(x.localPosition, targetPos) < 0.1f);
                            if (point.isValid && point.owned < 0)
                                list.Add(point);
                        }
                    }
                }
            }

            //list = list.Distinct().ToList();
            return list;
        }
        #endregion
    }
}