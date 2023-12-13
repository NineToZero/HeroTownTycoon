using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConquestUI : BaseUI
{
    private StageBuilder _stageBuilder;

    [SerializeField] private GameObject _stageInfo;
    [SerializeField] private Transform _stageContent;
    [SerializeField] private TextMeshProUGUI _stageNameText;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _partyBtn;

    [SerializeField] private Transform _rewardContent;
    private List<TextSlot> _rewardTexts;
    [SerializeField] private Transform _enemyContent;
    private List<TextSlot> _enemyTexts;


    public void Init(StageBuilder stageBuilder)
    {
        _stageBuilder = stageBuilder;

        _rewardTexts = new();
        _enemyTexts = new();
        BuildScroll(); // Stage is not added due to user's play => If it's not, move to On(): ex. tire
        _startBtn.onClick.AddListener(StartStage);
        _partyBtn.onClick.AddListener(() => { Managers.Instance.UIManager.OpenUI<PartyUI>().Refresh(stageBuilder); });
    }

    public void SelectStage(int stageId)
    {
        _stageBuilder.SelectedStageId = stageId;
        _stageInfo.SetActive(true);
        ShowInfo(stageId);
    }

    private void BuildScroll()
    {
        DataManager dm = Managers.Instance.DataManager;
        int _stageSize = 0;
        UIButton uiButtonPrefab = null;
        UIButton newUiButton;
        StageSO stage;

        while (true) {
            stage = dm.GetSO<StageSO>(Const.SO_Stage, _stageSize);
            if (stage == null) break;

            if (uiButtonPrefab == null) uiButtonPrefab = Managers.Instance.DataManager.GetPrefab<UIButton>(Const.Prefabs_UIButton);

            newUiButton = Instantiate(uiButtonPrefab, _stageContent);
            newUiButton.SetParamAndText(_stageSize, stage.Name);
            newUiButton.SetButton(SelectStage);

            _stageSize++;
        }
    }

    public void ShowInfo(int stageId)
    {
        DataManager dm = Managers.Instance.DataManager;
        StageSO stage = dm.GetSO<StageSO>(Const.SO_Stage, stageId);

        _stageNameText.text = stage.Name;

        foreach (var rewardText in _rewardTexts) rewardText.gameObject.SetActive(false);
        foreach (var enemyText in _enemyTexts) enemyText.gameObject.SetActive(false);

        if (stage.Reward.Items.Count + 1 > _rewardTexts.Count || stage.EnemyIds.Count > _enemyTexts.Count)
        {
            TextSlot TextSlotPrefab = Managers.Instance.DataManager.GetPrefab<TextSlot>(Const.Prefabs_TextSlot);
            TextSlot newTextSlot;

            for (int i = _rewardTexts.Count; i < stage.Reward.Items.Count + 1; i++)
            {
                newTextSlot = Instantiate(TextSlotPrefab, _rewardContent);

                _rewardTexts.Add(newTextSlot);
            }

            for (int i = _enemyTexts.Count; i < stage.EnemyIds.Count; i++)
            {
                newTextSlot = Instantiate(TextSlotPrefab, _enemyContent);

                _enemyTexts.Add(newTextSlot);
            }
        }

        _rewardTexts[0].Name($"{stage.Reward.Gold} G").gameObject.SetActive(true);
        for (int i = 1; i < stage.Reward.Items.Count + 1; i++)
        {
            _rewardTexts[i].Name($"{dm.Items[stage.Reward.Items[i-1]].Name}").gameObject.SetActive(true);
        }

        for (int i = 0; i < stage.EnemyIds.Count; i++)
        {
            _enemyTexts[i].Name($"{dm.GetSO<EnemySO>(Const.SO_Enemy, stage.EnemyIds[i]).Name}").gameObject.SetActive(true);
        }
    }

    public override void Off()
    {
        base.Off();
        _stageInfo.SetActive(false);
    }

    private void StartStage()
    {
        UIManager ui = Managers.Instance.UIManager;

        if (_stageBuilder.PartyCount == 0)
        {
            ui.ShowPopupUI(
                () => ui.OpenUI<PartyUI>().Refresh(_stageBuilder),
                "용사 부족",
                "토벌대를 결성해주세요",
                null,
                "토벌대 설정",
                "확인");
        }
        else
        {
            Managers.Instance.UIManager.CloseUI<ConquestUI>();
            _stageBuilder.Build();
        }
    }
}
