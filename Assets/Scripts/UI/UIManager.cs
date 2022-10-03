using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    private Stack<GameObject> panelStack;
    public Stack<GameObject> PanelStack { get => panelStack; set => panelStack = value; }

    private void Awake()
    {
        panelStack = new Stack<GameObject>();
    }
}
