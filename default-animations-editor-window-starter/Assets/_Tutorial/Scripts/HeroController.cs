using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    SpriteRenderer _renderer;
    Animator _anim;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        _renderer.flipX = direction.x < 0;
        _anim.SetFloat("DirectionX", direction.x);
        _anim.SetFloat("DirectionY", direction.x == 0 ? direction.y : 0);
        _anim.SetBool("Walk", direction.magnitude > 0);
    }
}
