using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    private List<Transform> nearEnemy;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        nearEnemy = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        nearEnemy = DetectNearByEnemies();
        if(nearEnemy.Count>0)
        {
            for(int i = 0;i<nearEnemy.Count;i++)
            {
                if (Vector3.Distance(transform.position, nearEnemy[i].position) > 1)
                {
                    nearEnemy.Remove(nearEnemy[i]);
                }

            }
        }

    }

    public Transform TargetNearestEnemy(List<Transform> near)
    {
        Transform temp = null;
        if (near.Count == 0)
        {
            return null;
        }
        foreach (var i in near)
        {
            temp = i;
            temp = Vector3.Distance(temp.position, transform.position) > Vector3.Distance(i.position, transform.position) ? i : temp;
        }
        return temp;
    }

    public Transform TargetLeftEnemy(List<Transform> near)
    {
        return null;
    }

    public Transform TargetRightEnemy(List<Transform> near)
    {
        return null;
    }

    private List<Transform> DetectNearByEnemies()
    {
        int radius = 1;
        List<Transform> nearByEnemy = new List<Transform>();
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        if (cols.Length > 0)
        {
            foreach (var i in cols)
            {
                if(i.CompareTag("Enemy"))
                {
                    nearByEnemy.Add(i.transform);
                }
            }
            //Debug.Log(nearByEnemy.Count);
            return nearByEnemy;
        }
        else
        {
            return null;
        }
    }


}
