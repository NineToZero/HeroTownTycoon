#region Enum

using System.Collections.Generic;

public enum SceneType
{
    Title = 0,
    Game = 1,
    Load = 2
}

public enum FarmingItemType
{
    Moisture,
    Fertile,
    Pesticide
}

public enum ItemType
{
    DishType,
    EtcType,
    FarmingitemType,
    MedicineType,
    SeedsType,
    BlueprintType,
    NoneType
}

public enum Nutrients
{
    Carbs,
    Protein,
    Fat,
    Vitamin,
}

public enum Stats
{
    MaxHealth,
    GenHealth,
    InitMana,
    MaxMana,
    GenMana,
    AtkPower,
    AtkRange,
    AtkSpeed,
    MagPower,
    CriticalChance,
    CriticalDamage,
    DodgeChance,
    Armor,

    CurHealth,
    CurMana,
}

public enum GrowStep
{
    start,
    growing,
    harvestable
}

public enum BuildingActionType { Farmland, Passive, Active, Storage }

public enum BuildingType
{
    None            = 0,
    Farmland        = 1001,
    Well            = 2001,
    House           = 2002,
    Town            = 3001,
    ConquestGuild   = 3002,
    TravelerGuild   = 3003,
    Countertop1     = 3004,
    Countertop2     = 3005,
    Countertop3     = 3006,
    Hospital1       = 3007,
    Hospital2       = 3008,
    Hospital3       = 3009,
    Workshop1       = 3010,
    Workshop2       = 3011,
    Workshop3       = 3012,
    Store1          = 3013,
    Store2          = 3014,
    Store3          = 3015,
    Storage1        = 4001,
    Storage2        = 4002,
    Storage3        = 4003,
}

public enum BuildingPassiveType
{
    Water           = 1001,
    HeroCount       = 2001,
}

public enum UIType
{
    Popup, // ī�޶� ���, ���콺 ��Ÿ��
    Scene, // ���� ����
    Notification, // ���� ���������� ���� ����
    Modal
}

public enum InteractionButton
{
    Cancel      = 0,
    Info        = 1,
    Destroy     = 2,
    Upgrade     = 3,
    Craft       = 4,
    Medicine    = 5,
    Cook        = 6,
    Conquest    = 7,
    Travel      = 8,
    Trade       = 9,
    Keep        = 10,
    Cure        = 11,
    Employ      = 12
}

public enum ModifyType
{
    Add,
    Override,
    Multiple,
}

public enum PhaseType
{
    Battle,
    Bonus,
}

public enum AnimationState
{
    Idle,
    Ready,
    Walking,
    Running,
    Jumping,
    Blocking,
    Crawling,
    Climbing,
    Dead
}

public enum AnimationTrigger
{
    Attack,
    Jab,
    Push,
    Hit,
    Slash,
    Shot,
    Fire1H,
    Fire2H,
    GetUp,
    GetDown,
    Landed,
    Heal
}

public enum CursorSource
{
    None,
    Inventory,
    Shop,
    Storage,
    Popup,
    Hero
}

public enum UnitState
{
    Idle,
    Move,
    Attack,
    Skill,
    Dead,
}

#endregion
#region Struct

[System.Serializable]
public struct NutrientValue
{
    public Nutrients nutrient;
    public int value;
}
[System.Serializable]
public struct StatValue
{
    public Stats Stat;
    public float Value;
}

[System.Serializable]
public struct Reward
{
    public int Gold;
    public List<int> Items;

    public Reward(List<int> items, int gold)
    {
        Items = items;
        Gold = gold; 
    }
    
    public void Add(Reward add)
    {
        Gold += add.Gold;

        if (Items == null) Items = new(add.Items);
        else Items.AddRange(add.Items);
    }
}

public struct ItemIdQuantityPair
{
    public int ItemId;
    public int Quantity;
}
#endregion