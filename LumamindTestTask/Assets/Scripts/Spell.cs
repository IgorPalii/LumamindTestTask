using UnityEngine;

public class Spell : MonoBehaviour
{
    private const int MIN_FLIP_ANGLE = 90, MAX_FLIP_ANGLE = 270, MAX_ANGLE = 360;
    public const string SPELL_ENDS_ANIM_NAME = "SpellEnds";
    [SerializeField]
    private SpriteRenderer sRenderer;
    [SerializeField]
    private Animator animator;
    public Animator Animator { get { return animator; } }

    public Transform target { set; private get; }

    public bool hitTarget { get; private set; }
    public int damage { get; private set; }

    private void Start()
    {
        damage = 10;
        hitTarget = false;
    }

    private void Update()
    {
        var normalizedAngle = transform.localEulerAngles.z % MAX_ANGLE;

        sRenderer.flipY = (normalizedAngle > MIN_FLIP_ANGLE && normalizedAngle < MAX_FLIP_ANGLE) 
            || (normalizedAngle < -MIN_FLIP_ANGLE && normalizedAngle > -MAX_FLIP_ANGLE);

        if (target != null) 
        { 
            transform.right = target.transform.position - transform.position;
            transform.localScale = new Vector2(Vector2.Distance(target.transform.position, transform.position) / 4.2f, transform.localScale.y);
        }
        else
        {
            animator.SetTrigger(SPELL_ENDS_ANIM_NAME);
        }
    }

    public void DestroySpell() //animation event, triggers when "SpellEnd" animation ends
    {
        Destroy(gameObject);
    }

    public void SpellHitTheTarget() //animation event, triggers when "SpellLoop" animation start playing
    {
        hitTarget = true;
    }
}
