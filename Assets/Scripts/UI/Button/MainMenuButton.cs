using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.MusicClear();
        AudioManager.Instance.EffectClear();
        Global.Instance.LoadNextScene("StartMenu");
    }
}
