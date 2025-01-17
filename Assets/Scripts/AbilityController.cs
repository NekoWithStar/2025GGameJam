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
        GUILayout.Label(text: "当前可用能力列表：" + mLevelSettings.可用能力列表);
        GUILayout.Label("总可用能力次数:" + mLevelSettings.总可用能力次数);
    }
    public void Update()
    {

    }

    public void UseAbility()
    {
    
    }

}
