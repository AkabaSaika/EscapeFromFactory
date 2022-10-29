using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent
{
    private string m_skillName;
    private int m_damage;
    public Vector3 m_hitOffset{get;set;}


    public HitEvent(string skillName,int damage,Vector3 hitOffset)
    {
        this.m_skillName = skillName;
        this.m_damage = damage;
        this.m_hitOffset=hitOffset;
    }

    public string SkillName { get => m_skillName; set => m_skillName = value; }
    public int Damage { get => m_damage; set => m_damage = value; }
}
