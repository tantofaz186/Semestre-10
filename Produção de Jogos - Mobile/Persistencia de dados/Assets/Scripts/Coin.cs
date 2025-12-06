using UnityEngine;


    public class Coin : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
        
        public void RotateCoin()
        {
            transform.Rotate(Vector3.forward, 100 * Time.deltaTime);
        }
        
        private void Update()
        {
            RotateCoin();
        }
        
    }
