using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManeger : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private AIControl controller2;
    private HitScan hitScan = new HitScan();
    void Start()
    {
        hitScan.ParametersReset();
        hitScan.AddPlayerParameter(controller.SendPlayerParameter);
        hitScan.AddPlayerParameter(controller2.SendPlayerParameter);
        hitScan.Test();
    }

    private void FixedUpdate()
    {
        
    }

}
