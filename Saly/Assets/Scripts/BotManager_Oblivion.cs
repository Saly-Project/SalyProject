using UnityEngine;

public class BotManager_Oblivion : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private float baseBotSpeed = 20f;
    [SerializeField] private float botSpeedVariance = 3f;

    private void Start()
    {
        if (MenuScript.isSoloMode)
        {
            SpawnBots();
        }
    }

    private void SpawnBots()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn positions assigned!");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject bot = Instantiate(botPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            AIController ai = bot.GetComponent<AIController>();
            if (ai != null)
            {
                ai.SetCheckpoints(checkpoints);
                float randomSpeed = baseBotSpeed + Random.Range(-botSpeedVariance, botSpeedVariance);
                ai.SetSpeed(randomSpeed);
                ai.SetSpeedVariance(botSpeedVariance);
            }
        }
    }
}