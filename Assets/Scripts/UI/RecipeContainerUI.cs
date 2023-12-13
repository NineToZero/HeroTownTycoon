using System.Collections.Generic;
using UnityEngine;

public class RecipeContainerUI : MonoBehaviour
{
    [SerializeField] private List<RecipeUI> _recipes;
    private CraftingController _controller;
    
    public void Init(CraftingController controller)
    {
        _controller = controller;
        _recipes = new();
    }

    public void SetRecipes(BuildingType buildingType)
    {
        DataManager dm = Managers.Instance.DataManager;
        RecipeData[] recipes = dm.GetRecipeDatas(buildingType);

        foreach(var recipe in _recipes) { recipe.gameObject.SetActive(false); }

        if(recipes.Length > _recipes.Count)
        {
            RecipeUI prefab = dm.GetPrefab<RecipeUI>(Const.Prefabs_RecipeUI);

            for (int i = _recipes.Count; i < recipes.Length; i++)
            {
                RecipeUI recipe = Instantiate(prefab, transform);
                recipe.Init(_controller);
                _recipes.Add(recipe);
            }
        }

        for (int i = 0; i < recipes.Length; i++)
        {
            _recipes[i].Refresh(recipes[i], buildingType);
            _recipes[i].gameObject.SetActive(true);
        }
    }
}