using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingGameObject : MonoBehaviour
{
    [SerializeField] private bool _facingRight;
    public void FlipRight() {
        if (!_facingRight) {
            Flip();
        }
    }
    public void FlipLeft() {
        if (_facingRight) {
            Flip();
        }
    }
    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
        _facingRight = !_facingRight;
    }
}
