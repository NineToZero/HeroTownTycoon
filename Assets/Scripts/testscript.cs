using UnityEngine;

public class testscript : MonoBehaviour
{
    public int itemCode;
    private void Start()
    {
        Managers.Instance.ItemManager.SpawnCollectable(itemCode, transform.position);
    }
}
