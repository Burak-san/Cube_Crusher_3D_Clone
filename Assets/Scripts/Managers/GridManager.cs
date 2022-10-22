using System.Collections.Generic;
using Controllers.Cube;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public Tile[,] Nodes;
        public List<BaseCube> BaseCubeList;
        
        [SerializeField]private int height;
        [SerializeField]private int width;
        [SerializeField]private Transform cellHolder;
        [SerializeField]private Tile gridCellPrefab;
        [SerializeField]private BaseCube baseCubePrefab;
        
        private ObjectPooler _objectPooler;
        private void Awake()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
            CreateGrid();
            BaseCubeSpawn();
        }
        private void CreateGrid()
        {
            Nodes = new Tile[width, height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPosition = new Vector3(x, 0, z);
                    
                    Tile newTile = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                    newTile.name =  $"Cell {z} {x}";
                    newTile.transform.SetParent(cellHolder);
                    if (z > 4)
                    {
                        newTile.Init(false,true,false, new Vector2Int(x,z));
                        newTile.GetComponentInChildren<Renderer>().material.color = newTile.enemySideMaterial.color;
                    }
                    else
                    {
                        newTile.Init(true,false,false, new Vector2Int(x,z));
                    }
                    
                    Nodes[x, z] = newTile;
                }
            }
        }

        private void BaseCubeSpawn()
        {
            for (int i = 0; i < Nodes.GetLength(0); i++)
            {
                BaseCube BaseCube = Instantiate(baseCubePrefab, Nodes[i,0].transform.position, Quaternion.identity);
                
                BaseCube.BaseCubeTilePosition = new Vector2Int(i, 0);
                Nodes[i, 0].HeldCube = BaseCube;
                Nodes[i, 0].IsPlaceable = false;
                Nodes[i, 0].IsBaseTile = true;
                Nodes[i, 0].SnapPoint();
                BaseCube.transform.SetParent(Nodes[i, 0].gameObject.transform);
                BaseCubeList.Add(BaseCube);
            }
        }
    }
}
