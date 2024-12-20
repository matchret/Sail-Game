using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EasingFunctions
{
    // EaseOutBounce
    public static float EaseOutBounce(float t)
    {
        if (t < (1/2.75f))
        {
            return 7.5625f*t*t;
        }
        else if (t < (2/2.75f))
        {
            t -= 1.5f/2.75f;
            return 7.5625f*(t)*t + 0.75f;
        }
        else if (t < (2.5f/2.75f))
        {
            t -= 2.25f/2.75f;
            return 7.5625f*(t)*t + 0.9375f;
        }
        else
        {
            t -= 2.625f/2.75f;
            return 7.5625f*(t)*t + 0.984375f;
        }
    }

    // EaseInOutQuad
    public static float EaseInOutQuad(float t)
    {
        t *= 2f;
        if (t < 1f)
            return 0.5f*t*t;
        t -= 1f;
        return 0.5f*(t*(t-2f)-1f);
    }
}
