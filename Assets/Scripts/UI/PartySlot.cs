using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _image;
    [SerializeField] private TextSlot _originSlot;
    [SerializeField] private TextSlot _natureSlot;
    [SerializeField] private TextSlot _JobSlot;

    private void Awake()
    {
        _originSlot.Name("출신");
        _natureSlot.Name("성격");
        _JobSlot.Name("직업");
    }

    public void Refresh(HeroHandler hero)
    {
        DataManager dm = Managers.Instance.DataManager;
        _image.sprite = dm.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, hero.SpriteId).GetSprite("Idle", "0");

        Dictionary<int, BaseIndividualityStatData> indis = dm.Indis;
        _nameText.text = hero.IndividualityStat.Name;
        _originSlot.Value(indis[hero.IndividualityStat.OriginCode].Name);
        _natureSlot.Value(indis[hero.IndividualityStat.NatureCode].Name);
        _JobSlot.Value(indis[hero.IndividualityStat.JobCode].Name);
    }
}