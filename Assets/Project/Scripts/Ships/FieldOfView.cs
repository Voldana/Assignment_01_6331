using System;
using UnityEngine;

namespace Project.Scripts.Ships
{
    public class FieldOfView : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.tag);
        }
    }
}