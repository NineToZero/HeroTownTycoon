using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoUI : BaseUI
{
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private TMP_Text _name; 
    [SerializeField] private TMP_Text _tier;
    [SerializeField] private TMP_Text _disc;

    private void Awake()
    {
        _cancelBtn.onClick.AddListener(Managers.Instance.UIManager.CloseUI<BuildingInfoUI>);
    }

    public override void On()
    {
        base.On ();
        Managers.Instance.SoundManager.PlaySFX(SFXSource.Pop);
    }

    public void Refresh(string name, string tier, string disc)
    {
        _name.text = name;
        _tier.text = $"Tier : {tier}";
        _disc.text = disc;
    }
}
