using System.Collections.Generic;
using UnityEngine;

public enum ��������
{
    �������� = 0,
    �̶����� = 1,
    �������� = 2,
    �������� = 3,
}

public class AbilityBase : MonoBehaviour
{
    public static Dictionary<int, string> ���������� = new Dictionary<int, string>
    {
        { 0, "��������" },
        { 1, "�̶�����" },
        { 2, "��������" },
        { 3, "��������" }
    };

}
