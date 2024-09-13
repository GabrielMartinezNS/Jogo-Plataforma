using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Rigidbody2D rb;
    float vel = 5f;
    float Jforce = 5f;
    bool onGround;
    bool jump2;
    bool canDash = true;
    bool dashing;
    float dashCooldown;
    float dashTime = 0.2f;
    float Dforce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Jump();
        Moviment();
    }

    void Moviment()
    {
        // Movimentação base
        if(Input.GetButton("Horizontal") && !dashing)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * vel;
        }

        //  Dash
        if(Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            if(Dforce < 0)
            {
                Dforce = Dforce * -1;
            }
        }
        if(Input.GetAxis("Horizontal") < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            if(Dforce > 0)
            {
                Dforce = Dforce * -1;
            }
        }
    }

    void Jump()
    {
        //Pulo & pulo duplo
        if(Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.velocity = (new Vector2(rb.velocity.x, Jforce));
            jump2 = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && jump2)
        {
            rb.velocity = (new Vector2(rb.velocity.x, Jforce));
            jump2 = false;
        }
    }

    //Verificar se o player está no chão
    void OnCollisionEnter2D(Collision2D ground)
    {
        if(ground.gameObject.layer == 8)
        {
            onGround = true;
        }
    }
    void OnCollisionExit2D(Collision2D ground)
    {
        if(ground.gameObject.layer == 8)
        {
            onGround = false;
        }
    }

    IEnumerator Dash()
    {
        if(onGround)
        {
            dashCooldown = 0.5f;
        }
        else
        {
            dashCooldown = 1f;
        }
        //Inciar ação do dash
        canDash = false;
        dashing = true;

        //desliga gravidade para um dash reto, adciona a velocidade do dash
        //inicia o tempo em que o jogador estara na ação de dash
        float gravipadrao = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = (new Vector2(Dforce, 0));
        yield return new WaitForSeconds(dashTime);

        //retira a velocidade do dash, retorna a gravidade ao normal
        //retida o jogardor da ação de dash e inicia o tempo para o jogador uzar o dash novamente
        rb.velocity = (new Vector2(0, 0));
        rb.gravityScale = gravipadrao;
        dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}