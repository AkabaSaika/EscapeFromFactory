using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private GameObject countdown;

    private int damage = 30;
    [SerializeField]
    private float damageRadius = 3;
    private string damageName = "Explosion";
    [SerializeField]
    private float countdownTime = 0.5f;
    private string soundEffectPath = "Audio/Grenade1Short";

    [SerializeField]
    private GameObject particleObject;


    // Start is called before the first frame update
    void Start()
    {
        particleObject.transform.localScale = new Vector3(damageRadius / 2, damageRadius / 2, damageRadius / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explosion()
    {
        AudioManager.Instance.EffectPlay(soundEffectPath,false);
        HitEvent he = new HitEvent(damageName, damage);
        explosionEffect.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius,1<<11);
        foreach(var i in colliders)
        {
            Debug.Log(i.gameObject.name);
            i.SendMessage("Damaged", he);
        }
        Camera.main.transform.DOShakePosition(1, new Vector3(2, 2, 0));
    }

    public void ExplosionCountDown()
    {
        countdown.SetActive(true);
        Invoke("Explosion", countdownTime);
        countdown.transform.DOScale(0, countdownTime);
    }
}
