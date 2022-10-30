using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {
        GameManager.Instance.isRespawnFromCheckPoint=true;
        AudioManager.Instance.MusicClear();
        AudioManager.Instance.EffectClear();
        Time.timeScale = 1.0f;
        Global.Instance.LoadNextScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.gameState = GameState.running;
    }
}
