using System;
using UnityEngine;

namespace Controllers.EnemyCube
{
    public class EnemyCubePhysicsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FriendCube"))
            {
                Destroy(other.gameObject);
                Debug.Log("HitFriendCube");
            }
        }
    }
}