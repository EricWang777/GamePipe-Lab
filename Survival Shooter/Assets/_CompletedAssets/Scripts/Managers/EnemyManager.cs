using UnityEngine;
using System.Xml;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;
using System;


namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
        //public PlayerHealth playerHealth;       // Reference to the player's heatlh.
        public GameObject player;
        public GameObject[] enemy;                // The enemy prefab to be spawned
        //public float spawnTime = 3f;            // How long between each spawn.
        public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
        public string xmlFile = "";
        public Text eventOverText;
        // HUDCanvas
        //public GameObject HUDCanvas;
        

        // xml related
        private XmlDocument xmlDocument;
        private XmlNodeList eventsList;
        int currentEvent = 0;
        int lastEvent;
        private float eventInterval = 2;
        private const float offset = 1.0f;

        // track the current process in the event
        private int remainingEnemies;
        private bool inEvent = false;
        private bool eventSuccess = false;
        private float eventEndUITime = 3.0f;
        private string playerName;
        private float time;
        private StreamWriter recordFile;



        // four spots for spawn
        private float[,] spots = new float[,]
        {
            {12.0f, 0f },
            {0f, -12.0f },
            {-12.0f, 0f },
            {0f, 12.0f },
        };

        void Start()
        {
            time = 0f;
            
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            //InvokeRepeating ("Spawn", spawnTime, spawnTime);
            xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/LevelXML/" + xmlFile);
            if (xmlDocument.FirstChild.Name != "Level")
            {
                Debug.LogError("Wrong level XML file");
            }
            else
            {
                eventsList = xmlDocument.GetElementsByTagName("Event");
                currentEvent = 0;
                lastEvent = eventsList.Count - 1;
                //for (int i = 0; i < eventsList.Count; i++)
                //{
                //    XmlNodeList currentEventNode = eventsList[i].ChildNodes;
                //    for (int j = 0; j < currentEventNode.Count; j++)
                //    {
                //        StartCoroutine(SpawnEnemy(currentEventNode[j]));
                //    }
                //}
                //StartLevel();
            }

            
        }

        public void StartLevel()
        {
            // TODO
            playerName = playerName + "_" + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            new FileInfo(Application.dataPath + "/DataCollection/" + playerName + ".txt").Directory.Create();
            recordFile = new StreamWriter(Application.dataPath + "/DataCollection/" + playerName + ".txt", false);
            SetUpEvent();
        }
        

        void SetUpEvent()
        {
            
            ScoreManager.currentEvent = currentEvent;
           
            

            ShowEventStartUI();
            player.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponentInChildren<PlayerShooting>().enabled = true;
            player.GetComponent<PlayerMovement>().GetComponent<Animator>().SetTrigger("Idle");
            player.GetComponent<PlayerHealth>().Reset();
            
            
        }

        void ShowEventStartUI()
        {
            Invoke("StartEvent", eventInterval);
        }

        void StartEvent()
        {
            inEvent = true;
            
            XmlNodeList currentEventNode = eventsList[currentEvent].ChildNodes;
            remainingEnemies = currentEventNode.Count;
            ScoreManager.enemyLeft = remainingEnemies;
            //for (int i = 0; i < eventsList.Count; i++)
            //{
            //    XmlNodeList currentEventNode = eventsList[i].ChildNodes;
            //    for (int j = 0; j < currentEventNode.Count; j++)
            //    {
            //        StartCoroutine(SpawnEnemy(currentEventNode[j]));
            //    }
            //}
            for (int i = 0; i < currentEventNode.Count; i++)
            {
                StartCoroutine(SpawnEnemy(currentEventNode[i]));
            }
            inEvent = true;
        }


        IEnumerator SpawnEnemy(XmlNode enemyNode)
        {
            
            int type = 0;
            int location = 0;
            float start = 0;
            float speed = 0;
            float fireCooldown = 0;
            float bulletSpeed = 0;
            int index = currentEvent;
            for(int i = 0; i < enemyNode.Attributes.Count; i++)
            {
                switch (enemyNode.Attributes[i].Name)
                {
                    case "type":
                        type = int.Parse(enemyNode.Attributes[i].Value);
                        break;
                    case "location":
                        location = int.Parse(enemyNode.Attributes[i].Value);
                        break;
                    case "start":
                        start = float.Parse(enemyNode.Attributes[i].Value);
                        break;
                    case "speed":
                        speed = float.Parse(enemyNode.Attributes[i].Value);
                        break;
                    case "fireCooldown":
                        fireCooldown = float.Parse(enemyNode.Attributes[i].Value);
                        break;
                    case "bulletSpeed":
                        bulletSpeed = float.Parse(enemyNode.Attributes[i].Value);
                        break;

                    default:
                        break;

                }
            }
            yield return new WaitForSeconds(start + offset);

            if(index != currentEvent)
            {
                // do nothing
            }
            else
            {
                //player = GameObject.FindGameObjectWithTag("Player");
                Vector3 loc = player.transform.position;

                float randomX = UnityEngine.Random.insideUnitCircle.x * 3;
                loc.x += spots[location, 0] + randomX;
                
                loc.z += spots[location, 1] + UnityEngine.Random.insideUnitCircle.x * 3;
             

                GameObject zom = enemy[type];
                zom.GetComponent<NavMeshAgent>().speed = speed;
                if (type == 1 && fireCooldown > 0)
                {
                    // zom bunny
                    zom.GetComponent<BearShooting>().fireCooldown = fireCooldown;
                    zom.GetComponent<BearShooting>().bulletSpeed = bulletSpeed;

                }
                if (type == 2 && fireCooldown > 0)
                {
                    // hellephant
                    zom.GetComponent<HellephantShooting>().fireCooldown = fireCooldown;
                    zom.GetComponent<HellephantShooting>().bulletSpeed = bulletSpeed;

                }

                Instantiate(zom, loc, spawnPoints[type].rotation);
            }
            
            
        }

        public void EnemyKilled()
        {
            remainingEnemies--;
            ScoreManager.enemyLeft = remainingEnemies;
            if(remainingEnemies <= 0)
            {
                // EventOver
                EventOver();
            }
        }
       
        public void EventOver()
        {
            if (player.GetComponent<PlayerHealth>().currentHealth > 0)
                eventSuccess = true;
            else eventSuccess = false;

            ScoreManager.score += player.GetComponent<PlayerHealth>().currentHealth;

            recordFile.WriteLine(player.GetComponent<PlayerHealth>().currentHealth + " " + (ScoreManager.score - ScoreManager.lastScore));
            ScoreManager.lastScore = ScoreManager.score;


            if (eventSuccess)
            {
                eventOverText.text = "Congradulations! You Survival.\n\nPrepare for the next Trial.";
            }
            else
            {
                eventOverText.text = "You die...\n\nPrepare for the next Trial.";
            }

            Invoke("ShowEventEndUI", 1.0f);
            inEvent = false;
        }

        void ShowEventEndUI()
        {
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }

            if(currentEvent == lastEvent)
            {
                // Game over animation
                currentEvent++;

                eventOverText.text = "All finished! Thanks for your cooperation!\nYour Score: " + ScoreManager.score;
                GameOverManager.anim.SetTrigger("GameOver");
                recordFile.Close();
                Invoke("pause", 2.5f);
                return;
            }
            else
            {
                currentEvent++;
                GameOverManager.anim.SetTrigger("GameOver");
                Invoke("SetUpEvent", eventEndUITime);
            }
            
        }

        public void setPlayerName(string _name)
        {
            this.playerName = _name;
        }

        void pause()
        {
            Time.timeScale = 0;
        }
    }
}