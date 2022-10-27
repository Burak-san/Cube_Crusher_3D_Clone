using UnityEngine;

namespace Controllers.Army
{
    public class ArmyAnimationController : MonoBehaviour
    {
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _animator.Play("Running");
        }
    }
}

