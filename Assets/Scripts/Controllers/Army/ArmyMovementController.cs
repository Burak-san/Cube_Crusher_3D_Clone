using UnityEngine;

namespace Controllers
{
    public class ArmyMovementController: MonoBehaviour
    {

        [SerializeField] private new Rigidbody rigidbody;
        private float ForwardSpeed =3f;
        public void Move()
        {
            rigidbody.velocity = new Vector3(0, 0,ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}