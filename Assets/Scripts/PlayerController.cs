using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Variable Declaration
    private float zInput;
    private float xInput;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    public Animator animator;
    private AudioSource playerAudio;
    public AudioClip attackSound;
    public AudioClip damageSound;

    private RaycastHit hit;
    private Ray ray;
    private EnemyAI enemy;
    private int enemiesDefeated;
    private int questGoal = 3;

    public TextMeshProUGUI enemyDefeatedTxt;
    public TextMeshProUGUI healthTxt;
    public GameObject gameOverMenu;

    //Stat variables
    private int healthPoints = 20;
    private int attackPoints = 5;
    private float attackRange = 3f;
    private float attackCooldown = 1f;
    private float timePassed;
    private int level;
    private int experience;

    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Input*/
        zInput = Input.GetAxis("Vertical");
        xInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", zInput);

        /*Movement*/
        transform.Translate(Vector3.forward * zInput * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, xInput * rotateSpeed * Time.deltaTime);

        /*Target Objects*/
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log("Hit " + objectHit.name);

            //Determine target object type
            switch (objectHit.tag)
            {
                case "Enemy":
                    enemy = objectHit.gameObject.GetComponent<EnemyAI>();
                    enemy.isTargeted = true;
                    break;
                case "NPC":
                case "Evironment":
                    enemy.isTargeted = false;
                    break;
                default:
                    break;
            }
        }

        /*Attack Input*/
        if(Input.GetKeyDown(KeyCode.Alpha1) && enemy)
        {
            if(Time.time - timePassed >= attackCooldown)
            {
                Attack();
                timePassed = Time.time;
            }
            else
            {
                Debug.Log("Attack is on Cooldown!");
            }
        }

        /*Attack Animation*/
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            animator.SetBool("isAttacking", false);
        }

        enemyDefeatedTxt.text = "Kill Three Goblins (" + enemiesDefeated + "/" + questGoal + ")";
        if(enemiesDefeated == questGoal)
        {
            enemyDefeatedTxt.gameObject.SetActive(false);
        }

        /*Interaction Input*/
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Interact (get NPC quest)
        }

        /*Health Update*/
        healthTxt.text = "Health: " + healthPoints;

        /*Player Death*/
        if(healthPoints == 0)
        {
            Debug.Log("Player is Defeated!");
            gameOverMenu.SetActive(true);
        }
    }

    private void Attack()
    {
        if(Mathf.Abs(transform.position.magnitude - enemy.transform.position.magnitude) <= attackRange)
        {
            animator.SetBool("isAttacking", true);
            playerAudio.PlayOneShot(attackSound, 1f);
            enemy.TakeDamage(attackPoints);
        }
        else
        {
            Debug.Log("Attack is out of range!");
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        playerAudio.PlayOneShot(damageSound, 1f);
        Debug.Log(name + " took " + damage + " damage!");
    }

    public void SetEnemiesDefeated(int defeatedEnemies)
    {
        enemiesDefeated += defeatedEnemies;
    }
}
