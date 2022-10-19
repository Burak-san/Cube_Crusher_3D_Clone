using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.EnemyCube
{
    public class EnemyCubeMeshController : MonoBehaviour
    {
        private float _scale;
        private void OnEnable()
        {
            GetRandomScale();
            GetMaterial();
        }

        private void GetMaterial()
        {
            
        }
        
        private void GetRandomScale()
        {
            float tempScale = Random.Range(0.8f, 1.6f);
            _scale = tempScale;
            transform.DOScaleY(tempScale, 3f).SetEase(Ease.OutElastic);
        }
    }
}