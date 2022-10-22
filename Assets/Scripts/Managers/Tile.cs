using System.Collections.Generic;
using Controllers;
using Controllers.Cube;
using UnityEngine;
namespace Managers
{
    public class Tile : MonoBehaviour
    {
        public bool IsPlaceable;
        public bool IsEnemyTile;
        public bool IsBaseTile;
        public Vector2Int CellIndex;
        public Material enemySideMaterial;


        public Cube HeldCube;
        
        [SerializeField] public Vector3 OffSetVector;

        public void Init(bool isPlaceable,bool isEnemyTile,bool isBaseTile, Vector2Int cellIndex){
            this.IsPlaceable = isPlaceable;
            this.CellIndex = cellIndex;
            this.IsEnemyTile = isEnemyTile;
            this.IsBaseTile = isBaseTile;
        }
        
        public void SnapPoint()
        {
            HeldCube.transform.position = transform.position + OffSetVector;
        }
    }
}
