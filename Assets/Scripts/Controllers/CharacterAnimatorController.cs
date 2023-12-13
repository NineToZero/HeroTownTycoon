using System;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer bodySprite;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Slash = Animator.StringToHash("Slash");
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetState(AnimationState state)
    {
        foreach (var variable in new[] { Idle, Walking })
        {
            animator.SetBool(variable, false);
        }
        
        switch (state)
        {
            case AnimationState.Idle: animator.SetBool(Idle, true); break;
            case AnimationState.Walking: animator.SetBool(Walking, true); break;
            default: throw new NotSupportedException();
        }
    }

    public void SetTrigger(AnimationTrigger trigger)
    {
        switch (trigger)
        {
            case AnimationTrigger.Slash: animator.SetTrigger(Slash); break;
        }
    }

    public void SetSpriteFlip(bool isFlipLeft)
    {
        bodySprite.flipX = isFlipLeft;
    }
    
    public AnimationState GetState()
    {
        if (animator.GetBool(Idle)) return AnimationState.Idle;
        if (animator.GetBool(Walking)) return AnimationState.Walking;

        return AnimationState.Ready;
    }
}


