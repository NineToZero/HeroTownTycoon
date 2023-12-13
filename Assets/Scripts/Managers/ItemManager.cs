using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{ 
    public void SpawnCollectable(int itemId, Vector3 position, int count = 1)
    {
        for (int i = 0; i < count; ++i)
        {
            float radius = Random.Range(0f, 0.5f);
            float angle = Random.Range(0f, 2 * Mathf.PI);
            position += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

            Collectable collectablePrefab = Managers.Instance.DataManager.GetPrefab<Collectable>(Const.Prefabs_Collectable);
            Collectable collectable = Instantiate(collectablePrefab, position + 0.5f * Vector3.up, Quaternion.identity);
            collectable.Init(itemId);
        }
    }
}
