using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] public GameObject[] checkpoints;

    private int currentIndex = 0;
    


    void Start()
    {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].SetActive(false);

        currentIndex = 0;
        checkpoints[currentIndex].SetActive(true); // ← important pour qu’il soit visible
        
        
        
    }


    public void NextCheckpoint()
    {
        checkpoints[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < checkpoints.Length)
        {
            checkpoints[currentIndex].SetActive(true);

            var indicator = FindObjectOfType<CheckpointIndicator>();
            if (indicator != null)
                indicator.checkpointTarget = checkpoints[currentIndex].transform;
        }

        
    }



    public Transform GetCurrentCheckpoint()
    {
        return checkpoints[currentIndex].transform;
        
    }

}
