using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public UIType Type => _type;
    [SerializeField] private UIType _type;

    public virtual void On() { gameObject.SetActive(true); }
    public virtual void Off() { gameObject.SetActive(false); }
}