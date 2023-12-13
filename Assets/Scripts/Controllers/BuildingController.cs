using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public InventoryController InventoryController { get; set; }

    public BlueprintHandler BlueprintHandler;
    private List<Mesh> _meshData;
    private List<Material> _materialData;
    private Transform _rootBuildingObj;

    private Dictionary<BuildingActionType, BaseBuilding> _buildingPrefabList;
    private Dictionary<BuildingType, List<BaseBuilding>> _buildingList;

    #region UnityEvent
    private void Awake()
    {
        Init();

        Managers.Instance.DayManager.DayChangeEvent += ChangeBuildingFromBlueprint;
        Managers.Instance.DayManager.AfterDayChangeEvent += SaveAllBuildings;
    }
    #endregion

    #region Build
    public void Init()
    {
        _rootBuildingObj = new GameObject("Buildings").transform;
        _buildingList = new();
        _meshData = new()
        {

        };
        _materialData = new()
        {

        };

        _buildingPrefabList = new()
        {
            { BuildingActionType.Farmland, Resources.Load<BaseBuilding>(Const.Prefabs_Farmland) },
            { BuildingActionType.Passive, Resources.Load<BaseBuilding>(Const.Prefabs_PassiveBuilding) },
            { BuildingActionType.Active, Resources.Load<BaseBuilding>(Const.Prefabs_ActiveBuilding) },
            { BuildingActionType.Storage, Resources.Load<BaseBuilding>(Const.Prefabs_Storage) }
        };
        Managers.Instance.UIManager.GetUI<InteractionUI>();
        Managers.Instance.UIManager.CloseUI<InteractionUI>();
    }

    public void OnBlueprintHandler(BuildingType type, Action action) // 커밋 전에 실제 사용 용도로 바꿀 것
    {
        var buildingData = Managers.Instance.DataManager.BaseBuildings[type];
        var size = new Vector2Int(buildingData.SizeX, buildingData.SizeY);
        var mesh = Resources.Load<Mesh>($"{Const.Meshes_BuildingPath}/{type}");

        BlueprintHandler.On(type, size, mesh, action);
    }

    public void OffBlueprintHandler()
    {
        BlueprintHandler.Off();
    }

    private void ChangeBuildingFromBlueprint()
    {
        var blueprintQueue = BlueprintHandler.EnabledBluePrints;
        while (blueprintQueue.Count != 0)
        {
            var blueprintBuilding = blueprintQueue.Dequeue();
            CreateBuilding(blueprintBuilding);
            BlueprintHandler.DisabledBlueprints.Enqueue(blueprintBuilding);
            blueprintBuilding.gameObject.SetActive(false);
        }
    }

    private BaseBuilding CreateBuilding(BuildingType type)
    {
        BaseBuilding obj = null;

        if (_buildingList.TryGetValue(type, out List<BaseBuilding> list))
        {
            foreach (var building in list)
            {
                if (building.gameObject.activeSelf)
                    continue;

                obj = building;
                break;
            }
        }

        if (obj == null)
        {
            BuildingActionType actionType = ConvertBuildTypeToActionType(type);
            obj = Instantiate(_buildingPrefabList[actionType], _rootBuildingObj);
            if (!_buildingList.ContainsKey(type))
                _buildingList.Add(type, new List<BaseBuilding>());
            _buildingList[type].Add(obj);
        }


        obj.name = $"{type}";

        return obj;
    }

    private BaseBuilding CreateBuilding(BuildingType type, Vector3 position, int rotate90Count = 0)
    {
        BaseBuilding obj = CreateBuilding(type);
        Quaternion Rotate = Quaternion.Euler(0, rotate90Count * 90, 0);
        obj.transform.SetPositionAndRotation(position, Rotate);
        obj.Init(type);
        return obj;
    }

    private BaseBuilding CreateBuilding(BlueprintBuilding blueprint)
    {
        BaseBuilding obj = CreateBuilding(blueprint.Type);
        var blueprintTransform = blueprint.transform;
        obj.transform.SetPositionAndRotation(blueprintTransform.position, blueprintTransform.rotation);
        obj.Init(blueprint.Type);

        return obj;
    }

    public void CompleteCreateBuilding(BuildingType type, Vector3 pos, int rotate90)
    {
        pos = BlueprintHandler.CompleteCreateBuilding(type, pos);
        CreateBuilding(type, pos, rotate90);
    }
    #endregion

    #region Interaction
    public void ClickInteractButton(BaseBuilding target, int index)
    {
        switch (target.Buttons[index])
        {
            case InteractionButton.Cancel:
                return;
            case InteractionButton.Info:
                InfoBuilding(target);
                break;
            case InteractionButton.Destroy:
                Managers.Instance.UIManager.ShowPopupUI(() =>{ DestroyBuilding(target); }, "경고", "파괴하시겠습니까?");
                ;
                break;
            case InteractionButton.Upgrade:
                Managers.Instance.UIManager.ShowPopupUI(() => { UpgradeBuilding(target, InventoryController.Inventory); });
                break;
            case InteractionButton.Craft:
                // TODO : 농사 도구 제작 UI 연결
                break;
            case InteractionButton.Medicine:
            case InteractionButton.Cook:
                var craftingUI = Managers.Instance.UIManager.OpenUI<CraftingUI>();
                craftingUI.SetRecipes(target.BuildingType);
                break;
            case InteractionButton.Conquest:
                Managers.Instance.UIManager.OpenUI<ConquestUI>();
                break;
            case InteractionButton.Travel:
                // TODO : 모험 UI 연결
                break;
            case InteractionButton.Trade:
                var shopUI = Managers.Instance.UIManager.OpenUI<ShopUI>();
                shopUI.SetShopItems(target.BuildingType);
                break;
            case InteractionButton.Keep:
                Managers.Instance.UIManager.GetUI<StorageUI>().SetStorage(target as Storage);
                Managers.Instance.UIManager.OpenUI<StorageUI>();
                break;
            case InteractionButton.Cure:
                // TODO : 창고 UI 연결
                break;
            case InteractionButton.Employ:
                Managers.Instance.UIManager.OpenUI<HeroUI>();
                break;
        }
    }

    public void DestroyBuilding(BaseBuilding target)
    {
        Vector3 pos = target.DestroyOnGround();
        BlueprintHandler.RemoveBuiltArea(pos, target.Size);
        Managers.Instance.UIManager.ShowNotificationUI("건물을 파괴하였습니다.");

    }

    public void UpgradeBuilding(BaseBuilding target, Inventory inventory)
    {
        var um = Managers.Instance.UIManager;
        if (target.Upgrade == BuildingType.None)
        {
            um.ShowNotificationUI("건물 승급에 실패하였습니다.");
            return;
        }

        var slot = inventory.Slots;
        var datas = Managers.Instance.DataManager.Items;
        bool isFind = false;
        foreach (var item in slot)
        {
            if (item.id == 0
                 || datas[item.id] is not BlueprintData data
                 || data.BuildingType != target.Upgrade)
                continue;

            item.RemoveItem(1);
            isFind = true;
            break;
        }

        if (isFind == false)
        {
            um.ShowNotificationUI("건물 승급에 실패하였습니다.");
            return;
        }

        target.IsUpgradeToNextTier();
        um.ShowNotificationUI("건물 승급에 성공하였습니다.");

        var guideUI = um.GetUI<InteractionGuideUI>();
        guideUI.Refresh(target.gameObject);
    }

    public void InfoBuilding(BaseBuilding target)
    {
        var ui = Managers.Instance.UIManager.OpenUI<BuildingInfoUI>();
        ui.Refresh(target.Name, target.Tier.ToString(), target.Desc);
    }
    #endregion

    #region Save
    private void SaveAllBuildings()
    {
        List<BuildingSaveData> buildingSaveDatas = new();
        List<FarmlandSaveData> farmlandSaveDatas = new();
        List<StorageSaveData> storageSaveDatas = new();
        foreach (var list in _buildingList)
        {
            foreach (BaseBuilding building in list.Value)
            {
                if (building.gameObject.activeSelf == false) continue;
                buildingSaveDatas.Add(new BuildingSaveData(building));
                switch (building.BuildingType)
                {
                    case BuildingType.Farmland:
                        farmlandSaveDatas.Add(new FarmlandSaveData(building as Farmland));
                        break;
                    case BuildingType.Storage1:
                    case BuildingType.Storage2:
                    case BuildingType.Storage3:
                        storageSaveDatas.Add(new StorageSaveData(building as Storage));
                        break;
                }
            }
        }

        ES3.Save(Const.Save_BuildingSaveDatas, buildingSaveDatas);
        ES3.Save(Const.Save_FarmlandSaveDatas, farmlandSaveDatas);
        ES3.Save(Const.Save_StorageSaveDatas, storageSaveDatas);
    }

    public void LoadAllBuildings()
    {
        List<BuildingSaveData> buildingSaveDatas = ES3.Load(Const.Save_BuildingSaveDatas, new List<BuildingSaveData>());
        if (buildingSaveDatas.Count <= 0)
        {
            LoadDefaultBuildings();
            return;
        }

        foreach (BuildingSaveData data in buildingSaveDatas)
            CompleteCreateBuilding(data.BuildingType, data.Position, data.Rotate90);

        List<FarmlandSaveData> farmlandSaveDatas = ES3.Load<List<FarmlandSaveData>>(Const.Save_FarmlandSaveDatas);
        if (farmlandSaveDatas.Count > 0)
        {
            List<BaseBuilding> farmlandList = _buildingList[BuildingType.Farmland];
            if (farmlandList.Count != farmlandSaveDatas.Count) Debug.LogWarning("Farmland Save Data have been corrupted");
            for (int i = 0; i < farmlandList.Count; ++i)
            {
                Farmland farmland = farmlandList[i] as Farmland;
                farmland.Load(farmlandSaveDatas[i]);
            }
        }

        List<StorageSaveData> storageSaveDatas = ES3.Load<List<StorageSaveData>>(Const.Save_StorageSaveDatas);
        if (storageSaveDatas.Count > 0)
        {
            Dictionary<BuildingType, int> idx = new();
            idx.Add(BuildingType.Storage1, 0);
            idx.Add(BuildingType.Storage2, 0);
            idx.Add(BuildingType.Storage3, 0);
            foreach (StorageSaveData data in storageSaveDatas)
            {
                BuildingType type = data.BuildingType;
                Storage storage = _buildingList[type][idx[type]++] as Storage;
                storage.Load(data);
            }
        }
    }

    private void LoadDefaultBuildings()
    {
        CompleteCreateBuilding(BuildingType.Well, new Vector3(5, 0, 5), 0);
        CompleteCreateBuilding(BuildingType.Farmland, new Vector3(3, 0, 4), 0);
        CompleteCreateBuilding(BuildingType.Farmland, new Vector3(3, 0, 5), 0);
        CompleteCreateBuilding(BuildingType.Farmland, new Vector3(3, 0, 6), 0);
        CompleteCreateBuilding(BuildingType.Storage1, new Vector3(9, 0, 9), 0);

        CompleteCreateBuilding(BuildingType.Hospital1, new Vector3(12, 0, 12), 0);
        CompleteCreateBuilding(BuildingType.Town, new Vector3(-5, 0, 6), 0);
        CompleteCreateBuilding(BuildingType.ConquestGuild, new Vector3(-5, 0, 3), 0);
        CompleteCreateBuilding(BuildingType.Countertop1, new Vector3(-5, 0, 0), 0);
        CompleteCreateBuilding(BuildingType.Store1, new Vector3(-5, 0, -3), 0);
    }
    #endregion

    #region Util
    public BuildingActionType ConvertBuildTypeToActionType(BuildingType type)
    {
        switch ((int)type)
        {
            case > 4000:
                return BuildingActionType.Storage;
            case > 3000:
                return BuildingActionType.Active;
            case > 2000: // 2001 : 패시브 건물 시작 번호 
                return BuildingActionType.Passive;
            case > 1000: // 3001 : 액션 건물 시작 번호
            default:
                return BuildingActionType.Farmland;
        }

    }
    #endregion
}
