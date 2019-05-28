using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ListExtensions
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void Shuffle<T>(this IList<T> list, int seed)
    {
        rng = new Random(seed);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Sprite ToSprite(this Texture2D tex)
    {
        var rect = new Rect(0,0,tex.width, tex.height);

        var spt = Sprite.Create(tex, rect, new Vector2(0,0), 100);

        return spt;
    }
    public static Sprite ToSprite(this Texture2D tex, int pixelPerUnity)
    {
        var rect = new Rect(0, 0, tex.texelSize.x, tex.texelSize.y);

        var spt = Sprite.Create(tex, rect, new Vector2(0, 0), pixelPerUnity);

        return spt;
    }
}
