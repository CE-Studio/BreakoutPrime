using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Core core;
    public Rigidbody2D rb;
    public CircleCollider2D circ;

    public float speed = 10;
    public float downwardNudge = -0.2f;
    
    void Start()
    {
        core = GameObject.Find("Core").GetComponent<Core>();
        rb = GetComponent<Rigidbody2D>();
        circ = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (rb.velocity == Vector2.zero)
            rb.velocity = Vector2.down;

        rb.position += speed * Time.deltaTime * rb.velocity;

        if (rb.position.x < -core.playRange && rb.velocity.x < 0)
        {
            Reflect(false);
            rb.position += (core.playRange - rb.position.x) * 2 * Vector2.right;
        }
        if (rb.position.x > core.playRange && rb.velocity.x > 0)
        {
            Reflect(false);
            rb.position += (core.playRange - rb.position.x) * 2 * Vector2.right;
        }
        if (rb.position.y > core.playHeight && rb.velocity.y > 0)
        {
            Reflect(true);
            rb.position += (core.playHeight - rb.position.y) * 2 * Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            switch (collision.tag)
            {
                default:
                    break;
                case "Paddle":
                    if (rb.velocity.y < 0)
                        ReflectOffPaddle(collision);
                    break;
            }
        }
    }

    public void Reflect(bool axis, bool nudgeDownward = false)
    {
        if (nudgeDownward)
        {
            if (axis)
            {
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                if (rb.velocity.y < 0)
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - downwardNudge).normalized;
            }
            else
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y - downwardNudge).normalized;
        }
        else
        {
            if (axis)
                rb.velocity *= Vector2.down;
            else
                rb.velocity *= Vector2.left;
        }
    }

    public void ReflectOffPaddle(Collider2D paddle)
    {
        float originToBall = Mathf.Abs(rb.position.x - paddle.transform.position.x);
        float reflectPoint = (100 * originToBall) / (paddle.GetComponent<BoxCollider2D>().size.x * 0.5f);
        if (rb.position.x < paddle.transform.position.x)
            reflectPoint = -reflectPoint;

        rb.velocity = new Vector2(paddle.GetComponent<Paddle>().reflectAngle * reflectPoint, 1).normalized;
    }
}
