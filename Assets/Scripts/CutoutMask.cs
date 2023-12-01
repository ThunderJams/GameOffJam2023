using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMask : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp",(int) CompareFunction.NotEqual);
            return material;
        }
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Fix());
    }

    /// Fix for async loading scenes
    private IEnumerator Fix()
    {
        while (true)
        {
            yield return null;
            maskable = false;
            maskable = true;

        }
    }
}
