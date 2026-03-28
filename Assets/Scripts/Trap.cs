using UnityEngine;

public class Trap : MonoBehaviour
{
    public float pushForce = 15f;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            player.TakeDamage(damage);

            Vector3 pushDirection = (other.transform.position - transform.position).normalized;
            pushDirection.y = 0;

            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
