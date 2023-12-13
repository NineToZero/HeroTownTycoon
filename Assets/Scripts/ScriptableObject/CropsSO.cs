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
    public int AbsorbWater; // �����κ��� ���� ���Ƶ��̴� ���з�
    public int ConsumeWater; // ���� �۹� ��ü�� �������κ��� ����ϴ� ���з�
    public int MinRecommendedWater = 40; // ���� �ּ� ���з� ����ġ
    public int MaxRecommendedWater = 60; // ���� �ִ� ���з� ����ġ
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
    public int NutritionByPesticide; //��࿡ ���� ���� ���ҷ�
    public int PestByFertile; //������� ���� ���� ������
}
