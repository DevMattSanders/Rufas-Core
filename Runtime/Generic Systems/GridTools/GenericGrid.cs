using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GenericGrid<TGridObject>
    {
        public int width;
        public int height;
        public int depth;
        public float cellSize;
        Vector3 originPosition;
        private TGridObject[,,] gridArray;


        public GenericGrid(int width, int height, int depth, float cellSize, Vector3 originPosition, Transform parentTransform)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height, depth];



        }

        #region Get Position

        public Vector3 GetWorldPositionFromGridPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * cellSize + originPosition;
        }

        public Vector3Int GetGridPositionFromWorldPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
            int z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

            return new Vector3Int(x, y, z);
        }

        #endregion

        #region Get & Set Values

        public void SetValueFromGridPosition(int x, int y, int z, TGridObject value)
        {
            if (x >= 0 && y >= 0 && y >= 0 && x < width && y < height && z < depth)
            {
                gridArray[x, y, z] = value;
                //textArray[x,y, z].text = value.ToString();
            }
        }

        public void SetValueFromWorldPosition(Vector3 worldPosition, TGridObject value)
        {
            Vector3Int gridPosition = GetGridPositionFromWorldPosition(worldPosition);
            SetValueFromGridPosition(gridPosition.x, gridPosition.y, gridPosition.z, value);
        }

        public TGridObject GetCellFromGridPosition(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < depth)
            {
                return gridArray[x, y, z];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetValueFromWorldPosition(Vector3 worldPosition)
        {
            Vector3Int gridPosition = GetGridPositionFromWorldPosition(worldPosition);
            return GetCellFromGridPosition(gridPosition.x, gridPosition.y, gridPosition.z);
        }

        #endregion


    }
}
