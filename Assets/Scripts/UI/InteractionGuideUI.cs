using System.Text;
using TMPro;
using UnityEngine;

public class InteractionGuideUI : BaseUI
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _Guide;
    [SerializeField] private TMP_Text _overview;

    private StringBuilder _keyText;
    private StringBuilder _overviewText;

    private readonly float LINEHEIGHT = 20;
    private float _fontSize;

    public void Awake()
    {
        _keyText = new StringBuilder();
        _overviewText = new StringBuilder();

        _fontSize = _Guide.fontSize;
    }

    public void Refresh(GameObject target)
    {
        _keyText.Clear();
        _overviewText.Clear();
        int textLine = 0;

        if (target.TryGetComponent(out IClickable IClickable))
        {
            textLine++;
            var farmland = IClickable as Farmland;
            if (farmland)
            {
                _overviewText.AppendLine($"수분 : {farmland.WaterRatio}");
                _overviewText.AppendLine($"농약 : {farmland.PesticideRatio}");
                _overviewText.AppendLine($"비옥도 : {farmland.FertileRatio}");
            }
            _keyText.AppendLine("Click");
        }
        if (target.TryGetComponent(out IInteractable IInteractable))
        {
            textLine++;
            _name.text = IInteractable.GetName();
            _keyText.AppendLine("E");
        }

        _Guide.text = _keyText.ToString();

        Vector3 overviewPos = new Vector3(0, -(textLine * _fontSize + LINEHEIGHT), 0);
        _overview.rectTransform.anchoredPosition = overviewPos;
        _overview.text = _overviewText.ToString();
    }

}
