using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdle : MonoBehaviour
{
    private Animator animator;
    private float timer;
    public float timeBetweenIdles = 5f; // Cambia cada 5 segundos

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenIdles)
        {
            // Elige un valor aleatorio entre 0, 1 y 2
            int randomWait = Random.Range(0, 3);
            animator.SetInteger("WaitIndex", randomWait);
            
            timer = 0f; // Reinicia el temporizador
        }
    }
}