using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class battleCameraTransition : MonoBehaviour
{
    // Defining the virtual camera for a battle
    [SerializeField]
    public CinemachineVirtualCamera battleCam;
    // Whether there is currently a battle
    public bool isBattle;

    private void Start()
    {
        battleCam.Priority = 1;
    }

    private void Update()
    {
        if (isBattle)
        {
            battleCam.Priority = 100;
        }
        else
        {
            battleCam.Priority = 1;
        }
    }
}
