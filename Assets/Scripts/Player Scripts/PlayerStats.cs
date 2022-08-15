using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Player_Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        private int gemsCount;
        private const string GemsPropertyString = "gemsCount";
        public static PlayerStats Instance;
        public GameObject GameScreen;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            AudioManager.instance.Play("main_music");
            gemsCount = PlayerPrefs.GetInt(GemsPropertyString, 0);
            StartCoroutine(RandomWind());
        }


        IEnumerator RandomWind()
        {
            while (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(Random.Range(15, 40));
                AudioManager.instance.Play("bird");
                yield return new WaitForSeconds(Random.Range(5, 20));
                AudioManager.instance.Play("wind");
                yield return new WaitForSeconds(Random.Range(15, 40));
                AudioManager.instance.Play("bird");
            }

            yield return null;
        }

        void Update()
        {
            if (Input.GetKeyDown("space") && _won)
            {
                SceneManager.LoadScene("FirstLocation");
                SaveProperty(GemsPropertyString, 0);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("FirstLocation");
                SaveProperty(GemsPropertyString, 0);
            }
        }

        private bool _won;

        public void AddGemsCount(int additionalGems)
        {
            gemsCount += additionalGems;
            if (gemsCount >= 12)
            {
                GameScreen.SetActive(true);
                _won = true;
            }

            SaveProperty(GemsPropertyString, gemsCount);
        }

        public void ChangeGemsCount(int newGems)
        {
            gemsCount = newGems;
            SaveProperty(GemsPropertyString, gemsCount);
        }

        public int GetGemsCount()
        {
            return gemsCount;
        }

        public void SaveProperty(string property, int count)
        {
            PlayerPrefs.SetInt(property, count);
        }
    }
}