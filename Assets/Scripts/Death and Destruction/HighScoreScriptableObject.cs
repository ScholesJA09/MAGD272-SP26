using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "HighScore", menuName = "ScriptableObjects/HighScore", order = 1)]

public class HighScoreScriptableObject: ScriptableObject
{
    public int highScore;
}
