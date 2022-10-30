using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {

            Application.Quit();    

    }
}
