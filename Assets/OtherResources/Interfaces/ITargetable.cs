using UnityEngine;
using System.Collections;

public interface ITargetable
{
    /// <summary>
    /// Only returns state, use Target() to set state
    /// </summary>
    bool isTargeted
    {
        get;
    }

    /// <summary>
    /// Should set isTargeted to true and whatever else (play animation, etc.)
    /// </summary>
    void Target();

    /// <summary>
    /// Should returns its location in worldspace
    /// </summary>
    /// <returns>Location in Worldspace</returns>
    Vector2 Location();
}
