using UnityEngine;

[CreateAssetMenu(fileName = "ItemImage", menuName = "Scriptable Object/ItemImage")]
public class ItemImageSO : ScriptableObject
{
    public SerializedDictionary<Sprite> ImageDictionary;
}
