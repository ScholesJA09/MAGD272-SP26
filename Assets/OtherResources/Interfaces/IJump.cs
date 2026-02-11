using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJump
{
    bool Jump();

    bool CheckGround();
    bool CheckEdge();
    bool CheckWall();

}
