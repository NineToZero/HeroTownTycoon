using System;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    private bool _isPossible;
    private int _day;
    public event Action DayChangeEvent;
    public event Action AfterDayChangeEvent;

    public bool IsPossible { get { return _isPossible; } set { _isPossible = value; } }
    public int Day { get { return _day; } }

    private void Awake()
    {
        _isPossible = true;
        _day = ES3.Load(Const.Save_DaySaveData, 0);
        AfterDayChangeEvent += () => ES3.Save(Const.Save_DaySaveData, Day);
    }

    public void ChangeDay()
    {
        if (!_isPossible)
        {
            Managers.Instance.UIManager.ShowNotificationUI("다음 날로 넘어갈 수 없습니다.");
            return;
        }

        _day++;
        DayChangeEvent?.Invoke();
        Managers.Instance.UIManager.ShowNotificationUI("다음 날...");
        AfterDayChangeEvent?.Invoke();
    }
}
