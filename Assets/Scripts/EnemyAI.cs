using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    //Variable Declarations
    [SerializeField] private int health = 10;
    [SerializeField] private int attackPoints = 4;
    [SerializeField] private float attackCooldown = 1f;
    private float timePassed;

    public GameObject targetField;
    public TextMeshPro healthTxt;

    public bool isTargeted;
    private bool isBattling;

    private GameManager gameManager;
    private Animator animator;
    private AudioSource enemyAudio;
    public AudioClip attackSound;
    public AudioClip damageSound;
    private PlayerController player;

    private Vector3 startingPosition;
    private float moveTargetX;
    private float moveTargetZ;
    private float moveRange = 3;
    private Vector3 moveTarget;
    private float rotateSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        enemyAudio = GetComponent<AudioSource>();

        startingPosition = transform.position;
        moveTargetX = Random.Range(startingPosition.x, startingPosition.x + moveRange);
        moveTargetZ = Random.Range(startingPosition.z, startingPosition.z + moveRange);
        moveTarget = new Vector3(moveTargetX, startingPosition.y, moveTargetZ);
    }

    // Update is called once per frame
    void Update()
    {
        /*Random Movement*/
        if (!isBattling)
        {
            animator.SetBool("isWalking", true);
            //Move towards target
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, Time.deltaTime);
            //Rotate towards target
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position - moveTarget, rotateSpeed * Time.deltaTime, 0.0f));
        }

        if(transform.position == moveTarget)
        {
            moveTargetX = Random.Range(startingPosition.x, startingPosition.x + moveRange);
            moveTargetZ = Random.Range(startingPosition.z, startingPosition.z + moveRange);
            moveTarget = new Vector3(moveTargetX, startingPosition.y, moveTargetZ);
        }

        /*Combat*/
        gameManager.setInCombat(isBattling);
        if(isBattling)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, transform.position - player.transform.position, rotateSpeed * Time.deltaTime, 0.0f));
        }

        if (isBattling && Time.time - timePassed >= attackCooldown)
        {
            Attack();
            timePassed = Time.time;
            animator.SetBool("isWalking", false);
            animator.SetBool("attack01", true);
        }

        if (isTargeted)
        {
            targetField.SetActive(true);
            healthTxt.gameObject.SetActive(true);
        }
        else
        {
            targetField.SetActive(false);
            healthTxt.gameObject.SetActive(false);
        }

        /*Update Health*/
        healthTxt.text = "Health " + health;

        if (health == 0)
        {
            Debug.Log(name + " has been defeated!");
            //gameObject.SetActive(false);
            player.SetEnemiesDefeated(1);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if(!isBattling)
        {
            timePassed = Time.time;
            isBattling = true;
        }
        
        health -= damage;
        StartCoroutine(DamageSound());
        Debug.Log(name + " took " + damage + " damage!");
    }

    private void Attack()
    {
        StartCoroutine(AttackSound());
        player.TakeDamage(attackPoints);
    }

    private IEnumerator DamageSound()
    {
        yield return new WaitForSeconds(0.25f);
        enemyAudio.PlayOneShot(damageSound, 1f);
        StopCoroutine(DamageSound());
    }

    private IEnumerator AttackSound()
    {
        yield return new WaitForSeconds(0.25f);
        enemyAudio.PlayOneShot(attackSound, 1f);
        StopCoroutine(AttackSound());
    }
}
