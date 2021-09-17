using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using Assets.Scripts.Items.Weapon;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(Entity))]
public class EquippedItems : MonoBehaviour
{
    public Transform leftHandEquipPoint;
    public Transform rightHandEquipPoint;

    public GameObject leftHand;
    public GameObject rightHand;

    private Entity entity;

    void Start(){
        entity = gameObject.GetComponent<Entity>();
    }

    void Update(){
        var itemAtCrosshair = GetItemAtCrosshair<HoldableItem>();
        var worldItemAtCrosshair = GetItemAtCrosshair<WorldItem>();

        ActionForHand(ref leftHand, leftHandEquipPoint, 0, KeyCode.Q, itemAtCrosshair);
        ActionForHand(ref rightHand, rightHandEquipPoint, 1, KeyCode.R, itemAtCrosshair);

        if (Input.GetKeyDown(KeyCode.E) && itemAtCrosshair != null){
            entity.Inventory.Items.Add(itemAtCrosshair.GetComponent<Item>());
            itemAtCrosshair.SetActive(false);
        } 
        else if (Input.GetKeyDown(KeyCode.E) && worldItemAtCrosshair != null){
            worldItemAtCrosshair.GetComponent<WorldItem>().Interact();
        }
    }

    private void ActionForHand(ref GameObject hand, Transform equipPoint, int actionButton, KeyCode dropKey, GameObject itemAtCrosshair){
        if (hand != null && hand.GetComponent<Weapon>() != null)
        {
            AnimateWeaponOnClick(hand, actionButton);
        }
        else if (hand == null && Input.GetMouseButtonDown(actionButton) && itemAtCrosshair != null)
        {
            EquipItemAt(ref hand, itemAtCrosshair, equipPoint);
        }
        else if (hand != null && Input.GetKeyDown(dropKey)){
            ThrowItemFrom(ref hand, equipPoint);
        }
    }

    private void EquipItemAt(ref GameObject slot, GameObject item, Transform hand)
    {
        var clonedItem = Instantiate(item, hand.position, Quaternion.identity);
        clonedItem.transform.parent ??= hand;
        clonedItem.GetComponent<Animator>().enabled = true;
        clonedItem.GetComponent<Rigidbody>().isKinematic = true;
        slot = clonedItem;

        item.GetComponent<HoldableItem>().Interact();
    }

    private void ThrowItemFrom(ref GameObject item, Transform hand)
    {
        var clone = Instantiate(item, hand.position, Quaternion.identity);
        GameObject.Destroy(item);
        item = null;

        clone.GetComponent<Animator>().enabled = false;
        clone.GetComponent<Rigidbody>().isKinematic = false;
        clone.GetComponent<Rigidbody>().AddForce(hand.forward * 500);
    }

    private GameObject GetItemAtCrosshair<T>()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 10.0f))
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<T>() != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private void AnimateWeaponOnClick(GameObject weapon, int mouseButton)
    {
        var weaponAnimator = weapon.GetComponent<Animator>();
        var animationName = weapon.GetComponent<Weapon>().AttackAnimation.name;
        var animationInfo = weaponAnimator.GetCurrentAnimatorStateInfo(0);

        if (!animationInfo.IsName(animationName) && Input.GetMouseButtonDown(mouseButton))
        {
            weaponAnimator.Play(animationName);
            weapon.GetComponent<Weapon>().Source.PlayOneShot(weapon.GetComponent<Weapon>().AttackSound);
        }
        else
        {
            weapon.tag = animationInfo.IsName(animationName) ? "ActiveWeapon" : "Weapon";
        }
    }
}

