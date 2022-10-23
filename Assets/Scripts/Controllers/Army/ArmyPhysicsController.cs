using System.Threading.Tasks;
using Controllers.Cube;
using Data.ValueObject;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers.Army
{
    public class ArmyPhysicsController : MonoBehaviour
    {
        private ArmyManager _armyManager;
        private MoneyData _moneyData;
        
        private void Awake()
        {
            _armyManager = FindObjectOfType<ArmyManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyCube"))
            {
                EnemyCubeSignals.Instance.onHitEnemyCube?.Invoke(other.transform);
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
            }
            

            if (other.CompareTag("EnemyBase"))
            {
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
                UISignals.Instance.onSetCoinText?.Invoke();
            }
            
            if (other.TryGetComponent(out IncrementCubes ıncrementCubes))
            {
                StartCoroutine(_armyManager.SpawnArmyInIncrementCube(ıncrementCubes));
                ColliderEnabled(other);
            }
        }

        private async void ColliderEnabled(Collider collider)
        {
            collider.enabled = false;
            await Task.Delay(5000);
            if (collider != null)
            {
                collider.enabled = true;
            }
            
        }
    }
}