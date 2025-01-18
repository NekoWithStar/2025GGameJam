using QFramework;
using UnityEngine;

/// <summary>
/// 必要前置需求：
///     1. 有InputSystem，且绑定好以下Actions（直接使用文件夹中的Controller也可）
///         a. Move中包含WASD、Arrows和LeftStick 其中action属性为（PassThrough,Vector2,True）
///             aa.使用Add UP/DOWN/LEFT/RIGHT Modifier来创建WASD和Arrows的绑定
///         b. Jump中包含Space和ButtonSouth
///         c. Run中包含Shift和ButtonWest
///     2. INPUT对象，包含一个InputManager脚本
///         a. 可以使用10.输入系统模板中的InputManager
///         b. 或包含必要的移动输入处理的框架(Qframework)
///     3. Player对象
///         a. 包含两个碰撞箱子对象，分别代表身体和脚部
///         b. 创建Rigidibody组件，质量设置为0.0001(尽可能低，但不要为0)
///         c. 阻力和重力设置为0(Angular Drag、LinearDrag和Gravity Scale)
///         d. Collision Dectection设置为Continuous （可以防止快速移动的物体穿过其他物体）
///         e. Interpolate设置为Interpolate(插值模式)，插值模式用于平滑物体的运动
///         f. Freeze Rotation设置为Freeze Rotation Z(冻结Z轴旋转)
///     4. 物理材质
///         a. 创建PhysicMaterial，摩擦力设置为0，弹力设置为0
///         b. 赋给Player的Rigidibody中的Material
/// </summary>
public class 移动 : MonoBehaviour
{
    [Header("输入变量")]
    public Vector2 Movement;
    public bool RunIsHeld;
    public bool JumpIsHeld;
    public bool JumpWasPressed;
    public bool JumpWasReleased;

    [Header("引用")]
    public 角色移动状态 移动状态;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    // 移动
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    // 碰撞检测
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _isBumpedHead;

    // 跳跃
    public float VetricalSpeed { get; private set; }
    private bool _jumping;
    private bool _fastFalling;
    private bool _falling;
    private float _fastFallingTime;
    private float _fastFallingSpeed;
    private int _已跳跃次数;

    // 最高点变量
    private float _最高点进度;
    private float _超过最高点阈值时间;
    private bool _是否超过最高点阈值;

    // 跳跃缓冲
    private float _跳跃缓冲计时器;
    private bool _跳跃缓冲期间释放跳跃键;


