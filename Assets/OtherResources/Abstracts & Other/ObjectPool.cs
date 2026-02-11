using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    // Used in code only.

    List<GameObject> pool;

    List<ProjectileMove> allProjectiles;

    public ObjectPool()
    {
        pool = new List<GameObject>();
        allProjectiles = new List<ProjectileMove>();
    }

    public GameObject pullObject(Vector2 location)
    {
        if(pool.Count <= 0)
        {
            return null;    // returns null if a new object needs to be instantiated
        }

        GameObject returnObject = pool[0];
        pool.RemoveAt(0);

        returnObject.SetActive(true);
        returnObject.transform.position = new Vector3(location.x, location.y, 0);
        returnObject.transform.rotation = Quaternion.identity;

        return returnObject;
    }

    public void addToAll(ProjectileMove pro)
    {
        if (pro) allProjectiles.Add(pro);
    }

    public void returnObject(GameObject thing)
    {
        thing.SetActive(false);
        pool.Add(thing);
    }

    public void objectDestroyed()
    {
        foreach(ProjectileMove a in allProjectiles)
        {
            a.disableReturn();
        }
    }
}
