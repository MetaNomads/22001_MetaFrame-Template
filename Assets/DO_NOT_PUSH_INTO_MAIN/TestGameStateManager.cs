using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

public class TestGameStateManager : MonoBehaviour
{

    public static TestGameStateManager instance;

    public enum GameStates
    {
        BEFORE_EXPERIMENT,
        TUTORIAL_1, TUTORIAL_2, TUTORIAL_3,
        ANOMALY_1, ANOMALY_2, ANOMALY_3,
    }

    private GameStates currentState = GameStates.BEFORE_EXPERIMENT;

    public event GameState GameStateTrigger;
    public delegate void GameState(GameStates state);

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("A second GameStateManager was detected and deleted!");
            Destroy(gameObject);
        }

        instance = this;
    }

    public void UpdateGameState(GameStates state)
    {

        //Update the current state
        currentState = state;

        //Send event signal
        GameStateTrigger.Invoke(currentState);


    }

    public GameStates GetGameState()
    {
        return currentState;
    }



}
