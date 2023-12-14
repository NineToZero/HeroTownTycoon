using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    private Queue<Collectable> _activeItems;
    private Queue<Collectable> _deactiveItems;

    private void Awake()
    {
        _activeItems = new();
        _deactiveItems = new();
    }

    public void SpawnCollectable(int itemId, Vector3 position, int count = 1)
    {
        Collectable item;
        float radius;
        float angle;

        if (_deactiveItems.Count < count)
        {
            int activeCount = _activeItems.Count;

            for (int i = 0; i < activeCount; ++i)
            {
                item = _activeItems.Dequeue();

                if (item.gameObject.activeSelf)
                    _activeItems.Enqueue(item);
                else
                    _deactiveItems.Enqueue(item);
            }

            if (_deactiveItems.Count < count)
            {
                Collectable collectablePrefab = Managers.Instance.DataManager.GetPrefab<Collectable>(Const.Prefabs_Collectable);

                for (int i = _deactiveItems.Count; i < count; ++i)
                {
                    _deactiveItems.Enqueue(Instantiate(collectablePrefab));
                }
            }
        }

        for (int i = 0; i < count; ++i)
        {
            radius = Random.Range(0f, 0.5f);
            angle = Random.Range(0f, 2 * Mathf.PI);

            position += new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            position.y = 0.5f;

            item = _deactiveItems.Dequeue();
            _activeItems.Enqueue(item);
            item.Init(itemId, position);
        }
    }
}
