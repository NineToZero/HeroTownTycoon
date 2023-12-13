using System;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{
    private enum ButtonType { GameStart, Option, Quit }
    private TitleUI _titleUI;
    private ButtonType[] _currentBtnTypes;
    private Action[] _buttonEvent;

    private void Awake()
    {
        _currentBtnTypes = new ButtonType[]
        {
            ButtonType.GameStart,
            ButtonType.Option,
            ButtonType.Quit
        };
        _buttonEvent = new Action[_currentBtnTypes.Length];
    }

    public void Start()
    {
        _titleUI = Managers.Instance.UIManager.UIs[typeof(TitleUI)] as TitleUI;
        _titleUI.OnClickButton += ExecuteButtonEvent;
        AddButtonAction(_currentBtnTypes);
    }

    public void ExecuteButtonEvent(int num)
    {
        _buttonEvent[num].Invoke();
    }

    private void AddButtonAction(ButtonType[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            switch (list[i])
            {
                case ButtonType.GameStart:
                    _buttonEvent[i] = StartNewGame;
                    _titleUI.ChangeButtonText(i, "시작하기");
                    break;
                case ButtonType.Option:
                    _buttonEvent[i] = OpenOptionUI;
                    _titleUI.ChangeButtonText(i, "설정");
                    break;
                case ButtonType.Quit:
                    _buttonEvent[i] = Quit;
                    _titleUI.ChangeButtonText(i, "게임 종료");
                    break;
                default:
                    break;
            }
        }
    }

    private void StartNewGame()
    {
        LoadingSceneController.LoadScene(SceneType.Game);
    }

    private void OpenOptionUI()
    {
        var um = Managers.Instance.UIManager;
        var ui = um.OpenUI<OptionUI>();
        um.CloseUI<TitleUI>();
        ui.Init(() => { um.OpenUI<TitleUI>(); });
    }

    private void Quit()
    {
        Util.TurnOffGame();
    }
}
