using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject nextPanel;
    [SerializeField]
    protected GameObject currentPanel;
    [SerializeField]
    protected GameObject canvas;
    [SerializeField]
    protected string audioEffectPath;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        audioEffectPath = "Audio/UI_Buttons_Pack2/Button_15_Pack2";
    }
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { PushDownAction(currentPanel, nextPanel); });
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.EffectPlay(audioEffectPath, false); });
    }


    protected virtual void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {
        return;
    }


}
