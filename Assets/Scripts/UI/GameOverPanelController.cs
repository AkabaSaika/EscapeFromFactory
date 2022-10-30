using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelController : MonoBehaviour
{
    private void OnEnable() {
        if(GameManager.Instance.lastCheckPoint!=null)
        {
            string[] pathArray=new string[] {"CheckPoint","Restart","MainMenu"};
            for(int i=0;i<3;i++)
            {
                GameObject checkPointPrefab = CreateButton(pathArray[i],-75*(i+1));
            }
        }
        else
        {
            string[] pathArray=new string[] {"Restart","MainMenu"};
            for(int i=0;i<2;i++)
            {
                GameObject checkPointPrefab = CreateButton(pathArray[i],-75*(i+1));
            }
        }
        
    }

    private GameObject CreateButton(string name,float PosY)
    {
        string path = "Prefabs/UI/Button/";
        GameObject prefab = Resources.Load(path+name) as GameObject;
        GameObject button = Instantiate(prefab);
        button.name=name;
        button.transform.parent=transform.Find("GameOverWindow");
        
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,1);
        button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,-PosY-40,55);
        button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,45,160);
        button.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        //button.GetComponent<RectTransform>().localPosition=new Vector3(0,PosY,0);
        
        return button;
    }
}
