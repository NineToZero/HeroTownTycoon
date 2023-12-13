using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;
    
    private Transform _camTransform; 
    private void Start()
    {
        _camTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.rotation = freezeXZAxis ? Quaternion.Euler(0f, _camTransform.rotation.eulerAngles.y, 0f) : _camTransform.rotation;
    }
}