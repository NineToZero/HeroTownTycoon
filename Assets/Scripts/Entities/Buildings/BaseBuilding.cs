using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IInteractable
{
    public BoxCollider Collider;
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;
    protected BuildingData _data;
    [SerializeField] protected Vector2Int _size;
    protected List<Action> _interactionActions;

    public BuildingActionType ActionType { get { return _data.ActionType; } }
    public BuildingType BuildingType { get { return _data.BuildingType; } }
    public BuildingType Upgrade { get { return _data.Upgrade; } }
    public string Name { get { return _data.Name; } }
    public string Desc { get { return _data.Desc; } }
    public int Tier { get { return _data.Tier; } }
    public Vector2Int Size { get { return _size; } }
    public InteractionButton[] Buttons { get { return _data.Buttons; } }

    public virtual void Interact()
    {
        var interactionUI = Managers.Instance.UIManager.OpenUI<InteractionUI>();
        interactionUI.Refresh(Buttons, this);
    }

    public virtual void Init(BuildingType type,bool isUpgrade = false)
    {
        _data = Managers.Instance.DataManager.BaseBuildings[type];
        gameObject.name = type.ToString();

        MeshFilter.mesh = Resources.Load<Mesh>($"{Const.Meshes_BuildingPath}/{type}");
        MeshRenderer.material = Resources.Load<Material>($"{Const.Materials_BuildingPath}/{type}");
        Collider.size = MeshFilter.mesh.bounds.size;
        Collider.center = MeshFilter.mesh.bounds.center;

        if ((transform.rotation.eulerAngles.y / 90) % 2 == 0)
            _size = new Vector2Int(_data.SizeX, _data.SizeY);
        else
            _size = new Vector2Int(_data.SizeY, _data.SizeX);
    }

    public virtual Vector3 DestroyOnGround()
    {
        gameObject.SetActive(false);
        return gameObject.transform.position;
    }

    public bool IsUpgradeToNextTier()
    {
        if (Upgrade == BuildingType.None)
            return false;

        Init(Upgrade,true);
        return true;
    }

    public string GetName()
    {
        return Name;
    }
}
