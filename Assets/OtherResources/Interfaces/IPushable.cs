using UnityEngine;
using System.Collections;

public interface IPushable
{
    bool Pushable
    {
        get;
    }

    /// <summary>
    /// Push and set Pushable property.
    /// </summary>
    void Push();
}
