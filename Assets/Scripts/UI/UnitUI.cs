using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private UnitAnimatorController _anim;

    private HeroHandler _hero;
    public void Init(HeroHandler hero)
    {
        _hero = hero;
        _anim.Init(hero.SpriteId);
        _anim.SetState(AnimationState.Walking);
    }

    private void LateUpdate()
    {
        _image.sprite = _sprite.sprite;
    }

    private void OnEnable()
    {
        if (!_hero.IsAlive) gameObject.SetActive(false);
        else _anim.SetState(AnimationState.Walking);
    }
}