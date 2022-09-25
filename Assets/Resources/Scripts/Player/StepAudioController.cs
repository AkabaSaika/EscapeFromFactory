using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAudioController : MonoBehaviour
{
    private CharacterController cc;
    private Animator anim;
    private float mTimer = 0;
    private float mStepIntervarl;

    private const float BASIC_STEP_INTERVARL = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mTimer += Time.deltaTime;
        RaycastHit hit;
        mStepIntervarl = BASIC_STEP_INTERVARL * (Input.GetKey(KeyCode.LeftShift) ? 0.5f : 1); 
        if(mTimer>=mStepIntervarl)//一定の時間で判定する
        {
            mTimer = 0;
            if(Physics.Linecast(transform.position,transform.position+new Vector3(0,-1,0),out hit))
            {
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
                {
                    if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
                    {
                        switch (hit.collider.tag)
                        {
                            case "Ground":
                                AudioManager.Instance.EffectPlay("Audio/Audio STEPS/Pasos/FOOT STEPS/TRAINERS - CONCRETE - COTTON/LEFT FOOT/LEFT FOOT CONCRETE COTTON DYN MED 4", false);
                                break;
                        }
                    }
                }
            }
        }
    }
}
