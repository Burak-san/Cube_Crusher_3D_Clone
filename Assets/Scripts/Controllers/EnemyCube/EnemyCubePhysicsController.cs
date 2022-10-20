using System;
using Controllers.Cube;
using Managers;
using UnityEngine;

namespace Controllers.EnemyCube
{
    public class EnemyCubePhysicsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Army"))
            {
            }
            
        }
    }
}