using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    public Transform rightHand;
    private Vector3 positionOffset;
    private Vector3 rotationOffset;
    private Vector3 scale;
    private GameObject weaponTrans;
    private DoubleHandSword dhs = new DoubleHandSword();
    private Animator anim;
    private GameObject hitPoint1;
    private GameObject hitPoint2;
    private Vector3[] attacklinePos = new Vector3[2];



    private void Awake()
    {
        //rightHand = transform.Find("Character1_RightHand");
        weaponTrans = Instantiate(Resources.Load<GameObject>("Models/Items/Weapons/Sword Two-Hander Purple"));
        weaponTrans.transform.parent = rightHand;
        weaponTrans.transform.localPosition = dhs.PositionOffset;
        weaponTrans.transform.localRotation = Quaternion.Euler(dhs.RotateOffset);
        weaponTrans.transform.localScale = dhs.Scale;

        hitPoint1 = new GameObject("Empty");
        hitPoint1.name = "hitPoint1";
        hitPoint1.transform.SetParent(weaponTrans.transform);
        hitPoint1.transform.localPosition = new Vector3(0, 4.54f, 0);

        hitPoint2 = new GameObject("Empty");
        hitPoint2.name = "hitPoint2";
        hitPoint2.transform.SetParent(weaponTrans.transform);
        hitPoint2.transform.localPosition = new Vector3(0.247f, 2.773f, 0);
        

        anim = gameObject.GetComponent<Animator>();
        AddAnimationEvent();

    }

    private void AddAnimationEvent()
    {
        DrawHit("great sword slash", "DrawLightHit", 0.02f, 0.24f, hitPoint1);
        DrawHit("great sword slash (3)", "DrawMediumHit", 0.20f, 1.07f, hitPoint1);
        DrawHit("great sword slash (4)", "DrawHeavyHit", 0.10f, 1.23f, hitPoint1);

        DrawHit("great sword slash", "DrawLightHit", 0.02f, 0.24f, hitPoint2);
        DrawHit("great sword slash (3)", "DrawMediumHit", 0.20f, 1.07f, hitPoint2);
        DrawHit("great sword slash (4)", "DrawHeavyHit", 0.10f, 1.23f, hitPoint2);
    }

    private void DrawHit(string clipName,string functionName,float startTime,float endTime,GameObject hitPoint)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (string.Equals(clip.name, clipName))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName = "ResetHit";
                events.time = 0.01f;
                clip.AddEvent(events);
            }
            for (float i = startTime; i <= endTime; i += 0.01f)
            {
                if (string.Equals(clip.name, clipName))
                {
                    AnimationEvent events = new AnimationEvent();
                    events.objectReferenceParameter=hitPoint;
                    events.functionName = functionName;
                    events.time = i;
                    clip.AddEvent(events);
                }
            }
        }
            
    }

    private void ResetHit()
    {
        attacklinePos[0] = attacklinePos[1] = Vector3.zero;
    }

    private void DrawLightHit(GameObject hitPoint)
    {
        int layA = LayerMask.NameToLayer("Default");
        attacklinePos[1] = hitPoint.transform.position;
        
        if (attacklinePos[0]==Vector3.zero)
        {
            attacklinePos[0] = attacklinePos[1];
        }
        Debug.DrawLine(attacklinePos[0], attacklinePos[1], Color.red, 60);
        RaycastHit hit;
        if (Physics.Linecast(attacklinePos[1], attacklinePos[0], out hit, 1 << layA))
        {
            Debug.Log("hit");
            switch (hit.collider.tag)
            {
                case "Enemy":
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.SendMessage("SmallDamage");
                    hit.collider.SendMessage("Damaged");
                    anim.speed = 0.3f;
                    Invoke("AnimPlay", 0.1f);
                    break;
            }

        }
        attacklinePos[0] = attacklinePos[1];
        
    }

    private void DrawMediumHit(GameObject hitPoint)
    {
        int layA = LayerMask.NameToLayer("Default");
        attacklinePos[1] = hitPoint.transform.position;

        if (attacklinePos[0] == Vector3.zero)
        {
            attacklinePos[0] = attacklinePos[1];
        }
        Debug.DrawLine(attacklinePos[0], attacklinePos[1], Color.blue, 60);
        RaycastHit hit;
        if (Physics.Linecast(attacklinePos[0], attacklinePos[1], out hit, 1 << layA))
        {
            Debug.Log("hit");
            switch (hit.collider.tag)
            {
                case "Enemy":
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.SendMessage("MediumDamage");
                    hit.collider.SendMessage("Damaged");
                    anim.speed = 0.3f;
                    Invoke("AnimPlay", 0.12f);

                    break;
            }

        }
        attacklinePos[0] = attacklinePos[1];

    }

    private void DrawHeavyHit(GameObject hitPoint)
    {
        int layA = LayerMask.NameToLayer("Default");
        attacklinePos[1] = hitPoint.transform.position;

        if (attacklinePos[0] == Vector3.zero)
        {
            attacklinePos[0] = attacklinePos[1];
        }
        Debug.DrawLine(attacklinePos[0], attacklinePos[1], Color.yellow, 60);
        RaycastHit hit;
        if (Physics.Linecast(attacklinePos[0], attacklinePos[1], out hit, 1 << layA))
        {
            Debug.Log("hit");
            switch (hit.collider.tag)
            {
                case "Enemy":
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.SendMessage("BigDamage");
                    hit.collider.SendMessage("Damaged");
                    anim.speed = 0.1f;
                    Invoke("AnimPlay", 0.15f);
                    break;
            }

        }
        attacklinePos[0] = attacklinePos[1];

    }

    private void AnimPlay()
    {
        anim.speed = 1;
    }

    private void DrawAttackTriggerDebugger()
    {
        if(hitPoint1.GetComponent<LineRenderer>())
        {
            Destroy(hitPoint1.GetComponent<LineRenderer>());
        }

        Vector3[] pos = new Vector3[2];
        pos[0] = attacklinePos[0];
        pos[1] = attacklinePos[1];

        LineRenderer line = new LineRenderer();
        line = GameObject.Find("RecordPoint").AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 2;
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.startWidth = 0.01f;
        line.endWidth = 0.1f;
        //line.useWorldSpace = false;
        line.SetPosition(0, pos[0]);
        line.SetPosition(1, pos[1]);
    }

    private string ListToString(List<Vector3> list)
    {
        if(list.Count==0)
        {
            return null;
        }
        string str = "";
        foreach(Vector3 i in list)
        {
            str += i.ToString() + " ";
        }
        return str;
    }
}
