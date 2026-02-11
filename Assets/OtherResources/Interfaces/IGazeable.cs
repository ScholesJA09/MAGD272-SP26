using UnityEngine;
using System.Collections;

//This is a basic interface with a single required
//method.
public interface IGazeable
{
    /// <summary>
    /// Returns its location in XY worldspace
    /// </summary>
    /// <returns></returns>
    Vector2 Location();
}
