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

    public float ����������;
    [SerializeField] private bool �Ƿ��ڷ�Χ��;
    private Player mPlayer;
    private Transform playerBody;

    public void Awake()
    {
        ʣ������������� = mLevelSettings.�ܿ�����������;
    }

    private void Start()
    {
        mPlayer = FindObjectOfType<Player>();
        playerBody = mPlayer.Body.transform;
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
        CheckDistance();
        if (ʣ������������� > 0 && �Ƿ��ڷ�Χ��)
        {
            ��������();
            �̶�����();
            ��������();
        }
    }

    public void CheckDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Debug.Log(playerBody.position);
        �Ƿ��ڷ�Χ�� = Vector3.Distance(mousePos, playerBody.position) < ����������;
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
            a.transform.gameObject.SetActive(true);
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
            a.transform.gameObject.SetActive(true);
            ʣ�������������--;
        }
    }

    public void ��������()
    {
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;

            Transform a = Holder.BubbleChat.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);

            a.transform.gameObject.SetActive(true);

            BubbleChat bubbleChat = a.GetComponent<BubbleChat>();
            bubbleChat.isRight = true;

            a.GetComponent<SpriteRenderer>().enabled = true;

            ʣ�������������--;
        }
    }
}
