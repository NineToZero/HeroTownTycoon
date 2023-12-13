using System;
using UnityEngine;

public class BuildingSaveData
{
    public BuildingType BuildingType;
    public Vector3 Position;
    public int Rotate90;

    public BuildingSaveData() { }
    public BuildingSaveData(BaseBuilding baseBuilding)
    {
        BuildingType = baseBuilding.BuildingType;
        Transform transform = baseBuilding.transform;
        Position = transform.position;
        Rotate90 = Convert.ToInt32(transform.rotation.y) / 90;
    }
}

public class StorageSaveData
{
    public BuildingType BuildingType;
    public Inventory inventory;
    public StorageSaveData() { }
    public StorageSaveData(Storage storage)
    {
        BuildingType = storage.BuildingType;
        inventory = storage.StorageInventory;
    }
}

public class FarmlandSaveData
{
    public int WaterRatio;
    public int FertileRatio;
    public int PesticideRatio;

    public int CropId;
    public int CropWater;
    public int CropNutrition;
    public int CropPest;
    public GrowStep GrowStep;
    public int DayCount;
    public int CropsScore;
    
    public FarmlandSaveData() { }
    public FarmlandSaveData(Farmland farmland)
    {
        WaterRatio = farmland.WaterRatio;
        FertileRatio = farmland.FertileRatio;
        PesticideRatio = farmland.PesticideRatio;

        Crops crops = farmland.Crop;
        if (crops.gameObject.activeSelf == false) CropId = -1;
        else {
        CropId = crops.CropId;
        CropWater = crops.MyWater;
        CropNutrition = crops.MyNutrition;
        CropPest = crops.MyPest;
        GrowStep = crops.GrowStepType;
        DayCount = crops.DayCount;
        CropsScore = crops.CropsScore;
        }
    }
}
