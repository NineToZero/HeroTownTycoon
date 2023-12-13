using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    
    public ImageSlot Sprite(Sprite sprite)
    {
        _image.sprite = sprite;

        return this;
    }
    public ImageSlot Text(string text)
    {
        _text.text = text;

        return this;
    }
}
