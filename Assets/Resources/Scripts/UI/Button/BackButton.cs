using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        currentPanel.SetActive(false);
        UIManager.Instance.PanelStack.Pop().SetActive(true);
    }
}
