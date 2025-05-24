using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPun
{
    public float maxHealth = 100;
    private float _Health;

    public Image HealthBar;
    public float ChargeRate;
    private Coroutine rechargeHealth;

    private void Awake()
    {
        _Health = maxHealth;
    }

    void Start()
    {
        // Disable this script for remote players
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    // Called by local player to apply damage across the network
    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, damage);
        }
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        _Health -= damage;
        Debug.Log("Current Health: " + _Health);
        HealthBar.fillAmount = _Health / maxHealth;

        if (_Health <= 0)
        {
            Die();
        }

        if (rechargeHealth != null) StopCoroutine(rechargeHealth);
        rechargeHealth = StartCoroutine(RechargeHealth());
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is dead.");
        // Handle death logic here
    }

    private IEnumerator RechargeHealth()
    {
        yield return new WaitForSeconds(5f);

        while (_Health < maxHealth)
        {
            _Health += ChargeRate / 50f;
            if (_Health > maxHealth) _Health = maxHealth;
            HealthBar.fillAmount = _Health / maxHealth;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Optional: Local use for instant feedback without syncing
    public void TakeLocalDamage(float amount)
    {
        if (!photonView.IsMine) return;

        _Health -= amount;
        Debug.Log(gameObject.name + " lost " + amount + " HP");
        HealthBar.fillAmount = _Health / maxHealth;

        if (_Health <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    public void TakeDamageRPC(int dmg)
    {
        _Health -= dmg;
        Debug.Log(gameObject.name + " lost " + dmg + " HP");
        HealthBar.fillAmount = _Health / maxHealth;

        if (_Health <= 0)
        {
            Debug.Log("💀 Player died");
            // Add death logic here
        }
    }

}
