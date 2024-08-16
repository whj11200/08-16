using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Է°� �����Ӱ� �и��ؼ� ��ũ��Ʈ�� �����.
// �Է°� ���� ������
public class Player : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string fireButtonName = "Fire1"; 
    public string reloadButtonName = "Relord";
    //Ű���� ������Ƽ �����
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
