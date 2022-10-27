using System.Collections.Generic;
using Controllers.Cube;
using UnityEngine;
using Data.ValueObject;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public TileData[,] Nodes;
        public List<BaseCube> BaseCubeList;
        
        #endregion

        #region Serialized Variables

        [SerializeField]private int height;
        [SerializeField]private int width;
        [SerializeField]private Transform cellHolder;
        [SerializeField]private TileData gridCellPrefab;
        [SerializeField]private BaseCube baseCubePrefab;

        #endregion

        #region Private Variables

        private ObjectPooler _objectPooler;

        #endregion

        #endregion
        private void Awake()
        {
            GetData();
            CreateGrid();
            BaseCubeSpawn();
        }

        private void GetData()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
        }
        
        private void CreateGrid()
        {
            Nodes = new TileData[width, height];
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPosition = new Vector3(x, 0, z);
                    
                    TileData newTileData = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                    newTileData.name =  $"Cell {z} {x}";
                    newTileData.transform.SetParent(cellHolder);
                    if (z > 4)
                    {
                        newTileData.Init(false,true,false, new Vector2Int(x,z));
                        newTileData.GetComponentInChildren<Renderer>().material.color = newTileData.EnemySideMaterial.color;
                    }
                    else
                    {
                        newTileData.Init(true,false,false, new Vector2Int(x,z));
                    }
                    
                    Nodes[x, z] = newTileData;
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
