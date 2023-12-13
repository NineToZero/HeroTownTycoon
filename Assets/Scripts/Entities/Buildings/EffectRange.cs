using UnityEngine;

public class EffectRange : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask;

    private int _layerIndex;
    private PassiveBuilding _parent;

    private void Awake()
    {
        _layerIndex = (int)Mathf.Log(_mask, 2);
        _parent = transform.parent.GetComponent<PassiveBuilding>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _layerIndex
            || gameObject.transform.parent == other.transform.parent)
            return;

        _parent.DetectivedBuilding.Add(other.transform.parent.GetComponent<BaseBuilding>());
    }
}