using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();

        ChangeAnimationSpeed();
    }

    // Update is called once per frame
    void ChangeAnimationSpeed()
    {
        anim.speed = 0.4f;
    }
}
