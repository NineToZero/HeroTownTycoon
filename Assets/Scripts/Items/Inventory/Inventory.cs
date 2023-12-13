using System.Collections.Generic;

[System.Serializable]
public class Inventory
{
    [ES3Serializable]
    public List<Slot> Slots;

    public Inventory()
    {
        //Only for Easy Save 3 !!!
    }
    
    public Inventory(int numSlots)
    {
        Slots = new List<Slot>(numSlots);
        
        for (int i = 0; i < numSlots; ++i)
        {
            Slot slot = new Slot();
            Slots.Add(slot);
        }
    }
    
    public bool Add(int id)
    {
        foreach (Slot slot in Slots)
        {
            if (slot.id == id && slot.CanAddItem(1))
            {
                slot.AddItem(id);
                return true;
            }
        }

        foreach (Slot slot in Slots)
        {
            if (slot.id == 0)
            {
                slot.AddItem(id);
                return true;
            }
        }

        return false;
    }

    public bool TryAdd(int id, int count, out int remain)
    {
        foreach (Slot slot in Slots)
        {
            if (slot.id == id)
            {
                int capacity = slot.GetCapacity();
                int quantity = count > capacity ? capacity : count;

                slot.AddItem(id, quantity);
                count -= quantity;

                if (count == 0)
                {
                    remain = count;
                    return true;
                }
            }
        }

        foreach (Slot slot in Slots)
        {
            if (slot.id == 0)
            {
                int capacity = slot.GetCapacity();
                int quantity = count > capacity ? capacity : count;

                slot.AddItem(id, quantity);
                count -= quantity;

                if (count == 0)
                {
                    remain = count;
                    return true;
                }
            }
        }

        remain = count;
        return false;
    }

    public void Remove(int id, int count)
    {
        foreach (Slot slot in Slots)
        {
            if (slot.id == id)
            {
                int reduction = slot.RemoveItem(count);
                if (reduction == count) return;
                else count -= reduction;
            }
        }
    }


}
