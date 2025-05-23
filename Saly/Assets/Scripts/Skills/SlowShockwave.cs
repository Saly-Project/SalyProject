using UnityEngine;

public class SlowShockwave : Skill
{
    public GameObject ShockwavePrefab;
    public float Duration;
    public float Size;
    public GameObject RechargeVFX;

    private bool IsActive = false;
    private float ShockwaveClock = 0;
    private Vector3 Hypocenter;

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
            if (Charged && !IsActive) // if the skill can be enabled
            {
                IsActive = true;
                Hypocenter = transform.position;
                SpawnShockwave();
            }
        }

        if (IsActive) 
        { 
            ShockwaveClock += Time.deltaTime;

            if (ShockwaveClock > Duration) 
            {
                IsActive = false;
                ShockwaveClock = 0;

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

                foreach (GameObject enemy in enemies)
                {
                    if (enemy != gameObject)
                    {
                        var dist = Vector3.Distance(Hypocenter, enemy.transform.position);
                        if (dist <= Size) 
                        {
                            // Slow enemy
                        }
                    }
                }
            }
        }
    }

    void SpawnShockwave()
    {
        Charged = false;
        UIskill.SetActive(false);
        GameObject Shockwave = Instantiate(ShockwavePrefab, Hypocenter, Quaternion.identity) as GameObject;
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
