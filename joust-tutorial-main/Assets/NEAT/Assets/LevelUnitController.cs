using SharpNeat.Phenomes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;

/// <summary>
/// This class serves as an example template for how to create a UnitController.
/// </summary>
public class LevelUnitController : UnitController
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInputManager input;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform enemyParent;

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        inputSignalArray[0] = player.transform.position.x;
        inputSignalArray[1] = player.transform.position.y;

        int iSignalArrayIndex = 2;
        for(int i = 0; i < enemyParent.childCount; i++)
        {
            inputSignalArray[iSignalArrayIndex] = 0;
            inputSignalArray[iSignalArrayIndex+1] = enemyParent.GetChild(i).transform.position.x;
            inputSignalArray[iSignalArrayIndex+2] = enemyParent.GetChild(i).transform.position.y;
            iSignalArrayIndex += 3;

        }
        for (int i = iSignalArrayIndex; i < inputSignalArray.Length; i++)
        {
            inputSignalArray[iSignalArrayIndex] = 0;
        }

        // Called by the base class on FixedUpdate

        // Feed inputs into the Neural Net (IBlackBox) by modifying its InputSignalArray
        // The size of the input array corresponds to NeatSupervisor.NetworkInputCount
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {

        // Called by the base class after the inputs have been processed

        // Read the outputs and do something with them
        // The size of the array corresponds to NeatSupervisor.NetworkOutputCount


        /* EXAMPLE */
        //someMoveDirection = outputSignalArray[0];
        //someMoveSpeed = outputSignalArray[1];
        //...
        input.FireMoveEvent(new Vector2(((float)outputSignalArray[0]-0.5f)*2, 0));
        if ((float)outputSignalArray[1] >= 0.5)
        {
            input.FireFlapEvent();
            Debug.Log("Flap");
        }
        //Debug.Log("Output: " + (float)outputSignalArray[0] + " " + ((float)outputSignalArray[0]-0.5f)*2);
        //Debug.Log((float)outputSignalArray[1]);
    }

    public override float GetFitness()
    {
        // Called during the evaluation phase (at the end of each trail)

        // The performance of this unit, i.e. it's fitness, is retrieved by this function.
        // Implement a meaningful fitness function here

        return scoreManager.Score;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // Called whenever the value of IsActive has changed

        // Since NeatSupervisor.cs is making use of Object Pooling, this Unit will never get destroyed. 
        // Make sure that when IsActive gets set to false, the variables and the Transform of this Unit are reset!
        // Consider to also disable MeshRenderers until IsActive turns true again.
        if (newIsActive)
        {
            //gameObject.SetActive(true);
            gameManager.StartGame();
            gameManager._playerSpawner.SpawnPlayer();
        }
        else
        {
            scoreManager.KillPlayer(player);
            enemyManager.DestroyAllEnemies();
            //gameObject.SetActive(false);
            gameManager.UnsubscribeFromEvents();
        }
    }
}