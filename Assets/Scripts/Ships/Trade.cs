using UnityEngine;
using System.Collections;

namespace Ships
{
    public class Trade : Base
    {
        private bool isDocked = false;

        void Update()
        {
            if (!isDocked)
            {
                Move();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Harbor"))
            {
                StartCoroutine(DockAtHarbor());
            }
        }

        private IEnumerator DockAtHarbor()
        {
            isDocked = true;
            velocity = Vector3.zero;
            yield return new WaitForSeconds(2); // Wait for unloading/loading
            isDocked = false;
            FindObjectOfType<Manager>().ShipReachedHarbor(this);
        }
    }
}