using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int DamageToGive = 10;
    public bool targetAll = false;
    public bool instantKill = false;

    private Vector2 _lastPosition;
    private Vector2 _velocity;

    public void LateUpdate()
    {
        //_velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //CharacterController2D controller;
        Health health = other.GetComponent<Health>();

        if (health == null)
            return;
         
        if (instantKill)
        {
            health.InstantKill();
            return;
        }

        if (other.CompareTag("Player"))
        {
            health.TakeDamage(DamageToGive, gameObject);
            // Do other things?
        }
        else if (targetAll)
        {
            health.TakeDamage(DamageToGive, gameObject);
        }

        //knockback

        //rb = other.GetComponent<Rigidbody>();
        //var totalVelocity = controller.Velocity + _velocity;
        //rb.SetForce(new Vector2(
        //    -1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 6, 10, 40),
        //    -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 6, 5, 30)));
    }
}

