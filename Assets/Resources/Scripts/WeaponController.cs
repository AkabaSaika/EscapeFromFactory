using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightJson;

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
    private GameObject[] hitPoints = new GameObject[5];
    private Vector3[] attacklinePos = new Vector3[2];
    private SkillParameter skillPara;
    private List<GameObject> hitObject = new List<GameObject>();


    private void Awake()
    {
        skillPara=GetComponent<SkillParameter>();
        //rightHand = transform.Find("Character1_RightHand");
        weaponTrans = Instantiate(Resources.Load<GameObject>("Models/Items/Weapons/Sword Two-Hander Purple"));
        weaponTrans.transform.parent = rightHand;
        weaponTrans.transform.localPosition = dhs.PositionOffset;
        weaponTrans.transform.localRotation = Quaternion.Euler(dhs.RotateOffset);
        weaponTrans.transform.localScale = dhs.Scale;

        hitPoints[0] = new GameObject("Empty");
        hitPoints[0].name = "hitPoint1";
        hitPoints[0].transform.SetParent(weaponTrans.transform);
        hitPoints[0].transform.localPosition = new Vector3(0, 4.54f, 0);

        hitPoints[1] = new GameObject("Empty");
        hitPoints[1].name = "hitPoint2";
        hitPoints[1].transform.SetParent(weaponTrans.transform);
        hitPoints[1].transform.localPosition = new Vector3(0.247f, 2.773f, 0);

        hitPoints[2] = new GameObject("Empty");
        hitPoints[2].name = "hitPoint3";
        hitPoints[2].transform.SetParent(weaponTrans.transform);
        hitPoints[2].transform.localPosition = new Vector3(0.261f, 3.615f, 0);

        hitPoints[3] = new GameObject("Empty");
        hitPoints[3].name = "hitPoint4";
        hitPoints[3].transform.SetParent(weaponTrans.transform);
        hitPoints[3].transform.localPosition = new Vector3(0.261f, 2.141f, 0);

        hitPoints[4] = new GameObject("Empty");
        hitPoints[4].name = "hitPoint5";
        hitPoints[4].transform.SetParent(weaponTrans.transform);
        hitPoints[4].transform.localPosition = new Vector3(0.261f, 1.382f, 0);

        anim = gameObject.GetComponent<Animator>();
        AddAnimationEvent();
    }

    private void AddAnimationEvent()
    {
        foreach(GameObject hp in hitPoints)
        {
            DrawHit("great sword slash", 0.02f, 0.24f, hp);
            DrawHit("great sword slash (3)", 0.20f, 1.07f, hp);
            DrawHit("great sword slash (4)", 0.10f, 1.23f, hp);
        }
    }

    private void DrawHit(string clipName,float startTime,float endTime,GameObject hitPoint)
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
                    events.functionName = "CastHitLine";
                    events.time = i;
                    clip.AddEvent(events);
                }
            }
            if(string.Equals(clip.name,clipName))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName="CallResetCanHit";
                events.time=1.0f;
                clip.AddEvent(events);
            }
        }
    }

    private void ResetHit()
    {
        attacklinePos[0] = attacklinePos[1] = Vector3.zero;
        hitObject.Clear();
    }

    private void CastHitLine(GameObject hitPoint)
    {
        int layA = LayerMask.NameToLayer("Default");
        attacklinePos[1] = hitPoint.transform.position;
        
        if (attacklinePos[0]==Vector3.zero)
        {
            attacklinePos[0] = attacklinePos[1];
        }
        Debug.DrawLine(attacklinePos[0], attacklinePos[1], Color.red, 60);
        RaycastHit hit;
        if (Physics.Linecast(attacklinePos[0], attacklinePos[1], out hit, 1 << layA)||Physics.Linecast(attacklinePos[1], attacklinePos[0], out hit, 1 << layA))
        {
            Debug.Log("hit");
            switch (hit.collider.tag)
            {
                case "Enemy":
                    if(hit.collider.gameObject.GetComponent<FSM>().canHit)
                    {      
                        hitObject.Add(hit.collider.gameObject);              
                        Debug.Log(hit.collider.gameObject.name);
                        hit.collider.SendMessage("Damaged");
                        anim.speed = 0.3f;
                        Invoke("AnimPlay", 0.1f);
                    }
                    break;
            }

        }
        attacklinePos[0] = attacklinePos[1];
    }

    private void AnimPlay()
    {
        anim.speed = 1;
    }

    private void CallResetCanHit()
    {
        foreach(var i in hitObject)
        {
            i.GetComponent<FSM>().canHit=true;
        }
    }
}
