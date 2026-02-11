using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Collectible : MonoBehaviour
{
    //This script holds the information about the collectible it is put on.

    [Header("Choose what type of collectible this is: ")]
    public Collectible_Type type;
    [Header("If the type is Coin Ammo, or Health set the amount: ")]
    public int amount = 1;
}
