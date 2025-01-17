using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public NekoUI NekoUI;
    public AbilityHolder Holder;

    public int ʣ�������������;
    public int ѡ������;

    public void Awake()
    {
        ʣ������������� = mLevelSettings.�ܿ�����������;
    }

    private void Start()
    {
    }

    private void OnGUI()
    {
        if (ѡ������ <= mLevelSettings.���������б�.Count - 1)
        {
            NekoUI.ѡ������.text = "ѡ��������" + AbilityBase.������[ѡ������];
        }
    }

    public void Update()
    {
        ����ѡ������();
        if (ʣ������������� > 0)
        {
            ��������();
            �̶�����();
            ��������();
        }
    }

    public void ����ѡ������()
    {
        if (ѡ������ < 0)
        {
            ѡ������ = mLevelSettings.���������б�.Count - 1;
        }
        else if (ѡ������ > mLevelSettings.���������б�.Count - 1)
        {
            ѡ������ = 0;
        }
    }

    public void ��������()
    {
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            a.GetComponent<BubbleNormal>().enabled = true;
            a.GetComponent<SpriteRenderer>().enabled = true;
            ʣ�������������--;
        }
    }

    public void �̶�����()
    {
        if (ѡ������ == (int)��������.�̶����� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleFixed.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            a.GetComponent<BubbleFixed>().enabled = true;
            a.GetComponent<SpriteRenderer>().enabled = true;
            ʣ�������������--;
        }
    }

    public void ��������()
    {
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;

            Vector3 playerPos = transform.position;
            playerPos.z = 0; 

            Vector3 directionToMouse = (localPos - playerPos).normalized;
            Vector3 bubblePos=playerPos;

            Transform a = Holder.BubbleChat.InstantiateWithParent(transform.Find("Root")).LocalPosition(bubblePos);

            BubbleChat bubbleChat = a.GetComponent<BubbleChat>();
            bubbleChat.enabled = true;
            bubbleChat.direction = directionToMouse; 

            a.GetComponent<SpriteRenderer>().enabled = true;

            ʣ�������������--;
        }
    }   
}
