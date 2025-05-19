using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private GameObject botPrefab; // Assign your bot prefab in the Unity Inspector
    [SerializeField] private Transform[] spawnPositions; // Assign 3 spawn positions in the Unity Inspector

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
        if (spawnPositions.Length < 3)
        {
            Debug.LogError("Not enough spawn positions assigned!");
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            Instantiate(botPrefab, spawnPositions[i].position, spawnPositions[i].rotation);
        }
    }
}