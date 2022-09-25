using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceBehaviour : VoiceBehaviour
{
    public void Play(string path)
    {
        AudioManager.Instance.EffectPlay(path, false);
    }
}
