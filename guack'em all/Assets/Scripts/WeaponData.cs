using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName = "New Weapon";
    public int damage = 1;
    public float swingSpeed = 0.5f; // Adjust the speed of the swing animation
    public float swingLength = 0.3f; // Duration of the swing/attack

    [Header("Visuals, Sound & Animation")]
    public AudioClip[] attackSounds;
    public GameObject trailPrefab; // Optional: Prefab for the trail effect
    public GameObject swipeParticlesPrefab; // Optional: Prefab for swipe particles
    public Sprite weaponSprite; // Sprite for the weapon
    public RuntimeAnimatorController weaponAnimatorController; // Animator Controller for the weapon
    public AnimatorOverrideController weaponAnimatorOverride; // Optional override

    [Header("Weapon Setup")]
    public GameObject weaponPrefab; // Entire prefab containing SpriteRenderer, Animator, Collider, etc.

}
