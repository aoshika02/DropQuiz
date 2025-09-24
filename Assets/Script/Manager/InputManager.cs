using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private PlayerAction _playerAction;

    #region Move

    public void SetMoveStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.started += action;
    }

    public void SetMovePerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.performed += action;
    }

    public void SetMoveCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.canceled += action;
    }

    public void RemoveMoveStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.started -= action;
    }

    public void RemoveMovePerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.performed -= action;
    }

    public void RemoveMoveCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Move.canceled -= action;
    }

    #endregion

    #region Camera

    public void SetCameraStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.started += action;
    }

    public void SetCameraPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.performed += action;
    }

    public void SetCameraCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.canceled += action;
    }

    public void RemoveCameraStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.started -= action;
    }

    public void RemoveCameraPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.performed -= action;
    }

    public void RemoveCameraCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Camera.canceled -= action;
    }

    # endregion

    #region Run

    public void SetRunStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.started += action;
    }

    public void SetRunPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.performed += action;
    }

    public void SetRunCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.canceled += action;
    }

    public void RemoveRunStartedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.started -= action;
    }

    public void RemoveRunPerformedAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.performed -= action;
    }

    public void RemoveRunCanceledAction(Action<InputAction.CallbackContext> action)
    {
        _playerAction.Player.Run.canceled -= action;
    }

    #endregion

    #region LeftCrossKey

    /// <summary>
    /// 左十字キーの入力
    /// </summary>
    public IReadOnlyReactiveProperty<Vector2> LeftCrossKey => _leftCrossKey;

    private readonly ReactiveProperty<Vector2> _leftCrossKey = new ReactiveProperty<Vector2>();

    /// <summary>
    /// 左十字キーの入力 Up
    /// </summary>
    public IReadOnlyReactiveProperty<float> LeftCrossKeyUp => _leftCrossKeyUp;

    private readonly ReactiveProperty<float> _leftCrossKeyUp = new ReactiveProperty<float>();

    /// <summary>
    /// 左十字キーの入力 Down
    /// </summary>
    public IReadOnlyReactiveProperty<float> LeftCrossKeyDown => _leftCrossKeyDown;

    private readonly ReactiveProperty<float> _leftCrossKeyDown = new ReactiveProperty<float>();

    /// <summary>
    /// 左十字キーの入力 Right
    /// </summary>
    public IReadOnlyReactiveProperty<float> LeftCrossKeyRight => _leftCrossKeyRight;

    private readonly ReactiveProperty<float> _leftCrossKeyRight = new ReactiveProperty<float>();

    /// <summary>
    /// 左十字キーの入力 Left
    /// </summary>
    public IReadOnlyReactiveProperty<float> LeftCrossKeyLeft => _leftCrossKeyLeft;

    private readonly ReactiveProperty<float> _leftCrossKeyLeft = new ReactiveProperty<float>();

    #endregion

    #region Other

    /// <summary>
    /// ThankYouForPlayingの入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> ThankYouForPlaying => _thankYouForPlaying;

    private readonly ReactiveProperty<float> _thankYouForPlaying = new ReactiveProperty<float>();
    
    /// <summary>
    /// ThankYouForPlayingの長押し進捗
    /// </summary>
    public IReadOnlyReactiveProperty<float> ThankYouForPlayingProgress => _thankYouForPlayingProgress;

    private readonly ReactiveProperty<float> _thankYouForPlayingProgress = new ReactiveProperty<float>();
    
    /// <summary>
    /// ライセンスのキー入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> License => _license;

    private readonly ReactiveProperty<float> _license = new ReactiveProperty<float>();

    /// <summary>
    /// 設定のキー入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> Setting => _setting;

    private readonly ReactiveProperty<float> _setting = new ReactiveProperty<float>();

    /// <summary>
    /// 決定キーの入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> Decision => _decision;

    private readonly ReactiveProperty<float> _decision = new ReactiveProperty<float>();

    /// <summary>
    /// キャンセルキーの入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> Cancel => _cancel;

    private readonly ReactiveProperty<float> _cancel = new ReactiveProperty<float>();


    /// <summary>
    /// 移動キーの入力
    /// </summary>
    public IReadOnlyReactiveProperty<Vector2> Move => _move;

    private readonly ReactiveProperty<Vector2> _move = new ReactiveProperty<Vector2>();


    /// <summary>
    /// 走るキーの入力
    /// </summary>
    public IReadOnlyReactiveProperty<float> Dash => _dash;

    private readonly ReactiveProperty<float> _dash = new ReactiveProperty<float>();

    #endregion

    protected override void Awake()
    {
        if (!CheckInstance())
        {
            return;
        }

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        _playerAction = new PlayerAction();
        _playerAction.Enable();
        _playerAction.Player.Direction.started += context => { _leftCrossKey.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Direction.performed += context => { _leftCrossKey.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Direction.canceled += context => { _leftCrossKey.Value = context.ReadValue<Vector2>(); };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //0!= で入力を処理しているのでperformed無し
        _playerAction.Player.LeftCrossKeyUp.started += context =>
        {
            _leftCrossKeyUp.Value = context.ReadValue<float>();
        };
        _playerAction.Player.LeftCrossKeyUp.canceled += context =>
        {
            _leftCrossKeyUp.Value = context.ReadValue<float>();
        };

        _playerAction.Player.LeftCrossKeyDown.started += context =>
        {
            _leftCrossKeyDown.Value = context.ReadValue<float>();
        };
        _playerAction.Player.LeftCrossKeyDown.canceled += context =>
        {
            _leftCrossKeyDown.Value = context.ReadValue<float>();
        };

        _playerAction.Player.LeftCrossKeyRight.started += context =>
        {
            _leftCrossKeyRight.Value = context.ReadValue<float>();
        };
        _playerAction.Player.LeftCrossKeyRight.canceled += context =>
        {
            _leftCrossKeyRight.Value = context.ReadValue<float>();
        };

        _playerAction.Player.LeftCrossKeyLeft.started += context =>
        {
            _leftCrossKeyLeft.Value = context.ReadValue<float>();
        };
        _playerAction.Player.LeftCrossKeyLeft.canceled += context =>
        {
            _leftCrossKeyLeft.Value = context.ReadValue<float>();
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _playerAction.Player.ThankYouForPlaying.started += context => { _thankYouForPlaying.Value = context.ReadValue<float>(); };
        _playerAction.Player.ThankYouForPlaying.performed += context => { _thankYouForPlaying.Value = context.ReadValue<float>(); };
        _playerAction.Player.ThankYouForPlaying.canceled += context => { _thankYouForPlaying.Value = context.ReadValue<float>(); };
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _playerAction.Player.License.started += context => { _license.Value = context.ReadValue<float>(); };
        _playerAction.Player.License.performed += context => { _license.Value = context.ReadValue<float>(); };
        _playerAction.Player.License.canceled += context => { _license.Value = context.ReadValue<float>(); };
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _playerAction.Player.Setting.started += context => { _setting.Value = context.ReadValue<float>(); };
        _playerAction.Player.Setting.performed += context => { _setting.Value = context.ReadValue<float>(); };
        _playerAction.Player.Setting.canceled += context => { _setting.Value = context.ReadValue<float>(); };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _playerAction.Player.Decision.started += context => { _decision.Value = context.ReadValue<float>(); };
        _playerAction.Player.Decision.performed += context => { _decision.Value = context.ReadValue<float>(); };
        _playerAction.Player.Decision.canceled += context => { _decision.Value = context.ReadValue<float>(); };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _playerAction.Player.Cancel.started += context => { _cancel.Value = context.ReadValue<float>(); };
        _playerAction.Player.Cancel.performed += context => { _cancel.Value = context.ReadValue<float>(); };
        _playerAction.Player.Cancel.canceled += context => { _cancel.Value = context.ReadValue<float>(); };


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // PlayerMove
        _playerAction.Player.Move.started += context => { _move.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Move.performed += context => { _move.Value = context.ReadValue<Vector2>(); };
        _playerAction.Player.Move.canceled += context => { _move.Value = context.ReadValue<Vector2>(); };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // PlayerRun
        _playerAction.Player.Run.started += context => { _dash.Value = context.ReadValue<float>(); };
        _playerAction.Player.Run.performed += context => { _dash.Value = context.ReadValue<float>(); };
        _playerAction.Player.Run.canceled += context => { _dash.Value = context.ReadValue<float>(); };
    }

    private void Update()
    {
        if (_playerAction is null) { return;}

        _thankYouForPlayingProgress.Value = _playerAction.Player.ThankYouForPlaying.GetTimeoutCompletionPercentage();
    }
}