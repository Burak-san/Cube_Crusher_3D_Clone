using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Controllers.EnemyCube
{
    public class EnemyCubeMovementController : MonoBehaviour
    {

        public void EnemyMove()
        {
            transform.DOMove(Vector3.back, .5f, false);
        }
    }
}

