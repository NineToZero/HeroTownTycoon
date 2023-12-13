using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider _collider;

    private WaitForSeconds _secondsByTriggerEnable = new WaitForSeconds(0.5f);

    public int ItemId;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void Init(int itemId)
    {
        ItemId = itemId;
        spriteRenderer.sprite = Managers.Instance.DataManager.GetItemSprites(itemId);
        StartCoroutine(EnableTrigger());
    }

    private IEnumerator EnableTrigger()
    {
        yield return _secondsByTriggerEnable;
        _collider.enabled = true;
    }

    private void OnEnable()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        bool isValid = collision.transform.root.TryGetComponent(out InventoryController inventoryController);

        if (isValid)
        {
            if (inventoryController.Inventory.Add(ItemId))
                Destroy(this.gameObject); // TODO : 오브젝트 풀링으로!
        }
    }
}
