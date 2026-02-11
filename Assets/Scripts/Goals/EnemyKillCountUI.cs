using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyKillCountUI : MonoBehaviour
{
    public int killCount = 0;
    public Text killCountUIText;

    // Start is called before the first frame update
    void Start()
    {
        killCount = 0;
    }
    
    public void updateKillCount(int count)
    {
        if (killCountUIText)
        {
            killCountUIText.text = count.ToString();
        }
    }
}
