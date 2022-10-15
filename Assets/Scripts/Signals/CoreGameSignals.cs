using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals: MonoSingleton<CoreGameSignals>
    {
        public UnityAction onGameOpen = delegate {  };
        public UnityAction onGameClose = delegate {  };
        public UnityAction onPlay = delegate {  };
        public UnityAction<GameStates> onChangeGameState = delegate {  };
        public UnityAction onReset = delegate {  };
        public UnityAction onApplicationQuit = delegate {  };
    }
}