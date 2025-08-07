using UnityEngine;

namespace Navigation
{
    public class NavigationMarker : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 180f;

        private void Update()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}