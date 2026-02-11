using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://medium.com/@trepala.aleksander/serializereference-in-unity-b4ee10274f48

//[Serializable]
public interface ICompletible
{
    bool Completed();
}
