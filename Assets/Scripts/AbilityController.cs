using UnityEngine;
using QFramework;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public int 剩余可用能力次数;
    public string 选中能力;

    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
    }

    private void OnGUI()
    {
        IMGUIHelper.SetDesignResolution(960, 640);
        string 可用能力 = "";
        foreach (var item in mLevelSettings.可用能力列表)
        {
            int num = (int)item;
            可用能力 += " " + AblilityBase.能力表[num];
        }
        GUILayout.Label("可用能力 " + 可用能力);
        GUILayout.Label("选中能力 " + 选中能力);
        GUILayout.Label("总可用能力次数:" + mLevelSettings.总可用能力次数);
    }
    public void Update()
    {

    }
}
