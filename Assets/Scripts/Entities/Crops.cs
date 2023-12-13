using UnityEngine;

public class Crops : MonoBehaviour
{
    [SerializeField] private CropsSO _cropsSO;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Farmland _farmland;

    private Sprite[] _sprites = new Sprite[3];

    public int CropId;
    
    public int MyWater = 50;
    public int MyNutrition = 50;
    public int MyPest = 0;

    private GrowStep _growStep = GrowStep.start;
    public GrowStep GrowStepType { get { return _growStep; } set { _growStep = value; } }
    public int DayCount { get; private set; }
    public int CropsScore { get; private set; }


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _farmland = transform.parent.GetComponent<Farmland>();
    }

    public void InitializeBySeed(int itemId)
    {
        DayCount = 0;
        CropId = itemId - 15000;
        _cropsSO = Managers.Instance.DataManager.GetSO<CropsSO>(Const.SO_Crops, CropId);

        SpriteInitialize();
    }

    public void InitializeBySaveData(FarmlandSaveData data)
    {
        CropId = data.CropId;
        if (CropId < 0)
        {
            gameObject.SetActive(false);
            return;
        }
        _cropsSO = Managers.Instance.DataManager.GetSO<CropsSO>(Const.SO_Crops, CropId);

        MyWater = data.CropWater;
        MyNutrition = data.CropNutrition;
        MyPest = data.CropPest;
        GrowStepType = data.GrowStep;
        DayCount = data.DayCount;
        CropsScore = data.CropsScore;
        
        SpriteInitialize();
    }

    public void ChangeByDay()
    {
        if (_growStep == GrowStep.harvestable) return;

        // 토지에 의한 벌레 증감치 및 영양치 계산
        MyPest += _farmland.FertileRatio / _cropsSO.PestByFertile;
        MyPest = (MyPest < 100) ? MyPest : 100;
        MyNutrition -= _farmland.PesticideRatio / _cropsSO.NutritionByPesticide;
        MyNutrition = (MyNutrition > 0) ? MyNutrition : 0;

        // 농지와 상호 작용
        int amount;
        amount = (MyWater + _cropsSO.AbsorbWater > 100) ? 100 - MyWater : _cropsSO.AbsorbWater;
        _farmland.WaterRatio -= amount;
        MyWater += amount;
        amount = (MyNutrition + _cropsSO.AbsorbNutrition > 100) ? 100 - MyNutrition : _cropsSO.AbsorbNutrition;
        _farmland.FertileRatio -= amount;
        MyNutrition += amount;
        amount = (MyPest - _cropsSO.PestAmountPerDay < 0) ? MyPest : _cropsSO.PestAmountPerDay;
        _farmland.PesticideRatio -= amount;
        MyPest -= amount;

        // 날마다 변화하는 것 적용
        MyWater -= _cropsSO.ConsumeWater;
        MyWater = (MyWater > 0) ? MyWater : 0;
        MyNutrition -= _cropsSO.ConsumeNutrition;
        MyNutrition = (MyNutrition > 0) ? MyNutrition : 0;
        MyPest += _cropsSO.PestAmountPerDay;
        MyPest = (MyPest < 100) ? MyPest : 100;

        DayCount++;
        if (DayCount == 1)
        {
            Grow();
        }
        else if (DayCount == _cropsSO.GrowCycle)
        {
            Grow();
        }

        // 수분 기준치 체크
        if (_cropsSO.MinRecommendedWater <= MyWater && MyWater <= _cropsSO.MaxRecommendedWater) { CropsScore += 10; }
        else if (MyWater < _cropsSO.MinRecommendedWater) { CropsScore -= 10; }

        // 영양 기준치 체크
        if (MyNutrition <= _cropsSO.MaxRecommendedNutrition && MyNutrition >= _cropsSO.MinRecommendedNutrition) { CropsScore += 15; }

        // 벌레 기준치 체크
        if (MyPest > _cropsSO.MaxRecommendedPest) { CropsScore -= 15; }
    }

    private void SpriteInitialize()
    {
        _sprites = Managers.Instance.DataManager.GetSO<ImageSO>(Const.SO_CropsImageSO).Images[CropId].SpriteList;
        
        _spriteRenderer.sprite = _sprites[(int)_growStep];
    }
    
    private void Grow()
    {
        switch (_growStep)
        {
            case GrowStep.start:
                _growStep = GrowStep.growing;
                _spriteRenderer.sprite = _sprites[1];
                break;
            case GrowStep.growing:
                _growStep = GrowStep.harvestable;
                _spriteRenderer.sprite = _sprites[2];
                break;
        }
    }

    public void Harvest()
    {
        int yield;
        if (CropsScore < 20) { yield = _cropsSO.DefaultYield[0]; }
        else if (CropsScore < 40) { yield = _cropsSO.DefaultYield[1]; }
        else if (CropsScore < 60) { yield = _cropsSO.DefaultYield[2]; }
        else if (CropsScore < 80) { yield = _cropsSO.DefaultYield[3]; }
        else { yield = _cropsSO.DefaultYield[4]; }
        
        Managers.Instance.ItemManager.SpawnCollectable(11000 + _cropsSO.CropID, transform.position, yield);
        CropId = 0;
    }
}