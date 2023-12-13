using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationUI : BaseUI
{
    [SerializeField] private TMP_Text _text;
    private Dictionary<float, WaitForSeconds> _timeList;
    private Coroutine _coroutine;

    private void Awake()
    {
        _timeList = new()
        {
            { 2.0f, new WaitForSeconds(2.0f) }
        };
    }

    public void Refresh(string text, float time)
    {
        if (!_timeList.ContainsKey(time))
            _timeList.Add(time, new WaitForSeconds(time));

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _text.text = text;
        _coroutine = StartCoroutine(AppearText(time));
    }

    public IEnumerator AppearText(float time)
    {
        yield return _timeList[time];
        Managers.Instance.UIManager.CloseUI<NotificationUI>();
    }
}
