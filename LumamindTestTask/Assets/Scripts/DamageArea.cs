using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private const int MAX_ATTACKABLE_ENEMIES = 5;
    private const string ENEMY_TAG = "Enemy";

    private int currentAttackedEnemies = 0;

    [SerializeField]
    private GameObject spellPref;
    [SerializeField]
    private Transform spellSpawner;

    private List<Enemy> notDamagedEnemies = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(ENEMY_TAG))
        {
            var enemy = col.GetComponent<Enemy>();
            if (currentAttackedEnemies >= MAX_ATTACKABLE_ENEMIES)
            {
                notDamagedEnemies.Add(enemy);
                return;
            }
            StartDamageEnemy(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(ENEMY_TAG))
        {
            if(col.TryGetComponent(out Enemy enemy))
            {
                if (enemy.boundSpell == null) 
                {
                    notDamagedEnemies.Remove(enemy);
                    return; 
                }
                currentAttackedEnemies--;
                enemy.boundSpell.Animator.SetTrigger(Spell.SPELL_ENDS_ANIM_NAME);
                enemy.StopTakeDamage();

                SearchNewEnemies();
            }
        }
    }

    private void SearchNewEnemies()
    {
        if (notDamagedEnemies.Count > 0)
        {
            for (int i = 0; i < notDamagedEnemies.Count; i++)
            {
                StartDamageEnemy(notDamagedEnemies[i]);

                notDamagedEnemies.RemoveAt(i);

                if (currentAttackedEnemies == MAX_ATTACKABLE_ENEMIES) return;
            }
        }
    }

    private void StartDamageEnemy(Enemy enemy)
    {
        currentAttackedEnemies++;
        var spell = Instantiate(spellPref, spellSpawner);
        var spellComponent = spell.GetComponent<Spell>();
        spellComponent.target = enemy.transform;
        enemy.boundSpell = spellComponent;
        enemy.StartTakeDamage();
    }
}
