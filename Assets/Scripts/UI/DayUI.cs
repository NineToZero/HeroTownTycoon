using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class DayUI : BaseUI
{
    [SerializeField] private Image _sun;
    [SerializeField] private GameObject Enter;
    [SerializeField] private TMP_Text _dayText;

    public event Action OnCompletePress;

    private void Start()
    {
        var manager = Managers.Instance.DayManager;
        _dayText.text = manager.Day.ToString();
        manager.DayChangeEvent 
            += () => { _dayText.text = manager.Day.ToString(); };
        OnCompletePress = manager.ChangeDay;
    }

    public void StartEnter()
    {
        StartCoroutine(nameof(PressEnter));
        Enter.SetActive(false);
    }

    public void CancelEnter()
    {
        StopCoroutine(nameof(PressEnter));
        _sun.fillAmount = 1.0f;
        Enter.SetActive(true);
    }

    private IEnumerator PressEnter()
    {
        while (_sun.fillAmount > 0.0f)
        {
            _sun.fillAmount -= 0.7f * Time.deltaTime;
            yield return null;
        }
        _sun.fillAmount = 1.0f;
        Enter.SetActive(true);
        Managers.Instance.UIManager.ShowPopupUI(OnCompletePress);
    }
}
