using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalInteractable : MonoBehaviour, IInteractable
{
    [Header("Put the sprite for when the object is turned OFF here")]
    public Sprite OffSprite;
    [Header("Put the sprite for when the object is turned ON here")]
    public Sprite OnSprite;

    private SpriteRenderer spriteRenderer;

    [Header("Is the interactable active on?")]
    [SerializeField]
    private bool active;

    [Header("How many objects would you like this interactable to turn on? Drag in the objects")]
    public GameObject[] target;

    [Header("Is this interactable usable at the start of the scene?")]
    public bool toggle;
    private bool triggerEntered;


    [Header("What conditionals activates this object? Must have Interactble Script")]
    [SerializeField, SerializeReference]
    public GameObject [] interactable;

    [Header("What key should the player press to use this interactable?")]
    public KeyCode keycode;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        for (int i = 0; i < target.Length; i++)
        {
            target[i].SetActive(false);
        }

    }

    void Update()
    {
        toggle = isInteractable();

        if (Input.GetKeyDown(keycode) && triggerEntered == true && toggle == true)
        {
            interact();
            Change();
        }
    }

    public bool isActive()
    {
        return active;
    }

    public bool isInteractable() //Bool for object being interactable
    {
         if (checkInteractables())
            {
                return true;
            }

        
        return false;
    }

    public void interact() //parameteres for interactable objects
    {
        if (toggle)
        {
            if (active)
            {
                for (int i = 0; i < target.Length; i++)
                {
                 active = false;
                 target[i].SetActive(false);
                }
               
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    active = true;
                    target[i].SetActive(true);
                }
            }
        }
        else Debug.Log("cannot interact");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            triggerEntered = true;
            Debug.Log("trigger entered");
        }
    }

    private void OnTriggerExit2D()
    {

        triggerEntered = false;
    }

    public void Change()
    {
        if (spriteRenderer.sprite == OnSprite)
            spriteRenderer.sprite = OffSprite;
        else
        {
            spriteRenderer.sprite = OnSprite;
        }
    }
    public bool checkInteractables()
    {
        bool checker = false;
        for(int i = 0; i < interactable.Length; i++)
        {
            if (interactable[i].GetComponent<IInteractable>() != null)
            {
                checker = interactable[i].GetComponent<IInteractable>().isActive();
                if (!checker)
                {
                    return checker;
                }
            } else
            {
                Debug.Log("not all objects in the interactles contain an interactable script on object: " + gameObject.name);
                return false;
            }
        }
        return checker;
    }
}
