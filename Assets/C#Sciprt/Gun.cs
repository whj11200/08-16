using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{                      // 준비준비됌, 탄창이 빔 , 재장전
    public enum State { Ready,Empty,Reloading}
    public State state { get; private set; }
    public Transform fireTransform; // 발사 위치
    public ParticleSystem muzzleFlashEffect; // 총구 불꽃
    public ParticleSystem shellEjectEffect; // 탄피 배출
    public LineRenderer lineRenderer; // 탄피 배출 효과
    private AudioSource gunAudioPlayer; //  플레이어 오디오
    public AudioClip shotClip; // 총 소리
    public AudioClip relordcilp; // 재장전소리
    public float damage = 25f; // 데미지
    public float firedistance = 50f; // 사정거리
    internal int ammoRemain = 50; // 남은 전체 탄약
    internal int magCapacity = 25; //총 탄약
    internal int magAmmo; // 현재 남은 용량
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

    public void Fire() // 발사시도
    {
        // 만약 발사할때 현재시간에서 지난 시간을 더함
        if (state == State.Ready && Time.time >= lastFiretime+timeBetfire)
        {
            lastFiretime = Time.time;
            Shot();
        }
    }
    private void Shot() // 실제 발사 차례
    {

        RaycastHit hit;
        Vector3 hitposition = Vector3.zero;
        if(Physics.Raycast(fireTransform.position , fireTransform.forward, out hit, firedistance))
        {
            // 충돌한 상대방으로 부터 IDamageable 오브젝트 가져오기를 시도 한다.
            IDeamage target = hit.collider.GetComponent<IDeamage>();
            if(target != null )
            {
                // 상대방의 OnDamage 함수를 실행시켜 상대방에 데미지 추가
                target.OnDeamge(damage, hit.point, hit.normal);
            }
            //레이가 충돌한 위치 저장
            hitposition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았으며
            // 탄알이 최대 사정 거리까지 날아갔을때의 후
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
        
        // 선의 끝점은 입력으로 들어온 총의 위치
        lineRenderer.SetPosition(0, fireTransform.position);
        //선의 시작점은 총구의 의지
        lineRenderer.SetPosition(1,hitposition);
        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }

    public bool Relord()
    {
        // 재장전 중이거나 || 남은 탄환이 없거나 || 탄창에 탄알이 가득하다면
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
        // 탄창에 채울 탄알을 계산함
        int ammoToFill = magCapacity - magAmmo;
        // 탄창에 채워야 할 탄알이 남은 탄알 보다 많다면
        // 채워야 할 탄알 수를 남은 탄알 수에 맞추어서 줄임
        if(ammoRemain < ammoToFill)
        {

            ammoToFill = ammoRemain;
        }
        // 탄창을 채움
        magAmmo += ammoToFill;
        // 남은 탄알에서 탄창에 채운 만큼 탄알을 뺌
        ammoRemain -= ammoToFill;


        state = State.Ready;
    }
}
