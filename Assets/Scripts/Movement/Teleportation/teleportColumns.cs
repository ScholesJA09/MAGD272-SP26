using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class teleportColumns: MonoBehaviour
{
    [SerializeField] teleportColumns teleportMatch;
    [SerializeField] bool canTeleport = true;
    [Header("Tag of object you want to teleport")]
    [SerializeField] string objectTag;

    // Start is called before the first frame update
    void Start()
    {
        if (teleportMatch == null)
        {
            Debug.Log("You have not matched your teleport columns");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<teleportTag>()) return;
        if(collision.CompareTag(objectTag)){
            var collisionGameObjectobject = collision.gameObject;
            collisionGameObjectobject.transform.position = new Vector2(teleportMatch.gameObject.transform.position.x, collisionGameObjectobject.transform.position.y);
            collision.gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(objectTag))
        {
            var collisionGameObjectobject = collision.gameObject;
            if (collisionGameObjectobject.TryGetComponent<teleportTag>(out var teleportTag))
            {
                Destroy(teleportTag);
            }
        }
    }

    void disablePortal()
    {
        canTeleport = false;
    }
}
