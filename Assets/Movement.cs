using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public ParticleSystem collisionParticleSystem;
    public bool once = true;

    public Rigidbody2D rb;
    public Animator anim;

    public float upForce = 100;
    public float speed = 1500;
    public float runSpeed = 2500;

    public bool isGrounded = false;

    public int maxHealth = 100;
    public int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TakeDamage(100);
        }

        float move = Input.GetAxis("Horizontal");
        if (move == 0)
        {
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsRun", false);
        }
        else
        {
            if (move > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (move < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Input.GetKey(KeyCode.LeftShift))

            {
                anim.SetBool("IsWalk", true);
                anim.SetBool("IsRun", true);
                rb.velocity = new Vector2(move * runSpeed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                anim.SetBool("IsRun", false);
                anim.SetBool("IsWalk", true);
                rb.velocity = new Vector2(move * speed * Time.deltaTime, rb.velocity.y);
            }

            anim.SetFloat("yVelocity", rb.velocity.y);

        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetBool("IsJump", true);
            rb.AddForce(Vector2.up * upForce);
            isGrounded = false;
        }

        if (currentHealth == 0)
        {
            anim.SetTrigger("IsDeath");
            rb.velocity = new Vector2(move * 0 * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            isGrounded = false;        

        }

        if (currentHealth == 0 && once)
        {
            var em = collisionParticleSystem.emission;
            var dur = collisionParticleSystem.duration;

            em.enabled = true;
            collisionParticleSystem.Play();

            once = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        anim.SetBool("IsJump", !isGrounded);
        
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(100);
    }


}