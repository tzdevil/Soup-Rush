using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DevilUtils
{
    // mathSign = plus.
    public static float ReturnClampedValue(this float thisVariable, float clampedValue, bool plus) // plus == true -> + : -.
        => Mathf.Clamp(clampedValue, 0, plus ? 100 - thisVariable : thisVariable);
}
