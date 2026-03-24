using UnityEngine;

public class CollisionDemo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter2D(Collision2D collision)
    {
        print("COLLISION Enter: " + collision.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        print("COLLISION Exit: " + collision.gameObject.name);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        print("COLLISION Exit: " + collision.gameObject.name);
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale *= 1.1f;
        collision.GetComponent<SpriteRenderer>().color = Color.blue;
        collision.GetComponent<SpriteRenderer>().flipY = true;
        print("TRIGGER Enter: " + collision.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        print("TRIGGER Exit: " + collision.gameObject.name);
    }

        void OnTriggerStay2D(Collider2D collision)
    {
        print("TRIGGER Exit: " + collision.gameObject.name);
    }

}


// EARTH.NA.USA.WISCONSIN.WALWORTHCOUNTY.WHITEWATER.UWW.ANDERSENLIBRARY.1230A