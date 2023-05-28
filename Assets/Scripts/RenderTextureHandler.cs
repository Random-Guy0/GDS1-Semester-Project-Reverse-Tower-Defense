using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureHandler
{
    public static void SaveRTToFile(RenderTexture rt, Camera cam, string path)
    {
        RenderTexture newRT = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.sRGB);
        newRT.antiAliasing = rt.antiAliasing;

        cam.targetTexture = newRT;
        cam.Render();

        RenderTexture.active = newRT;
        Texture2D tex = new Texture2D(newRT.width, newRT.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, newRT.width, newRT.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();
        
        System.IO.File.WriteAllBytes(path, bytes);
    }
}
