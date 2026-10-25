using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera firstOrSecondPersonCam;

    [Header("Configuración de Prioridades")]
    [SerializeField] private int activePriority = 10;
    [SerializeField] private int inactivePriority = 0;

    private bool isSecondPerson = false;

    void Update()
    {
        // Al presionar la tecla F3
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isSecondPerson = !isSecondPerson;
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        if (isSecondPerson)
        {
            // Activa la vista en segunda/primera persona
            thirdPersonCam.Priority = inactivePriority;
            firstOrSecondPersonCam.Priority = activePriority;
        }
        else
        {
            // Vuelve a la vista en tercera persona
            thirdPersonCam.Priority = activePriority;
            firstOrSecondPersonCam.Priority = inactivePriority;
        }
    }
}
