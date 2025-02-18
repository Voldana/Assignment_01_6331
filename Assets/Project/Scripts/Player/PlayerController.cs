using Project.Scripts.Environment;
using Project.Scripts.Ships;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Inject] private SignalBus signalBus;
        
        [SerializeField] private Company.CompanyName company;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float turnSpeed = 100f;
        [SerializeField] private Harbor harbor;

        private bool isUnloading;
        private int score;
        private Vector3 velocity;

        private void Start()
        {
            Subscribe();
        }



        private void Subscribe()
        {
            signalBus.Subscribe<GameEvents.OnCollision>(OnCollision);
        }

        private void OnCollision(GameEvents.OnCollision signal)
        {
            if (!signal.collided.Equals(gameObject)) return;
            OnCapture();
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            velocity = transform.forward * (vertical * moveSpeed * Time.deltaTime);
            transform.position += velocity;

            transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void OnCapture()
        {
            var respawn = harbor.GetRespawnPosition().position;
            transform.position = new Vector3(respawn.x, transform.position.y, respawn.z);
            signalBus.Fire(new GameEvents.OnScoreChange { company = company, score = -10 });
        }

        private void OnTriggerEnter(Collider other)
        {
           
        }
    }
}