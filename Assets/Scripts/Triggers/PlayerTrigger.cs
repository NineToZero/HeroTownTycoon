using Cinemachine;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Vector3 _initialCameraPosition;
    [SerializeField] private Vector3 _initialCameraRotation;

    public Inventory PlayerInventory;
    private void Awake()
    {
        GameObject TownMap = Resources.Load<GameObject>(Const.Prefabs_TownMap);
        Instantiate(TownMap).name = "TownMap";

        GameObject playerPrefab = Managers.Instance.DataManager.GetPrefab(Const.Prefabs_Player);
        GameObject spawnedPlayer = Instantiate(playerPrefab);

        PlayerControllerHandler controllerHandler = spawnedPlayer.AddComponent<PlayerControllerHandler>();
        controllerHandler.MovementController.Init(_virtualCamera);
        
        BuildingController buildingController = InitializeBuilding();
        buildingController.InventoryController = controllerHandler.InventoryController;
        controllerHandler.BuildingController = buildingController;
        controllerHandler.UIController.BuildingController = buildingController;

        Managers.Instance.UIManager.OpenUI<KeyGuideUI>();

        Managers.Instance.UIManager.Init(controllerHandler);

        Managers.Instance.SoundManager.PlayMusic(MusicSource.LikeAnAnt);
    }

    private BuildingController InitializeBuilding()
    {
        BuildingController controller = Managers.Instance.DataManager
        .GetPrefab(Const.Prefabs_BuildController).GetComponent<BuildingController>();

        BuildingController obj = Instantiate(controller);
        obj.name = obj.GetType().Name;
        
        obj.LoadAllBuildings();
        
        return obj;
    }
}
