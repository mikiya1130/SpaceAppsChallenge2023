using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RescaleValues
{
    // NOTE: 0-255 をリスケールするパラメータ
    private static float scale = 5.0f / 255.0f;

    public static float[] rescale(int[] values)
    {
        return values.Select(i => i * scale).ToArray();
    }
}
