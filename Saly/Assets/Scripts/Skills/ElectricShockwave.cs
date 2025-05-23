using UnityEngine;

public class ElectricShockwave : MonoBehaviour
{
    public bool Charged;
    public GameObject ShockwavePrefab;
    public float Duration;
    public float Size;
    public GameObject UIskill;
    public GameObject RechargeVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIskill.SetActive(Charged);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Charged) // if the skill can be enabled
            {
                SpawnShockwave();
            }
        }
    }

    void SpawnShockwave()
    {
        //Charged = false;
        //UIskill.SetActive(false);
        GameObject Shockwave = Instantiate(ShockwavePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem ShockwavePS = Shockwave.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (ShockwavePS != null)
        { 
            var main = ShockwavePS.main;
            main.startLifetime = Duration;
            main.startSize = Size;
        }

        Destroy(Shockwave, Duration + 1);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill Recharge"))
        {
            if (!Charged)
            {
                Charged = true;
                Destroy(other.gameObject);
                UIskill.SetActive(true);
                var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
                Destroy(recharge, 2f);
            }
        }
    }
}
