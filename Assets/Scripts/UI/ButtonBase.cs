using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンの基底クラス
/// </summary>
public class ButtonBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject nextPanel;//このボタンを押すを開くパネル
    [SerializeField]
    protected GameObject currentPanel;//このボタンが所属しているパネル
    [SerializeField]
    protected GameObject canvas;
    [SerializeField]
    protected string audioEffectPath;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        audioEffectPath = "Audio/UI_Buttons_Pack2/Button_15_Pack2";//ボタンエフェクトのパス
    }
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { PushDownAction(currentPanel, nextPanel); });
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.EffectPlay(audioEffectPath, false); });
    }

    /// <summary>
    /// ボタンを押すと発生するイベント。
    /// 派生クラスで実装する。
    /// </summary>
    /// <param name="currPanel"></param>
    /// <param name="nextPanel"></param>
    protected virtual void PushDownAction(GameObject currPanel,GameObject nextPanel)
    {
        return;
    }


}
