using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class BaseCubeSignals : MonoSingleton<BaseCubeSignals>
    {
        public UnityAction onBaseCubePowerIncrease = delegate {  };
    }
}