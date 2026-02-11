using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
