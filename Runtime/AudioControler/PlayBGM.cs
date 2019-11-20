using UnityEngine;
using System.Collections;

public class PlayBGM : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        Playbgm();
    }
    public void Playbgm()
    {
        GlobeAudio.Instance.PlayBGM(clip);
    }
}
