using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] pathsTo;
    public GameObject go;

    public Node( GameObject g ){
        go = g;
    }
}
