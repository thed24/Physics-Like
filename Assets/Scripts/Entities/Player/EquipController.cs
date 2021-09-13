using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EquipController : MonoBehaviour
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
            leftHandClone = InitializeWeaponObjects(leftHand, leftHandEquipPoint);
        }

        if (rightHand != null)
        {
            rightHandClone = InitializeWeaponObjects(rightHand, rightHandEquipPoint);
        }
    }

    void Update(){
        if (leftHandClone != null)
        {
            AnimateWeaponOnClick(leftHandClone, "Slash", 0);
        }

        if (rightHandClone != null)
        {
            AnimateWeaponOnClick(rightHandClone, "Slash", 1);
        }
    }

    private GameObject InitializeWeaponObjects(GameObject weapon, Transform weaponPoint)
    {
        var clone = Instantiate(weapon, weaponPoint.position, Quaternion.identity);
        clone.transform.parent ??= weaponPoint;

        return clone;
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

