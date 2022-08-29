using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelBase
{
    void Init();
    void OnShowing();
    void OnShowed();
    void Update();
    void OnClosing();
    void OnClosed();
}
