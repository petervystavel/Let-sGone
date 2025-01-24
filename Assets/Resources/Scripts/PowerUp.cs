using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Enemy.Type ProjectileType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player == null)
                return;

            player.SetProjectilePossessed(ProjectileType);
            Destroy(gameObject);
        }
    }
}