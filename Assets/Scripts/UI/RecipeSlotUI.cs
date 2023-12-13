using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemIdQuantityPair _recipe;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantity;
    [SerializeField] private Image _slot;

    public void Init(ItemIdQuantityPair recipe, int handQuantity = -1)
    {
        _recipe = recipe;

        _icon.sprite = Managers.Instance.DataManager.GetItemSprites(_recipe.ItemId);

        _quantity.text = handQuantity == -1 ? _recipe.Quantity == 1 ? string.Empty : _recipe.Quantity.ToString() : $"{handQuantity}/{_recipe.Quantity}";
    }
    public void Updated(int handQuantity, int count = 1)
    {
        _quantity.text = handQuantity == -1 ? _recipe.Quantity * count == 1 ? string.Empty :(_recipe.Quantity * count).ToString() : $"{handQuantity}/{_recipe.Quantity * count}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slot.color = Color.gray;
        Managers.Instance.UIManager.OpenUI<ItemInfoUI>().Refresh(_recipe.ItemId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _slot.color = Color.white;
        Managers.Instance.UIManager.CloseUI<ItemInfoUI>();
    }
}