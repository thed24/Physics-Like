using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class HudController : MonoBehaviour
    {
        public Texture2D Crosshair;
        public Texture2D CrosshairActive;
        public Font Font;
        public int FontSize;
        public HandController EquippedItems;
        private bool IsCrosshairActive = false;
        private string ItemUnderCrosshair;
        private string ItemInLeftHand;
        private string ItemInRightHand;
        private GUIStyle fontStyle = new GUIStyle();

        void Start()
        {
            fontStyle.font = Font;
            fontStyle.normal.textColor = Color.white;
            fontStyle.fontSize = FontSize;
        }

        void Update()
        {
            var leftHandItem = EquippedItems.player.Equipped.SeeItemAtSlot(0);
            var rightHandItem = EquippedItems.player.Equipped.SeeItemAtSlot(1);

            ItemInLeftHand = leftHandItem.HasValue ? leftHandItem.Value.Name : "Empty";
            ItemInRightHand = rightHandItem.HasValue ? rightHandItem.Value.Name : "Empty";

            var itemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<IInteractable>();
            if (itemUnderCrosshair.HasValue)
            {
                var name = itemUnderCrosshair.Value.GetComponent<IInteractable>().Name;
                IsCrosshairActive = name != "";
                ItemUnderCrosshair = name;
            }
            else
            {
                IsCrosshairActive = false;
                ItemUnderCrosshair = "";
            }
        }

        void OnGUI()
        {
            if (Cursor.lockState == CursorLockMode.None)
                return;

            // Crosshair
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, Crosshair.width + 25, Crosshair.height + 25), IsCrosshairActive ? CrosshairActive : Crosshair);
            GUI.Label(new Rect(Screen.width / 2 + 20, Screen.height / 2 + 75, 100, 50), ItemUnderCrosshair.Length != 0 ? ItemUnderCrosshair : "", fontStyle);

            // Equipped items
            GUI.Label(new Rect(100, Screen.height - 50, 100, 50), ItemInLeftHand.Replace("(Clone)", ""), fontStyle);
            GUI.Label(new Rect(Screen.width - 200, Screen.height - 50, 100, 50), ItemInRightHand.Replace("(Clone)", ""), fontStyle);

            // Health and Mana
            GUI.Label(new Rect(100, Screen.height - 75, 100, 50), $"Mana: {EquippedItems.player.Mana} / {EquippedItems.player.MaxMana}", fontStyle);
            GUI.Label(new Rect(Screen.width - 200, Screen.height - 75, 100, 50), $"Health: {EquippedItems.player.Health} / {EquippedItems.player.MaxHealth}", fontStyle);
        }
    }
}
