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
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        _renderer.flipX = h < 0;
        _anim.SetFloat("DirectionX", h);
        _anim.SetFloat("DirectionY", v);
        _anim.SetBool("Walk", !Mathf.Approximately(h, 0) || !Mathf.Approximately(v, 0));
    }
}
