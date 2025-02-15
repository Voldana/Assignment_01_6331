﻿using UnityEngine;

namespace Project.Scripts.Environment
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Transform zoneCenter;
        [SerializeField] private float zoneRadius = 50f;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float bounceDistance = 1f;

        private Vector3 direction;

        private void Start()
        {
            SetRandomDirection();
        }

        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);

            var distanceFromCenter = Vector3.Distance(transform.position, zoneCenter.position);
            if (distanceFromCenter > zoneRadius - bounceDistance)
            {
                Bounce();
            }
        }

        private void SetRandomDirection()
        {
            var randomAngle = Random.Range(0f, 360f);
            direction = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
        }

        private void Bounce()
        {
            var toCenter = (zoneCenter.position - transform.position).normalized;
            direction = Vector3.Reflect(direction, toCenter);
        }

        private void OnDrawGizmos()
        {
            if (!zoneCenter) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(zoneCenter.position, zoneRadius);
        }
    }
}