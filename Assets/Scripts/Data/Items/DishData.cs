using System;

[Serializable]
public class DishData : BaseItemData
{
    public int Tier;
    public NutrientValue[] Nutrients;
    public int Flavor;
}

