using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    Collider2D col;
    public Tower tower;
    public bool IsFull
    {
        get { return tower != null; }
    }

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }
}
