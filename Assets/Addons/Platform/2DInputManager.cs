using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public �ƶ� Player;
    public AbilityController Ability;

    #region 2Dƽ̨�ƶ�
    public static Vector2 Movement; // �ƶ�����
    public static bool JumpWasPressed; // ��Ծ�Ƿ���
    public static bool JumpIsHeld; // ��Ծ�Ƿ�ס
    public static bool JumpWasReleased; // ��Ծ�Ƿ��ͷ�
    public static bool RunIsHeld; // �ܲ��Ƿ�ס

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    #endregion

    #region ����ѡ��
    private InputAction _chooseAction;
    #endregion

    private void Awake()
    {
        Ability = GameObject.FindObjectOfType<AbilityController>();
        PlayerInput = GetComponent<PlayerInput>();

        #region 2Dƽ̨�ƶ�
        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        #endregion
    }

    void Update()
    {
        #region 2Dƽ̨�ƶ�
        Player.Movement = _moveAction.ReadValue<Vector2>();

        Player.JumpWasPressed = _jumpAction.WasPressedThisFrame();
        Player.JumpIsHeld = _jumpAction.IsPressed();
        Player.JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        Player.RunIsHeld = _runAction.IsPressed();
        #endregion

        #region ����ѡ��
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ability.ѡ������ = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ability.ѡ������ = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { Ability .ѡ������= 2; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { Ability .ѡ������= 3; } 

        if(Input.GetKeyUp(KeyCode.R))
        {
            LevelKit.LoadLevel(LevelKit.CurrentLevel);
        }
    }
    #endregion
}

