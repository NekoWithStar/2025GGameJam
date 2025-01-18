using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float ��������ˮƽ�ٶ�;
    public float �������������ٶ�;
    public float ����ʱ�� = 1f; // �����ٶȵ�ʱ��

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
        float initialˮƽ�ٶ� = bubble.ˮƽ�ٶ�;
        float initial�����ٶ� = bubble.�����ٶ�;

        while (elapsedTime < ����ʱ��)
        {
            bubble.ˮƽ�ٶ� = Mathf.Lerp(initialˮƽ�ٶ�, initialˮƽ�ٶ� + ��������ˮƽ�ٶ�, elapsedTime / ����ʱ��);
            bubble.�����ٶ� = Mathf.Lerp(initial�����ٶ�, initial�����ٶ� + �������������ٶ�, elapsedTime / ����ʱ��);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ�������ٶ�ֵ׼ȷ
        bubble.ˮƽ�ٶ� = initialˮƽ�ٶ� + ��������ˮƽ�ٶ�;
        bubble.�����ٶ� = initial�����ٶ� + �������������ٶ�;
    }
}

