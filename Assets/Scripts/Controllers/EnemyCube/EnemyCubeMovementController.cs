using System.Collections;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers.EnemyCube
{
    public class EnemyCubeMovementController : MonoBehaviour
    {
        public void Move()
        {
            transform.DOMove(transform.position + Vector3.back, 1, false);
        }
    }
}    