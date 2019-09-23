using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Transform eventGroup;
    public GameObject[] events;
    public float[] difficulty;
    private int eventsCount;
    private int currentEvent;
    private float[] spawnPoints;
    private float[] eventsEndx;

    public GameObject eventCanvas;
    public GameObject platform;
    public GameObject sphere;

    public bool isChallenge = false;
    public float eventInterval = 40.0f;
    public float restartInterval = 5.0f;
    public AudioSource bgm;

    public PlayerController pc;

    public float UIDelay = 2.0f;

    private bool eventSuccess = false;

    private StreamWriter recordFile;

    public int levelNum = 1;
    public GameObject levelCanvas;
    public Text score;

    private int playerScore;

    public int initalScore = 1000;
    public int penaltyScore = 100;
    public int scoreBase = 500;

    public bool isRandom;
    public GameObject[] eventsPool;

    private int bestEvent;



    // Use this for initialization
    void Start()
    {
        eventsCount = events.Length;
        currentEvent = 0;
        pc.SetChallengeMode(isChallenge);
        pc.EnableMove(false);

        if(isRandom)
        {
            PickRandomEvents();
            bestEvent = 0;
        }

        if(!isChallenge)
        {
            ShowLevelUI();
        }
    }

    void PickRandomEvents()
    {
        int totalEvents = PlayerPrefs.GetInt("EventsNum");
        for(int i = 0; i < 5; i++)
        {
            int tempEventIndex = Random.Range(0, eventsPool.Length);
            events[i] = eventsPool[tempEventIndex];
            PlayerPrefs.SetInt("Event_" + i, tempEventIndex);
        }
    }

    void ShowLevelUI()
    {
        levelCanvas.SetActive(true);
        levelCanvas.GetComponent<LevelCanvas>().SetLevelText("Level " + levelNum);
        if(isRandom)
        {
            levelCanvas.GetComponent<LevelCanvas>().SetLevelText("Ready?");
        }
        levelCanvas.GetComponent<LevelCanvas>().ShowButton(false);
        Invoke("StartLevel", UIDelay);
        playerScore = initalScore;
        score.text = playerScore.ToString();
    }

    void StartLevel()
    {
        bgm.Play();
        SetUpEvents();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isChallenge && eventsEndx != null && currentEvent < eventsCount)
        {
            if(pc.transform.position.x >= eventsEndx[currentEvent])
            {
                eventSuccess = true;
                EventFinish(pc.GetCurrentHP());
            }
        }
        else if(eventsEndx != null && currentEvent < eventsCount)
        {
            if (pc.transform.position.x >= eventsEndx[currentEvent])
            {
                //update score
                playerScore += (int)(scoreBase * difficulty[currentEvent] + Random.Range(1, 100)) ;
                //Debug.Log(playerScore);
                score.text = playerScore.ToString();
                currentEvent++;
                if(currentEvent >= eventsCount)
                {
                    LevelFinish(pc.GetCurrentHP());
                }
            }
        }
    }

    public void StartGame(string fileName)
    {
        bgm.Play();
        SetUpEvents();
        new FileInfo(Application.dataPath + "/DataCollection/" + fileName + ".txt").Directory.Create();
        recordFile = new StreamWriter(Application.dataPath + "/DataCollection/" + fileName + ".txt", false);
    }


    public void SetUpEvents()
    {
        float x = 0;
        currentEvent = 0;
        spawnPoints = new float[eventsCount];
        eventsEndx = new float[eventsCount];
        for (int i = 0; i < eventsCount; i++)
        {
            spawnPoints[currentEvent] = x + restartInterval;
            x = x + eventInterval;
            GameObject newEvent = Instantiate(events[i], eventGroup);
            newEvent.transform.position = new Vector3(x, 0, 0);
            x = newEvent.transform.FindChild("ExampleEvent").FindChild("end").position.x;
            eventsEndx[currentEvent] = x + restartInterval;
            currentEvent++;

        }
        currentEvent = 0;

        StartEventUI();
    }

    public void StartEventUI()
    {
        if (isChallenge)
        {
            eventCanvas.SetActive(true);
            eventCanvas.GetComponent<EventCanvas>().SetEventText("Challenge " + (currentEvent + 1));
            Invoke("StartEvent", UIDelay);
        }
        else
        {
            levelCanvas.SetActive(false);
            pc.EnableMove(true);
        }


    }

    public void RestartEventUI()
    {
        eventCanvas.SetActive(true);
        eventCanvas.GetComponent<EventCanvas>().SetEventText("Try again!");
        Invoke("StartEvent", UIDelay);
    }

    public void StartEvent()
    {
        eventCanvas.SetActive(false);
        pc.EnableMove(true);
    }

    public void RestartEvent(int remainingHP)
    {
        if (remainingHP > 0)
        {
            
            Invoke("ResetPlayer", UIDelay);
            if (isChallenge)
            {
                RestartEventUI();
            }
            else
            {
                playerScore -= penaltyScore;
                score.text = playerScore.ToString();
                RestartEventUI();
            }

        }
        else
        {
            //Event failed
            if (isChallenge)
            {
                eventSuccess = false;
                EventFinish(0);
            }
            else
            {
                playerScore -= penaltyScore;
                score.text = playerScore.ToString();
                LevelFinish(0);
            }
        }

    }

    void LevelFinish(int remainingHP)
    {
        PlayerPrefs.SetInt("Level" + levelNum, playerScore);
        levelCanvas.SetActive(true);
        levelCanvas.GetComponent<LevelCanvas>().ShowButton(true);
        int totalEvents = PlayerPrefs.GetInt("EventsNum");
        if (remainingHP > 0)
        {
            levelCanvas.GetComponent<LevelCanvas>().SetLevelText("You have completed all challenges!\nYour score is " + playerScore);
            if(isRandom)
            {
                levelCanvas.GetComponent<LevelCanvas>().SetLevelText("You have completed all challenges!");
                PlayerPrefs.SetInt("BestEvent", totalEvents);
            }
        }
        else
        {
            levelCanvas.GetComponent<LevelCanvas>().SetLevelText("You have completed " + (currentEvent) + " of 6 challenges!\nYour score is " + playerScore);
            if (isRandom)
            {
                
                levelCanvas.GetComponent<LevelCanvas>().SetLevelText("You have completed " + (currentEvent) + " of " + totalEvents + " challenges!");
                PlayerPrefs.SetInt("BestEvent", bestEvent);
            }
        }
    }

    void EventFinish(int remainingHP)
    {
        pc.EnableMove(false);
        recordFile.WriteLine(remainingHP);
        currentEvent++;
        if(isChallenge)
        {
            EventFinishUI();
            if (currentEvent < eventsCount)
            {
                Invoke("EventTransition", UIDelay);
            }
        }
       
    }

    void EventTransition()
    {
        ResetPlayer();
        StartEventUI();
        pc.ResetHP();
    }

    void GameOver()
    {
        eventCanvas.SetActive(true);
        eventCanvas.GetComponent<EventCanvas>().SetEventText("All challenges completed!");
        recordFile.Close();
    }

    void EventFinishUI()
    {
        eventCanvas.SetActive(true);
        if(currentEvent >= eventsCount)
        {
            Invoke("GameOver", UIDelay);
        }
        if (eventSuccess)
        {
            eventCanvas.GetComponent<EventCanvas>().SetEventText("Succeeded!!!");
        }
        else
        {
            eventCanvas.GetComponent<EventCanvas>().SetEventText("Failed :(");
        }
    }

    void ResetPlayer()
    {
        if(!isRandom)
        {
            pc.transform.position = new Vector3(spawnPoints[currentEvent], 0, 0);
        }
        else
        {
            //When play random level, restart from the beginning
            if(currentEvent > bestEvent)
            {
                bestEvent = currentEvent;
            }
            pc.transform.position = new Vector3(-25, 0, 0);
        }
        
        pc.model.transform.rotation = Quaternion.Euler(Vector3.zero);
        pc.ResetPlayer();
    }
}
