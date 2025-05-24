using Photon.Pun;
using UnityEngine;

public class SlowShockwave : Skill
{
    public GameObject ShockwavePrefab;
    public float Duration;
    public float Size;
    public GameObject RechargeVFX;

    public float SlowValue;
    public float SlowDuration;

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
        // Exécuter uniquement sur le joueur local
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Charged && !IsActive) // if the skill can be enabled
            {
                IsActive = true;
                Hypocenter = transform.position;
                // Appelle l'effet sur TOUS les clients via RPC
                photonView.RPC("SpawnShockwave", RpcTarget.All);
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
                        if (dist <= Size / 2)
                        {
                            var ship = enemy.GetComponent<ShipController>();
                            var view = enemy.GetComponent<PhotonView>();

                            if (ship != null && view != null)
                            {
                                view.RPC("Slow", RpcTarget.All, new object[] { new float[] { SlowValue, SlowDuration } });
                            }
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
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
