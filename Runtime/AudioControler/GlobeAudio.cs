using SeanLib.Core;
using System.Collections.Generic;

using UnityEngine;
public class GlobeAudio : AudioControler, IGameVolume
{
    public AudioSource Player;
    public AudioSource Recorder;

    [Range(0,1)]
    public float BackgroundSound=0.3f;
    [Range(0, 1)]
    public float EffectSound=1;

    public float backgroundSound
    {
        get
        {
            return BackgroundSound;
        }

        set
        {
            BackgroundSound = value;
        }
    }

    public float effectSound { get { return EffectSound; } set
        {
            EffectSound = value;
        } }
}
