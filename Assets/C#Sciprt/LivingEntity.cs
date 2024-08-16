using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDeamage
{
    public float startingHealth = 100f;// ���� ü��
    public float health { get; protected set; } // ���� ü��

    public bool dead { get; protected set; } //��� ü��
    public event Action onDeath; // ��� �� �ߵ� �Ǵ� �̺�Ʈ

    //public delegate void OnDeath();
    //public static event OnDeath onDeathEvent;
    // ����ü�� Ȱ��ȭ �� �� ���¸� ����
    protected virtual void OnEnable()
    {                //���� ���� ���� �Լ�
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
            //�̹� �׾��ٸ� ü���� ȸ���� �� ����.
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
// LivingEntity Ŭ������ IDamageable �� ����ϹǷ� onDamage()�޼��带 �ݵ�� ����
