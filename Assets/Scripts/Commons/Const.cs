using System.Collections.Generic;
using UnityEngine;

public abstract class Const
{
    #region Jsons
    public const string Item_Data = "Jsons/ItemData";
    public const string Jsons_BaseBuilding = "Jsons/Buildings/BuildingData";
    public const string Jsons_PassiveBuilding = "Jsons/Buildings/PassiveBuildingData";
    public const string Jsons_BuildingInteraction = "Jsons/Buildings/BuildingInteractionData";
    public const string Recipe_Data = "Jsons/RecipeData";
    public const string ShopItems_Data = "Jsons/ShopItemsData";
    #endregion

    #region SaveKey
    public const string Save_DaySaveData = "DaySaveData";
    public const string Save_InventorySaveData = "InventorySaveData";
    public const string Save_GoldSaveData = "GoldSaveData";
    public const string Save_HeroSaveData = "HeroSaveData";
    public const string Save_BuildingSaveDatas = "BuildingSaveDatas";
    public const string Save_FarmlandSaveDatas = "FarmlandSaveDatas";
    public const string Save_StorageSaveDatas = "StorageSaveDatas";
    #endregion

    #region Meshes
    public const string Meshes_BuildingPath = "Meshes/Building";
    #endregion

    #region Meterials
    public const string Materials_BuildingPath = "Materials/Building";
    public const string Materials_Blueprint_false = "Materials/Building/Blueprint_false";
    public const string Materials_Blueprint_true = "Materials/Building/Blueprint_true";
    #endregion


    #region Prefabs
    public const string Prefabs_ActiveBuilding = "Prefabs/Building/ActiveBuilding";
    public const string Prefabs_BuildController = "Prefabs/Building/BuildController";
    public const string Prefabs_Farmland = "Prefabs/Building/Farmland";
    public const string Prefabs_PassiveBuilding = "Prefabs/Building/PassiveBuilding";
    public const string Prefabs_Storage = "Prefabs/Building/Storage";
    public const string Prefabs_TitleBG = "Prefabs/TitleBG";
    public const string Prefabs_Unit = "Prefabs/Unit";
    public const string Prefabs_Player = "Prefabs/Player";
    public const string Prefabs_Collectable = "Prefabs/Item/Collectable";
    public const string Prefabs_BattleTile = "Prefabs/BattleTile";

    public const string Prefabs_UIPath = "Prefabs/UI";
    public const string Prefabs_StorageUI = "Prefabs/UI/StorageUI";

    public const string Coin = "Images/Coin";

    public const string Prefabs_CrosshairUI = "Prefabs/UI/SubUI/CrosshairUI";
    public const string Prefabs_SlotUI = "Prefabs/UI/SubUI/SlotUI";
    public const string Prefabs_ShopSlotUI = "Prefabs/UI/SubUI/ShopSlotUI";
    public const string Prefabs_RecipeUI = "Prefabs/UI/SubUI/RecipeUI";
    public const string Prefabs_ObjectPointingArrow = "Prefabs/UI/SubUI/ObjectPointingArrow";
    public const string Prefabs_UIButton = "Prefabs/UI/SubUI/UIButton";
    public const string Prefabs_TextSlot = "Prefabs/UI/SubUI/TextSlot";
    public const string Prefabs_SubInventoryUI = "Prefabs/UI/SubUI/SubInventoryUI";
    public const string Prefabs_PartySlot = "Prefabs/UI/SubUI/PartySlot";
    public const string Prefabs_ImageSlot = "Prefabs/UI/SubUI/Result_ImageSlot";
    public const string Prefabs_HeroSlot = "Prefabs/UI/SubUI/Result_HeroSlot";
    public const string Prefabs_UnitUI = "Prefabs/UI/SubUI/UnitUI";

    public const string Prefabs_TownMap = "Prefabs/Map/TownMap";
    #endregion


    #region ScriptableObjects

    public const string SO_BuildingSO = "ScriptableObjects/Buildings/BuildingSO";

    public const string SO_ItemImageSO = "ScriptableObjects/Images/ItemImages";
    public const string SO_NullImageSO = "ScriptableObjects/Images/NullImage";
    public const string SO_CropsImageSO = "ScriptableObjects/Images/CropsImages";
    public const string SO_InteractionIcons = "ScriptableObjects/Images/InteractionIcon";
    

    public const string SO_Crops = "ScriptableObjects/Crops";

    public const string SO_Stage = "ScriptableObjects/Stages";

    public const string SO_Hero_Combat = "ScriptableObjects/Stats/HeroStat";
    public const string SO_Hero_Health = "ScriptableObjects/Stats/BaseHealthStat";
    public const string SO_Hero_Individuality = "ScriptableObjects/Stats/IndividualityStat";

    public const string SO_Synergy = "ScriptableObjects/Synergy";

    public const string SO_Enemy = "ScriptableObjects/Enemies";

    public const string SO_Event = "ScriptableObjects/EventEntry";

    public const string SO_CharacterSprites = "CharacterSprites";

    #endregion

    #region Audio
    public const string Sound_SFX = "Sounds/SFX";
    public const string Sound_Music = "Sounds/Music";
    #endregion

    #region Sprites

    #endregion

    public static readonly Dictionary<int, List<Vector2Int>> Dirs = new Dictionary<int, List<Vector2Int>>
    {
        {1, new List<Vector2Int> {
            new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1),
        }},
        {2, new List<Vector2Int> {
            new Vector2Int(2, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(-1, 1), new Vector2Int(-2, 0), new Vector2Int(0, -2), new Vector2Int(-1, -1), new Vector2Int(1, -1),
        }},
        {3, new List<Vector2Int> {
            new Vector2Int(3, 0), new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(0, 3), new Vector2Int(-1, 2), new Vector2Int(-2, 1), new Vector2Int(-3, 0), new Vector2Int(0, -3), new Vector2Int(-1, -2), new Vector2Int(-2, -1), new Vector2Int(2, -1), new Vector2Int(1, -2),
        }},
        {4, new List<Vector2Int> {
            new Vector2Int(4, 0), new Vector2Int(3, 1), new Vector2Int(2, 2), new Vector2Int(1, 3), new Vector2Int(0, 4), new Vector2Int(-1, 3), new Vector2Int(-2, 2), new Vector2Int(-3, 1), new Vector2Int(-4, 0), new Vector2Int(0, -4), new Vector2Int(-1, -3), new Vector2Int(-2, -2), new Vector2Int(-3, -1), new Vector2Int(3, -1), new Vector2Int(2, -2), new Vector2Int(1, -3),
        }},
        {5, new List<Vector2Int> {
            new Vector2Int(5, 0), new Vector2Int(4, 1), new Vector2Int(3, 2), new Vector2Int(2, 3), new Vector2Int(1, 4), new Vector2Int(0, 5), new Vector2Int(-1, 4), new Vector2Int(-2, 3), new Vector2Int(-3, 2), new Vector2Int(-4, 1), new Vector2Int(-5, 0), new Vector2Int(0, -5), new Vector2Int(-1, -4), new Vector2Int(-2, -3), new Vector2Int(-3, -2),  new Vector2Int(-4, -1), new Vector2Int(4, -1), new Vector2Int(3, -2), new Vector2Int(2, -3), new Vector2Int(1, -4),
        }},
        {6, new List<Vector2Int> {
            new Vector2Int(6, 0), new Vector2Int(5, 1), new Vector2Int(4, 2), new Vector2Int(3, 3), new Vector2Int(2, 4), new Vector2Int(1, 5), new Vector2Int(0, 6), new Vector2Int(-1, 5), new Vector2Int(-2, 4), new Vector2Int(-3, 3), new Vector2Int(-4, 2), new Vector2Int(-5, 1), new Vector2Int(-6, 0), new Vector2Int(0, -6), new Vector2Int(-1, -5), new Vector2Int(-2, -4), new Vector2Int(-3, -3),  new Vector2Int(-4, -2),  new Vector2Int(-5, -1), new Vector2Int(5, -1),  new Vector2Int(4, -2), new Vector2Int(3, -3), new Vector2Int(2, -4), new Vector2Int(1, -5),
        }},
        {7, new List<Vector2Int> {
            new Vector2Int(7, 0), new Vector2Int(6, 1), new Vector2Int(5, 2), new Vector2Int(4, 3), new Vector2Int(3, 4), new Vector2Int(2, 5), new Vector2Int(1, 6), new Vector2Int(0, 7), new Vector2Int(-1, 6), new Vector2Int(-2, 5), new Vector2Int(-3, 4), new Vector2Int(-4, 3), new Vector2Int(-5, 2), new Vector2Int(-6, 1), new Vector2Int(-7, 0), new Vector2Int(0, -7), new Vector2Int(-1, -6), new Vector2Int(-2, -5), new Vector2Int(-3, -4),  new Vector2Int(-4, -3),  new Vector2Int(-5, -2), new Vector2Int(-6, -1), new Vector2Int(6, -1),  new Vector2Int(5, -2), new Vector2Int(4, -3), new Vector2Int(3, -4), new Vector2Int(2, -5), new Vector2Int(1, -6),
        }},
    };
    #region PlayerPrefs
    public const string PlayerPrefs_NewGame = "NewGame";
    public const string PlayerPrefs_MouseSensitivity = "MouseSensitivity";
    public const string PlayerPrefs_MasterToggle = "MasterToggle";
    public const string PlayerPrefs_MusicToggle = "MusicToggle";
    public const string PlayerPrefs_SFXToggle = "SFXToggle";
    public const string PlayerPrefs_MasterVolume = "MasterVolume";
    public const string PlayerPrefs_MusicVolume = "MusicVolume";
    public const string PlayerPrefs_SFXVolume = "SFXVolume";
    #endregion
}
