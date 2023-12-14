using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider _collider;
    private int _itemId;

    public void Init(int itemId, Vector3 position)
    {
        _itemId = itemId;
        spriteRenderer.sprite = Managers.Instance.DataManager.GetItemSprites(itemId);
        transform.position = position;
        gameObject.SetActive(true);

        _collider.enabled = false;
        Invoke(nameof(SetEnabled), 0.5f);
    }

    private void SetEnabled() => _collider.enabled = true;

    private void OnTriggerEnter(Collider collision)
    {
        bool isValid = collision.transform.root.TryGetComponent(out InventoryController inventoryController);

        if (isValid)
        {
            if (inventoryController.Inventory.Add(_itemId))
                gameObject.SetActive(false);
        }
    }
}
