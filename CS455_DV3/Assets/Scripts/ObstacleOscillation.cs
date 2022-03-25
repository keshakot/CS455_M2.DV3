using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleOscillation : Kinematic
{

    public float speed = 0.2f;
    public float strength = 5f;

    private float posOffset;
    private float sinOffset;

    // Start is called before the first frame update
    void Start()
    {
        sinOffset = Random.Range(0f, 2f);
        posOffset = this.transform.position.x;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Vector3 pos = this.transform.position;
        pos.x = posOffset + Mathf.Sin(Time.time * speed + sinOffset) * strength;
        this.transform.position = pos;
    }
}
