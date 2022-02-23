﻿using UnityEngine;

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
            var itemUnderCrosshair = UnityExtensions.GetItemAtCrosshair<BaseItem>();

            ItemInLeftHand = EquippedItems.leftHand != null ? EquippedItems.leftHand.name : "Empty";
            ItemInRightHand = EquippedItems.rightHand != null ? EquippedItems.rightHand.name : "Empty";

            if (itemUnderCrosshair != null)
            {
                var name = itemUnderCrosshair.GetComponent<BaseItem>().Details.Name;
                IsCrosshairActive = name != "";
                ItemUnderCrosshair = name;
            } else {
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
            GUI.Label(new Rect(100, Screen.height - 75, 100, 50), $"Mana: {EquippedItems.entity.Mana} / {EquippedItems.entity.MaxMana}", fontStyle);
            GUI.Label(new Rect(Screen.width - 200, Screen.height - 75, 100, 50), $"Health: {EquippedItems.entity.Health} / {EquippedItems.entity.MaxHealth}", fontStyle);
        }
    }
}