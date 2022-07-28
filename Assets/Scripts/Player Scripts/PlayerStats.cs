using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            gemsCount=PlayerPrefs.GetInt(GemsPropertyString,0);
        }
        void Update()
        {
            if (Input.GetKeyDown("space") && _won)
            {
                SceneManager.LoadScene("FirstLocation");
            }
        }
        private bool _won;
        public void AddGemsCount(int additionalGems)
        {/*
            gemsCount += additionalGems;
            if (gemsCount >= 10)
            {
                GameScreen.SetActive(true);
                _won = true;
            }
            */
            //SaveProperty(GemsPropertyString,gemsCount);
            
            
        } 
        
        public void ChangeGemsCount(int newGems)
        {
            gemsCount = newGems;
            SaveProperty(GemsPropertyString,gemsCount);
        }
        public int GetGemsCount()
        {
            return gemsCount; 
        }
    
        public void SaveProperty(string property, int count)
        {
            PlayerPrefs.SetInt(property,count);
        }
    }
}