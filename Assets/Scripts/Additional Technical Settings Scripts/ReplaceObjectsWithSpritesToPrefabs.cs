using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class ReplaceObjectsWithSpritesToPrefabs : MonoBehaviour
    {
        public GameObject ReplaceObject;
        public Sprite ReplacedSprite;

        [Button]
        public void Replace()
        {
            var objects = GameObject.FindObjectsOfType<SpriteRenderer>();
            var list = new List<SpriteRenderer>();
            foreach (var obj in objects)
            {
                if (obj.sprite == ReplacedSprite)
                {
                    list.Add(obj);
                }
            }

            foreach (var sprite in list)
            {
                var transform1 = sprite.transform;
                //var obj=PrefabUtility.InstantiatePrefab(ReplaceObject) as GameObject;
               //obj.transform.position = transform1.position;
                //obj.transform.rotation = transform1.rotation;
                //obj.transform.parent = transform1.parent;
                                

                DestroyImmediate(sprite.gameObject);
            }
        }
    }
}