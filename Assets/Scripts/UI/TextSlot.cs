using TMPro;
using UnityEngine;

public class TextSlot : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;

    public TextSlot Name(string name)
    {
        NameText.text = name;
        return this;
    }

    public TextSlot Value(string name)
    {
        ValueText.text = name;
        return this;
    }
}