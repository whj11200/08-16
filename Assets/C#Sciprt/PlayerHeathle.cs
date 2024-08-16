using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeathle : LivingEntity
{
    public Slider healthSliber;
    public AudioClip hitClip;
    public AudioClip itemPickupCilp;
    public AudioClip Deadclip;
    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerShoter playerShoter;
    private readonly int HashDie = Animator.StringToHash("Die");
    private void Awake()
    {
        playerShoter = GetComponent<PlayerShoter>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthSliber.gameObject.SetActive(true);
        healthSliber.maxValue = startingHealth;
        healthSliber.value = health;
        playerMovement.enabled = true;
        playerShoter.enabled = true;
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
        healthSliber.value = health;
        //������Ʈ��
    }

    public override void OnDeamge(float damge, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
            // ��������ʾ����� ȿ������ �����Ŵ
        }
        base.OnDeamge(damge, hitPoint, hitDirection);
        healthSliber.value = health;
    }

    public override void Die()
    {
        base.Die();
        healthSliber.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(Deadclip);
        playerAnimator.SetTrigger(HashDie);
        playerMovement.enabled=false;
        playerShoter.enabled=false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if(!dead)
        {
            //�浹�� �������� ���� item���۳�Ʈ�� �������
            Iitem item = other.GetComponent<Iitem>();
            if(item != null)
            {
                item.Use(gameObject);
                // ������ ���� �Ҹ� �����
                playerAudioPlayer.PlayOneShot(itemPickupCilp, 1.0f);
            }
        }
    }
}
