using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefensePath : MonoBehaviour
{
    [SerializeField] private List<Transform> pathNodes;

    public static TowerDefensePath towerDefensePath;
    public bool useAsSingleton = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!useAsSingleton) return;
        if ( towerDefensePath == null)
        {
            towerDefensePath = this;
        } else {
            Destroy(this);
        }
    }

    public List<Transform> GetPath()
    {
        return pathNodes;
    }
}
