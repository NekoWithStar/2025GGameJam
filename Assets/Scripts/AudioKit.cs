using QFramework;
using System.Collections;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
    public LevelSettings levelSettings;
    void Start()
    {
        QFramework.AudioKit.PlayMusic("resources://Audios/Musics/" + levelSettings.BGMÃû³Æ);
        AudioKit.Settings.IsMusicOn.Value = true;
    }
}
