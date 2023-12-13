using System.Collections.Generic;
using UnityEngine;


public class BattleController : MonoBehaviour
{
    private IBattleHandler _battleHandler;

    private List<UnitController> _units;
    private List<SpriteRenderer> _tiles;

    private Camera _battleViewCamera;
    private RenderTexture _viewTexture;

    private Transform _map;

    private void Awake()
    {
        transform.position = new Vector3(0, -100, 0);
        _units = new();
        _tiles = new();

        _battleViewCamera = new GameObject("Battle View Camera").AddComponent<Camera>();
        _battleViewCamera.transform.SetParent(transform);

        Render();
    }

    private void OnEnable()
    {
        Managers.Instance.UIManager.GetUI<TravelUI>().SetBattleView(true);
    }

    private void OnDisable()
    {
        Managers.Instance.UIManager.GetUI<TravelUI>().SetBattleView(false);
    }

    public void SetBattle(IBattleHandler battleHandler)
    {
        Managers.Instance.SoundManager.PlaySFX(SFXSource.BattleHorn);

        _battleHandler = battleHandler;

        int width = battleHandler.Map.GetLength(0);
        _battleViewCamera.transform.SetLocalPositionAndRotation(new Vector3(2.5f + 0.5f * (width - 6), (width + 12 / width) * 0.5f, -2.5f - 0.25f * (width - 6)), Quaternion.Euler(50, 0f, 0f));

        MakeMap(width, battleHandler.Map.GetLength(1));

        foreach (var unit in _units) unit.gameObject.SetActive(false);

        if (battleHandler.Units.Count > _units.Count)
        {
            UnitController unitPrefab = Managers.Instance.DataManager.GetPrefab<UnitController>(Const.Prefabs_Unit);

            for (int i = _units.Count; i < battleHandler.Units.Count; i++)
            {
                _units.Add(Instantiate(unitPrefab, transform));
            }
        }

        for (int i = 0; i < battleHandler.Units.Count; i++)
        {
            if(_battleHandler.Units[i].CombatStat.CurHealth > 0)
            {
                _units[i].Init(_battleHandler.Units[i]);
                _units[i].gameObject.SetActive(true);
            }
        }
    }

    private void Render()
    {
        if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
        {
            if (_viewTexture != null)
            {
                _viewTexture.Release();
            }

            _viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            _battleViewCamera.targetTexture = _viewTexture;

            Managers.Instance.UIManager.GetUI<TravelUI>().SetTexture(_viewTexture);
        }
    }

    private void MakeMap(int width, int height)
    {
        if (_tiles.Count == 0)
        {
            _map = new GameObject("Map").transform;
            _map.SetParent(transform);
            _map.localPosition = Vector3.zero;
        }

        foreach (var tile in _tiles) tile.gameObject.SetActive(false);

        if (width * height > _tiles.Count)
        {
            SpriteRenderer tile = Managers.Instance.DataManager.GetPrefab<SpriteRenderer>(Const.Prefabs_BattleTile);

            for (int i = _tiles.Count; i < width * height; i++)
            {
                _tiles.Add(Instantiate(tile, _map));
            }
        }

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int tileIndex = h + w * height;
                _tiles[tileIndex].transform.localPosition = new Vector3(w, 0, h);
                _tiles[tileIndex].color = h % 2 == w % 2 ? new Color(20 / 255f, 90 / 255f, 20 / 255f)  : new Color(90 / 255f, 60 / 255f, 20 / 255f);
                _tiles[tileIndex].gameObject.SetActive(true);
            }
        }
    }
}

