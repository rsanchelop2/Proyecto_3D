using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorUnityChan : MonoBehaviour
{
    private Animator animator;
    public float velocidadMovimiento = 3.0f;
    public float velocidadRotacion = 100.0f;

    void Start()
    {
        // Obtiene la referencia del componente Animator adjunto
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura las entradas del teclado (Flechas o WASD)
        float movimientoVertical = Input.GetAxis("Vertical");
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Lógica de Movimiento hacia adelante / atrás
        if (movimientoVertical > 0)
        {
            transform.position += transform.forward * velocidadMovimiento * Time.deltaTime;
            animator.SetBool("is_running", true); // Activa animación de correr
        }
        else
        {
            animator.SetBool("is_running", false); // Regresa a estado Idle
        }

        // Lógica de Rotación sobre su propio eje (Y)
        if (movimientoHorizontal != 0)
        {
            transform.Rotate(0, movimientoHorizontal * velocidadRotacion * Time.deltaTime, 0);
        }
    }
}
