using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsUI : MonoBehaviour
{
    public static PointsUI pointsUI;

    public Text pointsUI_Text;
    public int Points = 0;

    private void Awake()
    {
        pointsUI = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pointsUI_Text == null)
        {
            Debug.LogError("Points UI is missing the destination UI text");
        }
        else
        {
            displayPoints();
        }
    }

    public void addPoints(int points)
    {
        points += Points;
        print(Points);
        displayPoints();
    }

    void displayPoints()
    {
        pointsUI_Text.text = Points.ToString();
    }
}
