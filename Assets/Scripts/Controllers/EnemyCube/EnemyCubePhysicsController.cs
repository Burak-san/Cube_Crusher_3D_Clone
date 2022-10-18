using System;
using Controllers.Cube;
using UnityEngine;

namespace Controllers.EnemyCube
{
    public class EnemyCubePhysicsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IncrementCubes _))
            {
                Destroy(other.gameObject);
                
            }
        }
    }
}