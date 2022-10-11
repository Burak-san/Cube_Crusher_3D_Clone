using System;
using Controllers;
using Managers;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public Transform GridCellPrefab;

        #endregion

        #region Serialized Variables

        [SerializeField]private int height;

        [SerializeField]private int width;
    
        [SerializeField]private Transform cellHolder;
        

        #endregion

        #region Private Variables

        public NodeManager[,] _nodes;
        
        #endregion

        #endregion
    
        private void Awake()
        {
            CreateGrid();
        }
        private void CreateGrid()
        {
            _nodes = new NodeManager[height, width];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPosition = new Vector3(x, 0, z);
                    Transform obj = Instantiate(GridCellPrefab, worldPosition, Quaternion.identity);
                    obj.name = $"Cell {z} {x}";
                    obj.SetParent(cellHolder);
                    _nodes[z, x] = new NodeManager(true, worldPosition, obj);
                }
            }
        }
        
        
        
    }
}
