using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderAllChildren : MonoBehaviour
{
    public Color color = Color.white;
    void Start()
    {
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
        {
            if (r.enabled)
            {
               LineRender3DObject o =  r.gameObject.AddComponent<LineRender3DObject>();
               o.color = color;
            }
        }   
    }

}
