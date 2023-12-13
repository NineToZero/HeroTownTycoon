using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    [SerializeField] private GameObject _mainTitle;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private Button[] _btns;
    [SerializeField] private TMP_Text[] _texts;

    private bool _isMainTitle = true;
    public event Action<int> OnClickButton;

    private void Update()
    {
        //TODO 업데이트문에서 뺄것
        if (Input.anyKey)
        {
            if (_isMainTitle)
            {
                _isMainTitle = true;
                _mainTitle.SetActive(false);
                _mainMenu.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < _btns.Length; i++)
        {
            int num = i;
            _btns[i].onClick.AddListener(() => ClickButton(num));
        }
    }

    public void ClickButton(int num)
    {
        OnClickButton?.Invoke(num);
    }

    public void ChangeButtonText(int num, string text)
    {
        _texts[num].text = text;
    }
}
