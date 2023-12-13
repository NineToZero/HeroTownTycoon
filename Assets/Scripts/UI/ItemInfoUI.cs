using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : BaseUI
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _itemimage;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _category;
    [SerializeField] private TMP_Text _disc;
    [SerializeField] private TMP_Text _price;

    [SerializeField] private Transform _extra;
    [SerializeField] private ImageSlot[] _slots;
    [SerializeField] private Sprite[] _icons;

    private DataManager _dm;

    private void Awake()
    {
        _dm = Managers.Instance.DataManager;
    }
    public void Refresh(int itemId)
    {
        if (itemId == 0)
        {
            Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
            return;
        }
        var data = _dm.Items[itemId];
        _name.text = data.Name;
        _price.text = data.Price.ToString();
        _itemimage.sprite = _dm.GetItemSprites(itemId);


        switch (Util.GetItemType(itemId))
        {
            case ItemType.EtcType:
                _category.text = "[기타]";
                _disc.text = "쓸모없어 보입니다. 다른 아이템의 재료로 쓰일 수는 있을 것 같습니다.";
                break;
            case ItemType.BlueprintType:
                _category.text = "[청사진]";
                _disc.text = $"{_dm.BaseBuildings[((BlueprintData)data).BuildingType].Name}의 청사진입니다. 설치하고 하루가 지나면 완성됩니다.";
                break;
            case ItemType.SeedsType:
                _category.text = "[씨앗]";
                _disc.text = $"{_dm.Items[itemId - 4000].Name}의 씨앗입니다. 수확까지 {(_dm.GetSO<CropsSO>(Const.SO_Crops, itemId - 15000)).GrowCycle}일이 소요됩니다.";
                break;
            case ItemType.DishType:
                DishData dish = data as DishData;
                _category.text = "[음식]";
                _disc.text = $"맛있어 보이는 음식입니다. {_dm.GetSO<IndividualityStatSO>(Const.SO_Hero_Individuality).Flavors[dish.Flavor % 100].Name}이 일품입니다.";
                for (int i = 0; i < 4; i++)
                    if (dish.Nutrients[i].value != 0)
                        _slots[i].Sprite(_icons[i]).Text(dish.Nutrients[i].value.ToString()).gameObject.SetActive(true);
                _extra.gameObject.SetActive(true);
                break;
            case ItemType.MedicineType:
                MedicineData medicine = data as MedicineData;
                _category.text = "[치료]";
                _disc.text = "용사의 부상에 도움이 되는 아이템입니다. 용사를 치료해 줄 수 있습니다.";
                _slots[0].Sprite(_icons[8]).Text(medicine.HealthValue.ToString()).gameObject.SetActive(true);
                _extra.gameObject.SetActive(true);
                break;
            case ItemType.FarmingitemType:
                FarmingItemData farming = data as FarmingItemData;
                _category.text = "[농사]";
                _disc.text = "농사에 도움이 되는 아이템입니다. 농지에 사용할 수 있습니다.";
                _slots[0].Sprite(_icons[4 + (int)farming.FarmingItemType]).Text(farming.Value.ToString()).gameObject.SetActive(true);
                _extra.gameObject.SetActive(true);
                break;
        }
    }

    public override void On()
    {
        base.On();
        Cursor.visible = false;

        _extra.gameObject.SetActive(false);
        foreach (var slot in _slots) slot.gameObject.SetActive(false);
    }

    public override void Off()
    {
        base.Off();
        Cursor.visible = true;
    }
}
