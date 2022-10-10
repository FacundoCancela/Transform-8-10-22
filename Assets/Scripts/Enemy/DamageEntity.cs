using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEntity : MonoBehaviour
{
    //Para usar:
    //1) Agregar a cualquier objeto que haga daño (puede ser un pozo, la parte de algun enemigo que haga daño, etc).
    //2) Elegir que tipo de daño hace en el inspector. Si tiene mas de un efecto, usar mas de uno (igual por ahora solo esta el de teleport.
    //3) Si hay un SFX asociado al golpe, indicar el nombre del sfx (en el inspector).

    private Transform player;

    private enum DamageEffect
    {
        TeleportToWaypoint,
        Knockback,
        Damage
    }

    [Header("Set-Up")]
    [SerializeField] private DamageEffect[] damageEffects = new DamageEffect[1];
    [SerializeField] private string sfxToPlayOnHit;

    [Header("Teleport Waypoint")]
    [SerializeField] private Vector2 respawnWaypoint;

    //################ #################
    //------------UNITY F--------------
    //################ #################

    private void Start()
    {
        player = PlayerController.instance.gameObject.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerHit();
        }
        AudioManager.instance.PlaySFX(sfxToPlayOnHit);
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    //Cuando la entidad golpea al jugador
    private void OnPlayerHit()
    {
        //Aplicar todos los efectos indicados
        for (int i = 0; i < damageEffects.Length; i++)
        {
            switch (damageEffects[i])
            {
                //Mueve el jugador a las coordenadas especificadas (en el inspector)
                case DamageEffect.TeleportToWaypoint:
                    player.position = new Vector3(respawnWaypoint.x, respawnWaypoint.y, 0);
                    break;
            }
        }
    }
}
