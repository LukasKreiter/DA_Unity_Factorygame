using UnityEngine;

public static class NoiseBlend
{
    public static Texture2D Blend(Texture2D a, Texture2D b, int mode)
    {
        int width = a.width;
        Texture2D result = new Texture2D(width, width);

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float A = a.GetPixel(x,y).r;
                float B = b.GetPixel(x,y).r;

                float r = 0;

                switch(mode)
                {
                    case 0: r = A + B; break;
                    case 1: r = A * B; break;
                    case 2: r = Mathf.Max(A, B); break;
                    case 3: r = Mathf.Min(A, B); break;
                }

                r = Mathf.Clamp01(r);
                result.SetPixel(x,y, new Color(r,r,r));
            }
        }

        result.Apply();
        return result;
    }
}