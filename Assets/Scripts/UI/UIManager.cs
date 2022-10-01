using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    public Image hpBar;
    public Image hpBarBackGround;
    [SerializeField]
    private Stack<GameObject> panelStack;
    private GameObject player;
    private PlayerController pc;
    public Stack<GameObject> PanelStack { get => panelStack; set => panelStack = value; }

    private void Awake()
    {
        panelStack = new Stack<GameObject>();
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("StartMenu"))
        {
            
            
        }
        else
        {
            //player = GameObject.FindGameObjectWithTag("Player");
            //pc = player.GetComponent<PlayerController>();
            //hpBar.fillMethod = Image.FillMethod.Horizontal;
        }

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("StartMenu"))
        {

        }
        else
        {
            //hpBar.fillAmount = pc.CurrentHp / 100;
        }

    }
}
