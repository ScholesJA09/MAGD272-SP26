using UnityEngine;
using System.Collections;

public interface IPullable
{
    bool Pullable
    {
        get;
    }

    /// <summary>
    /// Push and set Pullable property.
    /// </summary>
    void Pull();
}
