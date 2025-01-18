using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cmr;
    public Transform page0;
    public Transform page1;
    public Transform page2;
    public Transform page3;
    private List<Transform> mlist;
    private int index = 0;
    private void Start()
    {
        mlist = new List<Transform>
        {
            page0,
            page1,
            page2,
            page3
        };
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index >= mlist.Count)
            {
                LevelKit.LoadLevel(1);
            }
            else
            {
                cmr.LookAt = mlist[index];
                cmr.Follow = mlist[index];
            }
        }
    }
}
