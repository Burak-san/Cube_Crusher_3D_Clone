using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class EnemyCubeSignals : MonoSingleton<EnemyCubeSignals>
    {
        public UnityAction<Transform> onHitEnemyCube = delegate {  };
    }
}