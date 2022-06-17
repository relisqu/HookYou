using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using UnityEngine;

namespace AI.Bosses.BatBossAttacks
{
    public class MeleeAttack : Attack
    {
        [SerializeField] private Transform HandsModule;
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private GameObject RockObject;
        [SerializeField] private float RockSpawnDistance;
        [SerializeField] private float RockCount;
        [SerializeField] private float TimeBetweenRockSpawns;
        [SerializeField] private float AngleBetweenRocks;
        [SerializeField] private float TimeBeforeAttack;

        private List<GameObject> _rockPool = new List<GameObject>();

        void Start()
        {
            Animator = GetComponentInParent<Animator>();
            Boss = GetComponentInParent<FireBoss>();
            for (int i = 0; i < RockCount*3; i++)
            {
                var rock = Instantiate(RockObject);
                rock.SetActive(false);
                _rockPool.Add(rock);
            }
        }

        public override IEnumerator StartAttack()
        {
            animationStopped = true;
            var lookRotation = (Player.transform.position - HandsModule.position).normalized;
            var directionOfMainRock =
                Quaternion.Euler(new Vector3(0f, 0f, -Vector2.SignedAngle(lookRotation, Vector3.right))).normalized *
                Vector3.right * RockSpawnDistance;
            yield return SpawnRocks(directionOfMainRock);
            yield return new WaitForSeconds(0.3f);

        }

        public IEnumerator SpawnRocks(Vector3 mainDirection)
        {
            PlayAttackAnimation();
            yield return new WaitForSeconds(TimeBeforeAttack);

            var angle = -AngleBetweenRocks * (RockCount * 0.5f);
            for (int i = 0; i < RockCount; i++)
            {
                 PlayAttackAnimation(AttackNameTrigger+"Rock");
                 SpawnGameObject(transform.position + Rotate(mainDirection, angle));
                 yield return new WaitForSeconds(TimeBetweenRockSpawns);
                 angle += AngleBetweenRocks;
            }
            PlayAttackAnimation(AttackNameTrigger+"End");
        }

        public void SpawnGameObject(Vector3 position)
        {
            var _rock = FindNonactiveRock();
            _rock.SetActive(true);
            _rock.transform.position = position;
        }


        public GameObject FindNonactiveRock()
        {
            foreach (var _rock in _rockPool)
            {
                if (!_rock.gameObject.activeInHierarchy) return _rock;
            }

            return null;
        }

        public override Attack GetCurrentAttack()
        {
            return this;
        }

        private bool animationStopped;
        private static readonly int Attack = Animator.StringToHash("Attack");

        public static Vector3 Rotate(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}