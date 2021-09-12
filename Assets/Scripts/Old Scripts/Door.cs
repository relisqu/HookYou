using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Old_Scripts
{
    public class Door : MonoBehaviour
    {
        public GameObject closeDoorObject;
        public GameObject openDoorObject;
        public LevelManager manager;
        [HideInInspector] public DoorData data;
        public Sprite[] doorSprites;
        public SpriteRenderer openDoorRenderer;
        public SpriteRenderer closeDoorRenderer;
        private bool isCurrentlyOpened;

        private void Start()
        {
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            print(isCurrentlyOpened);
            if (!isCurrentlyOpened || !other.gameObject.TryGetComponent(out Player player)) return;
            switch (data.teleportType)
            {
                case DoorTeleportType.AnotherRoom:
                {
                    var nextRoom = GetLevel(data.toLevelIndex);
                    var ourRoom = GetLevel(data.fromLevelIndex);

                    player.Teleport();
                    player.transform.position = data.newPlayerPosition;
                    //player.currentRoomIndex = data.toLevelIndex;
                    print(nextRoom);
                    print(ourRoom);
                    nextRoom.SetActive(true);
                    ourRoom.SetActive(false);
                    break;
                }
                case DoorTeleportType.NewScene:
                    SceneManager.LoadScene(data.nextScene);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(data.newPlayerPosition, 0.2f);
        }

        public GameObject GenerateDoor()
        {
            var doorObj = Instantiate(gameObject, data.doorPosition, Quaternion.identity);
            var closeDoorObj = Instantiate(closeDoorObject, data.doorPosition, Quaternion.identity);
            var openDoorObj = Instantiate(openDoorObject, data.doorPosition, Quaternion.identity);
            closeDoorObj.transform.parent = doorObj.transform;
            openDoorObj.transform.parent = doorObj.transform;
            SetDoorOpened(isCurrentlyOpened);
            return doorObj;
        }

        public void SetDoorOpened(bool val)
        {
            if (!data.isAlwaysOpened)
            {
                isCurrentlyOpened = val;
                if (isCurrentlyOpened)
                {
                    openDoorObject.gameObject.SetActive(true);
                    closeDoorObject.gameObject.SetActive(false);
                }
                else
                {
                    openDoorObject.gameObject.SetActive(false);
                    closeDoorObject.gameObject.SetActive(true);
                }
            }
            else
            {
                isCurrentlyOpened = true;
                closeDoorObject.gameObject.SetActive(false);
                openDoorObject.gameObject.SetActive(true);
            }
        }

        public void SetStats(DoorType type)
        {
            switch (type)
            {
                case DoorType.Left:
                    closeDoorRenderer.sprite = doorSprites[1];
                    openDoorRenderer.sprite = doorSprites[0];
                    break;
                case DoorType.Right:
                    closeDoorRenderer.sprite = doorSprites[3];
                    openDoorRenderer.sprite = doorSprites[2];
                    break;
                case DoorType.Vertical:
                    closeDoorRenderer.sprite = doorSprites[4];
                    openDoorRenderer.sprite = doorSprites[5];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private GameObject GetLevel(int index)
        {
            foreach (var room in manager.Rooms)
                if (room.data.index == index)
                    return room.level;

            return null;
        }
    }
}