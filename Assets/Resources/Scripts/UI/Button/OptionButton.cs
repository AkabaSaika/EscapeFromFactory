using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {
        nextPanel.SetActive(true);
        UIManager.Instance.PanelStack.Push(currPanel);
        currPanel.SetActive(false);     
    }
}
