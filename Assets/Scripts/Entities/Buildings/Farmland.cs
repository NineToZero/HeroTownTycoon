using UnityEngine;

public class Farmland : BaseBuilding, IClickable
{
    public Crops Crop;

    #region FertilityData
    private int _waterMaxRatio = 100;
    private int _waterRatio = 50;
    public int WaterRatio
    {
        get
        {
            return _waterRatio;
        }
        set
        {
            _waterRatio = value < 0 ? 0 : value;
            _waterRatio = _waterRatio > _waterMaxRatio ? _waterMaxRatio : _waterRatio;
        }
    }

    private int _fertileMaxRatio = 100;
    private int _fertileRatio = 50;
    public int FertileRatio
    {
        get
        {
            return _fertileRatio;
        }
        set
        {
            _fertileRatio = value < 0 ? 0 : value;
            _fertileRatio = _fertileRatio > _fertileMaxRatio ? _fertileMaxRatio : _fertileRatio;
        }
    }

    public int _pesticideMaxRatio = 100;
    private int _pesticideRatio = 50;
    public int PesticideRatio
    {
        get
        {
            return _pesticideRatio;
        }
        set
        {
            _pesticideRatio = value < 0 ? 0 : value;
            _pesticideRatio = _pesticideRatio > _pesticideMaxRatio ? _pesticideMaxRatio : _pesticideRatio;
        }
    }


    public int WaterReduction = 10;
    public int FertileReduction = 5;
    public int PesticideReduction = 5;

    #endregion

    public bool isUsed { get { return Crop.gameObject.activeSelf; } }
    public bool isHarvestable { get { return Crop.GrowStepType == GrowStep.harvestable; } }


    public override void Init(BuildingType type, bool isUpgrade = false)
    {
        base.Init(type);
        Managers.Instance.DayManager.DayChangeEvent += ChangeByDay;
    }

    public void Load(FarmlandSaveData data)
    {
        WaterRatio = data.WaterRatio;
        FertileRatio = data.FertileRatio;
        PesticideRatio = data.PesticideRatio;

        Crop.gameObject.SetActive(true);
        Crop.InitializeBySaveData(data);
    }

    public void Click(int handledItemId, out int consumedCount)
    {
        Managers.Instance.SoundManager.PlaySFX(SFXSource.InteractionFarmland);
        consumedCount = 0;
        switch (Util.GetItemType(handledItemId))
        {
            case ItemType.SeedsType:
                if (!isUsed)
                {
                    Plant(handledItemId);
                    consumedCount = 1;
                }
                break;
            case ItemType.FarmingitemType:
                FarmingItemData farmingItem = Managers.Instance.DataManager.Items[handledItemId] as FarmingItemData;
                Fertilize(farmingItem);
                consumedCount = 1;
                var um = Managers.Instance.UIManager;
                um.ShowNotificationUI($"{farmingItem.Name}을(를) 사용하였습니다.");
                um.GetUI<InteractionGuideUI>().Refresh(gameObject);

                break;
            default:
                if (isHarvestable) Harvest();
                break;
        }
    }

    private void Fertilize(FarmingItemData farmingItem)
    {
        switch (farmingItem.FarmingItemType)
        {
            case FarmingItemType.Moisture:
                WaterRatio += farmingItem.Value;
                break;
            case FarmingItemType.Fertile:
                FertileRatio += farmingItem.Value;
                break;
            case FarmingItemType.Pesticide:
                PesticideRatio += farmingItem.Value;
                break;
        }
        return;
    }

    private void Plant(int id)
    {
        Crop.gameObject.SetActive(true);
        Crop.InitializeBySeed(id);
        Crop.GrowStepType = GrowStep.start;
    }

    private void Harvest()
    {
        Crop.Harvest();
        Crop.gameObject.SetActive(false);
        Crop.GrowStepType = GrowStep.start;
    }

    private void ChangeByDay() // 하루가 지날 때 마다 호출
    {
        WaterRatio -= WaterReduction;
        FertileRatio -= FertileReduction;
        PesticideRatio -= PesticideReduction;

        if (isUsed) Crop.ChangeByDay();
    }
}
