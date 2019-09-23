using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityStandardAssets._2D;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject platform;
    public GameObject trajectory;
    public AudioSource bgm;
    public string recordFileName;
    public int platformNumber;
    
    // member variable
    float stuckTimer = 0.5f;
    float lastX;
    List<GameObject> scenes = new List<GameObject>();
    private TextWriter tsw;
    XmlWriter xml;
    const int maxHeight = 5;
    const int startIndex = 5;
    List<int> platforms = new List<int>();
    List<float> jumpLocation = new List<float>();
    // used by xml
    List<float> xmlLocation = new List<float>();
    int jumpTimes = 0;
    float startLine; // the first threshold for jumping the a single event
    float endLine;  // if player go pass this endLine, it succeeds
    

    // used for each test , need reset after finishing
    float lastJumpLocation = 0f;
    bool successJump = false;
    bool jumpOnce = false;
    float jumpOnceTimer = 0.2f;
    int checkPoint = 0;
    float finalJumpLocation = 0f;

    bool eventstart = false;

    void ResetVar()
    {
        //tsw.WriteLine("----------------------");
        //tsw.WriteLine("Current test index: " + testIndex);
        //testHeight--;
        //tsw.WriteLine("Current test height: " + testHeight);
        //tsw.Flush();
        //scenes[testIndex].transform.localScale = new Vector2(1, testHeight);
        //jumpOnce = false;
        //successJump = false;
        //lastJumpLocation = 0f;

    }

    void Start()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = ("\t");
        settings.OmitXmlDeclaration = true;
        xml = XmlWriter.Create(Application.dataPath + "/RecordFolder/xml.xml", settings);


        xml.WriteStartDocument();
        


        // variable initialize
        // frame to 30fps
        Time.timeScale = 5f;
        jumpOnce = false;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;


        // Initalize file to store the AI record
        string recordFilePath = Application.dataPath + "/RecordFolder/" + recordFileName;
        tsw = new StreamWriter(recordFilePath, false);
        tsw.WriteLine("---------" + System.DateTime.Now + "---------");
        tsw.Flush();

        
        // generate basic platform floor, to index 15
        for(int i = 0; i <= 15; i++)
        {
            GameObject go = Instantiate(platform, new Vector2(i * 2.5f, 0), Quaternion.identity);
            scenes.Add(go);
        }

        // initalize the platform list to max Height
        for (int i = 0; i < platformNumber; i++) platforms.Add(maxHeight);
        

        // initalize the first jump (in all cases in one event) threshold and the finsih line
        startLine = scenes[startIndex].transform.position.x - 6.0f; // the first spot to jump
        endLine = scenes[startIndex + platformNumber].transform.position.x; // if player reach this point, success

        startEvent();
        

    }

    // Update is called once per frame
    void Update()
    {
        // check stuck
        if (lastX > player.transform.position.x - 0.05f && lastX < player.transform.position.x + 0.05f)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0f)
            {
                PlayerJump();
                jumpLocation[jumpTimes] = player.transform.position.x;
                jumpTimes++;

            }
        }
        else
        {
            lastX = player.transform.position.x;
            stuckTimer = 0.5f;
        }

        if (player.transform.position.x < startLine) return; // dont jump before the startLine; 
        if (player.transform.position.x > endLine)
        {
            // player succeeds
            // 1. record the jump location
            // 2. reset player's position with finish = true
            ResetPlayer(true);
            return;
        }

        if(eventstart && jumpLocation.Count == 0)
        {
            goNextEvent();
            eventstart = false;
        }


        //if (jumpLocation.Count != 0 && jumpLocation[0] > scenes[startIndex].transform.position.x - 1.0f)
        //{
        //    // this secenes has already finished
        //    // because the first jump location is too close to the first platform
        //    // Need to go to next event
        //    goNextEvent();
        //    Debug.Log("next event");
        //}

        if (jumpOnceTimer > 0)
        {
            // cannot jump within 0.2 seconds
            jumpOnceTimer -= Time.deltaTime;
            if(jumpOnceTimer < 0)
            {
                jumpOnceTimer = 0.2f;
            }
            else
            {
                return;
            }
        }
  

        if(jumpLocation.Count == 0 && PlayerJump())
        {
            // first time jump in the entire event
            eventstart = true;
            jumpOnce = true;
            jumpLocation.Add(player.transform.position.x);
            jumpTimes++;
            return;
        }


        if(jumpTimes < jumpLocation.Count)
        {
            if (player.transform.position.x > jumpLocation[jumpTimes] && PlayerJump())
            {
                
                
                if(jumpTimes == checkPoint - 1)
                {
                    jumpLocation[jumpTimes] = player.transform.position.x;
                }
                
                jumpTimes++;
                return;
            }
        }
        else
        {
            if (PlayerJump())
            {
                
               
                jumpLocation.Add(player.transform.position.x); 
                jumpTimes++;
                return;
            }
        }

       

    }

    void startEvent()
    {
        jumpLocation.Clear();
        int start = startIndex;
        foreach (int i in platforms)
        {
            SetHeightAtLoc(start, i);
            start++;
        }
        
        tsw.WriteLine();
        tsw.Write("---------- Height: ");
        string platStr = "";
        for (int i = 0; i < platformNumber; i++)
        {
            tsw.Write(platforms[i] + " ");
            platStr += platforms[i] + " ";
        }
        tsw.Write("----------");
        tsw.WriteLine();
        tsw.Flush();

        // xml write
        xml.WriteStartElement("Event");
        xml.WriteAttributeString("platforms", platStr);
        
        xml.Flush();
    }

    void goNextEvent()
    {
        for (int i = 0; i < xmlLocation.Count; i++)
        {
            xml.WriteEndElement();
        }
        //xml.WriteEndElement();
        platforms[0]--;
        for(int i = 0; i < platformNumber; i++)
        {
            if(platforms[i] == 0)
            {
                if (i == platformNumber - 1)
                {
                    Time.timeScale = 0f;
                    break;
                }
                platforms[i + 1]--;
                platforms[i] = maxHeight;
            }
        }
        
        ResetPlayer(false);
        
        startEvent();
    }



    public void ResetPlayer(bool finish)
    {
        // reset variable of single test, not the event
        
        player.transform.position = new Vector2(0, 3);
        
        for (int i = 0; i < jumpTimes; i++){
            tsw.WriteLine("    " + jumpLocation[i]);
        }
        if (finish)
        {
            tsw.WriteLine("Result: S");
            // write xml
            int diffIndex = 0;
            for(int i = 0; i < xmlLocation.Count; i++)
            {
                if(Math.Abs(xmlLocation[i] - jumpLocation[i]) > 0.0001)
                {
                    diffIndex = i;
                    break;
                }
                else
                {
                    diffIndex = i;
                }
            }
            Debug.Log("diffIndex = " + diffIndex);
            for(int i = diffIndex; i < xmlLocation.Count - 1; i++)
            {
                xml.WriteEndElement();
            }
            for(int i = diffIndex; i < jumpLocation.Count; i++)
            {
                xml.WriteStartElement("Jump");
                xml.WriteAttributeString("order", (i + 1).ToString());
                xml.WriteAttributeString("location", jumpLocation[i].ToString("0.0000"));
                if (i == jumpLocation.Count - 1) xml.WriteEndElement();
            }
            xml.Flush();
            xmlLocation.Clear();
            xmlLocation = new List<float>(jumpLocation);
        }
        else
        {
            tsw.WriteLine("Result: F");
        }
        tsw.Flush();
        checkPoint = jumpTimes;
        //Debug.Log("jumpTimes: " + jumpTimes);
        if (jumpTimes < jumpLocation.Count)
        {
            //Debug.Log("jump times < jumpLocation.Count");
            jumpLocation.RemoveRange(jumpTimes, jumpLocation.Count - jumpTimes);
        }
        jumpTimes = 0;
        
    }

    // if player can jump, return true
    bool PlayerJump()
    {
        return player.GetComponent<PlatformerCharacter2D>().Move(1.0f, false, true);
    }

    // set the platform to height at location
    void SetHeightAtLoc(int location, int height)
    {
        scenes[location].transform.localScale = new Vector2(1, height);
    }



}
 
