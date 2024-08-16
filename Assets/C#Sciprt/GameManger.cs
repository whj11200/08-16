using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;
    
    public static GameManger _Instance
    {
        get
        {
            if(Instance == null)
                Instance = FindObjectOfType<GameManger>();
            return Instance;
        }
    }
    public bool isGameOver
    {
        get; private set;
    }
    void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManger ������Ʈ�� �ִٸ�
       if(Instance != null)
       {
         Destroy(gameObject);
       }
    }

    private void Start()
    {
        //�÷��̾� ĳ������ ��� �̺�Ʈ �߻� �� ���ӿ���
        FindObjectOfType<PlayerHeathle>().onDeath += EndGame;

    }
    private int score = 0;
    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            UiManger.ui_instance.UpdateScoreText(score);
        }
    }
    public void EndGame()
    {
        isGameOver = true;
        UiManger.ui_instance.SetActiveGameOverUI(true);
        
    }


}
