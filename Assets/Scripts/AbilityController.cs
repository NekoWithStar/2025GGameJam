using QFramework;
using QFramework.Example;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public static bool CanPlace = true;

    public AbilityHolder Holder;

    public int ʣ�������������;
    public int listIndex;
    public int ѡ������key ;

    public float ����������;
    public static bool �Ƿ��ڷ�Χ��;
    public bool LockSwitch;
    private Player mPlayer;
    private Transform playerBody;

    private BubbleTele tempBubbleTele;

    public Panel panel;
    public List<Sprite> unlocked_sprites;
    public List<Sprite> selected_sprites;
    public List<Image> panel_images;
    public Sprite Locked;

    public void Awake()
    {
        ʣ������������� = mLevelSettings.�ܿ�����������;
        ѡ������key = (int)mLevelSettings.���������б�[0];
    }

    private void Start()
    {
        AudioKit.Settings.IsSoundOn.Value = true;
        mPlayer = FindObjectOfType<Player>();
        playerBody = mPlayer.Body.transform;
    }

    public void Update()
    {
        ����ѡ������();
        CheckDistance();
        UpdatedUI();
        panel.Count.text = "ʣ������ " + ʣ�������������;
        ѡ������key = (int)mLevelSettings.���������б�[listIndex];
        if (ʣ������������� > 0 && �Ƿ��ڷ�Χ�� && !InputManager.IsPaused && CanPlace)
        {
            ��������();
            �̶�����();
            ��������();
            ��������();
        }
    }

    private void UpdatedUI()
    {
        for (int i = 0 ; i < 4; i++)
        {
            panel_images[i].sprite = Locked;
        }
        for (int i = 0; i < mLevelSettings.���������б�.Count; i++)
        {
            int gkey = (int)mLevelSettings.���������б�[i];
            panel_images[i].sprite = unlocked_sprites[gkey];
        }
        int mkey = (int)mLevelSettings.���������б�[listIndex];
        panel_images[listIndex].sprite = selected_sprites[mkey];
    }

    public void CheckDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        �Ƿ��ڷ�Χ�� = Vector3.Distance(mousePos, playerBody.position) < ����������;
    }

    public void ����ѡ������()
    {
        if (listIndex < 0)
        {
            listIndex = mLevelSettings.���������б�.Count - 1;
        }
        else if (listIndex > mLevelSettings.���������б�.Count - 1)
        {
            listIndex = 0;
        }
        if (LockSwitch)
        {
            int index = mLevelSettings.���������б�.FindIndex(ability => (int)ability == 4);
            if (index != -1)
            {
                listIndex = index;
            }
        }
    }

    public void ��������()
    {
        if (ѡ������key == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);
            ʣ�������������--;
        }
    }

    public void �̶�����()
    {
        if (ѡ������key == (int)��������.�̶����� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleFixed.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);
            ʣ�������������--;
        }
    }

    public void ��������()
    {
        if (ѡ������key == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;

            Transform a = Holder.BubbleChat.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);

            BubbleChat bubbleChat = a.GetComponent<BubbleChat>();
            bubbleChat.isRight = true;

            a.GetComponent<SpriteRenderer>().enabled = true;

            ʣ�������������--;
        }
    }

    public void ��������()
    {
        if (ѡ������key == (int)��������.�������� && Input.GetMouseButtonDown(0))
        {
            if (!LockSwitch)
            {
                LockSwitch = true;
                Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                localPos.z = 0;
                Transform a = Holder.BubbleTele.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
                if (!CheckCollision(a))
                {
                    return;
                }
                tempBubbleTele = a.GetComponent<BubbleTele>();
                a.transform.gameObject.SetActive(true);
                ʣ�������������--;
            }
            else
            {
                LockSwitch = false;
                Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                localPos.z = 0;
                Transform a = Holder.BubbleTele.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
                if (!CheckCollision(a))
                {
                    return;
                }
                a.transform.gameObject.SetActive(true);
                a.GetComponent<BubbleTele>().pairBubble = tempBubbleTele;
                tempBubbleTele.pairBubble = a.GetComponent<BubbleTele>();
                ʣ�������������--;
            }
        }
    }

    public bool CheckCollision(Transform bubble)
    {
        // ��ȡ���ݵ���ײ��
        Collider2D bubbleCollider = bubble.GetComponent<Collider2D>();

        // ��������Ƿ����������巢����ײ
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bubble.position, bubbleCollider.bounds.size, 0f);

        foreach (var collider in colliders)
        {
            // �����ײ���Ĳ��������������Ϊ���ܷ���
            if (!collider.gameObject.CompareTag("Player"))
            {
                //return false;
            }
        }
        return true;
    }
}
