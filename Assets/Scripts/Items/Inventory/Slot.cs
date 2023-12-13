using System;

[Serializable]
public class Slot
{
    public int id;
    public int curStack;
    public int maxStack;

    public event Action<int> ItemChanged;
    public event Action<int> ItemCountChanged;
    
    public Slot()
    {
        ResetSlot();
    } 
    
    private void ResetSlot()
    {
        id = 0;
        curStack = 0;
        maxStack = 99;
        
        ItemChanged?.Invoke(0);
        ItemCountChanged?.Invoke(0);
    }

    public bool CanAddItem(int count)
    {
        if (curStack + count <= maxStack)
            return true;
        return false;
    }

    public void AddItem(int itemId)
    {
        if (id == 0)
        {
            id = itemId;
            ItemChanged?.Invoke(itemId);
        }
        // TODO : id를 통해 maxStack 초기화
        ItemCountChanged?.Invoke(++curStack);
    }
    
    public void AddItem(int itemId, int count)
    {
        if (id == 0)
        {
            id = itemId;
            ItemChanged?.Invoke(itemId);
        }
        // TODO : id를 통해 maxStack 초기화
        curStack += count;
        ItemCountChanged?.Invoke(curStack);
    }

    public int RemoveItem()
    {
        // Remove All Item
        int count = curStack;
        ResetSlot();
        return count;
    }
    
    public int RemoveItem(int count)
    {   
        int reduction = count > curStack ? curStack : count;
        // must reserve that we always get available count number as parameter.
        curStack -= count;
        
        if (curStack <= 0)
            ResetSlot();

        ItemCountChanged?.Invoke(curStack);
        return reduction;
    }

    public void UseBlurprint()
    {
        RemoveItem(1);
    }

    public int GetCapacity()
    {
        return maxStack - curStack;
    }
}
