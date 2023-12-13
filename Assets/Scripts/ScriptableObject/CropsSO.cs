using UnityEngine;

[CreateAssetMenu(fileName = "Crops", menuName = "Scriptable Object/Crops")]
public class CropsSO : ScriptableObject
{
    [Header("# Crop Data")]
    public int CropID;
    public string CropName;
    public int GrowCycle;
    public int[] DefaultYield;

    [Header("# Crops Stats")]
    public int AbsorbWater; // 농지로부터 매일 빨아들이는 수분량
    public int ConsumeWater; // 매일 작물 자체의 수분으로부터 사용하는 수분량
    public int MinRecommendedWater = 40; // 권장 최소 수분량 기준치
    public int MaxRecommendedWater = 60; // 권장 최대 수분량 기준치
    [Space()]
    public int AbsorbNutrition;
    public int ConsumeNutrition;
    public int MinRecommendedNutrition = 50;
    public int MaxRecommendedNutrition = 70;
    [Space()]
    public int PestAmountPerDay;
    public int MinRecommendedPest = 0;
    public int MaxRecommendedPest = 20;
    [Space()]
    public int NutritionByPesticide; //농약에 의한 영양 감소량
    public int PestByFertile; //비옥도에 의한 벌레 증가량
}
