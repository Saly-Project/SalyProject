using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;

    public int maxMissiles = 3;        // Nombre total autorisé
    private int missilesFired = 0;     // Nombre déjà tirés
    private GameObject currentMissile; // Missile actuellement actif

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Conditions : pas déjà un missile en vol ET limite pas atteinte
            if (currentMissile == null && missilesFired < maxMissiles)
            {
                currentMissile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
                missilesFired++;
                Debug.Log("Missile lancé (" + missilesFired + "/" + maxMissiles + ")");
            }
            else if (missilesFired >= maxMissiles)
            {
                Debug.Log("Plus de missiles disponibles !");
            }
        }

        // Réinitialiser si le missile a été détruit
        if (currentMissile == null && missilesFired < maxMissiles)
        {
            // Rien à faire ici sauf réarmement (optionnel)
        }
    }
}
