using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfoSPTrapBase : BallInfoSPBase
{
    public int ElimitNum = 3;
    public int ShowNum = 0;

    public override bool IsCanBeContentSP()
    {
        return false;
    }
}
