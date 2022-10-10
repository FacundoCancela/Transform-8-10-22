using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyAttackAI : MonoBehaviour
{
    //NOTA: Por ahora solo hay un tipo de enemigo

    //Para usar:
    //1) agregar script a cualquier objeto que sea un enemigo.
    //2) agregar la parte del cuerpo que va a hacer daño en enemyWeapon
    //3) settear los parametros deseados.

    private int playerLayer;

    [Header("Enemy Set-Up")]
    [SerializeField] private GameObject enemyWeapon;

    private Rigidbody2D enemyRb;
    private Collider2D enemyColl;
    private Animator enemyAnimator;
    private Vector3 initialPosition;

    [Header("Enemy Behaviour")]
    [SerializeField] private float detectRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackSpeed, resetSpeed;

    private bool isAttacking = false;
    private bool isPlayerInSight = false;
    private bool isAttackCompleted = false;

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void Start()
    {
        initialPosition = transform.position;
        enemyRb = GetComponent<Rigidbody2D>();
        enemyColl = GetComponent<Collider2D>();
        enemyAnimator = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player");
    }

    private void FixedUpdate()
    {
        //Si el jugador esta en rango y el enemigo no esta en medio de un ataque
        if (isAttacking == false && IsPlayerInSight() == true)
        {
            StartCoroutine(InitiateAttack());
        }
        //Resetear a comportamiento inicial cuando termine el ataque
        else if (isAttackCompleted)
        {
            ResetAttackStance();
        }
    }

    //El ataque termina solo cuando termina de caer completamente
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            EndAttack();
        }
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    //-----------------
    //-----ATAQUE------

    //Como Rocky es el unico enemigo y funca dejandose caer, vuelve a pos inicial, etc.
    //Desp. cuando haya otros se agrega el check segun el tipo y se cambia el comportamiento, o se hace otro script.

    private IEnumerator InitiateAttack()
    {
        isAttacking = true;
        FreezeEnemyInPlace(false);
        enemyAnimator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackDelay);

        //Para que tenga un poco de acceleracion y se sienta mas natural aplicamos fuerza.
        enemyWeapon.SetActive(true);
        enemyRb.AddForce(new Vector2(0, -attackSpeed * enemyRb.mass), ForceMode2D.Impulse);
    }

    //Terminar el ataque - ya no puede hacer mas daño hasta el proximo
    private void EndAttack()
    {
        isAttackCompleted = true;
        enemyWeapon.SetActive(false);
        enemyAnimator.SetBool("isAttacking", false);
    }

    //Volver a como estaba antes de atacar para que pueda lanzar un nuevo ataque.
    private void ResetAttackStance()
    {
        //Hasta que vuelva a la posicion inicial.
        if (transform.position != initialPosition)
        {
            //Para que sea natural, cuando vuelve no aplicamos fuerza, lo mandamos de forma constante a la pos inicial.
            enemyRb.velocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, resetSpeed * Time.deltaTime);
        }
        //Cuando llega, puede atacar otra vez.
        else
        {
            FreezeEnemyInPlace(true);
            isAttacking = false;
            isAttackCompleted = false;
        }
    }

    //-----------------
    //------AUX--------

    //Como usa un RB dinamico para recibir fuerza, y Rocky tmb es una plataforma, cuando el jugador este sobre el lo va a hundir de a poco.
    //Como no queremos que eso pase, lo freezamos en el lugar cuando no este haciendo nada. Tmb lo hago para mas adelante, asi se
    //puede habilitar la alternativa del jugador hundiendo a rocky y usandolo como una especie de plataforma que se mueve.
    private void FreezeEnemyInPlace(bool status)
    {
        if (status == true)
        {
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            //La Z hay que dejarla Freezada siempre
            enemyRb.constraints = RigidbodyConstraints2D.None;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //Para detectar al jugador, usamos el collider del enemigo asi no tiene puntos ciegos.
    private bool IsPlayerInSight()
    {
        isPlayerInSight = Physics2D.BoxCast(transform.position, new Vector2(enemyColl.bounds.size.x, enemyColl.bounds.size.y), 
            0, Vector2.down, detectRange, playerLayer);
        return isPlayerInSight;
    }
}
