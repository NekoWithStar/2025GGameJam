using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float 增加泡泡水平速度;
    public float 增加泡泡上升速度;
    public float 增加时间 = 1f; // 增加速度的时间

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BubbleNormal>())
        {
            BubbleNormal bubble = collision.gameObject.GetComponent<BubbleNormal>();
            StartCoroutine(IncreaseSpeedOverTime(bubble));
        }
    }

    private IEnumerator IncreaseSpeedOverTime(BubbleNormal bubble)
    {
        float elapsedTime = 0f;
        float initial水平速度 = bubble.水平速度;
        float initial上升速度 = bubble.上升速度;

        while (elapsedTime < 增加时间)
        {
            bubble.水平速度 = Mathf.Lerp(initial水平速度, initial水平速度 + 增加泡泡水平速度, elapsedTime / 增加时间);
            bubble.上升速度 = Mathf.Lerp(initial上升速度, initial上升速度 + 增加泡泡上升速度, elapsedTime / 增加时间);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终速度值准确
        bubble.水平速度 = initial水平速度 + 增加泡泡水平速度;
        bubble.上升速度 = initial上升速度 + 增加泡泡上升速度;
    }
}

