using UnityEngine;

public class GiveHealth : MonoBehaviour
{
    public GameObject healingEffect;
    public int healthToGive;
    public bool targetAll = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (targetAll || other.CompareTag("Player"))
        {
            var targetHealth = other.GetComponent<Health>();

            targetHealth.GiveHealth(healthToGive, gameObject);
            Instantiate(healingEffect, transform.position, transform.rotation);

            gameObject.SetActive(false);
        }
    }
}



//using UnityEngine;

//public class GiveHealth : MonoBehaviour, IPlayerRespawnListener
//{
//    public GameObject Effect;
//    public int HealthToGive;

//    public void OnTriggerEnter2D(Collider2D other)
//    {
//        var player = other.GetComponent<Player>();
//        if(player == null)
//        {
//            return;
//        }

//        player.GiveHealth(HealthToGive, gameObject);
//        Instantiate(Effect, transform.position, transform.rotation);

//        gameObject.SetActive(false);
//    }

//    public void OnPlayerRespoanInThisCheckpoint(Checkpoint checkpoint, Player player)
//    {
//        gameObject.SetActive(true);
//    }
//}
