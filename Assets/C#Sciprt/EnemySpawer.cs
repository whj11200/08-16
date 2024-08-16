using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawer : MonoBehaviour
{
    [SerializeField] public EnemyScript enemyPrefab; // �� ������Ʈ ��ũ��Ʈ (��Ÿ ����: enemyperfad -> enemyPrefab)
    [SerializeField] public Transform[] spawnPoints; // ���� ������ ��ġ �迭 (��Ÿ ����: spwanPoints -> spawnPoints)
    [SerializeField] public float damageMax = 40f; // �ִ� ���ط�
    [SerializeField] public float damageMin = 20f; // �ּ� ���ط�
    [SerializeField] public float healthMax = 200f; // �ִ� ü��
    [SerializeField] public float healthMin = 100f; // �ּ� ü��
    [SerializeField] public float speedMax = 3f; // �ִ� �ӵ�
    [SerializeField] public float speedMin = 1.0f; // �ּ� �ӵ�
    [SerializeField] public Color strongEnemyColor = Color.red; // ���� �� AI�� ������ �� �Ǻλ�
    private List<EnemyScript> enemies = new List<EnemyScript>(); // ���� ���ӿ� �����ϴ� �� ����Ʈ
    private int wave; // ���� ���̺� ��

    void Update()
    {
        // ������ ����� ���, �� ������ ����
        if (GameManger.Instance != null && GameManger.Instance.isGameOver)
        {
            return;
        }

        // ���� ��� ����ģ ��� ���� ���̺�� �Ѿ
        if (enemies.Count <= 0)
        {
            SpawnWave(); // ���ο� ���̺� ����
        }

        UpdateUi(); // UI ������Ʈ
    }

    void UpdateUi()
    {
        // ���� ���̺�� ���� ���� UI�� ������Ʈ
        UiManger.ui_instance.UpdateWaveText(wave, enemies.Count);
    }

    void SpawnWave() // ���� ���̺꿡 ���缭 �� ����
    {
        wave++; // ���� ���̺� �� ����
        int spawnCount = Mathf.RoundToInt(wave * 1.5f); // ���̺� ���� ���� �� ���� �� ���
        for (int i = 0; i < spawnCount; i++)
        {
            // ���� ���� (����)�� 0���� 1 ���̿��� �������� ����
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity); // �� ����
        }
    }

    void CreateEnemy(float intensity) // �� ������Ʈ �����ϰ� ������ ����� �Ҵ�
    {
        // intensity �� ������� ���� �ɷ�ġ ����
        float health = Mathf.Lerp(healthMin, healthMax, intensity); // ü��
        float damage = Mathf.Lerp(damageMin, damageMax, intensity); // ���ط�
        float speed = Mathf.Lerp(speedMin, speedMax, intensity); // �ӵ�
        // intensity �� ������� �Ͼ���� ���� �� ���� ���̿��� �Ǻλ� ����
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);
        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �� ������Ʈ ���� �� �ʱ�ȭ
        EnemyScript enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.SetUp(health, damage, speed, skinColor); // ���� �ɷ�ġ ����
        enemies.Add(enemy); // ���� ����Ʈ�� �߰�

        // ���� ���� �� �̺�Ʈ ���
        enemy.onDeath += () => enemies.Remove(enemy); // ����Ʈ���� ����
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f); // 10�� �� �� ������Ʈ �ı�
        enemy.onDeath += () => GameManger._Instance.AddScore(100); // ���� �߰�
    }
}
