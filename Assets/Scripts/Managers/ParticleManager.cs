using System;
using System.Threading.Tasks;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        private ObjectPooler _objectPooler;
        private string particlePoolTag;

        private void Awake()
        {
            _objectPooler = FindObjectOfType<ObjectPooler>();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            
        }
        private void SubscribeEvents()
        {
            ParticleSignals.Instance.onHitEnemyCube += OnHitEnemyCube;
            ParticleSignals.Instance.onHitEnemyBaseCube += OnHitEnemyBaseCube;
        }
        private void UnSubscribeEvents()
        {
            ParticleSignals.Instance.onHitEnemyCube -= OnHitEnemyCube;
            ParticleSignals.Instance.onHitEnemyBaseCube -= OnHitEnemyBaseCube;
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        private void OnHitEnemyCube(Vector3 transformHit)
        {
            particlePoolTag = "ArmyHitEnemyCubeParticle";
            GetEnemyCubeHitParticleFromPool(transformHit,particlePoolTag);
        }

        private void OnHitEnemyBaseCube(Vector3 transformHit)
        {
            particlePoolTag = "ArmyHitEnemyBaseParticle";
            GetEnemyCubeHitParticleFromPool(transformHit,particlePoolTag);
        }

        private void GetEnemyCubeHitParticleFromPool(Vector3 _transform,string particlePoolTag)
        {   
            ParticleSystem Particle = _objectPooler.SpawnFromPool(particlePoolTag, _transform + new Vector3(0,0,-.75f), Quaternion.identity,transform).GetComponent<ParticleSystem>();
            Particle.Play();
            ReturnToPool(Particle,particlePoolTag);
        }
        
        public async void ReturnToPool(ParticleSystem particle, string particlePoolTag)
        {
            await Task.Delay(250);
            _objectPooler.ReturnToPool(particlePoolTag,particle.gameObject);
        }
    }
}