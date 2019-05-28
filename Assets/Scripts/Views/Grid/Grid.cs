using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InterativaSystem.Views.Grid
{
    public enum GridType
    {
        Quad,
        Hex
    }

    public class Grid
    {
        public List<GridPoint> Points;
        public float PointSize;
        public Vector3 GridSize;

        #region Creation
        public virtual void CreateGrid(Vector3 size, Vector3 position, int quantity)
        {
            GridSize = size;
        }
        public virtual void CreateGrid(Vector3 size, Vector3 position, int quantity, bool useHeight, LayerMask mask) { }
        #endregion

        #region Debug
        public virtual void PrintGrid() { }
        public virtual void PrintGrid(Color color) { }
        public virtual void PrintAreaGrid(List<GridPoint> area) { }
        #endregion

        #region Area AndPosition
        public virtual GridPoint GetClosestGridPoint(GridPoint point)
        {
            var closer = Points.OrderBy(a => Vector2.Distance(a.localPosition, point.localPosition)).ToList();
            
            return closer.Find(x => x.isValid && x.owned < 0);
        }

        public virtual GridPoint GetGridPoint(Vector2 localPoint)
        {
            if (Points.Exists(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f))
            {
                return Points.Find(x => Vector2.Distance(x.localPosition, localPoint) < 0.1f);
            }
            return null;
        }
        public virtual GridPoint GetGridPoint(Vector3 worldPosition)
        {
            return GetGridPoint(worldPosition, 0.1f);
        }
        public virtual GridPoint GetGridPoint(Vector3 worldPosition, float distance)
        {
            if (Points.Exists(x => Vector3.Distance(x.worldPosition, worldPosition) < distance))
            {
                return Points.Find(x => Vector3.Distance(x.worldPosition, worldPosition) < distance);
            }
            return null;
        }

        public virtual List<GridPoint> GetValidGridPoints()
        {
            return GetValidGridPoints(false);
        }
        public virtual List<GridPoint> GetValidGridPoints(bool checkOwn)
        {
            if(checkOwn)
                return Points.FindAll(x => x.isValid && x.owned<0);

            return Points.FindAll(x => x.isValid);
        }

        public virtual float GetDefaultSize() { return PointSize; }

        public virtual List<GridPoint> ExpandArea(int area, GridPoint point) { return null; }
        public virtual List<GridPoint> ExpandArea(int area, Vector2 localPoint) { return null; }
        public virtual List<GridPoint> GetNeighbours(Vector2 localPoint) { return null; }
        public virtual List<GridPoint> GetNeighbours(GridPoint point) { return null; }
        public virtual List<GridPoint> GetNeighbours(int distance, Vector2 localPoint) { return null; }
        public virtual List<GridPoint> GetNeighbours(int distance, GridPoint localPoint) { return null; }

        #endregion

        public virtual int CountValid()
        {
            return CountValid(false);
        }
        public virtual int CountValid(bool checkOwn)
        {
            if(checkOwn)
                return Points.FindAll(x => x.isValid && x.owned<0).Count;

            return Points.FindAll(x => x.isValid).Count;
        }
    }

    public class GridPoint
    {
        public Vector2 localPosition;
        public Vector3 worldPosition;
        public Vector3[] cornerPoints;
        public int usableCorners;

        public GenericView objScript;

        public int weight;
        public int owned;

        public bool isValid;
    }
}