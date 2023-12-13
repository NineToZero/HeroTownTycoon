using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HeroPopupUI : PopupUI
{
    [SerializeField] private Image _heroImage;
    [SerializeField] private GameObject _detail;
    
    [SerializeField] private TextSlot[] _textSlots;
    [SerializeField] private Button _detailButton;
    [SerializeField] private TextMeshProUGUI _detailText;

    private HeroBuilder _builder;

    private int _rerollCount;

    public void Init(HeroBuilder builder)
    {
        _builder = builder;
        _detailButton.onClick.AddListener(ShowDetail);
    }

    public override void On()
    {
        base.On();

        _detailButton.interactable = true;
        _detailText.text = "����";
        _rerollCount = 3;
        _heroImage.color = Color.black;

        _detail.SetActive(false);
    }

    private void ShowDetail()
    {
        if(_rerollCount == 3) _heroImage.color = Color.white;
        _detailText.text = $"����({--_rerollCount})";

        IndividualityStat indi = _builder.Prebuild();
        IndividualityStatSO so = Managers.Instance.DataManager.GetSO<IndividualityStatSO>(Const.SO_Hero_Individuality);

        _heroImage.sprite = Managers.Instance.DataManager.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, indi.SpriteCode).GetSprite("Idle", "0");
        _textSlots[0].Name("�̸�").Value(indi.Name).gameObject.SetActive(true);
        _textSlots[1].Name("���").Value(so.Origins[indi.OriginCode % 100].Name).gameObject.SetActive(true);
        _textSlots[2].Name("����").Value(so.Natures[indi.NatureCode % 100].Name).gameObject.SetActive(true);
        _textSlots[3].Name("����").Value(so.Jobs[indi.JobCode % 100].Name).gameObject.SetActive(true);
        _textSlots[4].Name("��ȣ").Value(so.Flavors[indi.FlavorCode % 100].Name).gameObject.SetActive(true);

        _detail.SetActive(true);

        if (_rerollCount == 0) _detailButton.interactable = false;
    }
}