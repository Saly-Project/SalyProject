using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] GameObject[] CheckPoints;

    void Start()
    {
        for (int i = 0; i < CheckPoints.Length; i++)
        {
            CheckPoints[i].SetActive(false);
        }
        CheckPoints[0].SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited checkpoint");

            // Find the index of the current checkpoint
            int checkpointIndex = GetCheckpointIndex(gameObject);
            if (checkpointIndex >= 0)
            {
                Toggle(checkpointIndex + 1);
            }
        }
    }

    private int GetCheckpointIndex(GameObject checkpoint)
    {
        for (int i = 0; i < CheckPoints.Length; i++)
        {
            if (CheckPoints[i] == checkpoint)
            {
                return i;
            }
        }
        return -1; // Not found
    }

    public void Toggle(int checks)
    {
        Debug.Log($"Toggling checkpoint {checks}");

        // Deactivate the current checkpoint
        if (checks - 1 >= 0 && checks - 1 < CheckPoints.Length)
        {
            CheckPoints[checks - 1].SetActive(false);
        }

        // Activate the next checkpoint
        if (checks < CheckPoints.Length)
        {
            CheckPoints[checks].SetActive(true);
        }
    }
}