using UnityEngine;
using QFramework;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public int 剩余可用能力次数;
    public 能力 选中能力;

    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
    }

    private void OnGUI()
    {
        IMGUIHelper.SetDesignResolution(960, 640);
        string 可用能力 = "";
        foreach(var 能力 in mLevelSettings.可用能力列表)
        {
            可用能力 += nameof(能力);
            if (GUILayout.Button(能力.ToString()))
            {
                选中能力 = 能力;
            }
        }
        GUILayout.Label("可用能力 " + 可用能力);
        GUILayout.Label("总可用能力次数:" + mLevelSettings.总可用能力次数);
    }
    public void Update()
    {

    }

    public void UseAbility()
    {
    
    }

}
