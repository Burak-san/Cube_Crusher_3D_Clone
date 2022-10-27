using System.Threading.Tasks;
using Controllers.Cube;
using Data.ValueObject;
using Managers;
using Signals;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers.Army
{
    public class ArmyPhysicsController : MonoBehaviour
    {
        private ArmyManager _armyManager;
        private MoneyData _moneyData;
        [SerializeField] private GameObject colorParticle;
        [SerializeField] private GameObject coinParticle;

        private void Awake()
        {
            _armyManager = FindObjectOfType<ArmyManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyCube"))
            {
              //  GameObject ColorParticle =
               Instantiate(colorParticle, transform.position + new Vector3(0, 0, -0.25f),
                    Quaternion.identity);
                //ColorParticle.transform.SetParent(transform);
                EnemyCubeSignals.Instance.onHitEnemyCube?.Invoke(other.transform);
                _armyManager.ReturnToPoolArmy(gameObject);
                _armyManager.ArmyCheck();
            }


            if (other.CompareTag("EnemyBase"))
            {
              //  GameObject CoinParticle = 
                    Instantiate(coinParticle, transform.position + new Vector3(0, 0, -0.25f),
                    Quaternion.identity);
              //  CoinParticle.transform.SetParent(transform);
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