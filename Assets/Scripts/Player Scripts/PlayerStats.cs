using System;
using UnityEngine;

namespace Player_Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        private int gemsCount;
        private const string GemsPropertyString = "gemsCount";
        public static PlayerStats Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            gemsCount=PlayerPrefs.GetInt(GemsPropertyString,0);
        }
        
        public void AddGemsCount(int additionalGems)
        {
            gemsCount += additionalGems;
            SaveProperty(GemsPropertyString,gemsCount);
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