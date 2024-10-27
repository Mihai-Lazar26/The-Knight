using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private int _damage = 20;
    private Rigidbody2D _rb;
    [SerializeField] private float _despawnTime = 5;
    private float _despawnTimer = 0;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * _speed;
        Physics2D.IgnoreLayerCollision(10, 9);
        Physics2D.IgnoreLayerCollision(10, 8);
    }

    private void Update() {
        _despawnTimer += Time.deltaTime;
        if (_despawnTimer > _despawnTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            collider.GetComponent<EntityHealthSystem>().TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}
