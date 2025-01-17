using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelSettings", menuName = "LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    public string 关卡名称;
    public int 关卡序号;
    public int 总可用能力次数;
    public List<可用能力> 可用能力列表;
}
