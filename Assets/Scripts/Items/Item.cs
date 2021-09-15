using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Item : MonoBehaviour
    {
        public string Name;

        private void Start()
        {
            tag = "Item";
        }
    }
}