    private float _悬空跳跃计时器;

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
            Move(移动状态.地面加速度, 移动状态.地面减速度, Movement);
        }
        else
        {
            Move(移动状态.空中加速度, 移动状态.空中减速度, Movement);
        }
    }

    #region 移动
    private void Move(float 加速度, float 减速度, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TrunCheck(moveInput); // 转向检测

            Vector2 目标速度 = Vector2.zero;
            if (RunIsHeld)
            {
                目标速度 = new Vector2(moveInput.x, 0f) * 移动状态.最大跑步速度;
            }
            else
            {
                目标速度 = new Vector2(moveInput.x, 0f) * 移动状态.最大走动速度;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, 目标速度, 加速度 * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, 减速度 * Time.fixedDeltaTime);
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

    #region 跳跃
    private void JumpCheck()
    {
        /// 按下跳跃键
        if (JumpWasPressed)
        {
            _跳跃缓冲计时器 = 移动状态.跳跃缓冲时间; // 重置跳跃缓冲计时器
            _跳跃缓冲期间释放跳跃键 = false; // 标记未释放跳跃键
        }
        // 释放跳跃键
        if (JumpWasReleased)
        {
            // 如果在跳跃缓冲期间释放了跳跃键
            if (_跳跃缓冲计时器 > 0)
            {
                _跳跃缓冲期间释放跳跃键 = true;
            }
            // 在跳跃过程中释放跳跃键
            if (_jumping && VetricalSpeed > 0)
            {
                if (_是否超过最高点阈值)
                {
                    // 取消最高点的状态，启动快速下落
                    _是否超过最高点阈值 = false;
                    _fastFalling = true;
                    _fastFallingTime = 移动状态.跳跃取消窗口; // 快速下落窗口时间
                    VetricalSpeed = 0f;
                }
                else
                {
                    // 直接启动快速下落
                    _fastFalling = true;
                    _fastFallingSpeed = VetricalSpeed;
                }

            }
        }

        // 初始化跳跃
        if (_跳跃缓冲计时器 > 0f && !_jumping && (_isGrounded || _悬空跳跃计时器 > 0))
        {
            InitiateJump(1); // 执行第一次跳跃

            // 如果在跳跃缓冲期间释放了跳跃键，立即触发快速下落
            if (_跳跃缓冲期间释放跳跃键)
            {
                _fastFalling = true;
                _fastFallingSpeed = VetricalSpeed;
            }
        }

        // 多级跳跃
        else if (_跳跃缓冲计时器 > 0f && _jumping && _已跳跃次数 < 移动状态.最大跳跃次数)
        {
            _fastFalling = false; // 禁用快速下落状态
            InitiateJump(1); // 执行多级跳跃
        }
        // 空中跳跃，即玩家从地面非跳跃自然转移到空中后，再次按下跳跃键
        else if (_跳跃缓冲计时器 > 0f && _falling && _已跳跃次数 < 移动状态.最大跳跃次数 - 1)
        {
            InitiateJump(2); // 执行空中跳跃，要用掉2次跳跃次数
            _fastFalling = false; // 禁用快速下落状态
        }
        // 着陆检测
        if ((_jumping || _falling) && _isGrounded && VetricalSpeed <= 0f)
        {
            _jumping = false;
            _falling = false;
            _fastFalling = false;
            _fastFallingTime = 0f;
            _是否超过最高点阈值 = false;
            _已跳跃次数 = 0;
            // 将垂直速度设置为默认重力值
            VetricalSpeed = Physics2D.gravity.y;

        }
    }

    private void InitiateJump(int 已用跳跃次数)
    {
        AudioKit.PlaySound("resources://Audios/Sounds/jump");
        if (!_jumping)
        {
            _jumping = true;
        }
        _跳跃缓冲计时器 = 0f;
        _已跳跃次数 += 已用跳跃次数;
        VetricalSpeed = 移动状态.初始跳跃速度;
    }

    private void Jump()
    {
        // 应用重力
        if (_jumping)
        {
            // 检查触顶
            if (_isBumpedHead)
            {
                _fastFalling = true;
            }

            // 处理上升阶段的重力逻辑
            if (VetricalSpeed >= 0f)
            {
                // 计算跳跃的最高点进度
                _最高点进度 = Mathf.InverseLerp(移动状态.初始跳跃速度, 0f, VetricalSpeed);
                // 判断是否超过最高点阈值
                if (_最高点进度 > 移动状态.最高点阈值)
                {
                    处理超过最高点阈值();
                }
                else
                {
                    // 正常上升逻辑，应用重力加速度
                    VetricalSpeed += 移动状态.重力加速度 * Time.fixedDeltaTime;
                    重置最高点阈值判断();
                }
            }
            // 处理跳跃释放后的快速下落逻辑
            else if (!_fastFalling)
            {
                VetricalSpeed += 移动状态.重力加速度 * 移动状态.跳跃释放重力倍率 * Time.fixedDeltaTime;
            }
            // 处理进入下落状态的标记
            else if (VetricalSpeed < 0f)
            {
                SetFallingState();
            }
        }

        // 跳跃取消
        if (_fastFalling)
        {
            处理快速下落();
        }

        // 正常下落逻辑（非跳跃状态且未接触地面）
        if (!_isGrounded && !_jumping)
        {
            SetFallingState();

            // 应用正常重力加速度
            VetricalSpeed += 移动状态.重力加速度 * Time.fixedDeltaTime;
        }
        // 限制垂直速度范围
        VetricalSpeed = Mathf.Clamp(VetricalSpeed, -移动状态.最大下落速度, 50f);
        // 更新刚体速度
        _rb.velocity = new Vector2(_rb.velocity.x, VetricalSpeed);
    }

    private void SetFallingState()
    {
        if (!_falling)
        {
            _falling = true;
        }
    }

    private void 处理快速下落()
    {
        if (_fastFallingTime >= 移动状态.跳跃取消窗口)
        {
            VetricalSpeed += 移动状态.重力加速度 * 移动状态.跳跃释放重力倍率 * Time.fixedDeltaTime;
        }
        else if (_fastFallingTime <= 移动状态.跳跃取消窗口)
        {
            VetricalSpeed = Mathf.Lerp(_fastFallingSpeed, 0f, (_fastFallingTime / 移动状态.跳跃取消窗口));
        }
        _fastFallingTime += Time.fixedDeltaTime;
    }

    private void 重置最高点阈值判断()
    {
        if (_是否超过最高点阈值)
        {
            _是否超过最高点阈值 = false;
        }
    }

    private void 处理超过最高点阈值()
    {
        if (!_是否超过最高点阈值)
        {
            _是否超过最高点阈值 = true;
            _超过最高点阈值时间 = 0f;
        }

        if (_是否超过最高点阈值)
        {
            _超过最高点阈值时间 += Time.fixedDeltaTime;
            if (_超过最高点阈值时间 < 移动状态.最高点停留时间)
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

    #region 碰撞检测
    private void isGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, 移动状态.地面射线长度);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, 移动状态.地面射线长度, 移动状态.地面层);

        _isGrounded = _groundHit.collider != null;

        DebugUIDrawer(boxCastOrigin, boxCastSize);
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * 移动状态.顶部宽度, 移动状态.顶部射线长度);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, 移动状态.顶部射线长度, 移动状态.地面层);

        _isBumpedHead = _headHit.collider != null;

        DebugUIDrawer(boxCastOrigin, boxCastSize);
    }
    private void CollisionCheck()
    {
        isGrounded();
        BumpedHead();
    }
    #endregion

    #region 计时器
    private void CountTimer()
    {
        _跳跃缓冲计时器 -= Time.deltaTime;
        if (!_isGrounded)
        {
            _悬空跳跃计时器 -= Time.deltaTime;
        }
        else
        {
            _悬空跳跃计时器 = 移动状态.悬空跳跃缓冲时间;
        }
    }
    #endregion

    #region DebugUI
    private void DebugUIDrawer(Vector2 boxCastOrigin, Vector2 boxCastSize)
    {
        if (移动状态.Debug头部碰撞箱)
        {
            float headWidth = 移动状态.顶部宽度;
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
                Vector2.up * 移动状态.顶部射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y),
                Vector2.up * 移动状态.顶部射线长度, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - 移动状态.顶部射线长度),
                Vector2.right * boxCastSize.x * headWidth, rayColor);
        }


        if (移动状态.Debug地面碰撞箱)
        {
            Color rayColor = _isGrounded ? Color.green : Color.red;

            // 绘制BoxCast的四条边
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