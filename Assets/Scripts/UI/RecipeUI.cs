using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    private RecipeData _recipeData;
    private CraftingController _controller;

    [SerializeField] private RecipeSlotUI _output;
    [SerializeField] private RecipeSlotUI[] _inputs;
    [SerializeField] private Button _craftBtn;
    [SerializeField] private TextMeshProUGUI _craftBtnName;

    public void Init(CraftingController controller)
    {
        _controller = controller;
        controller.OnCacheInventoryEvent += UpdateInputData;
        _craftBtn.onClick.AddListener(() => { controller.TryCraft(_recipeData); });
        controller.OnChangeCraftingCountEvent += ApplyCount;
    }

    public void Refresh(RecipeData recipeData, BuildingType buildingType)
    {
        _recipeData = recipeData;

        foreach (var input in _inputs) { input.gameObject.SetActive(false); }

        _output.Init(_recipeData.Output);
        for (int i = 0; i < _recipeData.Inputs.Length; i++)
        {
            _inputs[i].Init(_recipeData.Inputs[i], _controller.GetQuantity(_recipeData.Inputs[i].ItemId));
            _inputs[i].gameObject.SetActive(true);
        }

        _craftBtnName.text = buildingType switch
        {
            BuildingType.Countertop1 or BuildingType.Countertop2 or BuildingType.Countertop3 => "조리",
            _ => "제작"
        } ;
    }

    public void UpdateInputData()
    {
        for (int i = 0; i < (_recipeData.Inputs.Length > _inputs.Length ? _inputs.Length : _recipeData.Inputs.Length); i++) {
            _inputs[i].Updated(_controller.GetQuantity(_recipeData.Inputs[i].ItemId), _controller.CraftingCount);
        }
    }

    public void ApplyCount()
    {
        _output.Updated(-1, _controller.CraftingCount);
        UpdateInputData();
    }
}