using System.Collections.Generic;
using UnityEngine;

public class SlotsContainterUI : MonoBehaviour
{
    public List<SlotUI> Slots;
    
    public void Init(Inventory inventory, int count)
    {
        Slots = new List<SlotUI>();
        SlotUI slotUIPrefab = Managers.Instance.DataManager.GetPrefab<SlotUI>(Const.Prefabs_SlotUI);
        
        for (int i = 0; i < count; ++i)
        {
            Slot slot = inventory.Slots[i];
        
            SlotUI slotUI = Instantiate(slotUIPrefab, transform);
            slotUI.Init(slot);
            Slots.Add(slotUI);
        }
    }
    public void Refresh(Inventory inventory)
    {
        for (int i = 0;i < inventory.Slots.Count; ++i)
        {
            Slots[i].Refresh(inventory.Slots[i]);
        }
    }
}
