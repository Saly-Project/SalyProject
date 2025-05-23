using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private GameObject botPrefab; // Assign your bot prefab in the Unity Inspector
    [SerializeField] private Transform[] spawnPositions; // Assign 3 spawn positions in the Unity Inspector
    [SerializeField] private Transform[] checkpoints; // Assign these in the Inspector
    [SerializeField] private float baseBotSpeed = 20f;      // Set this in the Inspector
    [SerializeField] private float botSpeedVariance = 3f;   // Set this in the Inspector

    private void Start()
    {
        // Only spawn bots if solo mode is enabled
        if (MenuScript.isSoloMode) // Corrected variable name
        {
            SpawnBots();
        }
    }

    private void SpawnBots()
    {
        if (spawnPositions == null || spawnPositions.Length == 0)
        {
            Debug.LogError("No spawn positions assigned!");
            return;
        }

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject bot = Instantiate(botPrefab, spawnPositions[i].position, spawnPositions[i].rotation);
            AIController ai = bot.GetComponent<AIController>();
            if (ai != null)
            {
                ai.SetCheckpoints(checkpoints);

                float randomSpeed = baseBotSpeed + Random.Range(-botSpeedVariance, botSpeedVariance);
                ai.SetSpeed(randomSpeed);
                ai.SetSpeedVariance(botSpeedVariance); // Pass the variance for future randomization
            }
            else
            {
                Debug.LogWarning("Spawned bot has no AIController attached!");
            }
        }
    }
}