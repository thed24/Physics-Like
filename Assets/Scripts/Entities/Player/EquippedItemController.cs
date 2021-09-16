using Assets.Scripts.Entities;
using Assets.Scripts.Items.Weapon;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Entity))]
public class EquippedItemController : MonoBehaviour
{
    public Transform leftHandEquipPoint;
    public Transform rightHandEquipPoint;

    public GameObject leftHand;
    public GameObject rightHand;

    private GameObject leftHandClone;
    private GameObject rightHandClone;

    void Start(){
        if (leftHand != null)
        {
            leftHandClone = InitializeItems(leftHand, leftHandEquipPoint);
        }

        if (rightHand != null)
        {
            rightHandClone = InitializeItems(rightHand, rightHandEquipPoint);
        }
    }

    void Update(){
        if (leftHandClone != null && leftHandClone.GetComponent<Weapon>() != null)
        {
            AnimateWeaponOnClick(leftHandClone, "Slash", 0);
        }
        else if (leftHandClone == null && Input.GetMouseButtonDown(0))
        {
            leftHandClone = PickUpItemAtCrosshair(leftHandEquipPoint);
        }

        if (rightHandClone != null && rightHandClone.GetComponent<Weapon>() != null)
        {
            AnimateWeaponOnClick(rightHandClone, "Slash", 1);
        }
        else if (rightHandClone == null && Input.GetMouseButtonDown(1))
        {
            rightHandClone = PickUpItemAtCrosshair(rightHandEquipPoint);
        }
    }

    private GameObject InitializeItems(GameObject weapon, Transform weaponPoint)
    {
        var clone = Instantiate(weapon, weaponPoint.position, Quaternion.identity);
        clone.transform.parent ??= weaponPoint;
        clone.GetComponent<Animator>().enabled = true;

        return clone;
    }

    private GameObject PickUpItemAtCrosshair(Transform equipPoint)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5.0f))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Weapon") || hit.collider.gameObject.CompareTag("Item"))
            {
                var clone = InitializeItems(hit.collider.gameObject, equipPoint);
                Destroy(hit.collider.gameObject);
                return clone;
            }
        }
        return null;
    }

    private void AnimateWeaponOnClick(GameObject weapon, string animationName, int mouseButton)
    {
        var weaponAnimator = weapon.GetComponent<Animator>();
        var animationInfo = weaponAnimator.GetCurrentAnimatorStateInfo(0);

        if (!animationInfo.IsName(animationName) && Input.GetMouseButtonDown(mouseButton))
        {
            weaponAnimator.Play(animationName);
        }
        else
        {
            weapon.tag = animationInfo.IsName(animationName) ? "ActiveWeapon" : "Weapon";
        }
    }
}

