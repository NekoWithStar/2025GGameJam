using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public AblilityHolder AblilityHolder;

    public int ʣ�������������;

    public int ѡ������;

    public void Awake()
    {
        ʣ������������� = mLevelSettings.�ܿ�����������;
    }

    private void Start()
    {
        var a = AblilityHolder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(new Vector3(0, 2));
        a.GetComponent<BubbleNormal>().enabled = true;
        a.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnGUI()
    {
        IMGUIHelper.SetDesignResolution(960, 640);
        string �������� = "";
        foreach (var item in mLevelSettings.���������б�)
        {
            int num = (int)item;
            �������� += " " + AblilityBase.������[num];
        }
        GUILayout.Label("�������� " + ��������);
        GUILayout.Label("ѡ������ " + AblilityBase.������[ѡ������]);
        GUILayout.Label("�ܿ�����������:" + mLevelSettings.�ܿ�����������);
    }
    public void Update()
    {
        
    }
}
