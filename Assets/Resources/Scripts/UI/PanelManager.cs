using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoSingleton<PanelManager>
{
    public void OpenPanel<T>() where T:IPanelBase
    {

    }
}
