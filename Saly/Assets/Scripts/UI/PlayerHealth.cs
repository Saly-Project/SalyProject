using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer.Center.Common;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerHealth : NetworkBehaviour
{
    public float maxHealth = 100;
    private float _Health;

    // UI and recharge
    public Image HealthBar;
    public float ChargeRate;
    private Coroutine rechargeHealth;




    private void Awake(){
        _Health = maxHealth;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner){
            enabled = false;
            return;
        }

        
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {   

        _Health -= damage;
        Debug.Log(_Health / maxHealth);
        HealthBar.fillAmount = _Health / maxHealth;

        

        if (_Health <= 0) Die();

        if (rechargeHealth != null) StopCoroutine(rechargeHealth);
        rechargeHealth = StartCoroutine(RechargeHealth());
    }

    private void Die()
    {
        Debug.Log("player is dead");
    }

    private IEnumerator RechargeHealth(){
        yield return new WaitForSeconds(5f);

        while (_Health < maxHealth) 
        {
            _Health += (int)ChargeRate / 50;
            if (_Health > maxHealth) _Health = maxHealth;
            HealthBar.fillAmount = _Health / maxHealth;
            yield return new WaitForSeconds(.05f);
        
        }

        
    }

    public void TakeDamage(float amount)
    {
        _Health -= amount;
        Debug.Log(gameObject.name + " perd " + amount + " PV");
        if (_Health <= 0) Destroy(gameObject);
    }
}
