using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanelController : MonoBehaviour
{
    [SerializeField]
    private Image hpBar;
    private PlayerController player;
    [SerializeField]
    private GameObject statusWindow;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();    
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = player.CurrentHp / player.parameter.MaxHp;

        switch (GameManager.Instance.gameState)
        {
            case GameState.over:
                statusWindow.SetActive(false);
                break;
            case GameState.pause:
                statusWindow.SetActive(false);
                break;
            case GameState.clear:
                statusWindow.SetActive(false);
                break;
            case GameState.running:
                statusWindow.SetActive(true);
                break;
        }
    }
}
