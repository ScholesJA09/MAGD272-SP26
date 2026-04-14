using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// MAKE SURE THE FILE IS NAMED: HealthIconsUI.cs
public class HealthIconsUI : HealthUI
{
    [Header("Health icon when full")]
    public Sprite healthIcon;

    [Header("Health icon when empty")]
    [Tooltip("If nothing here, icons will turn off instead")]
    public Sprite emptyIcon;

    Image[] icons;
    int currentIcon = -1;
    bool turningOffIcons = false;

    public override void setHealth(int max)
    {
        base.setHealth(max);

        if (!healthIcon)
        {
            Debug.Log("No health icon found!");
            return;
        }

        if (!emptyIcon)
        {
            turningOffIcons = true;
        }

        if (icons != null)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (turningOffIcons) icons[i].gameObject.SetActive(true);
                else icons[i].sprite = healthIcon;
            }
        }
        else
        {
            icons = new Image[maxHealth];
            for (int i = 0; i < maxHealth; i++)
            {
                icons[i] = createNewIcon(i);
            }
        }

        currentIcon = maxHealth - 1;
        currentHealth = maxHealth;
    }

    public override void updateHealth(int newHealth)
    {
        if (newHealth < 0) newHealth = 0;
        if (newHealth > maxHealth) newHealth = maxHealth;

        for (int i = 0; i < icons.Length; i++)
        {
            if (i < newHealth)
            {
                if (turningOffIcons) icons[i].gameObject.SetActive(true);
                else icons[i].sprite = healthIcon;
            }
            else
            {
                if (turningOffIcons) icons[i].gameObject.SetActive(false);
                else icons[i].sprite = emptyIcon;
            }
        }

        currentHealth = newHealth;
        currentIcon = newHealth - 1;
    }

    Image createNewIcon(int index)
    {
        GameObject temp = new GameObject("HealthIcon_" + index);
        temp.AddComponent<RectTransform>();
        Image i = temp.AddComponent<Image>();
        i.sprite = healthIcon;
        temp.transform.SetParent(gameObject.transform);
        temp.GetComponent<RectTransform>().localScale = Vector3.one;
        return i;
    }
}