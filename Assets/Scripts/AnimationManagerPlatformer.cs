using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManagerPlatformer : MonoBehaviour
{
    [Header("Do you want me to flip?")]
    public bool doIFlip;

    [Header("Am I facing right?")]
    public bool facingRight;

    [Header("Drag in the animations you want to use.")]
    public AnimationClip idle;
    public AnimationClip moveSide;
    public AnimationClip moveUp;
    public AnimationClip moveDown;
    public AnimationClip attack;
    public AnimationClip death;
    public AnimationClip action1;
    public AnimationClip action2;
    public AnimationClip action3;

    private SpriteRenderer sr;

    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    protected AnimationClipOverrides clipOverrides;
    public void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        clipOverrides["Idle"] = idle;
        clipOverrides["Side"] = moveSide;
        clipOverrides["Up"] = moveUp;
        clipOverrides["Down"] = moveDown;
        clipOverrides["Attack"] = attack;
        clipOverrides["Death"] = death;
        clipOverrides["Action1"] = action1;
        clipOverrides["Action2"] = action2;
        clipOverrides["Action3"] = action3;
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }

    public void Update()
    {
        FlipSprite();
    }

    public void FlipSprite()
    {
        if (doIFlip)
        {
            if(animator.GetFloat("HorizontalSpeed") < 0 && facingRight)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
            }
            else if(animator.GetFloat("HorizontalSpeed") > 0 && !facingRight)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
                
            }
        }
    }
}

