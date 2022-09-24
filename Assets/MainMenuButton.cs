using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        SceneManager.LoadScene(0);
    }
}
