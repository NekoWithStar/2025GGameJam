using QFramework;
using UnityEngine;

/// <summary>
/// ��Ҫǰ������
///     1. ��InputSystem���Ұ󶨺�����Actions��ֱ��ʹ���ļ����е�ControllerҲ�ɣ�
///         a. Move�а���WASD��Arrows��LeftStick ����action����Ϊ��PassThrough,Vector2,True��
///             aa.ʹ��Add UP/DOWN/LEFT/RIGHT Modifier������WASD��Arrows�İ�
///         b. Jump�а���Space��ButtonSouth
///         c. Run�а���Shift��ButtonWest
///     2. INPUT���󣬰���һ��InputManager�ű�
///         a. ����ʹ��10.����ϵͳģ���е�InputManager
///         b. �������Ҫ���ƶ����봦��Ŀ��(Qframework)
///     3. Player����
///         a. ����������ײ���Ӷ��󣬷ֱ��������ͽŲ�
///         b. ����Rigidibody�������������Ϊ0.0001(�����ܵͣ�����ҪΪ0)
///         c. ��������������Ϊ0(Angular Drag��LinearDrag��Gravity Scale)
///         d. Collision Dectection����ΪContinuous �����Է�ֹ�����ƶ������崩���������壩
///         e. Interpolate����ΪInterpolate(��ֵģʽ)����ֵģʽ����ƽ��������˶�
///         f. Freeze Rotation����ΪFreeze Rotation Z(����Z����ת)
///     4. �������
///         a. ����PhysicMaterial��Ħ��������Ϊ0����������Ϊ0
///         b. ����Player��Rigidibody�е�Material
/// </summary>
public class �ƶ� : MonoBehaviour
{
    [Header("�������")]
    public Vector2 Movement;
    public bool RunIsHeld;
    public bool JumpIsHeld;
    public bool JumpWasPressed;
    public bool JumpWasReleased;

    [Header("����")]
    public ��ɫ�ƶ�״̬ �ƶ�״̬;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    // �ƶ�
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    // ��ײ���
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _isBumpedHead;

    // ��Ծ
    public float VetricalSpeed { get; private set; }
    private bool _jumping;
    private bool _fastFalling;
    private bool _falling;
    private float _fastFallingTime;
    private float _fastFallingSpeed;
    private int _����Ծ����;

    // ��ߵ����
    private float _��ߵ����;
    private float _������ߵ���ֵʱ��;
    private bool _�Ƿ񳬹���ߵ���ֵ;

    // ��Ծ����
    private float _��Ծ�����ʱ��;
    private bool _��Ծ�����ڼ��ͷ���Ծ��;


    private float _������Ծ��ʱ��;

