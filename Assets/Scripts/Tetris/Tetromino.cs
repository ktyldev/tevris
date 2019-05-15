using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Color Colour
    {
        get { return renderer_.material.color; }
        set { renderer_.material.color = value; }
    }

    private Renderer renderer_;

    private void Awake()
    {
        renderer_ = GetComponentInChildren<Renderer>();
        if (renderer_ == null)
            throw new System.Exception("need a renderer fam");
    }
}
