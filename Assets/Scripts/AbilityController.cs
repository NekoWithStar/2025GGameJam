using QFramework;
using QFramework.Example;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public static bool CanPlace;

    public NekoUI NekoUI;
    public AbilityHolder Holder;

    public int ʣ�������������;
    public int ѡ������;

    public float ����������;
    public bool �Ƿ��ڷ�Χ��;
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
    }

    private void Start()
    {
        AudioKit.Settings.IsSoundOn.Value = true;
        mPlayer = FindObjectOfType<Player>();
        playerBody = mPlayer.Body.transform;
    }

    private void OnGUI()
    {
        
    }

    public void Update()
    {
        ����ѡ������();
        CheckDistance();
        UpdatedUI();
        if (ʣ������������� > 0 && �Ƿ��ڷ�Χ��)
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
            panel_images[i].sprite = unlocked_sprites[i];
        }
        panel_images[ѡ������].sprite = selected_sprites[ѡ������];
    }

    public void CheckDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
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
        if (LockSwitch)
        {
            ѡ������ = (int)��������.��������;
        }
    }

    public void ��������()
    {
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
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
        if (ѡ������ == (int)��������.�̶����� && Input.GetMouseButtonDown(0))
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
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
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
        if (ѡ������ == (int)��������.�������� && Input.GetMouseButtonDown(0))
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
