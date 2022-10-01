using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearTimer : MonoBehaviour
{
    [SerializeField]
    private float timer;
    [SerializeField]
    private string timeStr;
    [SerializeField]
    private TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.GetPauseState()&&!GameManager.Instance.GameClearState)
        {
            timer += Time.deltaTime;
            timeStr = System.TimeSpan.FromSeconds(timer).ToString(@"hh\:mm\:ss\:ff");
           
        }
        timeText.text = "Clear Time:\n" + timeStr;
    }
}
