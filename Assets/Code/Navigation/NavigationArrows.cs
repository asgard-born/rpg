using UnityEngine;

namespace Navigation
{
    public class NavigationArrows : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 180f;
        [SerializeField] private float lifetime = 1f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
}