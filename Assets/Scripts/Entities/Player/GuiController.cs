using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class GuiController : MonoBehaviour
    {
        public Texture2D Crosshair;
        public Texture2D CrosshairActive;

        private bool IsCrosshairActive = false;
        private string ItemUnderCrosshair;

        void Start()
        {
            
        }

        void Update()
        {
            var itemUnderCrosshair = GetItemUnderCrosshair();

            IsCrosshairActive = itemUnderCrosshair != "";
            ItemUnderCrosshair = itemUnderCrosshair;
            
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, Crosshair.width + 25, Crosshair.height + 25), IsCrosshairActive ? CrosshairActive : Crosshair);
            GUI.Label(new Rect(Screen.width / 2 - 5, Screen.height / 2 + 75, 100, 50), ItemUnderCrosshair.Length != 0 ? ItemUnderCrosshair : "");
        }

        private string GetItemUnderCrosshair()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5.0f))
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Weapon") || hit.collider.gameObject.CompareTag("Item"))
                {
                    return hit.collider.GetComponent<Item>().Name;
                }
            }
            return "";
        }
    }
}
