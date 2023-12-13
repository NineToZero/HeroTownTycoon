using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public StageHandler StageHandler { get; private set; }
    public bool IsTraveling;
    public bool IsTraveled;

    private BattleController _battleController;

    private void Awake()
    {
        _battleController = new GameObject("BattleController").AddComponent<BattleController>();
        _battleController.gameObject.SetActive(false);
        Managers.Instance.DayManager.DayChangeEvent += () => IsTraveled = false;
    }

    public void SetHandler(StageHandler stageHandler)
    {
        StageHandler = stageHandler;

        StageHandler.OnStartPhaseEvent += OnStartPhase;
        StageHandler.OnEndPhaseEvent += OnEndPhase;
        StageHandler.OnEndEvent += () => IsTraveling = false;
    }

    public void StartStage()
    {
        IsTraveling = true;
        IsTraveled = true;

        Managers.Instance.UIManager.OpenUI<TravelSummaryUI>();
        Invoke(nameof(StartPhase), 5);
    }

    #region PhaseControll
    private void OnStartPhase()
    {
        if (StageHandler.PhaseHandler is BattlePhaseHandler)
        {
            _battleController.SetBattle(StageHandler.PhaseHandler as BattlePhaseHandler);
            _battleController.gameObject.SetActive(true); StartCoroutine(nameof(ExcutePhaseCoroutine));
        }
        else
        {
            Invoke(nameof(ExcutePhase), 5);
        }
    }

    private void OnEndPhase()
    {
        StopCoroutine(nameof(ExcutePhaseCoroutine));

        if (StageHandler.PhaseHandler is BattlePhaseHandler)
        {
            _battleController.gameObject.SetActive(false);
        }

        Invoke(nameof(StartPhase), 5);
    }


    private void ExcutePhase()
    {
        StageHandler.PhaseHandler.Excute();
    }

    private IEnumerator ExcutePhaseCoroutine()
    {
        while (true)
        {
            yield return null;
            ExcutePhase();
        }
    }

    private void StartPhase()
    {
        StageHandler.StartNext();
    }

    #endregion
}
