using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace UnityStandardAssets._2D
{

    public class Restarter : MonoBehaviour
    {
        public AudioSource deadsong;
        public GameObject gm;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                //deadsong.Play();
                //other.gameObject.SetActive(false);


            }
        }

        IEnumerator Example()
        {
            yield return new WaitForSeconds(0.6f);

            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }

}