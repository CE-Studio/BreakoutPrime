using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Core core;
    public Rigidbody2D rb;
    public BoxCollider2D box;

    public float speed = 12.5f;
    public float reflectScale = 1.5f;

    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        rb.position = new Vector2(rb.position.x + Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, core.paddleLevel);
        if (Mathf.Abs(rb.position.x) > core.playRange - (box.size.x * 0.5f))
            rb.position += (Mathf.Abs(rb.position.x) - core.playRange + box.size.x * 0.5f) * -Mathf.Sign(rb.position.x) * Vector2.right;
    }
}
