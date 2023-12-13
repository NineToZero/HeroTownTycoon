using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class TravelResultUI : PopupUI
{
    [SerializeField] private Transform _result;
    [SerializeField] private Transform _content;
    [SerializeField] private List<Image> _characters;

    private List<ImageSlot> _heroSlots;
    private List<ImageSlot> _itemAndGoldSlots;

    private InventoryController _inventoryController;
    private StageController _stageController;

    // 임시
    private Sprite _coin;

    public void Init(InventoryController inventoryController, StageController stageController)
    {
        _inventoryController = inventoryController;
        _stageController = stageController;

        _heroSlots = new();
        _itemAndGoldSlots = new();

        // 임시
        _coin = Resources.Load<Sprite>(Const.Coin);
    }
    public override void On()
    {
        base.On();
        // 임시
        _stageController.IsTraveling = false;

        _confirmButton.interactable = true;
        _result.gameObject.SetActive(false);
    }

    public void Refresh(bool isWon)
    {
        foreach (var character in _characters) character.gameObject.SetActive(false);

        DataManager dataManager = Managers.Instance.DataManager;
        List<HeroHandler> party = _stageController.StageHandler.Party;

        if (party.Count > _characters.Count)
        {
            for (int i = _characters.Count; i < party.Count; i++)
            {
                GameObject go = new GameObject($"Hero{i}");
                go.transform.SetParent(_content);
                Image image = go.AddComponent<Image>();
                image.raycastTarget = false;
                image.rectTransform.sizeDelta = new Vector2(50f, 50f);
                image.rectTransform.localScale = new Vector3(-1f, 1f, 1f);

                _characters.Add(image);
            }
        }

        for (int i = 0; i < party.Count; i++)
        {
            _characters[i].sprite = dataManager.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, party[i].SpriteId).GetSprite("Idle", "0");
            _characters[i].gameObject.SetActive(true);
        }

        if (isWon) Refresh("토벌 성공", "토벌에 성공했습니다.");
        else Refresh("토벌 실패", "토벌에 실패했습니다.");
    }

    private void Refresh(string title, string body)
    {
        Refresh(ShowReward, title, body, Close, "상세 결과", "확인");
    }

    private void ShowReward()
    {
        if(!_result.gameObject.activeSelf)
        {
            _confirmButton.interactable = false;

            foreach (ImageSlot slot in _heroSlots) slot.gameObject.SetActive(false);
            foreach (ImageSlot slot in _itemAndGoldSlots) slot.gameObject.SetActive(false);

            List<HeroHandler> deadHeros = _stageController.StageHandler.Party.Where(x => !x.IsAlive).ToList();

            DataManager dm = Managers.Instance.DataManager;

            #region HeroSlot
            if (deadHeros.Count > _heroSlots.Count)
            {
                ImageSlot prefab = dm.GetPrefab<ImageSlot>(Const.Prefabs_HeroSlot);

                for(int i = _heroSlots.Count; i < deadHeros.Count; i++)
                {
                    _heroSlots.Add(Instantiate(prefab, _result));
                }
            }

            for (int i = 0; i < deadHeros.Count; i++)
            {
                _heroSlots[i].Text("치명상").Sprite(dm.GetSO<SpriteLibraryAsset>(Const.SO_CharacterSprites, deadHeros[i].SpriteId).GetSprite("Idle", "0"));
                _heroSlots[i].gameObject.SetActive(true);
            }
            #endregion

            #region ItemAndGoldSlot
            List<int> items = _stageController.StageHandler.TotalReward.Items;

            if (items.Count + 1 > _itemAndGoldSlots.Count)
            {
                ImageSlot prefab = dm.GetPrefab<ImageSlot>(Const.Prefabs_ImageSlot);

                for (int i = _itemAndGoldSlots.Count; i < items.Count + 1; i++)
                {
                    _itemAndGoldSlots.Add(Instantiate(prefab, _result));
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                _itemAndGoldSlots[i].Text(dm.Items[items[i]].Name).Sprite(dm.GetItemSprites(items[i]));
                _itemAndGoldSlots[i].gameObject.SetActive(true);
            }

            // 임시
            _itemAndGoldSlots[items.Count].Text($"{_stageController.StageHandler.TotalReward.Gold} G").Sprite(_coin);

            #endregion

            _result.gameObject.SetActive(true);
        }
    }

    private void Close()
    {
        Reward reward = _stageController.StageHandler.TotalReward;
        _inventoryController.Gold += reward.Gold;
        foreach (var item in reward.Items)
            _inventoryController.Inventory.Add(item);

        Managers.Instance.UIManager.ShowNotificationUI("성공적으로 보상을 획득했습니다.");
        Managers.Instance.UIManager.CloseUI<TravelResultUI>();
    }
}