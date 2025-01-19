using UnityEngine;

namespace QFramework.Example
{
    public partial class CursorKit : ViewController
    {
        public Color 允许创建;
        public Color 不可创建;
        public LayerMask layerGround;
        public float radius = 5f;

        private void Update()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0);

            // 使用 Physics2D.OverlapCircleAll 检测碰撞
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y), radius, layerGround);
            // 检查是否包含 layerGround
            AbilityController.CanPlace = colliders.Length == 0;

            // 更新颜色
            if (AbilityController.CanPlace && AbilityController.是否在范围内)
            {
                Circle.color = 允许创建;
            }
            else
            {
                Circle.color = 不可创建;
            }

            if(InputManager.IsPaused)
            {
                Circle.color = Color.clear;
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 设置 Gizmos 颜色
            Gizmos.color = AbilityController.CanPlace ? 允许创建 : 不可创建;

            // 绘制圆形范围
            Gizmos.DrawWireSphere(pos, radius);
        }
    }
}
