using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public 移动 Player;
    public AbilityController Ability;

    #region 2D平台移动
    public static Vector2 Movement; // 移动方向
    public static bool JumpWasPressed; // 跳跃是否按下
    public static bool JumpIsHeld; // 跳跃是否按住
    public static bool JumpWasReleased; // 跳跃是否释放
    public static bool RunIsHeld; // 跑步是否按住

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    #endregion

    #region 能力选择
    private InputAction _chooseAction;
    private int selectedIndex = 0;
    #endregion

    private void Awake()
    {
        Ability = GameObject.FindObjectOfType<AbilityController>();
        PlayerInput = GetComponent<PlayerInput>();

        #region 2D平台移动
        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        #endregion

        #region 能力选择
        _chooseAction = PlayerInput.actions["choose"];
        #endregion
    }

    void Update()
    {
        #region 2D平台移动
        Player.Movement = _moveAction.ReadValue<Vector2>();

        Player.JumpWasPressed = _jumpAction.WasPressedThisFrame();
        Player.JumpIsHeld = _jumpAction.IsPressed();
        Player.JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        Player.RunIsHeld = _runAction.IsPressed();
        #endregion

        #region 能力选择
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedIndex = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedIndex = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedIndex = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { selectedIndex = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { selectedIndex = 4; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { selectedIndex = 5; }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { selectedIndex = 6; }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { selectedIndex = 7; }
    }

    #endregion
}

