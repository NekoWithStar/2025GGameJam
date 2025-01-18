using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public int gotoLevel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelKit.CurrentLevel = gotoLevel;
            LevelKit.LoadLevel(gotoLevel);
        }
    }
}
