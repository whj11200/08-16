using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDeamage
{
    public float startingHealth = 100f;// 시작 체력
    public float health { get; protected set; } // 현재 체력

    public bool dead { get; protected set; } //사망 체력
    public event Action onDeath; // 사망 시 발동 되는 이벤트

    //public delegate void OnDeath();
    //public static event OnDeath onDeathEvent;
    // 생명체가 활성화 될 때 상태를 리셋
    protected virtual void OnEnable()
    {                //물려 받을 가상 함수
        dead = false;
        health = startingHealth;
    }


    public virtual void OnDeamge(float damge, Vector3 hitPoint, Vector3 hitNormal)
    {
        
        health -= damge;
        if(health <=0&& !dead)
        {
            Die();
        }
      
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        { 
            //이미 죽었다면 체력을 회복할 수 없다.
            return;
        }
        health += newHealth;
    }
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}
// LivingEntity 클래스는 IDamageable 을 상속하므로 onDamage()메서드를 반드시 구현
