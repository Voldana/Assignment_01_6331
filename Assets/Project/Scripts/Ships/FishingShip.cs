using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Environment;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.Ships
{
    public class FishingShip : MonoBehaviour
    {
        [Inject] private List<Harbor> harbors;
        [Inject] private SignalBus signalBus;

        [SerializeField] private float fishingZoneRadius = 40f;
        [SerializeField] private Transform fishingZoneCenter;
        [SerializeField] private float fishingDuration = 5f;
        [SerializeField] private float speed = 4f;

        private bool returningToHarbor;
        private Vector3 currentTarget;
        private bool isFishing;

        private void Start()
        {
            SetNewFishingTarget();
        }

        private void Update()
        {
            if (isFishing) return;

            MoveToTarget();

            if (!(Vector3.Distance(transform.position, currentTarget) < 5f)) return;

            if (returningToHarbor)
            {
                returningToHarbor = false;
                currentTarget = Vector3.zero;
                Invoke(nameof(SetNewFishingTarget), 0.5f);
            }
            else
            {
                StartCoroutine(Fish());
            }
        }


        private void MoveToTarget()
        {
            var direction = (currentTarget - transform.position).normalized;
            direction.y = 0;
            transform.rotation =
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
            transform.position += direction * (speed * Time.deltaTime);
        }

        private void SetNewFishingTarget()
        {
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) *
                         Random.Range(5f, fishingZoneRadius - 5f);
            currentTarget = fishingZoneCenter.position + offset;
        }

        private IEnumerator Fish()
        {
            isFishing = true;
            yield return new WaitForSeconds(fishingDuration);
            isFishing = false;
            MoveToClosestHarbor();
        }

        private void MoveToClosestHarbor()
        {
            Transform closestHarbor = null;
            var shortestDistance = Mathf.Infinity;

            foreach (var harbor in harbors)
            {
                var distance = Vector3.Distance(transform.position, harbor.GetDockingPosition().position);
                if (!(distance < shortestDistance)) continue;
                shortestDistance = distance;
                closestHarbor = harbor.GetDockingPosition();
            }

            if (!closestHarbor) return;
            currentTarget = closestHarbor.position;
            returningToHarbor = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Trading") && !other.gameObject.CompareTag("Pirate") &&
                !other.gameObject.CompareTag("Player")) return;
            signalBus.Fire(new GameEvents.OnCollision { collided = other.gameObject });
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if (fishingZoneCenter != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(fishingZoneCenter.position, fishingZoneRadius);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget, 1f);
        }
    }
}