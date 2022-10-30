using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    private GameObject enemyRoot;
    private GameObject patrolPointRoot;
    private GameObject respawnPointRoot;

    public void InitStage(bool isRespawnFromCheckPoint)
    {

           
        enemyRoot = new GameObject("Enemy");
        patrolPointRoot = new GameObject("PatrolRoot");
        patrolPointRoot.transform.SetParent(enemyRoot.transform);
        respawnPointRoot = new GameObject("RespawnRoot");
        respawnPointRoot.transform.SetParent(enemyRoot.transform);
        enemyRoot.transform.SetParent(GameObject.Find("Character").transform);
        for(int i=0;i<12;i++)
        {
            InitCharacter(TablesSingLeton.Instance.Tables.TbRespawnPoints.Get(3001+i));
        }
        InitPlayer(TablesSingLeton.Instance.Tables.TbRespawnPoints.Get(3013),isRespawnFromCheckPoint);
        if(isRespawnFromCheckPoint)
        {
            Camera.main.transform.position=GameObject.FindGameObjectWithTag("Player").transform.position+Vector3.forward*10;
        }
        
    }
    private void InitCharacter(cfg.stage.Respawn respawn)
    {
        string path = "Prefabs/Object/" + respawn.PrefabName;
        GameObject prefab = Resources.Load(path) as GameObject;
        GameObject enemy = Instantiate(prefab,respawn.RespawnPoint,Quaternion.Euler(respawn.Rotation));
        
        enemy.transform.SetParent(enemyRoot.transform);
        enemy.name = "Enemy" + respawn.Id.ToString();
        GameObject respawnPoint = new GameObject(enemy.name + "RespawnPoint");
        respawnPoint.transform.SetParent(respawnPointRoot.transform);
        respawnPoint.transform.position = respawn.RespawnPoint;
        Transform[] pp = new Transform[respawn.PatrolPoints.Length];
        for(int i=0;i<pp.Length;i++)
        {
            GameObject patrolPointObject = new GameObject(enemy.name + "PatrolPoint" +i.ToString());
            patrolPointObject.transform.SetParent(patrolPointRoot.transform);
            patrolPointObject.transform.position = respawn.PatrolPoints[i];
            enemy.GetComponent<FSM>().parameter.patrolPoints[i] = patrolPointObject.transform;
        }
        enemy.GetComponent<FSM>().parameter.MAX_CHASE_DISTANCE = respawn.MaxChaseDistance;
        enemy.GetComponent<FSM>().parameter.respawnPoint = respawnPoint.transform;
    }

    private void InitPlayer(cfg.stage.Respawn respawn,bool isRespawnFromCheckPoint)
    {
        string path = "Prefabs/Object/" + respawn.PrefabName;
        GameObject prefab = Resources.Load(path) as GameObject;
        GameObject player;
        if(isRespawnFromCheckPoint)
        {
            player = Instantiate(prefab,GameManager.Instance.checkPointRespawnPos,Quaternion.identity);
        }
        else
        {
            player = Instantiate(prefab,respawn.RespawnPoint,Quaternion.Euler(respawn.Rotation));
        }
        
        
        player.transform.SetParent(enemyRoot.transform);
        player.name = "Player";
        GameObject respawnPoint = new GameObject(player.name + "RespawnPoint");
        respawnPoint.transform.SetParent(respawnPointRoot.transform);
        respawnPoint.transform.position = respawn.RespawnPoint;
    }
}
