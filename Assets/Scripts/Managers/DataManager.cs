using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, GameObject> _prefabs;
    private Dictionary<string, ScriptableObject> _SOs;

    #region ItemsDictionary
    public Dictionary<int, BaseItemData> Items = new Dictionary<int, BaseItemData>();
    #endregion

    #region ItemImageDictionary
    private Dictionary<int, Sprite> _itemImages;
    #endregion

    #region BuildingDictionary
    public Dictionary<BuildingType, BuildingData> BaseBuildings;
    public Dictionary<BuildingType, PassiveBuildingData> PassiveBuildings;
    #endregion

    private Dictionary<BuildingType, RecipeData[]> RecipeLists;

    //Tier, ItemID List
    private Dictionary<BuildingType, int[]> ShopItemsLists;

    public Dictionary<int, BaseIndividualityStatData> Indis;

    private void Awake()
    {
        LoadItem();
        LoadItemImage();
        LoadBuilding();
        LoadShopItems();
        LoadRecipe();
        LoadIndividualityStat();
    }

    private void LoadItemImage()
    {
        
    }
    
    private void LoadItem()
    {
        string itemJson = Resources.Load<TextAsset>(Const.Item_Data).text;
        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(itemJson);

        foreach (BaseItemData itemdata in itemData.DishData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        foreach (BaseItemData itemdata in itemData.EtcItemData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        foreach (BaseItemData itemdata in itemData.FarmingItemData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        foreach (BaseItemData itemdata in itemData.MedicineData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        foreach (BaseItemData itemdata in itemData.SeedData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        foreach (BaseItemData itemdata in itemData.BlueprintData)
        {
            Items.Add(itemdata.Id, itemdata);
        }
        
    }

    public GameObject GetPrefab(string constPath)
    {
        if(_prefabs == null) _prefabs = new Dictionary<string, GameObject>();

        if(!_prefabs.TryGetValue(constPath, out GameObject prefab))
        {
            prefab = Resources.Load<GameObject>(constPath);
            if (prefab != null)
            {
                _prefabs.Add(constPath, prefab);
            }
        }

        return prefab;
    }

    public T GetSO<T>(string constPath) where T : ScriptableObject
    {
        if (_SOs == null) _SOs = new Dictionary<string, ScriptableObject>();

        T T_so = null;

        if (_SOs.TryGetValue(constPath, out ScriptableObject so))
        {
            T_so = so as T;
        }

        if(!T_so) {
            if(T_so = Resources.Load<T>(constPath))
            {
                if(so)
                {
                    _SOs.Remove(constPath);
                }

                _SOs.Add(constPath, T_so);
            }
        }

        return T_so;
    }

    public T GetSO<T>(string constPath, int id) where T : ScriptableObject
    {
        string path = Path.Combine(constPath, id.ToString("000"));

        return GetSO<T>(path);
    }

    public T GetPrefab<T>(string constPath) where T : Component
    {
        T component = null;
        GetPrefab(constPath)?.TryGetComponent(out component);

        return component;
    }

    public Sprite GetItemSprites(int itemId)
    {
        var itemImageDict = GetSO<ItemImageSO>(Const.SO_ItemImageSO).ImageDictionary;
        if (itemImageDict.TryGetValue(itemId, out var sprites)) return sprites;
        return GetSO<ImageSO>(Const.SO_NullImageSO).Images[0].SpriteList[0];
    }

    public int[] GetShopItemDatas(BuildingType buildingType)
    {
        if (ShopItemsLists.TryGetValue(buildingType, out var shopItemDatas)) return shopItemDatas;
        return null;
    }
    public RecipeData[] GetRecipeDatas(BuildingType buildingType)
    {
        if (RecipeLists.TryGetValue(buildingType, out var recipeDatas)) return recipeDatas;
        return null;
    }
    
    private void LoadBuilding()
    {
        BuildingSO buildingSO = Resources.Load<BuildingSO>(Const.SO_BuildingSO);

        BaseBuildings = buildingSO.BaseList.ToDictionary(item => item.BuildingType, Item => Item);
        PassiveBuildings = buildingSO.PassiveList.ToDictionary(item => item.BuildingType, Item => Item);
    }

    private void LoadShopItems()
    {
        ShopItemsLists = new ();

        string shopItemJson = Resources.Load<TextAsset>(Const.ShopItems_Data).text;
        ShopItemsContainer[] shopItemsDatas = JsonConvert.DeserializeObject<ShopItemsContainer[]>(shopItemJson);
        
        foreach (var container in shopItemsDatas)
            ShopItemsLists.Add(container.BuildingType, container.Items);
    }
    
    private void LoadRecipe()
    {
        RecipeLists = new Dictionary<BuildingType, RecipeData[]>();

        string recipeJson = Resources.Load<TextAsset>(Const.Recipe_Data).text;
        RecipeContainer[] recipeDatas = JsonConvert.DeserializeObject<RecipeContainer[]>(recipeJson).ToArray();
        
        foreach (var container in recipeDatas)
            RecipeLists.Add(container.BuildingType, container.RecipeDatas);
    }

    private void LoadIndividualityStat()
    {
        IndividualityStatSO indiSO = Resources.Load<IndividualityStatSO>(Const.SO_Hero_Individuality);
        Indis = new();

        foreach (var origin in indiSO.Origins) Indis.Add(origin.id, origin);
        foreach (var nature in indiSO.Natures) Indis.Add(nature.id, nature);
        foreach (var job in indiSO.Jobs) Indis.Add(job.id, job);
        foreach (var flavor in indiSO.Flavors) Indis.Add(flavor.id, flavor);
    }
}
