using UnityEngine;

public class WeaponEquipper : MonoBehaviour
{
    public Transform weaponAttachmentPoint;
    public GameObject currentWeaponInstance;
    private AttackManager attackManager;
    [HideInInspector] public Animator weaponAnimator; // Make these public if needed elsewhere
    [HideInInspector] public TrailRenderer trailRenderer;
    [HideInInspector] public Collider weaponCollider;
    // Assuming you have a script to control animation speed
    private AnimationSpeed animationSpeed;

    public WeaponData currentWeaponData;

    void Start()
    {
        attackManager = FindFirstObjectByType<AttackManager>();
        if (attackManager == null)
        {
            Debug.LogError("AttackManager not found in children!");
        }

        if (weaponAttachmentPoint == null)
        {
            Debug.LogError("Weapon Attachment Point not assigned!");
        }

        // Equip the initial weapon if one is set
        if (currentWeaponData != null)
        {
            EquipWeapon(currentWeaponData);
        }
    }

    public void EquipWeapon(WeaponData newWeaponData)
    {
        // Unequip the current weapon
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            weaponAnimator = null;
            trailRenderer = null;
            weaponCollider = null;
            animationSpeed = null;
        }

        currentWeaponData = newWeaponData;

        if (currentWeaponData != null && currentWeaponData.weaponPrefab != null && weaponAttachmentPoint != null)
        {
            // Instantiate the new weapon prefab
            currentWeaponInstance = Instantiate(currentWeaponData.weaponPrefab, weaponAttachmentPoint);
            currentWeaponInstance.transform.localPosition = Vector3.zero;
            currentWeaponInstance.transform.localRotation = Quaternion.identity;

            // Set the tag
            currentWeaponInstance.tag = "Weapon";

            // Get references to the necessary components
            weaponAnimator = currentWeaponInstance.GetComponent<Animator>();
            trailRenderer = currentWeaponInstance.GetComponent<TrailRenderer>();
            weaponCollider = currentWeaponInstance.GetComponent<Collider>();
            animationSpeed = currentWeaponInstance.GetComponent<AnimationSpeed>();

            // Inform the AttackManager that the weapon has changed
            if (attackManager != null)
            {
                attackManager.UpdateWeaponComponents();
            }

            Debug.Log($"Equipped {currentWeaponData.weaponName}");
        }
        else
        {
            Debug.LogError("Weapon Data or Weapon Prefab is missing!");
        }
    }

    public void SwitchWeapon(WeaponData newWeaponData)
    {
        EquipWeapon(newWeaponData);
    }
}