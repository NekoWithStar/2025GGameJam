using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class CursorKit : ViewController
	{
        public Color 允许创建;
        public Color 不可创建;
        public LayerMask layerGround;

        private void Update()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0);
            if (AbilityController.CanPlace)
            {
                Circle.color = 允许创建;
            }
            else
            {
                Circle.color = 不可创建;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 6)
            {
                AbilityController.CanPlace = false;
            }
            else
            {
                AbilityController.CanPlace = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 6)
            {
                AbilityController.CanPlace = true;
            }
        }
    }
}
