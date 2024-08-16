using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{                      // �غ��غ��, źâ�� �� , ������
    public enum State { Ready,Empty,Reloading}
    public State state { get; private set; }
    public Transform fireTransform; // �߻� ��ġ
    public ParticleSystem muzzleFlashEffect; // �ѱ� �Ҳ�
    public ParticleSystem shellEjectEffect; // ź�� ����
    public LineRenderer lineRenderer; // ź�� ���� ȿ��
    private AudioSource gunAudioPlayer; //  �÷��̾� �����
    public AudioClip shotClip; // �� �Ҹ�
    public AudioClip relordcilp; // �������Ҹ�
    public float damage = 25f; // ������
    public float firedistance = 50f; // �����Ÿ�
    internal int ammoRemain = 50; // ���� ��ü ź��
    internal int magCapacity = 25; //�� ź��
    internal int magAmmo; // ���� ���� �뷮
    public float timeBetfire = 0.12f;
    public float reloadTime = 1.0f;
    private float lastFiretime;


    void Awake()
    {
       gunAudioPlayer = GetComponent<AudioSource>();
       lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFiretime = 0f;
        
    }

    public void Fire() // �߻�õ�
    {
        // ���� �߻��Ҷ� ����ð����� ���� �ð��� ����
        if (state == State.Ready && Time.time >= lastFiretime+timeBetfire)
        {
            lastFiretime = Time.time;
            Shot();
        }
    }
    private void Shot() // ���� �߻� ����
    {

        RaycastHit hit;
        Vector3 hitposition = Vector3.zero;
        if(Physics.Raycast(fireTransform.position , fireTransform.forward, out hit, firedistance))
        {
            // �浹�� �������� ���� IDamageable ������Ʈ �������⸦ �õ� �Ѵ�.
            IDeamage target = hit.collider.GetComponent<IDeamage>();
            if(target != null )
            {
                // ������ OnDamage �Լ��� ������� ���濡 ������ �߰�
                target.OnDeamge(damage, hit.point, hit.normal);
            }
            //���̰� �浹�� ��ġ ����
            hitposition = hit.point;
        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾ�����
            // ź���� �ִ� ���� �Ÿ����� ���ư������� ��
            hitposition = fireTransform.position + fireTransform.forward * firedistance;
        }
        StartCoroutine(ShotEffect(hitposition));
        magAmmo--;
     
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }
    IEnumerator ShotEffect(Vector3 hitposition)
    {
        lineRenderer.enabled = true;
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);
        
        // ���� ������ �Է����� ���� ���� ��ġ
        lineRenderer.SetPosition(0, fireTransform.position);
        //���� �������� �ѱ��� ����
        lineRenderer.SetPosition(1,hitposition);
        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }

    public bool Relord()
    {
        // ������ ���̰ų� || ���� źȯ�� ���ų� || źâ�� ź���� �����ϴٸ�
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;

        }
       
        StartCoroutine(RelordRoutine());
        return true;
    }

    IEnumerator RelordRoutine()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(relordcilp);


        yield return new WaitForSeconds(reloadTime);
        // źâ�� ä�� ź���� �����
        int ammoToFill = magCapacity - magAmmo;
        // źâ�� ä���� �� ź���� ���� ź�� ���� ���ٸ�
        // ä���� �� ź�� ���� ���� ź�� ���� ���߾ ����
        if(ammoRemain < ammoToFill)
        {

            ammoToFill = ammoRemain;
        }
        // źâ�� ä��
        magAmmo += ammoToFill;
        // ���� ź�˿��� źâ�� ä�� ��ŭ ź���� ��
        ammoRemain -= ammoToFill;


        state = State.Ready;
    }
}
