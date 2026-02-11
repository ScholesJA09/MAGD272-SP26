using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [Header("This manages cooldowns/damage for any attacks player might have; it is required for ANY attacking")]
    // list of attacks on the player; set on start
    [HideInInspector]
    public Attack[] playerAttacks;

    [Header("Disables all attacks on player")]
    public bool attacksEnabled = true;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerAttacks = gameObject.GetComponents<Attack>();
        if(playerAttacks == null || playerAttacks.Length < 1)
        {
            Debug.LogError("No attacks found on " + gameObject.name + "!" + gameObject.name + " is unable to attack.");
            this.enabled = false;
            attacksEnabled = false;
        }
    }

    void Update()
    {
        if (attacksEnabled && !playerHealth.dead)
        {
            executeAttacks();
        }
    }

    private void executeAttacks()
    {
        foreach (Attack a in playerAttacks)
        {
            if (a.enabled)
            {
                if (!a.attacking && Input.GetKeyDown(a.attackInput))
                {
                    print($"Attacking with  {a.ToString()}");
                    StartCoroutine(a.ExecuteAttack(a.attackSpeed));
                }
            }
        }
    }
}
