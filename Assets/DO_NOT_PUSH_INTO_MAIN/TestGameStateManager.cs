using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MetaFrame.Data;
using static MetaFrame.Data.SurveyDataRecorder;
using UnityEngine.UI;
using static TestGameStateManager;

public class TestGameStateManager : MonoBehaviour
{
    public static TestGameStateManager instance;


    [Header("Reference Scripts")]
    [SerializeField]
    private SurveyDataRecorder surveyDataRecorder;
    [SerializeField]
    private SpawnMechanism spawnMechanism;


    //Keeps track of the current session
    public enum SessionType
    {
        TUTORIAL,
        SESSION_A,
        SESSION_B,
        SESSION_C
    }
    [SerializeField]
    private SessionType currentSessionType = SessionType.TUTORIAL;


    //Keeps track of the current trial
    private int trialNumber = 0;



    public enum TrialType
    {
        BEFORE_EXPERIMENT,
        NORMAL,
        ANOMOLY_1_OPTION_1,
        ANOMOLY_1_OPTION_2,

        ANOMOLY_2,
        ANOMOLY_3,
        ANOMOLY_4,
        ANOMOLY_5,
        ANOMOLY_6,
        ANOMOLY_7,
        ANOMOLY_8,
        ANOMOLY_9,
        ANOMOLY_10,
        ANOMOLY_11,
        ANOMOLY_12,

        CANCEL //Unsure if we need this one but just to be safe I left it as an option
    }

    public enum TrialStates
    {
        AT_SOURCE,
        IN_HAND,
        AT_TARGET
    }

    [System.Serializable]
    public struct TrialData
    {
        [Tooltip("Set the state for each trial to the anomoly you want to occur during this trial.")]
        public TrialType state;
        [System.NonSerialized]
        public string trialStartTime;
        [System.NonSerialized]
        public string trialEndTime;
    }


    [Header("List of Trials for Experiment")]
    [Tooltip("Set the state for each trial to the anomoly you want to occur during this trial.")]
    [SerializeField]
    private List<TrialData> sessionTutorialListData = new List<TrialData>();
    [SerializeField]
    private List<TrialData> sessionAListData = new List<TrialData>();
    [SerializeField]
    private List<TrialData> sessionBListData = new List<TrialData>();
    [SerializeField]
    private List<TrialData> sessionCListData = new List<TrialData>();

    private List<TrialData> currentTrialList;

    public event GameState GameStateTrigger;
    public delegate void GameState(SessionType sessionType, int trialNumber, TrialData trialData);

    public event TrialStateUpdate TrialStatesTrigger;
    public delegate void TrialStateUpdate(SessionType sessionType, int trialNumber, TrialStates trialState);

    private bool experimentInProgress = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("A second GameStateManager was detected and deleted!");
            Destroy(gameObject);
        }
        instance = this;
    }


    //private void Start()
    //{
    //    StartCoroutine(testTrialStart());
    //}

    //private IEnumerator testTrialStart()s
    //{
    //    yield return new WaitForSeconds(5);
    //    BeginNextTrial();
    //    yield return new WaitForSeconds(5);
    //    //BeginNextTrial();
    //}

    public void UpdateSessionType(SessionType sessionType)
    {
        switch (sessionType)
        {
            case SessionType.TUTORIAL:
                currentSessionType = sessionType;
                currentTrialList = sessionTutorialListData;
                break;
            case SessionType.SESSION_A:
                currentSessionType = sessionType;
                currentTrialList = sessionAListData;
                break;
            case SessionType.SESSION_B:
                currentSessionType = sessionType;
                currentTrialList = sessionBListData;
                break;
            case SessionType.SESSION_C:
                currentSessionType = sessionType;
                currentTrialList = sessionCListData;
                break;
            default:
                Debug.LogError("No more sessions for trial!");
                break;
        }
    }

    public void ProgressTrial()
    {
        if (experimentInProgress == false)
        {

            BeginNextTrial();
            experimentInProgress = true;


            
            Debug.Log("GSM: Experiment started");
        }
        else
        {
            UpdateDataThenBeginNextTrial(surveyDataRecorder.stateD);
        }
    }

    public (int, TrialData) GetGameState()
    {
        return (trialNumber, currentTrialList[trialNumber]);
    }

    public void BeginNextTrial()
    {
        if (trialNumber >= currentTrialList.Count)
        {
            Debug.LogWarning("No more trials available!");
            return;
        }


        //Update the current state
        trialNumber += 1;

        Debug.Log("GSM: Trial " + trialNumber + " Is Starting.");

        //Update start time
        TrialData trialData = currentTrialList[trialNumber];
        trialData.trialStartTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        currentTrialList[trialNumber] = trialData;

        //Send event signal
        GameStateTrigger.Invoke(currentSessionType, trialNumber, currentTrialList[trialNumber]);

        spawnMechanism.SpawnCup();
    }

    public void UpdateDataThenBeginNextTrial(StateData stateData)
    {
        //Update trial end time
        TrialData trialData = currentTrialList[trialNumber];
        trialData.trialEndTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        currentTrialList[trialNumber] = trialData;

        //Add the trial start and end time to the surveyData
        StateData stateDataUpdated = stateData;
        stateDataUpdated.triallNo = trialNumber.ToString();
        stateDataUpdated.triall_S = trialData.trialStartTime;
        stateDataUpdated.triall_E = trialData.trialEndTime;
        stateDataUpdated.placed = spawnMechanism.CupPlacementCompletion();
        surveyDataRecorder.StoreToggleValues();
        Debug.Log("GSM Survey Data: " + surveyDataRecorder.stateD.triallNo + " " + surveyDataRecorder.surveyD.detection + " " + surveyDataRecorder.surveyD.confidence + " " + surveyDataRecorder.surveyD.explanation + " " + surveyDataRecorder.stateD.triall_S + " " + surveyDataRecorder.surveyD.report_S + " " + surveyDataRecorder.stateD.triall_E + " " + surveyDataRecorder.stateD.placed);
        surveyDataRecorder.surveyCompletion();
        if (surveyDataRecorder.survey_C == true && surveyDataRecorder.surveyD.detection == "Yes")
        {
            surveyDataRecorder.firstTimeActivation = true;
            surveyDataRecorder.SaveTrial(stateDataUpdated);
            EndCurrentTrial();
            BeginNextTrial();
        }
        else if (surveyDataRecorder.surveyD.detection == "No")
        {
            surveyDataRecorder.noSelection();
            surveyDataRecorder.SaveTrial(stateDataUpdated);
            EndCurrentTrial();
            BeginNextTrial();
        }
        else
        {
            Debug.Log("survey incomplete");
        }
    }


    public void EndCurrentTrial()
    {
        spawnMechanism.DestroyCup();
        Debug.Log("GSM: Ending trial " + trialNumber);
    }



    public void DELETE()
    {
        gameObject.SetActive(false);
        Image testImage = null;
        testImage.enabled = false;
    }


}







