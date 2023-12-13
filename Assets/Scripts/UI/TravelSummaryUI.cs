using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TravelSummaryUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _travelName;
    [SerializeField] private TextMeshProUGUI _progress;

    private StageController _stageController;
    private StageHandler _stageHandler;
    public void Init(StageController stageController)
    {
        _stageController = stageController;
    }

    public override void On()
    {
        base.On();

        _stageHandler = _stageController.StageHandler;

        _stageHandler.OnEndPhaseEvent += Refresh;
        _stageHandler.OnEndEvent += Managers.Instance.UIManager.CloseUI<TravelSummaryUI>;

        _travelName.text = $"{_stageHandler.Name} 토벌 중 (M)";
        Refresh();
    }

    public void Refresh()
    {
        _progress.text = $"진행도: {_stageHandler.CurPhaseNum + 1} / {_stageHandler.PhaseSize}";
    }
}
