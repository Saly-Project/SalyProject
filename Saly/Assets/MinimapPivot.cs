using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player == null) return;

        // Reste en place, mais tourne pour suivre l'orientation du joueur
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
