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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ability.选中能力 = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ability.选中能力 = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Ability .选中能力= 2; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { Ability .选中能力= 3; } 

        if(Input.GetKeyUp(KeyCode.R))
        {
            LevelKit.LoadLevel(LevelKit.CurrentLevel);
        }
    }
    #endregion
}

