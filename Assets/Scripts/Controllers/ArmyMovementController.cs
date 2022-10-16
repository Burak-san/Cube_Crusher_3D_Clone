using UnityEngine;

namespace Controllers
{
    public class ArmyMovementController: MonoBehaviour
    {

        [SerializeField] private new Rigidbody rigidbody;
        public float ForwardSpeed = 1f;
        public void Move()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y,ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}