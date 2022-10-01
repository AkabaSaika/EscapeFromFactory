using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent
{
    private string m_skillName;
    private int m_damage;


    public HitEvent(string skillName,int damage)
    {
        this.m_skillName = skillName;
        this.m_damage = damage;
    }

    public string SkillName { get => m_skillName; set => m_skillName = value; }
    public int Damage { get => m_damage; set => m_damage = value; }
}
