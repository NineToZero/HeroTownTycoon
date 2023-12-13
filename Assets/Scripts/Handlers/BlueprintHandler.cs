using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlueprintHandler : MonoBehaviour
{
    [Header("일반")]
    [SerializeField] private Transform _RootBlueprint;
    [SerializeField] private GameObject _blueprintPrefab;
    [SerializeField] private Grid _buildGrid;
    [SerializeField] private LayerMask _buildLayer;
    private HashSet<Vector2Int> _builtArea;
    private List<Vector2Int> _blueprintArea;
    private Camera _camera;

    [Header("건설 제한 거리")]
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    [Header("샘플 블루 프린트")]
    [SerializeField] private Transform _blueprint;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [HideInInspector] public Queue<BlueprintBuilding> EnabledBluePrints;
    [HideInInspector] public Queue<BlueprintBuilding> DisabledBlueprints;
    private List<Material> _blueprintMaterials;

    private BuildingType BuildingType;
    private Vector2Int _currentSize;
    private Vector2Int _forwardSize;
    private Vector3 _cameraCenter;
    private Vector3 _currentGridPos;
    private bool _isPlaceable;

    public event Action OnSuccessBuild;

    #region UnityEvent
    private void Awake()
    {
        _builtArea = new(64);
        _blueprintArea = new List<Vector2Int>(16);
        EnabledBluePrints = new();
        DisabledBlueprints = new();
        _camera = Camera.main;
        _blueprintMaterials = new()
        {
            Resources.Load<Material>($"{Const.Materials_Blueprint_false}"),
            Resources.Load<Material>($"{Const.Materials_Blueprint_true}")
        };
        this.Off();
    }

    private void Update()
    {
        if (!TryRaycast(out Vector3 pos))
            return;

        if (!IsChangeCellPosition(pos))
            return;

        ChangeRotation();
        SetBlueprintArea(_currentGridPos);
        CheckPlaceable();
        MoveBlueprint();
    }
    #endregion


    #region Method
    public void Init(BuildingType type, Vector2Int size, Mesh mesh)
    {
        BuildingType = type;
        _currentGridPos = Vector3.zero;
        _forwardSize = size;
        _meshFilter.mesh = mesh;
        _meshRenderer.material = _blueprintMaterials[Convert.ToInt32(_isPlaceable)];
        _cameraCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        SetBlueprintArea(_currentGridPos);
        CheckPlaceable();
        MoveBlueprint();
    }


    public void On(BuildingType type, Vector2Int size, Mesh mesh, Action action)
    {
        gameObject.SetActive(true);
        Init(type, size, mesh);
        Managers.Instance.SoundManager.PlaySFX(SFXSource.Build);
        OnSuccessBuild = action;
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void IsSuccessBlueprint()
    {
        if (!gameObject.activeSelf)
            return;

        if (!_isPlaceable)
        {
            Managers.Instance.UIManager.ShowNotificationUI("건설 실패하였습니다.");
            return;
        }

        Managers.Instance.SoundManager.PlaySFX(SFXSource.Coin);
        SetBlueprintBuilding(_currentGridPos,_currentSize,_blueprint.rotation);
        ConvertBuiltArea(_blueprintArea);
        OnSuccessBuild?.Invoke();
        Off();
    }

    private bool TryRaycast(out Vector3 pos)
    {
        pos = default;
        Ray ray = Camera.main.ScreenPointToRay(_cameraCenter);
        if (!Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _buildLayer)
            || hit.distance < _minDistance)
            return false;

        pos = hit.point;
        return true;
    }

    private bool IsChangeCellPosition(Vector3 pos)
    {
        Vector3Int nextGridPos = _buildGrid.WorldToCell(pos);
        _currentGridPos = _buildGrid.GetCellCenterWorld(nextGridPos);

        if (_currentGridPos == nextGridPos)
            return false;

        if (_currentSize.x % 2 == 0)
            _currentGridPos.x -= 0.5f;
        if (_currentSize.y % 2 == 0)
            _currentGridPos.z -= 0.5f;

        return true;
    }

    private void SetBlueprintArea(Vector3 pos)
    {
        _blueprintArea.Clear();

        int wolrdX = (int)(pos.x);
        int wolrdZ = (int)(pos.z);
        if (pos.x < 0 && _currentSize.x % 2 == 1)
            wolrdX--;
        if (pos.z < 0 && _currentSize.y % 2 == 1)
            wolrdZ--;

        for (int z = 0; z < _currentSize.y; z++)
        {
            for (int x = 0; x < _currentSize.x; x++)
            {
                int _x = wolrdX + x - (int)(_currentSize.x * 0.5f);
                int _y = wolrdZ + z - (int)(_currentSize.y * 0.5f);
                _blueprintArea.Add(new Vector2Int(_x, _y));
            }
        }
    }

    private void ChangeRotation()
    {
        Vector3 vector3Dir = (_camera.transform.position - _blueprint.transform.position);
        vector3Dir.y = 0;
        vector3Dir.Normalize();
        float vector3DirY = Quaternion.LookRotation(vector3Dir).eulerAngles.y;
        int dirStep = (int)math.round(vector3DirY / 90f);
        if (dirStep % 2==1)
        {
            _currentSize.x = _forwardSize.y;
            _currentSize.y = _forwardSize.x;
        }
        else
        {
            _currentSize = _forwardSize;
        }

        int rotationY = dirStep * 90;
        _blueprint.rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));
    }

    private void ConvertBuiltArea(List<Vector2Int> list)
    {
        foreach (var item in list)
        {
            _builtArea.Add(item);
        }
        list.Clear();
    }

    public void RemoveBuiltArea(Vector3 pos, Vector2Int size)
    {
        int wolrdX = (int)(pos.x);
        int wolrdZ = (int)(pos.z);
        if (_currentGridPos.x < 0 && size.x % 2 == 1)
            wolrdX--;
        if (_currentGridPos.z < 0 && size.y % 2 == 1)
            wolrdZ--;

        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int _x = wolrdX + x - (int)(size.x * 0.5f);
                int _y = wolrdZ + z - (int)(size.y * 0.5f);
                _builtArea.Remove(new Vector2Int(_x, _y));
            }
        }
    }

    private void MoveBlueprint()
    {
        _blueprint.position = _currentGridPos;
    }

    private void CheckPlaceable()
    {
        foreach (var item in _blueprintArea)
        {
            if (!_builtArea.Contains(item))
                continue;

            _isPlaceable = false;
            _meshRenderer.material = _blueprintMaterials[Convert.ToInt32(_isPlaceable)];
            return;
        }

        _isPlaceable = true;
        _meshRenderer.material = _blueprintMaterials[Convert.ToInt32(_isPlaceable)];
    }

    private void CancelBlueprint(BlueprintBuilding blueprint)
    {
        while (true)
        {
            var temp = EnabledBluePrints.Dequeue();
            if (temp == blueprint)
            {
                DisabledBlueprints.Enqueue(blueprint);
                RemoveBuiltArea(blueprint.transform.position, blueprint.Size);
                break;
            }
            EnabledBluePrints.Enqueue(temp);
        }
    }

    public void SetBlueprintBuilding(Vector3 pos,Vector2Int size ,quaternion dir)
    {
        if (!DisabledBlueprints.TryDequeue(out BlueprintBuilding obj))
        {
            obj = Instantiate(_blueprintPrefab, _RootBlueprint).GetComponent<BlueprintBuilding>();
            obj.gameObject.name = "BluePrint";
        }
        else
        {
            obj.gameObject.SetActive(true);
        }

        obj.gameObject.transform.SetPositionAndRotation(pos, dir);
        obj.Init(BuildingType,size, _meshFilter.mesh,CancelBlueprint);
        EnabledBluePrints.Enqueue(obj);
    }

    public Vector3 CompleteCreateBuilding(BuildingType type, Vector3 pos)
    {
        var typeData = Managers.Instance.DataManager.BaseBuildings[type];
        _currentSize = new Vector2Int(typeData.SizeX, typeData.SizeY);

        IsChangeCellPosition(pos);
        SetBlueprintArea(_currentGridPos);
        ConvertBuiltArea(_blueprintArea);
        return _currentGridPos;
    }
    #endregion
}
