using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력과 움직임과 분리해서 스크립트를 만든다.
// 입력과 액터 나누기
public class Player : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string fireButtonName = "Fire1"; 
    public string reloadButtonName = "Relord";
    //키관련 프로퍼티 만들기
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    void Start()
    {
     
    }


    void Update()
    {
        if (GameManger.Instance != null && GameManger.Instance.isGameOver)
        {
            move = 0f;
            rotate = 0; 
            fire = false;
            reload = false;
            return;
        }
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
