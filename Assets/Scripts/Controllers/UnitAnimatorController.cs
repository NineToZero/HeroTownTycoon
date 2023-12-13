using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class UnitAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteLibrary _spriteLibrary;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Move = Animator.StringToHash("Walking");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private static readonly int Attack = Animator.StringToHash("Jab");
    private static readonly int Skill = Animator.StringToHash("Slash");
    private static readonly int Hit = Animator.StringToHash("Hit");

    public void Init(int spriteId)
    {
        SpriteLibraryAsset sprite = Managers.Instance.DataManager.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, spriteId);
        _spriteLibrary.spriteLibraryAsset = sprite;
    }

    public void SetState(AnimationState state)
    {
        foreach (var variable in new[] { Idle, Move, Dead })
        {
            _animator.SetBool(variable, false);
        }

        switch (state)
        {
            case AnimationState.Idle: _animator.SetBool(Idle, true); break;
            case AnimationState.Walking: _animator.SetBool(Move, true); break;
            case AnimationState.Dead: _animator.SetBool(Dead, true); break;
            default: throw new NotSupportedException();
        }
    }

    public void SetTrigger(AnimationTrigger trigger)
    {
        switch (trigger)
        {
            case AnimationTrigger.Jab: _animator.SetTrigger(Attack); break;
            case AnimationTrigger.Slash: _animator.SetTrigger(Skill); break;
            case AnimationTrigger.Hit: _animator.SetTrigger(Hit); break;
        }
    }

    public void Turn(bool isTurnLeft)
    {
        _body.flipX = isTurnLeft;
    }

    public AnimationState GetState()
    {
        if (_animator.GetBool(Idle)) return AnimationState.Idle;
        if (_animator.GetBool(Move)) return AnimationState.Walking;

        return AnimationState.Ready;
    }
}


