using UnityEngine;
namespace Managers
{
    public class NodeManager : MonoBehaviour
    {
        public bool IsPlaceable;
        public Vector3 CellPosition;
        public Transform Obj;
        
        //NODUN GRİD ÜZERİNDEKİ İNT DEĞERLERİ
    
        public NodeManager(bool isPlaceable, Vector3 cellPosition, Transform obj){
            this.IsPlaceable = isPlaceable;
            this.CellPosition = cellPosition;
            this.Obj = obj;
        }
    }
}
