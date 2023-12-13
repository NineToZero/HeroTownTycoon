using System;
using System.Collections.Generic;
using UnityEngine;

public class HerosController : MonoBehaviour
{
    public List<HeroHandler> HeroHandlers { get; private set; }
    [ES3NonSerializable]
    public int SelectedHeroIndex;

    public event Action OnChangeHeroEvnet;
    private void Awake()
    {
        HeroHandlers = ES3.Load(Const.Save_HeroSaveData, new List<HeroHandler>());
        foreach(HeroHandler hero in HeroHandlers) Managers.Instance.DayManager.DayChangeEvent += hero.DoDayEvent;

        Managers.Instance.DayManager.AfterDayChangeEvent += SaveAllHeroes;
        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Subscribe(CursorSource.Inventory, CursorSource.Hero, InteractHeroWithItem);
    }
    public void AddHero(HeroHandler heroHandler)
    {
        HeroHandlers.Add(heroHandler);
        OnChangeHeroEvnet?.Invoke();
        Managers.Instance.DayManager.DayChangeEvent += heroHandler.DoDayEvent;
        SaveAllHeroes();
    }

    public void InteractHeroWithItem(ItemIdQuantityPair item)
    {
        HeroHandler selectedHero = HeroHandlers[SelectedHeroIndex];

        switch (Util.GetItemType(item.ItemId))
        {
            case ItemType.DishType:
                DishData dish = Managers.Instance.DataManager.Items[item.ItemId] as DishData;
                if (!ReferenceEquals(dish, null))
                {
                    int quantity = item.Quantity;
                    if (selectedHero.IndividualityStat.FlavorCode == dish.Flavor) quantity *= 2;
                    for (int i = 0; i < quantity; ++i)
                        selectedHero.Eat(dish.Nutrients);

                    Managers.Instance.SoundManager.PlaySFX(SFXSource.Eating);
                }
                break;
            case ItemType.MedicineType:
                if (selectedHero.IsBusied)
                {
                    Managers.Instance.UIManager.ShowNotificationUI("용사가 마을에 있지 않습니다!");
                    Managers.Instance.ItemManager.SpawnCollectable(item.ItemId, transform.position, item.Quantity);
                }
                else
                {
                    MedicineData medicine = Managers.Instance.DataManager.Items[item.ItemId] as MedicineData;
                    selectedHero.Cure(medicine.HealthValue);
                }
                break;
        }

        OnChangeHeroEvnet?.Invoke();
    }

    private void SaveAllHeroes()
    {
        ES3.Save(Const.Save_HeroSaveData, HeroHandlers);
    }

    public void RemoveHero(HeroHandler heroHandler)
    {
        HeroHandlers.Remove(heroHandler);
    }
}
