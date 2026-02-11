using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUsingKey : MonoBehaviour
{
    [TextArea]
    public string note = "This object will destroy when the player runs into it while having a key.";

    [Header("Is the key removed when used?")]
    public bool removeKey = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<CollectibleManager>())
            {
                if (collision.gameObject.GetComponent<CollectibleManager>().keyCollected)
                {
                    if (removeKey) collision.gameObject.GetComponent<CollectibleManager>().keyCollected = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}
