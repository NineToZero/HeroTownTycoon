using System;
using TMPro;
using UnityEngine;

public class CraftingUI : BaseUI
{
    private CraftingController _controller;
    [SerializeField] private RecipeContainerUI _recipeContainer;
    [SerializeField] private TMP_InputField _countInputField;


    public override void On()
    {
        base.On();
        _controller.CacheInventory();

        _controller.CraftingCount = 1;
    }

    public void Init(CraftingController controller)
    {
        _controller = controller;

        _recipeContainer.Init(_controller);
        _countInputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        controller.OnChangeCraftingCountEvent += () => _countInputField.SetTextWithoutNotify(controller.CraftingCount.ToString());
        _countInputField.onValueChanged.AddListener(text => _controller.CraftingCount = (string.IsNullOrEmpty(text) || text.Equals("0")) ? 1 : Convert.ToInt32(text));
    }

    public void SetRecipes(BuildingType buildingType)
    {
        _recipeContainer.SetRecipes(buildingType);
    }
}