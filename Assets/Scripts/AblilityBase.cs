using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum 可用能力
{
    上升泡泡 = 0,
    固定泡泡 = 1,
    聊天泡泡 = 2,
}

public class AblilityBase : MonoBehaviour
{
    public static Dictionary<int, string> 能力表 = new Dictionary<int ,string>
    {
        { 0, "上升泡泡" },
        { 1, "固定泡泡" },
        { 2, "聊天泡泡" }
    };
}
