using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    public �ƶ� Player;
    public AbilityController Ability;
    public static bool IsPaused;

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

    private bool switchKey;
    public Texture2D cursorTexture;

    private void Awake()
    {
        Ability = GameObject.FindObjectOfType<AbilityController>();
        PlayerInput = GetComponent<PlayerInput>();
        IsPaused = false;
        #region 2Dƽ̨�ƶ�
        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        #endregion
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
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
        if (!IsPaused)
        {
            #region ����ѡ��
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Ability.listIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Ability.listIndex = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Ability.listIndex = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Ability.listIndex = 3;
            }
            #endregion
            #region ��ͣ��Ϸ
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Time.timeScale = 0;
                IsPaused = true;
            }
            #endregion
            #region ���¼��س���
            if (Input.GetKeyUp(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            #endregion
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = 1;
            IsPaused = false;
        }
        #region Debugѡ��
        if(Input.GetKeyDown(KeyCode.M))
        {
            switchKey = !switchKey;
        }
        #endregion

    }
    private void OnGUI()
    {
        if (switchKey)
        {
            IMGUIHelper.SetDesignResolution(320, 80);
            for (int i = 1; i <= 6; i++)
            {
                if (GUILayout.Button("Level " + i.ToString())) LevelKit.LoadLevel(i);
            }
        }
    }

}

