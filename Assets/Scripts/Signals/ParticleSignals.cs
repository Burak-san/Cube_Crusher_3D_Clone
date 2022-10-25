using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class ParticleSignals : MonoSingleton<ParticleSignals>
    {
        public UnityAction<Vector3> onHitEnemyCube = delegate {  };
        public UnityAction<Vector3> onHitEnemyBaseCube = delegate {  };
    }
}