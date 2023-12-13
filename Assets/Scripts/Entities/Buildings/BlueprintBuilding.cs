using System;
using UnityEngine;

public class BlueprintBuilding : MonoBehaviour, IInteractable
{
    private string _name = "청사진";
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private BoxCollider _boxCollider;
    public BuildingType Type;
    public Vector2Int Size;
    public event Action<BlueprintBuilding> InteractAction;

    public void Init(BuildingType type,Vector2Int size, Mesh mesh,Action<BlueprintBuilding> method)
    {
        Type = type;
        Size = size;
        _meshFilter.mesh = mesh;
        InteractAction = method;

        _boxCollider.size = mesh.bounds.size;
        _boxCollider.center = mesh.bounds.center;
    }

    public void Interact()
    {
        Managers.Instance.UIManager.ShowPopupUI(CancelBlueprint,"취소","청사진을 취소하시겠습니까?");
    }

    private void CancelBlueprint()
    {
        int itemCode = Util.GetBlueprintItemCode(Type);
        Vector3 pos = transform.position;
        Managers.Instance.ItemManager.SpawnCollectable(itemCode,pos);
        InteractAction(this);
        gameObject.SetActive(false);
    }

    public string GetName()
    {
        return _name;
    }
}
