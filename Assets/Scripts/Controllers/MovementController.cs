using Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Transform _cam;
    private Transform _cameraArm;

    private Vector2 _moveInput;

    private bool _canMove;

    private void Awake()
    {
        _cameraArm = new GameObject("CameraArm").transform;
        _cameraArm.SetParent(transform);
        _cameraArm.transform.position += Vector3.up * 1.5f;

        Managers.Instance.DataManager.GetSO<EventEntrySO>(Const.SO_Event).Subscribe(UIType.Popup, (isOpened) => _canMove = !isOpened);
    }

    public void FixedUpdate()
    {
        if(_canMove)
            Move();
    }

    public void Init(CinemachineVirtualCamera camera)
    {
        camera.Follow = _cameraArm;
        camera.LookAt = _cameraArm;
        _cam = camera.gameObject.transform;
    }

    public void OnMoveInput(Vector2 dir)
    {
        _moveInput = dir;
    }

    private Vector3 GetPlayerForwardDir()
    {
        Vector3 forward = _cam.transform.forward;
        Vector3 right = _cam.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return (forward * _moveInput.y + right * _moveInput.x).normalized;
    }

    private void Move()
    {
        if (_moveInput.magnitude == 0) return;

        Vector3 moveDir = GetPlayerForwardDir();
        transform.position += moveDir * (Time.deltaTime * 5f);
    }
}
