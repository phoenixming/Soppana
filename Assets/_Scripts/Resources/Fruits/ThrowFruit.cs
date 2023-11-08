using System.Collections;
using System;
using UnityEngine;

public class ThrowFruit : MonoBehaviour
{
    public static event Action ThrowFruitHit;
    [SerializeField] private float speed;

    private Vector2 direction;
    private bool canMove = false;

    private bool isStop = false;

    public bool CanMove { get => canMove; set => canMove = value; }
    public Vector2 Direction { get => direction; set => direction = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            return;
        }

        if (canMove)
        {
            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, direction, step);

            if (transform.position.x == direction.x && transform.position.y == direction.y)
            {
                isStop = true;
            }

        }

        if (isStop)
        {
            StartCoroutine(StartDestroy());
        }
    }


    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Stranger"))
            {
                GameManager.Instance.KarmaScore = GameManager.Instance.KarmaScore - 10;
                // npc sad face
                StartCoroutine(MoveSadFace(collision.GetComponent<NPC>()));
                transform.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    IEnumerator MoveSadFace(NPC interactNPC)
    {
        ThrowFruitHit?.Invoke();
        var face = interactNPC.transform.GetChild(1);
        face.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        face.gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
