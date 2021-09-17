using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    // Player
    public Transform player = null;

    // HUD
    public float playerTargeting = 0.0f;
    public bool playerWarning = false;

    private void Awake()
    {
        gameManager = this;    
    }

    private void Update()
    {
        if (playerTargeting > 0.0f)
            playerWarning = true;
        else
            playerWarning = false;
            
        Debug.Log(playerWarning ? 1 : 0);
    }
}
