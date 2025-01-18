using QFramework;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
    public LevelSettings levelSettings;
    void Start()
    {
        QFramework.AudioKit.PlayMusic("resources://Audios/Musics/" + levelSettings.BGM����);
        AudioKit.Settings.IsMusicOn.Value = true;
    }
}
