using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Application.VFX
{
    public class FloatingTorchJiggle : MonoBehaviour
    {
        public Time lerpDuration;
        public float lerpDistance;

        private Vector3 startPosition;
        private Vector3 randomizedEndPosition;

        //void Start()
        //{
        //    startPosition = transform.position;
        //}

        //void Update()
        //{
        //    float progress = (Time.time - fadeStartTime) / fadeDuration;
        //}
    }
}
