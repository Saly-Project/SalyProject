using System;
using Photon.Pun;
using UnityEngine;

public class CheckpointManager : MonoBehaviourPunCallbacks
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
        if (!photonView.IsMine) return;

        checkpoints[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < checkpoints.Length)
        {
            checkpoints[currentIndex].SetActive(true);

            var indicator = FindObjectOfType<CheckpointIndicator>();
            if (indicator != null)
            {
                indicator.checkpointTarget = checkpoints[currentIndex].transform;
            }
        }
        else
        {
            
        }

        
    }



    public Transform GetCurrentCheckpoint()
    {
        return checkpoints[currentIndex].transform;
        
    }

}
