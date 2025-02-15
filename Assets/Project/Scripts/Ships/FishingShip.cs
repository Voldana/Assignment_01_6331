using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Ships
{
    public class FishingShip : MonoBehaviour
    {
        [SerializeField] private Transform fishingZoneCenter;
        [SerializeField] private float fishingZoneRadius = 40f;
        [SerializeField] private float speed = 4f;
        [SerializeField] private float fishingDuration = 5f;
        [SerializeField] private List<Transform> harbors;

        private Vector3 currentTarget;
        private bool isFishing = false;
        private bool returningToHarbor = false;

        private void Start()
        {
            SetNewFishingTarget();
        }

        private void Update()
        {
            if (isFishing) return;

            MoveToTarget();

            if (!(Vector3.Distance(transform.position, currentTarget) < 1f)) return;
            if (returningToHarbor)
            {
                returningToHarbor = false;
                SetNewFishingTarget();
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
            transform.position += direction * (speed * Time.deltaTime);
        }

        private void SetNewFishingTarget()
        {
            var randomAngle = Random.Range(0f, 360f);
            var offset = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * Random.Range(5f, fishingZoneRadius - 5f);
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
                var distance = Vector3.Distance(transform.position, harbor.position);
                if (!(distance < shortestDistance)) continue;
                shortestDistance = distance;
                closestHarbor = harbor;
            }

            if (!closestHarbor) return;
            currentTarget = closestHarbor.position;
            returningToHarbor = true;
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
