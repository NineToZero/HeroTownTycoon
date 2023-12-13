using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : BaseUI
{
    [SerializeField] protected Button _confirmButton;
    [SerializeField] protected TMP_Text _confirmText;
    [SerializeField] protected Button _cancelButton;
    [SerializeField] protected TMP_Text _cancelText;
    [SerializeField] protected TMP_Text _headText;
    [SerializeField] protected TMP_Text _bodyText;

    protected event Action _confirmEvent;
    protected event Action _cancelEvent;


    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _confirmButton.onClick.AddListener(ConfirmCallback);
        _cancelButton.onClick.AddListener(CancelCallback);
    }


    public void Refresh(Action confirm, string title, string body, Action cancel, string confirmText, string cancelText)
    {
        _confirmEvent = confirm;
        _headText.text = title;
        _bodyText.text = body;
        _cancelEvent = cancel;
        _confirmText.text = confirmText;
        _cancelText.text = cancelText;
    }


    private void ConfirmCallback()
    {
        _confirmEvent?.Invoke();
    }


    private void CancelCallback()
    {
        _cancelEvent?.Invoke();
    }
}
