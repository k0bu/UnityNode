using Project.Networking;
using Project.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {

        const float BARREL_PIVOT_OFFSET = 90.0f;

        [Header("Data")]
        [SerializeField]
        private float speed = 2;
        [SerializeField]
        private float rotation = 60;

        [Header("Object References")]
        [SerializeField]
        private Transform barrelPivot;
        [SerializeField]
        private Transform bulletSpawnPoint;

        [Header("Class References")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        private float lastRotation;

        //Shooting
        private BulletData bulletData;
        private Cooldown shootingCooldown;

        private void Start()
        {
            shootingCooldown = new Cooldown(1);
            bulletData = new BulletData();
            bulletData.position = new Position();
            bulletData.direction = new Position();
        }

        public void Update()
        {
            if (networkIdentity.IsControlling())
            {
                CheckMovement();
                CheckAiming();
                CheckShooting();
            }
        }

        public float GetLastRotation()
        {
            return lastRotation;
        }

        public void SetRotation(float Value)
        {
            barrelPivot.rotation = Quaternion.Euler(0, 0, Value + BARREL_PIVOT_OFFSET);
        }

        private void CheckMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            transform.position += -transform.up * vertical * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -horizontal * rotation * Time.deltaTime));
        }

        private void CheckAiming()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = mousePosition - transform.position;
            diff.Normalize();
            float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            lastRotation = rot;

            barrelPivot.rotation = Quaternion.Euler(0, 0, rot + BARREL_PIVOT_OFFSET);

        }

        private void CheckShooting()
        {
            shootingCooldown.CoolDownUpdate();

            if (Input.GetMouseButton(0) && !shootingCooldown.IsOnCoolDown() )
            {
                shootingCooldown.StartCoolDown();

                //Define Bullet
                bulletData.position.x = bulletSpawnPoint.position.x.TwoDecimals();
                bulletData.position.y = bulletSpawnPoint.position.y.TwoDecimals();
                bulletData.direction.x = bulletSpawnPoint.up.x;
                bulletData.direction.y = bulletSpawnPoint.up.y;

                //Send Bullet
                networkIdentity.GetSocket().Emit("fireBullet", new JSONObject(JsonUtility.ToJson(bulletData) ) );

            }
        }
    }
}

