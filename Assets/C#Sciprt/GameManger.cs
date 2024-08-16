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
        // 씬에 싱글톤 오브젝트가 된 다른 GameManger 오브젝트가 있다면
       if(Instance != null)
       {
         Destroy(gameObject);
       }
    }

    private void Start()
    {
        //플레이어 캐릭터의 사망 이벤트 발생 시 게임오버
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
