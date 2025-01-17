using System.Collections.Generic;
using UnityEngine;


public enum 能力
{
    上升泡泡,
    聊天框泡泡,
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    public int 总可用能力次数;
    public List<能力> 可用能力列表;
}