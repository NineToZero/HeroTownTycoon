using UnityEngine;
using UnityEngine.EventSystems;

public class PointerFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CursorSource _cursorSource;
    private EventEntrySO _eventEntrySO;

    private void Awake()
    {
        _eventEntrySO = Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event);
    }
    public void Init(CursorSource source)
    {
        _cursorSource = source;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _eventEntrySO.Publish(EventTriggerType.PointerEnter, _cursorSource);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _eventEntrySO.Publish(EventTriggerType.PointerExit, CursorSource.None);
    }
}