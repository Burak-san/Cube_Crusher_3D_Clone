using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public Tile GridCellPrefab;

        #endregion

        #region Serialized Variables

        [SerializeField]private int height;

        [SerializeField]private int width;
    
        [SerializeField]private Transform cellHolder;

        [field: SerializeField] public List<BaseCube> BaseCubeList { get; set; }

        #endregion

        #region Private Variables

        public Tile[,] _nodes;
        
        #endregion

        #endregion
    
        private void Awake()
        {
            CreateGrid();
        }
        private void CreateGrid()
        {
            _nodes = new Tile[width, height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPosition = new Vector3(x, 0, z);
                    
                    Tile newTile = Instantiate(GridCellPrefab, worldPosition, Quaternion.identity);
                    newTile.name =  $"Cell {z} {x}";
                    newTile.transform.SetParent(cellHolder);
                    newTile.Init(true, new Vector2Int(x,z));
                    _nodes[x, z] = newTile;
                }
            }
        }
    }
}
