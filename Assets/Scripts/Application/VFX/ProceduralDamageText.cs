using TMPro;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class ProceduralDamageText : MonoBehaviour
    {
        public Color textColor;
        public Vector3 initialOffset, finalOffset;
        public float fadeDuration;
        private Vector3 initialPosition, finalPosition;
        private float fadeStartTime;
        void Start()
        {
            fadeStartTime = Time.time;
            initialPosition = transform.position + initialOffset;
            finalPosition = transform.position + finalOffset;
        }

        void Update()
        {
            float progress = (Time.time - fadeStartTime) / fadeDuration;
            if (progress <= 1)
            {
                transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, progress);
                gameObject.GetComponent<TextMeshPro>().color = Color.Lerp(new Color(textColor.r, textColor.g, textColor.b, 1), new Color(textColor.r, textColor.g, textColor.b, 0), progress);
            }
            else Destroy(gameObject);
        }
    }
}
