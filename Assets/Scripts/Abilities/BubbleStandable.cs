using UnityEngine;

public class BubbleNormal : MonoBehaviour
{
    public float 上升速度 = 1f;

    private void Awake()
    {
        enabled = false;
    }
    private void Update()
    {
        // 每秒上升
        transform.position += new Vector3(0, 上升速度 * Time.deltaTime, 0);
    }
}
