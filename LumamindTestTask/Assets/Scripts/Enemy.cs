using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    private const float MOVE_PAUSE_DELAY = 1f, MAX_MOVE_OFSET_Y = 2f, MAX_MOVE_OFSET_X = 2f, DAMAGE_FREQUENCY = 1f, DEATH_SHRINK_COEF = 3f, 
        DEATH_SPEED_COEF = 5f, MIN_DISTANCE_TO_SPELL_PARENT = 0.1f;
    private const string HEALTH_TEXT_PATTERN = "{0} hp", SPELLSPAWNER_TAG = "SpellSpawner";
    private float maxHealth = 50, health;
    public Spell boundSpell { get; set; }
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private Slider healthSlider;

    public delegate void EnemyHandler(Enemy enemy);
    public static event EnemyHandler enemyDeath;

    private void Start()
    {
        health = maxHealth;
        healthText.text = System.String.Format(HEALTH_TEXT_PATTERN, health);
        StartCoroutine(Move());
    }

    private IEnumerator TakeDamage()
    {
        while (boundSpell != null)
        {
            yield return new WaitForSeconds(DAMAGE_FREQUENCY);
            if (!boundSpell.hitTarget)
            {               
                continue;
            }
            
            health = Mathf.Clamp(health - boundSpell.damage, 0, maxHealth);
            healthText.text = System.String.Format(HEALTH_TEXT_PATTERN, health);
            healthSlider.value = health / maxHealth;

            if (health <= 0)
            {
                if (boundSpell != null)
                {
                    StartCoroutine(DeathBehaviour());
                }
                StopTakeDamage();
                healthSlider.gameObject.SetActive(false);
            }
        }        
    }

    private IEnumerator DeathBehaviour()
    {

        var spellParrentT = boundSpell.transform.parent.transform;
        boundSpell.target = null;
               
        while (Vector2.Distance(transform.position, spellParrentT.position) > MIN_DISTANCE_TO_SPELL_PARENT)
        {
            var distance = Vector2.Distance(transform.position, spellParrentT.position);
            transform.position = Vector3.MoveTowards(transform.position, spellParrentT.position, Time.deltaTime * DEATH_SPEED_COEF);
            transform.localScale = new Vector2(Mathf.Clamp01(distance / DEATH_SHRINK_COEF), Mathf.Clamp01(distance / DEATH_SHRINK_COEF));
            yield return null;
        }        
    }

    private IEnumerator Move()
    {
        var newPos = transform.position;
        while (health > 0)
        {
            if (newPos == transform.position)
            {
                yield return new WaitForSeconds(MOVE_PAUSE_DELAY);
                var x = Random.Range(transform.position.x - MAX_MOVE_OFSET_X, transform.position.x + MAX_MOVE_OFSET_X);
                var y = Random.Range(transform.position.y - MAX_MOVE_OFSET_Y, transform.position.y + MAX_MOVE_OFSET_Y);
                newPos = new Vector2(x, y);
            }            
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime);
            yield return null;
        }
    }

    public void StartTakeDamage()
    {
        StartCoroutine(TakeDamage());
    }

    public void StopTakeDamage()
    {
        StopCoroutine(TakeDamage());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(SPELLSPAWNER_TAG))
        {
            if (health <= 0)
            {
                enemyDeath?.Invoke(this);
                Destroy(gameObject);
            }            
        }
    }
}
