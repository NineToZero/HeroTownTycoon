using System;

[Serializable]
public class BuildingData
{
    public BuildingType BuildingType;
    public BuildingActionType ActionType;
    public BuildingType Upgrade;
    public string Name;
    public string Desc;
    public int Tier;
    public int SizeX;
    public int SizeY;
    public InteractionButton[] Buttons;

}

[Serializable]
public class PassiveBuildingData
{
    public BuildingType BuildingType;
    public BuildingPassiveType PassiveType;
    public float Range;
    public float Value;
}