using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    private GameObject enemyRoot;

    public void InitStage()
    {
        
        enemyRoot = new GameObject("Enemy");
        enemyRoot.transform.SetParent(GameObject.Find("Character").transform);
        for(int i=0;i<2;i++)
        {
            InitCharacter(TablesSingLeton.Instance.Tables.TbRespawnPonts.Get(2001+i));
        }
        
    }
    private void InitCharacter(cfg.test.Respawn respawn)
    {
        string path = "Prefabs/Object/" + respawn.PrefabName;
        GameObject prefab = Resources.Load(path) as GameObject;
        GameObject enemy = Instantiate(prefab,respawn.RespawnPoint,Quaternion.Euler(respawn.Rotation));
        enemy.transform.SetParent(enemyRoot.transform);
        enemy.name = "Enemy" + respawn.Id.ToString();
        Transform[] pp = new Transform[respawn.PatrolPoints.Length];
        for(int i=0;i<pp.Length;i++)
        {
            GameObject patrolPointObject = new GameObject("PatrolPoint"+i.ToString());
            patrolPointObject.transform.SetParent(enemy.transform);
            patrolPointObject.transform.position = respawn.PatrolPoints[i];
            enemy.GetComponent<FSM>().parameter.patrolPoints[i] = patrolPointObject.transform;
        }
        enemy.GetComponent<FSM>().parameter.MAX_CHASE_DISTANCE = respawn.MaxChaseDistance;
        
    }
}
