using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;

public class PlayerCollisionsController : MonoBehaviour
{
    public List<GameObject> CollectedWood = new List<GameObject>();
    
    [SerializeField] private Transform woodStackPos;
    [SerializeField] private Transform woodStackUnderPos;
    [SerializeField] private GameObject wood;
    
    public float heightOffset = .3f;
    public float underOffset = 1f;
    private float currentHeight;
    private float currentUnder;
   
    private GameObject newWood;

    private Sequence _sequence;
    private bool jumpForward = false;

    private Rigidbody playerBody;
    private PlayerMovement playerMovement;

    private Animator animator;

    public LayerMask GroundLayer;
    public GameObject GameOverPanel;
    
    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collectableWood = other.GetComponent<CollectableWood>();
        if (collectableWood)
        {
            Destroy(other.gameObject);
            StackSpawnWood();
        }

        var sea = other.GetComponent<Sea>();
        if (sea)
        {
            InvokeRepeating("UnderSpawnWood",0f,.1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var sea = other.GetComponent<Sea>();
        if (sea && CollectedWood.Count == 0)
        {
            JumpForward();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
        currentUnder -= underOffset;
    }


    private void StackSpawnWood()
    {
       newWood =  Instantiate(wood, woodStackPos);
       newWood.transform.localPosition =  new Vector3(0, currentHeight, 0);
       currentHeight += heightOffset;
       CollectedWood.Add(newWood);
    }

    private void UnderSpawnWood()
    {
        if (CollectedWood.Count>0)
        {
            GameObject newUnderWood = Instantiate(wood, woodStackUnderPos);
            newUnderWood.transform.localPosition = new Vector3(0, 0, currentUnder);
            currentUnder += underOffset;
            Destroy(CollectedWood[CollectedWood.Count -1]);
            CollectedWood.RemoveAt(CollectedWood.Count -1);
            currentHeight -= heightOffset;
            newUnderWood.transform.parent = null;
        }
    }

    private void JumpForward()
    {
        if (!jumpForward)
        {
            StartCoroutine(EnablePhysics());
            _sequence = DOTween.Sequence();
            animator.SetTrigger("Jump");
            _sequence.Append(transform.DOMove(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 2f), .5f));
            _sequence.Append(
                transform.DOMove(new Vector3(transform.position.x, 0f, transform.position.z +4f),
                    .5f));
        }
        animator.SetTrigger("JumpOver");
        jumpForward = true;
        StartCoroutine(JumpFalse());
        StartCoroutine(DisablePhysics());
    }


    private IEnumerator JumpFalse()
    {
        yield return new WaitForSeconds(4f);
        jumpForward = false;
    }

    private IEnumerator EnablePhysics()
    {
        yield return new WaitForSeconds(.5f);
        playerBody.isKinematic = false;
        playerMovement.enabled = false;
    }
    
    private IEnumerator DisablePhysics()
    {
        yield return new WaitForSeconds(2f);
        playerBody.isKinematic = true;
        playerMovement.enabled = true;
    }

    private float timer;
    private void Update()
    {
        RaycastHit hit;
        if (!Physics.SphereCast(transform.position+new Vector3(0,1,0),.1f,-transform.up,out hit,100,GroundLayer))
        {
            timer += Time.deltaTime;
            if (timer>=0.1f)
            {
                UnderSpawnWood();
                timer = 0;
            }
            
            if (CollectedWood.Count<=0)
            {
                JumpForward();
            }

            if (jumpForward &&  !Physics.SphereCast(transform.position+new Vector3(0,1,4),.1f,-transform.up,out hit,100,GroundLayer))
            {
                Debug.Log("Game Over");
                GameOverPanel.SetActive(true);
            }
        }
    }
}
