using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelKit : MonoBehaviour
{
    public static int CurrentLevel = 0;
    public static void LoadLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level.ToString());
    }
}
