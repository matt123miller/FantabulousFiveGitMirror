using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IGiveHealth, ITakeDamage
{
    //private HealthBar healthBar; //Maybe needed later.
    [SerializeField]
    private int _healthValue;
    private bool _isDead = false;

    public int maxHealth;
    public AudioClip healthGainSound;
    public AudioClip healthLossSound;
    public GameObject OuchEffect;
    public HealthBar healthBar;

    public int HealthValue
    {
        get { return _healthValue; }
        set { _healthValue = value; }
    }

    public bool IsDead 
    {
        get { return _isDead; }
        private set { _isDead = value; }
    }


    // Use this for initialization
    private void Awake()
    {
        IsDead = _isDead;
        HealthValue = maxHealth;
    }


    public void GiveHealth(int healthtogive, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(healthGainSound, transform.position);
        _healthValue = Mathf.Min(_healthValue + healthtogive, maxHealth); // returns the smallest of the 2 arguments
    }



    public void TakeDamage(int damage, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(healthLossSound, transform.position);
        Instantiate(OuchEffect, transform.position, transform.rotation);

        _healthValue -= damage;

        print("I have been hit and have " + _healthValue + " health remaining");

        if (_healthValue <= 0)
        {
            IsDead = true;
            print("I am dead :(");
            Kill();
        }
        else if (healthBar != null)
        {
            healthBar.UpdateBar(_healthValue, maxHealth);
        }
    }

    //should only be used internally. There is public void InstantKill() below if necessary
    private void Kill()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        IsDead = true;
        _healthValue = 0;
    }

    //currently untested
    public void InstantKill()
    {
        //For killing players
        if (gameObject.CompareTag("Player"))
        {
            //Which method to use? 
            //GetComponent<Player>().Kill();
            // Do other things?
            return;
        }

        //For killing non player objects who have health.
        Kill();
    }
}

