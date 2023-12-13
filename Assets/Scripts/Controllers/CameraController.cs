using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] public CinemachinePOV _virtualCamera;

    private float _defaultVerticalSpeed;
    private float _defaultHorizontalSpeed;

    private void Awake()
    {
        _virtualCamera = _camera.GetCinemachineComponent<CinemachinePOV>();
        _defaultVerticalSpeed = _virtualCamera.m_VerticalAxis.m_MaxSpeed;
        _defaultHorizontalSpeed = _virtualCamera.m_HorizontalAxis.m_MaxSpeed;
        Managers.Instance.OptionManager.OptionData.OnChangeMouseSensitivity += ChangeMouseSensitivity;

        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Subscribe(UIType.Popup, (isOpened) => StopCameraRotation(!isOpened));
    }

    private void ChangeMouseSensitivity()
    {
        float value = Managers.Instance.OptionManager.OptionData.MouseSensitivity;
        _virtualCamera.m_VerticalAxis.m_MaxSpeed = _defaultVerticalSpeed * value;
        _virtualCamera.m_HorizontalAxis.m_MaxSpeed = _defaultHorizontalSpeed * value;
    }

    private void StopCameraRotation(bool canPlay)
    {
        if (canPlay)
        {
            float value = Managers.Instance.OptionManager.OptionData.MouseSensitivity;
            _virtualCamera.m_VerticalAxis.m_MaxSpeed = _defaultVerticalSpeed * value;
            _virtualCamera.m_HorizontalAxis.m_MaxSpeed = _defaultHorizontalSpeed * value;
        }
        else
        {
            _virtualCamera.m_VerticalAxis.m_MaxSpeed = 0;
            _virtualCamera.m_HorizontalAxis.m_MaxSpeed = 0;
        }
    }
}
