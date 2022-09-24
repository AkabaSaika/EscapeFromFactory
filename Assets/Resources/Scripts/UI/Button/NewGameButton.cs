using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        Global.Instance.LoadNextScene("MainStage");
    }
}
