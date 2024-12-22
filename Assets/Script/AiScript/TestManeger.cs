using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManeger : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private AIControl controller2;
    void Start()
    {
        PlayerManager.ParametersReset();
        PlayerManager.SetPlayerDate(controller);
    }

    private void FixedUpdate()
    {
        
    }

}
