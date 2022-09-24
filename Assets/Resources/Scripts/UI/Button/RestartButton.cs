using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        Time.timeScale = 1.0f;
        Global.Instance.LoadNextScene(SceneManager.GetActiveScene().name);
    }
}
