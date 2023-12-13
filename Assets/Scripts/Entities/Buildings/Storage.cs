public class Storage : BaseBuilding
{
    public Inventory StorageInventory;

    public override void Init(BuildingType type, bool isUpgrade = false)
    {
        base.Init(type);
        if (isUpgrade == false)
        {
            StorageInventory = new Inventory(60);
        }
        else
        {
            return;
        }
    }

    public void Load(StorageSaveData data)
    {
        Init(data.BuildingType);
        StorageInventory = data.inventory;
    }
    
    public override void Interact()
    {
        base.Interact();
    }
}