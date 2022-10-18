using System.Collections.Generic;
using Controllers.Cube;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public Tile GridCellPrefab;
        public Tile[,] _nodes;
        
        [SerializeField]private int height;
        [SerializeField]private int width;
        [SerializeField]private Transform cellHolder;
        [field: SerializeField] public List<BaseCubeController> BaseCubeList { get; set; }


    
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
