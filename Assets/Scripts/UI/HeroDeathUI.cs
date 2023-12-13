using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HeroDeathUI : PopupUI
{
    [SerializeField] private Image _image;

    private HerosController _heroController;
    private HeroHandler _heroHandler;
    public void Init(HerosController heroController)
    {
        _heroController =  heroController;
    }

    public void SetHero(HeroHandler hero)
    {
        _heroHandler = hero;
        _image.sprite = Managers.Instance.DataManager.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, hero.SpriteId).GetSprite("Death", "2");
    }

    public override void On()
    {
        base.On();

        Refresh(null, "용사의 여행", "용사가 떠났습니다.", () => { Managers.Instance.DayManager.DayChangeEvent -= _heroHandler.DoDayEvent; _heroController.RemoveHero(_heroHandler); Managers.Instance.UIManager.CloseUI(this); }, "", "확인");
    }
}
