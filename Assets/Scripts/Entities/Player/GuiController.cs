using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class GuiController : MonoBehaviour
    {
        public Texture2D Crosshair;
        public Texture2D CrosshairActive;
        public Font Font;
        public int FontSize;
        private bool IsCrosshairActive = false;
        private string ItemUnderCrosshair;
        private string ItemInLeftHand;
        private string ItemInRightHand;
        private EquippedItems EquippedItems;
        private GUIStyle fontStyle = new GUIStyle();

        void Start()
        {
            EquippedItems = GetComponent<EquippedItems>();
            fontStyle.font = Font;
            fontStyle.normal.textColor = Color.white;
            fontStyle.fontSize = FontSize;
        }

        void Update()
        {
            var itemUnderCrosshair = GetItemUnderCrosshair();

            ItemInLeftHand = EquippedItems.leftHand != null ? EquippedItems.leftHand.name : "Empty";
            ItemInRightHand = EquippedItems.rightHand != null ? EquippedItems.rightHand.name : "Empty";

            IsCrosshairActive = itemUnderCrosshair != "";
            ItemUnderCrosshair = itemUnderCrosshair;
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, Crosshair.width + 25, Crosshair.height + 25), IsCrosshairActive ? CrosshairActive : Crosshair);
            GUI.Label(new Rect(Screen.width / 2 + 15, Screen.height / 2 + 75, 100, 50), ItemUnderCrosshair.Length != 0 ? ItemUnderCrosshair : "", fontStyle);
            GUI.Label(new Rect(100, Screen.height - 50, 100, 50), ItemInLeftHand.Replace("(Clone)", ""), fontStyle);
            GUI.Label(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), ItemInRightHand.Replace("(Clone)", ""), fontStyle);
        }

        private string GetItemUnderCrosshair()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10.0f))
            {
                if (hit.collider != null && hit.collider.gameObject.GetComponent<IItem>() != null)
                {
                    return hit.collider.GetComponent<IItem>().Name;
                }
            }
            return "";
        }
    }
}
