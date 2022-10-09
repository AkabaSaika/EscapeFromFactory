using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        currentPanel.SetActive(false);
        UIManager.Instance.PanelStack.Pop().SetActive(true);
        if (GameObject.Find("LogoImage"))
        {
            GameObject.Find("LogoImage").GetComponent<Image>().enabled = true;
        }
    }
}
