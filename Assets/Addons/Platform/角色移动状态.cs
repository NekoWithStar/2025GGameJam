using UnityEngine;


/// <summary>
/// 2D平台角色移动状态
/// 本脚本用于定义角色的移动状态，包括走、跑、地面检测、碰撞检测等
/// 可移植到qframework框架中进行全局管理
/// </summary>

[CreateAssetMenu(menuName = "@Physics/2D平台角色移动")]
public class 角色移动状态 : ScriptableObject
{
    [Header("走")]
    [Range(1f, 100f)] public float 最大走动速度 = 12.5f;
    [Range(0.25f, 50f)] public float 地面加速度 = 5f; // 即起步的加速度
    [Range(0.25f, 50f)] public float 地面减速度 = 20f; // 即停止时的加速度
    [Range(0f, 50f)] public float 空中加速度 = 5f; // 即空中移动时的加速度
    [Range(0f, 50f)] public float 空中减速度 = 5f; // 即空中停止时的加速度

    [Header("跑")]
    [Range(1f, 100f)] public float 最大跑步速度 = 20f;

    [Header("地面检测/碰撞检测")]
    public LayerMask 地面层;
    public float 地面射线长度 = 0.02f;
    public float 顶部射线长度 = 0.02f;
    [Range(0f, 1f)] public float 顶部宽度 = 0.75f;

    [Header("跳")]
    public float 跳跃高度 = 6.5f;
    [Range(1f, 1.1f)] public float 跳跃高度补偿因子 = 1.054f; // 跳跃高度的补偿，用于调整跳跃高度
    public float 达顶时间 = 0.35f; // 到达最高点的时间
    [Tooltip("这是对重力的修正因子")]
    [Range(0f, 5f)] public float 跳跃释放重力倍率 = 1f; // 这是对重力的修正因子
    public float 最大下落速度 = 26f;
    public int 最大跳跃次数 = 2;

    [Header("跳跃行为细节")]
    [Tooltip("玩家在跳跃过程中松开跳跃键，会在一个短时间窗口内取消向上运动")]
    [Range(0f, 0.3f)] public float 跳跃取消窗口 = 0.027f;
    [Tooltip("达到最高点要求的百分之多少就算达到最高点")]
    [Range(0.5f, 1f)] public float 最高点阈值 = 0.97f;
    [Tooltip("达到最高点后停留的时间")]
    [Range(0.01f, 1f)] public float 最高点停留时间 = 0.075f;

    [Tooltip("当玩家落地前按下跳跃键，跳跃指令会存储一段时间，等玩家落地后再执行跳跃")]
    [Range(0f, 1f)] public float 跳跃缓冲时间 = 0.125f;
    [Tooltip("当玩家从地面转移到空中，会允许一段时间内执行跳跃指令，这就是Coyote Jump")]
    [Range(0f, 1f)] public float 悬空跳跃缓冲时间 = 0.1f;


    [Header("碰撞Debug")]
    public bool Debug地面碰撞箱 = false;
    public bool Debug头部碰撞箱 = false;

    [Header("跳跃可视化工具")]
    public bool 显示移动跳跃轨迹 = false;
    public bool 显示跑动跳跃轨迹 = false;
    public bool 轨迹碰撞截止 = true;
    public bool 绘制在右 = true;
    [Range(5, 100)] public int 轨迹分辨率 = 20;
    [Range(0, 500)] public int 轨迹点步长 = 90;

    public float 重力加速度 { get; private set; }
    public float 初始跳跃速度 { get; private set; }

    public float 调整高度 { get; private set; }

    private void OnValidate()
    {

        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();

    }

    private void CalculateValues()
    {
        调整高度 = 跳跃高度 * 跳跃高度补偿因子;
        重力加速度 = -(2f * 调整高度) / Mathf.Pow(达顶时间, 2f);
        初始跳跃速度 = Mathf.Abs(重力加速度) * 达顶时间;
    }
}
