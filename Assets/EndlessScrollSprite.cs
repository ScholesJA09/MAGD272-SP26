using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessScrollSprite : MonoBehaviour
{
    [Header("Sprite that will scroll endlessly")]
    public Transform spriteToScroll;
    public bool scrollAtStart = true;
    bool scrolling = false;
    public float scrollSpeed = 0.1f;
    public Vector3 spriteDimension;
    
    public int minNumSpriteInstances = 2;
    public List<Transform> allSprites = new List<Transform>();

    public enum ScrollDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    
    public ScrollDirection scrollDirection = ScrollDirection.Left;
    
    // Start is called before the first frame update
    void Start()
    {
        if(minNumSpriteInstances < 2)
        {
            minNumSpriteInstances = 2;
        }
        // set the sprite to scroll
        scrolling = scrollAtStart;
        // get the sprite dimension
        if(scrollDirection is ScrollDirection.Left or ScrollDirection.Right)
        {
            spriteDimension = new Vector2(spriteToScroll.GetComponent<SpriteRenderer>().bounds.size.x, 0);
        }
        else
        {
            spriteDimension = new Vector2 (0, spriteToScroll.GetComponent<SpriteRenderer>().bounds.size.y);
        }
        // create the sprite instances
        for (int i = 0; i < minNumSpriteInstances; i++)
        {
            
            Transform newSprite = Instantiate(spriteToScroll, spriteToScroll.position + (i * spriteDimension), Quaternion.identity);
            allSprites.Add(newSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // return;
        if (!scrolling) return;
        // move the sprite
        if (scrollDirection is ScrollDirection.Left)
        {
            spriteToScroll.transform.position = new Vector3(spriteToScroll.position.x - scrollSpeed, spriteToScroll.position.y, spriteToScroll.position.z);
        }
        else if (scrollDirection is ScrollDirection.Right)
        {
            spriteToScroll.position = new Vector3(spriteToScroll.position.x + scrollSpeed, spriteToScroll.position.y, spriteToScroll.position.z);
        }
        else if (scrollDirection is ScrollDirection.Up)
        {
            spriteToScroll.position = new Vector3(spriteToScroll.position.x, spriteToScroll.position.y + scrollSpeed, spriteToScroll.position.z);
        }
        else if (scrollDirection is ScrollDirection.Down)
        {
            spriteToScroll.position = new Vector3(spriteToScroll.position.x, spriteToScroll.position.y - scrollSpeed, spriteToScroll.position.z);
        }
        //
        // // check if the sprite is out of bounds
        // if (scrollDirection is ScrollDirection.Left && spriteToScroll.position.x <= -spriteDimension)
        // {
        //     spriteToScroll.position = new Vector3(spriteDimension, spriteToScroll.position.y, spriteToScroll.position.z);
        // }
        // else if (scrollDirection is ScrollDirection.Right && spriteToScroll.position.x >= spriteDimension)
        // {
        //     spriteToScroll.position = new Vector3(-spriteDimension, spriteToScroll.position.y, spriteToScroll.position.z);
        // }
        // else if (scrollDirection is ScrollDirection.Up && spriteToScroll.position.y >= spriteDimension)
        // {
        //     spriteToScroll.position = new Vector3(spriteToScroll.position.x, -spriteDimension, spriteToScroll.position.z);
        // }
        // else if (scrollDirection is ScrollDirection.Down && spriteToScroll.position.y <= -spriteDimension)
        // {
        //     spriteToScroll.position = new Vector3(spriteToScroll.position.x, spriteDimension, spriteToScroll.position.z);
        // }
    }
    
    public void StartScrolling()
    {
        scrollAtStart = true;
    }
}
