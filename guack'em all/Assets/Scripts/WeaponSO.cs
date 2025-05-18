using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName = "New Weapon";
    public int damage = 1;
    public float swingSpeed = 0.5f; // Adjust the speed of the swing animation
    public float swingLength = 0.3f; // Duration of the swing/attack

    [Header("Visuals, Sound & Animation")]
    public AudioClip[] attackSounds;
    public Sprite weaponSprite; // Sprite for the weapon

    [Header("Weapon Setup")]
    public GameObject weaponPrefab; // Entire prefab containing SpriteRenderer, Animator, Collider, etc.
}
