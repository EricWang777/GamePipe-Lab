using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PlayerController pc;
    public bool isChallenge =false;

    public AudioSource bgm;
    public string levelXMLPath = "";

    //xml related
    private XmlDocument xmlDocument;
    private XmlNodeList eventsList;
    private int currentEvent = 0;
    private int lastEvent;
    private float eventInterval = 5;

    public GameObject enemyType1;
    public GameObject enemyType2;

    public GameObject eventCanvas;

    public ChallengeUI challengeUi;

    public Transform[] spawnPositions;
    public GameObject enemyGroup;

    //Track the current progress in the event
    private int remainingEnemies;
    private bool inEvent = false;
    private bool eventSuccess = false;

    public float eventEndUITime = 2.0f;

    //private string recordFileName = "Tian_12345.txt";
    private StreamWriter recordFile;

    public int levelNum = 1;

    private int playerScore;
    public int initialScore = 1000;
    public int penalty = 100;
    public int enemyScore = 200;
    public float[] difficulty;
    public Text score;



    // Use this for initialization
    void Start()
    {
        if (pc)
        {
            pc.SetChallenge(isChallenge);
        }


        xmlDocument = new XmlDocument();
        xmlDocument.Load(Application.dataPath + "/LevelXML/" + levelXMLPath);
        if(xmlDocument.FirstChild.Name != "Level")
        {
            Debug.LogError("Wrong level XML file!");
        }
        else
        {
            eventInterval = float.Parse(xmlDocument.GetElementsByTagName("Interval").Item(0).InnerText);
            //Debug.Log(eventInterval);
            eventsList = xmlDocument.GetElementsByTagName("Event");
            currentEvent = 0;
            //currentEvent = eventsList.Count - 1;
            lastEvent = eventsList.Count - 1;
            //lastEvent = 0;
        }

        if(!isChallenge)
        {
            eventCanvas.SetActive(true);
            eventCanvas.GetComponent<EventCanvas>().SetText("Level " + levelNum);
            eventCanvas.GetComponent<EventCanvas>().ShowButton(false);
            Invoke("StartLevel", eventInterval);
            UpdateScore(initialScore);
        }
    }

    void UpdateScore(int _score)
    {
        playerScore = _score;
        score.text = playerScore.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartLevel()
    {
        bgm.Play();
        SetUpEvent();
    }

    public void StartGame(string fileName)
    {
        bgm.Play();
        //load level
        SetUpEvent();
        //Debug.Log(fileName);
        new FileInfo(Application.dataPath + "/DataCollection/" + fileName + ".txt").Directory.Create();
        recordFile = new StreamWriter(Application.dataPath + "/DataCollection/" + fileName + ".txt", false);
    }

    void SetUpEvent()
    {
        ShowEventStartUI();
        if(isChallenge)
        {
            pc.ResetHP();
        }
    }

    void SetUpEnemy(XmlNode enemyNode)
    {
        float maxHP = 100.0f;
        //Enemy Type 1 specific
        float timer = 5.0f;
        float delay = 1.0f;
        //Enemy Type 2 sepcific
        BulletType bulletType = BulletType.Curve;
        float firingInterval = 2.0f;
        float flyingTime = 5.0f;
        float xScale = 1.0f;
        float yScale = 1.0f;

        Transform spawnPosition = spawnPositions[0];

        

        int type = 1;

        for(int i = 0; i < enemyNode.Attributes.Count; i++)
        {
            switch(enemyNode.Attributes[i].Name)
            {
                case "maxHP":
                    maxHP = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "timer":
                    timer = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "delay":
                    delay = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "bulletType":
                    if(enemyNode.Attributes[i].Value == "Straight")
                    {
                        bulletType = BulletType.Straight;
                    }
                    else if (enemyNode.Attributes[i].Value == "Curve")
                    {
                        bulletType = BulletType.Curve;
                    }
                    break;
                case "firingInterval":
                    firingInterval = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "flyingTime":
                    flyingTime = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "xScale":
                    xScale = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "yScale":
                    yScale = float.Parse(enemyNode.Attributes[i].Value);
                    break;
                case "sp":
                    if(enemyNode.Attributes[i].Value == "sp1")
                    {
                        spawnPosition = spawnPositions[0];
                    }
                    else if(enemyNode.Attributes[i].Value == "sp2")
                    {
                        spawnPosition = spawnPositions[1];
                    }
                    else if (enemyNode.Attributes[i].Value == "sp3")
                    {
                        spawnPosition = spawnPositions[2];
                    }
                    else if (enemyNode.Attributes[i].Value == "sp4")
                    {
                        spawnPosition = spawnPositions[3];
                    }
                    else if (enemyNode.Attributes[i].Value == "sp5")
                    {
                        spawnPosition = spawnPositions[4];
                    }
                    break;
                case "type":
                    if(enemyNode.Attributes[i].Value == "1")
                    {
                        type = 1;
                    }
                    else if(enemyNode.Attributes[i].Value == "2")
                    {
                        type = 2;
                    }
                    break;
                default:
                    break;
            }
            
        }
        if (type == 1)
        {
            SetUpEnemyType1(spawnPosition, maxHP, timer, delay);
            remainingEnemies++;
        }
        else if (type == 2)
        {
            SetUpEnemyType2(spawnPosition, maxHP, bulletType, firingInterval, flyingTime, xScale, yScale);
            remainingEnemies++;
        }
    }

    void SetUpEnemyType1(Transform spawnPoint, float _maxHP = 100.0f, float _timer = 5.0f, float _delay = 1.0f)
    {
        GameObject newEnemy = Instantiate(enemyType1, spawnPoint.position, spawnPoint.rotation, enemyGroup.transform);
        newEnemy.GetComponent<EnemyControllerType1>().maxHP = _maxHP;
        newEnemy.GetComponent<EnemyControllerType1>().rt.shrinkTime = _timer;
        newEnemy.GetComponent<EnemyControllerType1>().rt.delay = _delay;
    }

    void SetUpEnemyType2(Transform spawnPoint, float _maxHP, BulletType _bulletType, float _firingInterval, float _flyingTime, float _xScale, float _yScale)
    {
        GameObject newEnemy = Instantiate(enemyType2, spawnPoint.position, spawnPoint.rotation, enemyGroup.transform);
        newEnemy.GetComponent<EnemyControllerType2>().maxHP = _maxHP;
        newEnemy.GetComponent<EnemyControllerType2>().bulletType = _bulletType;
        newEnemy.GetComponent<EnemyControllerType2>().firingInterval = _firingInterval;
        newEnemy.GetComponent<EnemyControllerType2>().flyingTime = _flyingTime;
        newEnemy.GetComponent<EnemyControllerType2>().xScale = _xScale;
        newEnemy.GetComponent<EnemyControllerType2>().yScale = _yScale;
    }

    void ShowEventStartUI()
    {
        
        if(isChallenge)
        {
            eventCanvas.SetActive(true);
            eventCanvas.GetComponent<EventCanvas>().SetText("Challenge " + (currentEvent + 1));
            Invoke("StartEvent", eventInterval);
        }
        else
        {
            StartEvent();
        }
        
    }

    void ShowEventEndUI()
    {
        if(isChallenge)
        {
            eventCanvas.SetActive(true);
            if (eventSuccess)
            {
                eventCanvas.GetComponent<EventCanvas>().SetText("Succeeded!!!");
            }
            else
            {
                eventCanvas.GetComponent<EventCanvas>().SetText("Failed :(");
            }
        }
        else
        {

        }
       

        foreach (Transform child in enemyGroup.transform)
        {
            Destroy(child.gameObject);
        }

        if (currentEvent == lastEvent)
        {
            currentEvent++;
            Invoke("GameOver", eventInterval);
        }
        else
        {
            currentEvent++;
            Invoke("SetUpEvent", eventEndUITime);
        }

    }

    void GameOver()
    {
        foreach (Transform child in enemyGroup.transform)
        {
            Destroy(child.gameObject);
        }
        eventCanvas.SetActive(true);
        if(isChallenge)
        {
            eventCanvas.GetComponent<EventCanvas>().SetText("All Challenges completed");
            recordFile.Close();
        }
        else
        {
            eventCanvas.GetComponent<EventCanvas>().ShowButton(true);
            PlayerPrefs.SetInt("Level" + levelNum, playerScore);
            if (currentEvent > lastEvent)
            {
                eventCanvas.GetComponent<EventCanvas>().SetText("You have completed all challenges\nYour score is " + playerScore);
            }
            else
            {
                eventCanvas.GetComponent<EventCanvas>().SetText("You have completed " + currentEvent + " of 6 challenges\nYour score is " + playerScore);
            }
            
        }
        
    }

    void StartEvent()
    {
        eventCanvas.SetActive(false);
        XmlNodeList currentEventNode = eventsList[currentEvent].ChildNodes;
        //Debug.Log(currentEventNode.Count);
        remainingEnemies = 0;
        for (int i = 0; i < currentEventNode.Count; i++)
        {
            SetUpEnemy(currentEventNode[i]);
        }

        inEvent = true;
    }

    public void EventOver(int remainingHP)
    {
        if(isChallenge && inEvent)
        {
            if (remainingHP > 0)
            {
                eventSuccess = true;
            }
            else
            {
                eventSuccess = false;
            }

            Invoke("ShowEventEndUI", 1.0f);

            recordFile.WriteLine(remainingHP);

            inEvent = false;
        }
        else if(!isChallenge && inEvent)
        {
            if(remainingHP > 0)
            {
                Invoke("ShowEventEndUI", 1.0f);
            }
            else
            {
                GameOver();
            }
            inEvent = false;
        }

    }

    public void EnemyKilled()
    {
        remainingEnemies--;
        UpdateScore(playerScore + (int)( enemyScore * difficulty[currentEvent]) + Random.Range(1, 20));
        //Debug.Log("remain: " + remainingEnemies);
        if(remainingEnemies <= 0)
        {
            EventOver(pc.GetCurrentHP());
        }
    }

    public void Penalty()
    {
        UpdateScore(playerScore - penalty);
    }
}
