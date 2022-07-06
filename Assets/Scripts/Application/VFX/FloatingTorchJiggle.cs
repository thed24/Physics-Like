using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Application.VFX
{
    public class FloatingTorchJiggle : MonoBehaviour
    {
        public float lerpDuration;
        public float lerpDistance;
        
        private float startTime { get; set; }
        private Vector3 startPosition { get; set; }
        private Vector3 randomizedEndPosition { get; set; }

        void Start()
        {
            startPosition = transform.position;
            startTime = Time.time;
            RandomizeEndPosition();
        }

        void Update()
        {
            float progress = (Time.time - startTime) / lerpDuration;

            if (progress <= 1)
            {
                transform.position = Vector3.Lerp(startPosition, randomizedEndPosition, progress);
            }
            if (progress > 1 && progress <= 2)
            {
                transform.position = Vector3.Lerp(randomizedEndPosition, startPosition, progress - 1);
            }
            if (progress > 2)
            {
                RandomizeEndPosition();
                startTime = Time.time;
            }
        }

        private void RandomizeEndPosition()
        {
            randomizedEndPosition = startPosition + new Vector3(Random.Range(-lerpDistance, lerpDistance), Random.Range(-lerpDistance, lerpDistance), Random.Range(-lerpDistance, lerpDistance));
        }
    }
}
