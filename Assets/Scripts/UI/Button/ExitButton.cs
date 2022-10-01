using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {
        if(currPanel!=null)
        {
            Application.Quit();    
        }
    }
}
