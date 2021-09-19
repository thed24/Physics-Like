using System.Linq;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class GuiController : MonoBehaviour
    {
        public Texture2D Crosshair;
        public Texture2D CrosshairActive;
        public Font Font;
        public int FontSize;
        private int hotBarIndex = 0;
        private bool IsCrosshairActive = false;
        private string ItemUnderCrosshair;
        private string ItemInLeftHand;
        private string ItemInRightHand;
        private EquippedItems EquippedItems;
        private Inventory Inventory;
        private GUIContent itemContent = new GUIContent();
        private GUIStyle fontStyle = new GUIStyle();

        void Start()
        {
            Inventory = GetComponent<Inventory>();
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

            var scrollPosistion = Input.GetAxis("Mouse ScrollWheel");
            if (scrollPosistion > 0f && hotBarIndex < Inventory.GetItems().Count - 1)
            {
                hotBarIndex++;
            }
            else if (scrollPosistion < 0f && hotBarIndex > 0)
            {
                hotBarIndex--;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var itemToRemove = Inventory.RetrieveItemAt(hotBarIndex);
                if (itemToRemove != null){
                    itemToRemove.GetComponent<IHoldable>().OnDrop();
                }
            }
        }

        void OnGUI()
        {
            // Crosshair
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, Crosshair.width + 25, Crosshair.height + 25), IsCrosshairActive ? CrosshairActive : Crosshair);
            GUI.Label(new Rect(Screen.width / 2 + 20, Screen.height / 2 + 75, 100, 50), ItemUnderCrosshair.Length != 0 ? ItemUnderCrosshair : "", fontStyle);
            GUI.Label(new Rect(100, Screen.height - 50, 100, 50), ItemInLeftHand.Replace("(Clone)", ""), fontStyle);
            GUI.Label(new Rect(Screen.width - 200, Screen.height - 50, 100, 50), ItemInRightHand.Replace("(Clone)", ""), fontStyle);

            // Hotbar
            Color restoreColor = GUI.contentColor;
            GUILayout.BeginArea(new Rect(400, Screen.height - 50, Screen.width - 800, 150));
            GUILayout.BeginHorizontal();

            for(int i = 0; i < 10; i++) {
                var item = Inventory.GetItems().ElementAtOrDefault(i);
                if (item == null) {
                    GUI.enabled = false;
                    GUILayout.Button(GUIContent.none);
                    GUI.enabled = true;
                }
                else {
                    itemContent.image = item.Icon;
                    itemContent.text = item.Name;
                    GUI.contentColor = (item == Inventory.GetItems().ElementAtOrDefault(hotBarIndex))
                     ? Color.red
                     : restoreColor;
                    if (GUILayout.Button(itemContent))
                        Inventory.RetrieveItemAt(hotBarIndex);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUI.contentColor = restoreColor;
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
