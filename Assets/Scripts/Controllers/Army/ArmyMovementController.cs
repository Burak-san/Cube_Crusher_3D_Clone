using UnityEngine;

namespace Controllers.Army
{
    public class ArmyMovementController: MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField] private new Rigidbody rigidbody;
        
        #endregion

        #region Private Variables

        private float ForwardSpeed =3f;
        
        #endregion

        #endregion
        
        
        public void Move()
        {
            rigidbody.velocity = new Vector3(0, 0,ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}