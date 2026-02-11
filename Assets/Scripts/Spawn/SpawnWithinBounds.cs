using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnWithinBounds : MonoBehaviour
{

    [Header("Bias to LEFT and BOTTOM of boundary")] 
    [SerializeField] bool showGridGizmos;
    [SerializeField] float columns;
    [SerializeField] float rows;

    [SerializeField] GameObject prefab;

    [Header("Boundary -- must fill all")]
    [SerializeField] GameObject leftBounds;
    [SerializeField] GameObject upperBounds;
    [SerializeField] GameObject rightBounds;
    [SerializeField] GameObject lowerBounds;

    [SerializeField] Rect bounds;
    [Header("Likelihood prefab spawns (0-1)")]
    [Tooltip("1 = 100% likely, 0 = not likely at all")]
    [SerializeField] float threshold;

    // Start is called before the first frame update
    void Start()
    {
        //
        bounds = new Rect(leftBounds.transform.position.x, upperBounds.transform.position.y, (rightBounds.transform.position.x - leftBounds.transform.position.x), (upperBounds.transform.position.y - lowerBounds.transform.position.y));
        print(bounds.size);

        spawnMushrooms();
    }
    
    void spawnMushrooms()
    {
        print("spawning");
        // likelihood to spawn
        float boundsWidth = bounds.width;
        float boundsHeight = bounds.height;
        float colWidth = boundsWidth / columns;
        float rowHeight = boundsHeight / rows;
        print("---------");
        print("bounds? " + boundsWidth + " - " + boundsHeight );
        print("spacing " + colWidth + " - "  + rowHeight) ;
        print("gz: " + columns + rows);
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                float rng = Random.Range(0, 1.0f);
                print("rng:" + rng);
                if (rng > (1-threshold))
                {
                    print("spawning");
                    Instantiate(prefab, new Vector3(bounds.x + (colWidth * i), bounds.y - (bounds.height) + (rowHeight * j), 0), Quaternion.identity);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (!showGridGizmos) return;
        bounds = new Rect(leftBounds.transform.position.x, upperBounds.transform.position.y, (rightBounds.transform.position.x - leftBounds.transform.position.x), (upperBounds.transform.position.y - lowerBounds.transform.position.y));

        float boundsWidth = bounds.width;
        float boundsHeight = bounds.height;
        float colWidth = boundsWidth / columns;
        float rowHeight = boundsHeight / rows;
        print("---------");
        print("bounds? " + boundsWidth + " - " + boundsHeight );
        print("spacing " + colWidth + " - "  + rowHeight) ;
        print("gz: " + columns + rows);
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var position = new Vector3(bounds.x + (colWidth * i),
                    bounds.y - (bounds.height) + (rowHeight * j));
                Gizmos.DrawIcon(position, "Square.png", true);
                print("gizmos ");
            }
        }
    }
}
