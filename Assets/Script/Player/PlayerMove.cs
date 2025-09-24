using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : SingletonMonoBehaviour<PlayerMove>
{
    private Vector2 _moveDirection;
    private Rigidbody _rb;

    //歩行時の速度
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _camera;

    //回転系統
    [SerializeField]
    float _maxCameraRotateY;

    [SerializeField]
    float _minCameraRotateY;

    private float _cameraAngleY = -20;
    [SerializeField] private float _rotationSpeed = 1f;
    private Vector2 _cameraRotation;

    [SerializeField] private float _gravitySpeed = 35;

    // Rayの長さ
    [SerializeField] private float _rayLength = 1f;

    // Rayをどれくらい身体にめり込ませるか
    [SerializeField] private float _rayOffset;

    // Rayの判定に用いるLayer
    [SerializeField] private LayerMask _layerMask = default;

    private bool _isGround;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //playerの移動イベントの登録
        InputManager.Instance.SetMoveStartedAction(PressedInputMovement);
        InputManager.Instance.SetMovePerformedAction(PressedInputMovement);
        InputManager.Instance.SetMoveCanceledAction(CanceledMoveDirection);

        InputManager.Instance.SetCameraStartedAction(PressedInputCameraRotation);
        InputManager.Instance.SetCameraPerformedAction(PressedInputCameraRotation);
        InputManager.Instance.SetCameraCanceledAction(CanceledCameraRotation);
    }

    void Update()
    {
        if (CheckGrounded() == false)
        {
            _rb.velocity = new Vector3(0, -1, 0) * _gravitySpeed;
            return;
        }
        transform.Rotate(0, _cameraRotation.x * _rotationSpeed, 0);

        _cameraAngleY += -_cameraRotation.y * _rotationSpeed;

        if (_maxCameraRotateY <= _cameraAngleY)
        {
            _cameraAngleY = _maxCameraRotateY;
        }

        if (_cameraAngleY <= _minCameraRotateY)
        {
            _cameraAngleY = _minCameraRotateY;
        }

        var sampleAngle = _camera.eulerAngles;
        sampleAngle.x = _cameraAngleY;
        _camera.eulerAngles = sampleAngle;

        var cameraAngle = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);

        var v = cameraAngle * new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed / 60;

        _rb.velocity = cameraAngle * new Vector3(_moveDirection.x, 0, _moveDirection.y) *
            _moveSpeed / 60;
    }
    private bool CheckGrounded()
    {
        return
            Physics.Raycast(transform.position + new Vector3(0.25f, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(-0.25f, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(0.25f, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(-0.25f, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(0, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(0, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(0.25f, 0, 0) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask) ||
            Physics.Raycast(transform.position + new Vector3(-0.25f, 0, 0) + Vector3.up * _rayOffset, Vector3.down, _rayLength, _layerMask);
    }
    // Debug用にRayを可視化する
    private void OnDrawGizmos()
    {
        // 接地判定時は緑、空中にいるときは赤にする
        Gizmos.color = _isGround ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0.25f, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(-0.25f, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(0.25f, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(-0.25f, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(0, 0, 0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(0, 0, -0.25f) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(0.25f, 0, 0) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
        Gizmos.DrawRay(transform.position + new Vector3(-0.25f, 0, 0) + Vector3.up * _rayOffset, Vector3.down * _rayLength);
    }

    #region PlayerMove

    private void PressedInputMovement(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    private void CanceledMoveDirection(InputAction.CallbackContext context)
    {
        _moveDirection = Vector2.zero;
    }

    #endregion

    #region CameraMove

    private void PressedInputCameraRotation(InputAction.CallbackContext context)
    {
        _cameraRotation = context.ReadValue<Vector2>();
    }

    private void CanceledCameraRotation(InputAction.CallbackContext context)
    {
        _cameraRotation = Vector2.zero;
    }

    #endregion

    private void OnDestroy()
    {
        //playerの移動イベントの登録解除
        InputManager.Instance.RemoveMoveStartedAction(PressedInputMovement);
        InputManager.Instance.RemoveMovePerformedAction(PressedInputMovement);
        InputManager.Instance.RemoveMoveCanceledAction(CanceledMoveDirection);

        InputManager.Instance.RemoveCameraStartedAction(PressedInputCameraRotation);
        InputManager.Instance.RemoveCameraPerformedAction(PressedInputCameraRotation);
        InputManager.Instance.RemoveCameraCanceledAction(CanceledCameraRotation);
    }
}