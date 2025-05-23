using UnityEngine;

public class Skill : MonoBehaviour
{
    public bool Charged;
    public GameObject UIskill;

    public void Disable()
    {
        Charged = false;
        UIskill.SetActive(false);
    }
}