    private void Awake()
    {
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimer();
        JumpCheck();
    }


    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        if (_isGrounded)
        {
            Move(�ƶ�״̬.������ٶ�, �ƶ�״̬.������ٶ�, Movement);
        }
        else
        {
            Move(�ƶ�״̬.���м��ٶ�, �ƶ�״̬.���м��ٶ�, Movement);
        }
    }

    #region �ƶ�
    private void Move(float ���ٶ�, float ���ٶ�, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TrunCheck(moveInput); // ת����

            Vector2 Ŀ���ٶ� = Vector2.zero;
            if (RunIsHeld)
            {
                Ŀ���ٶ� = new Vector2(moveInput.x, 0f) * �ƶ�״̬.����ܲ��ٶ�;
            }
            else
            {
                Ŀ���ٶ� = new Vector2(moveInput.x, 0f) * �ƶ�״̬.����߶��ٶ�;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, Ŀ���ٶ�, ���ٶ� * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, ���ٶ� * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
    }

    private void TrunCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Trun(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Trun(true);
        }
    }

    private void Trun(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    #endregion

    #region ��Ծ
    private void JumpCheck()
    {
        /// ������Ծ��
        if (JumpWasPressed)
        {
            _��Ծ�����ʱ�� = �ƶ�״̬.��Ծ����ʱ��; // ������Ծ�����ʱ��
            _��Ծ�����ڼ��ͷ���Ծ�� = false; // ���δ�ͷ���Ծ��
        }
        // �ͷ���Ծ��
        if (JumpWasReleased)
        {
            // �������Ծ�����ڼ��ͷ�����Ծ��
            if (_��Ծ�����ʱ�� > 0)
            {
                _��Ծ�����ڼ��ͷ���Ծ�� = true;
            }
            // ����Ծ�������ͷ���Ծ��
            if (_jumping && VetricalSpeed > 0)
            {
                if (_�Ƿ񳬹���ߵ���ֵ)
                {
                    // ȡ����ߵ��״̬��������������
                    _�Ƿ񳬹���ߵ���ֵ = false;
                    _fastFalling = true;
                    _fastFallingTime = �ƶ�״̬.��Ծȡ������; // �������䴰��ʱ��
                    VetricalSpeed = 0f;
                }
                else
                {
                    // ֱ��������������
                    _fastFalling = true;
                    _fastFallingSpeed = VetricalSpeed;
                }

            }
        }

        // ��ʼ����Ծ
        if (_��Ծ�����ʱ�� > 0f && !_jumping && (_isGrounded || _������Ծ��ʱ�� > 0))
        {
            InitiateJump(1); // ִ�е�һ����Ծ

            // �������Ծ�����ڼ��ͷ�����Ծ��������������������
            if (_��Ծ�����ڼ��ͷ���Ծ��)
            {
                _fastFalling = true;
                _fastFallingSpeed = VetricalSpeed;
            }
        }

        // �༶��Ծ
        else if (_��Ծ�����ʱ�� > 0f && _jumping && _����Ծ���� < �ƶ�״̬.�����Ծ����)
        {
            _fastFalling = false; // ���ÿ�������״̬
            InitiateJump(1); // ִ�ж༶��Ծ
        }
        // ������Ծ������Ҵӵ������Ծ��Ȼת�Ƶ����к��ٴΰ�����Ծ��
        else if (_��Ծ�����ʱ�� > 0f && _falling && _����Ծ���� < �ƶ�״̬.�����Ծ���� - 1)
        {
            InitiateJump(2); // ִ�п�����Ծ��Ҫ�õ�2����Ծ����
            _fastFalling = false; // ���ÿ�������״̬
        }
        // ��½���
        if ((_jumping || _falling) && _isGrounded && VetricalSpeed <= 0f)
        {
            _jumping = false;
            _falling = false;
            _fastFalling = false;
            _fastFallingTime = 0f;
            _�Ƿ񳬹���ߵ���ֵ = false;
            _����Ծ���� = 0;
            // ����ֱ�ٶ�����ΪĬ������ֵ
            VetricalSpeed = Physics2D.gravity.y;

        }
    }

    private void InitiateJump(int ������Ծ����)
    {
        AudioKit.PlaySound("resources://Audios/Sounds/jump");
        if (!_jumping)
        {
            _jumping = true;
        }
        _��Ծ�����ʱ�� = 0f;
        _����Ծ���� += ������Ծ����;
        VetricalSpeed = �ƶ�״̬.��ʼ��Ծ�ٶ�;
    }

    private void Jump()
    {
        // Ӧ������
        if (_jumping)
        {
            // ��鴥��
            if (_isBumpedHead)
            {
                _fastFalling = true;
            }

            // ���������׶ε������߼�
            if (VetricalSpeed >= 0f)
            {
                // ������Ծ����ߵ����
                _��ߵ���� = Mathf.InverseLerp(�ƶ�״̬.��ʼ��Ծ�ٶ�, 0f, VetricalSpeed);
                // �ж��Ƿ񳬹���ߵ���ֵ
                if (_��ߵ���� > �ƶ�״̬.��ߵ���ֵ)
                {
                    ��������ߵ���ֵ();
                }
                else
                {
                    // ���������߼���Ӧ���������ٶ�
                    VetricalSpeed += �ƶ�״̬.�������ٶ� * Time.fixedDeltaTime;
                    ������ߵ���ֵ�ж�();
                }
            }
            // ������Ծ�ͷź�Ŀ��������߼�
            else if (!_fastFalling)
            {
                VetricalSpeed += �ƶ�״̬.�������ٶ� * �ƶ�״̬.��Ծ�ͷ��������� * Time.fixedDeltaTime;
            }
            // �����������״̬�ı��
            else if (VetricalSpeed < 0f)
            {
                SetFallingState();
            }
        }

        // ��Ծȡ��
        if (_fastFalling)
        {
            �����������();
        }

        // ���������߼�������Ծ״̬��δ�Ӵ����棩
        if (!_isGrounded && !_jumping)
        {
            SetFallingState();

            // Ӧ�������������ٶ�
            VetricalSpeed += �ƶ�״̬.�������ٶ� * Time.fixedDeltaTime;
        }
        // ���ƴ�ֱ�ٶȷ�Χ
        VetricalSpeed = Mathf.Clamp(VetricalSpeed, -�ƶ�״̬.��������ٶ�, 50f);
        // ���¸����ٶ�
        _rb.velocity = new Vector2(_rb.velocity.x, VetricalSpeed);
    }

    private void SetFallingState()
    {
        if (!_falling)
        {
            _falling = true;
        }
    }

    private void �����������()
    {
        if (_fastFallingTime >= �ƶ�״̬.��Ծȡ������)
        {
            VetricalSpeed += �ƶ�״̬.�������ٶ� * �ƶ�״̬.��Ծ�ͷ��������� * Time.fixedDeltaTime;
        }
        else if (_fastFallingTime <= �ƶ�״̬.��Ծȡ������)
        {
            VetricalSpeed = Mathf.Lerp(_fastFallingSpeed, 0f, (_fastFallingTime / �ƶ�״̬.��Ծȡ������));
        }
        _fastFallingTime += Time.fixedDeltaTime;
    }

    private void ������ߵ���ֵ�ж�()
    {
        if (_�Ƿ񳬹���ߵ���ֵ)
        {
            _�Ƿ񳬹���ߵ���ֵ = false;
        }
    }

    private void ��������ߵ���ֵ()
    {
        if (!_�Ƿ񳬹���ߵ���ֵ)
        {
            _�Ƿ񳬹���ߵ���ֵ = true;
            _������ߵ���ֵʱ�� = 0f;
        }

        if (_�Ƿ񳬹���ߵ���ֵ)
        {
            _������ߵ���ֵʱ�� += Time.fixedDeltaTime;
            if (_������ߵ���ֵʱ�� < �ƶ�״̬.��ߵ�ͣ��ʱ��)
            {
                VetricalSpeed = 0f;
            }
            else
            {
                VetricalSpeed = -0.01f;
            }
        }
    }
    #endregion

    #region ��ײ���
    private void isGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, �ƶ�״̬.�������߳���);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, �ƶ�״̬.�������߳���, �ƶ�״̬.�����);

        _isGrounded = _groundHit.collider != null;

        DebugUIDrawer(boxCastOrigin, boxCastSize);
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * �ƶ�״̬.�������, �ƶ�״̬.�������߳���);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, �ƶ�״̬.�������߳���, �ƶ�״̬.�����);

        _isBumpedHead = _headHit.collider != null;

        DebugUIDrawer(boxCastOrigin, boxCastSize);
    }
    private void CollisionCheck()
    {
        isGrounded();
        BumpedHead();
    }
    #endregion

    #region ��ʱ��
    private void CountTimer()
    {
        _��Ծ�����ʱ�� -= Time.deltaTime;
        if (!_isGrounded)
        {
            _������Ծ��ʱ�� -= Time.deltaTime;
        }
        else
        {
            _������Ծ��ʱ�� = �ƶ�״̬.������Ծ����ʱ��;
        }
    }
    #endregion

    #region DebugUI
    private void DebugUIDrawer(Vector2 boxCastOrigin, Vector2 boxCastSize)
    {
        if (�ƶ�״̬.Debugͷ����ײ��)
        {
            float headWidth = �ƶ�״̬.�������;
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y),
                Vector2.up * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y),
                Vector2.up * �ƶ�״̬.�������߳���, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - �ƶ�״̬.�������߳���),
                Vector2.right * boxCastSize.x * headWidth, rayColor);
        }


        if (�ƶ�״̬.Debug������ײ��)
        {
            Color rayColor = _isGrounded ? Color.green : Color.red;

            // ����BoxCast��������
            Vector2 topLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);
            Vector2 topRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);
            Vector2 bottomLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);
            Vector2 bottomRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);

            Debug.DrawLine(topLeft, topRight, rayColor);
            Debug.DrawLine(topRight, bottomRight, rayColor);
            Debug.DrawLine(bottomRight, bottomLeft, rayColor);
            Debug.DrawLine(bottomLeft, topLeft, rayColor);
        }
    }

    #endregion
}