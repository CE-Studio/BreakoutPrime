using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Core core;
    public Rigidbody2D rb;
    public CircleCollider2D circ;

    public float downwardNudge = -0.2f;
    public float speed;
    public float speedDefault = 10f;
    public float speedIncrease = 0.1f;
    public float speedMax = 30f;
    
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        rb = GetComponent<Rigidbody2D>();
        circ = GetComponent<CircleCollider2D>();

        speed = speedDefault;
    }

    void Update()
    {
        if (rb.velocity == Vector2.zero)
            rb.velocity = Vector2.down;

        rb.position += speed * Time.deltaTime * rb.velocity;

        if (rb.position.x < -core.playRange && rb.velocity.x < 0)
        {
            Reflect(false);
            rb.position += (core.playRange + rb.position.x) * 2 * Vector2.right;
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

        if (rb.position.y < core.paddleLevel - 3)
        {
            rb.position = Vector2.zero;
            rb.velocity = Vector2.down;
            speed = speedDefault;
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
                case "Block":
                    Vector2 thisSize = collision.GetComponent<BoxCollider2D>().size;
                    Vector2 distance = new Vector2(collision.transform.position.x - rb.position.x, collision.transform.position.y - rb.position.y);
                    distance = new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y));
                    if (distance.x > thisSize.x * 0.25f &&
                        ((rb.position.x < collision.transform.position.x && rb.velocity.x > 0) ||
                        (rb.position.x > collision.transform.position.x && rb.velocity.x < 0)))
                        Reflect(false);
                    if (distance.y > thisSize.y * 0.25f &&
                        ((rb.position.y < collision.transform.position.y && rb.velocity.y > 0) ||
                        (rb.position.y > collision.transform.position.y && rb.velocity.y < 0)))
                        Reflect(true);

                    collision.GetComponent<Block>().Hit();
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
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            else
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
        speed += speedIncrease;
    }

    public void ReflectOffPaddle(Collider2D paddle)
    {
        Vector2 direction = rb.position - (Vector2)paddle.transform.position;
        rb.velocity = new Vector2(direction.x * paddle.GetComponent<Paddle>().reflectScale, 1).normalized;
        speed += speedIncrease;
    }
}
