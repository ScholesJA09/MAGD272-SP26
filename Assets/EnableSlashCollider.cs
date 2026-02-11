using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSlashCollider : MonoBehaviour
{
    public void EnableCollider()
    {
        GetComponentInChildren<Collider2D>().enabled = true;
    }
    
    public void DisableCollider()
    {
        GetComponentInChildren<Collider2D>().enabled = false;
    }
}
