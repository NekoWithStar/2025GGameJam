using UnityEngine;
using System.Collections;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
    public partial class Player : ViewController
	{
        Animator animator;

		void Start()
		{
			animator = GetComponent<Animator>();
            animator.SetBool("IsDead", false);
		}
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Spike"))
            {
                animator.SetBool("IsDead", true);
                StartCoroutine(Die());
            }
        }
        private IEnumerator Die()
        {
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
