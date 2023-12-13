public class RecipeData
{
    public ItemIdQuantityPair Output;
    public ItemIdQuantityPair[] Inputs;
}

public class RecipeContainer
{
    public BuildingType BuildingType;
    public RecipeData[] RecipeDatas;
}

public class ShopItemsContainer
{
    public BuildingType BuildingType;
    public int[] Items;
}