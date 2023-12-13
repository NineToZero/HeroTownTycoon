using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TravelGuideUI : BaseUI
{
    [SerializeField] private TMP_Text _conquestTime;
    [SerializeField] private RawImage _miniMap;
    [SerializeField] private TMP_Text _travelTime;
    [SerializeField] private Image _ring;

    private WaitForSeconds _waitTime;

    private Coroutine _coroutine;
    public event Action OnShortPress;
    public event Action OnLongPress;

    private StageController _stageController;

    public void Init(StageController stageController)
    {
        _stageController = stageController;
    }

    private void Awake()
    {
        _waitTime = new(0.2f);
    }

    private void Start()
    {
        OnLongPress += () =>
        {
            _miniMap.gameObject.SetActive(false);

            Managers.Instance.UIManager.ToggleUI<TravelUI>();
        };
        OnShortPress += () =>
        {
            _miniMap.gameObject.SetActive(!_miniMap.gameObject.activeSelf);
        };
    }

    public void RefreshConquest(float time)
    {
        _conquestTime.text = time.ToString();
    }

    public void RefreshTravel(float time)
    {
        _travelTime.text = time.ToString();
    }

    public void StartEnter()
    {
        //if (_stageController.IsTraveling)
            _coroutine = StartCoroutine(PressEnter());
    }

    public void CancelEnter()
    {
        if (_coroutine == null)
            return;

        if (_ring.fillAmount < 1.0f)
        {
            OnShortPress();
        }
        else
        {
            OnLongPress?.Invoke();
        }

        StopCoroutine(_coroutine);
        _ring.fillAmount = 0.0f;
        _coroutine = null;
    }

    private IEnumerator PressEnter()
    {
        yield return _waitTime;

        while (_ring.fillAmount < 1.0f)
        {
            yield return null;
            _ring.fillAmount += 1.375f * Time.deltaTime;
        }

        CancelEnter();
    }
}
