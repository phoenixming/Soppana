using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static event Action<FruitPrefab> StoreFruit;
    public static event Action<NPC> GiveFirstFruit;
    public static event Action<NPC> GiveSecondFruit;
    public static event Action<bool> ThrowFirstFruit;
    public static event Action<bool> ThrowSecondFruit;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 movedirection;
    private bool canStore = false;
    private bool canGive = false;

    private FruitPrefab fruit;

    private NPC interactNPC = null;

    private bool walking = false;
    private Vector3 moveToPosition = Vector3.zero;
    [SerializeField] private int gridLength = 1;
    [SerializeField] private int worldSize = 50;

    private bool faceRight = true;

    public bool CanGive { get => canGive; set => canGive = value; }

    void Start()
    {
        
    }


    void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            //rb.velocity = Vector2.zero;
            return;
        }

        if (!walking)
        {
            if (movedirection.x != 0)
            {
                movedirection.y = 0;
                if (movedirection.x != 1)
                {
                    movedirection.x = movedirection.x < 0 ? -1 : 1;
                }
            }

            if (movedirection != Vector2.zero)
            {

                if (faceRight)
                {
                    if (movedirection.x < 0)
                    {
                        this.transform.Rotate(0f, 180f, 0f);
                        faceRight = !faceRight;
                    }
                }
                else
                {
                    if (movedirection.x > 0)
                    {
                        this.transform.Rotate(0f, 180f, 0f);
                        faceRight = !faceRight;
                    }
                }

                moveToPosition = transform.position + new Vector3(movedirection.x, movedirection.y, 0) * gridLength;
                if (!(moveToPosition.x >= worldSize / 2 - 1 || moveToPosition.x <= (-1) * (worldSize / 2 - 1) || moveToPosition.y >= worldSize / 2 - 1 || moveToPosition.y <= (-1) * (worldSize / 2 - 1)))
                {
                    StartCoroutine(Move(moveToPosition));
                }
            }
        }



        if (canStore)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                fruit.IsStored = true;
                fruit.GetComponent<SpriteRenderer>().sprite = null;
                fruit.GetComponent<Collider2D>().enabled = false;
                OnStoreFruit();
            }
        }

        if (CanGive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnGiveFirstFruit();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnGiveSecondFruit();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Vector2 position = Input.mousePosition;
                //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
                OnThrowFirstFruit(faceRight);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                //Vector2 position = Input.mousePosition;
                //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
                OnThrowSecondFruit(faceRight);
            }
        }
    }

    IEnumerator Move(Vector3 newPos)
    {
        walking = true;
        while ((newPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.fixedDeltaTime);
            yield return null;
        }

        transform.position = newPos;
        walking = false;
    }

    //private void FixedUpdate()
    //{
    //    if (GameManager.Instance.isPaulsed)
    //    {
    //        return;
    //    }
    //    rb.velocity = movedirection * speed;
    //}


    public void OnStoreFruit()
    {
        StoreFruit?.Invoke(fruit);
    }

    public void OnGiveFirstFruit()
    {
        GiveFirstFruit?.Invoke(interactNPC);
    }

    public void OnGiveSecondFruit()
    {
        GiveSecondFruit?.Invoke(interactNPC);
    }

    public void OnThrowFirstFruit(bool direction)
    {
        ThrowFirstFruit?.Invoke(direction);
    }

    public void OnThrowSecondFruit(bool direction)
    {
        ThrowSecondFruit?.Invoke(direction);
    }



    private void OnMove(InputValue input)
    {
        movedirection = input.Get<Vector2>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Fruit"))
            {
                canStore = true;
                fruit = collision.GetComponent<FruitPrefab>();
            }
            else if (collision.CompareTag("NPC"))
            {
                CanGive = true;
                interactNPC = collision.gameObject.GetComponent<NPC>();
            }
            else if (collision.CompareTag("Stranger"))
            {
                CanGive = true;
                interactNPC = collision.gameObject.GetComponent<Stranger>();

            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Fruit"))
            {
                canStore = false;
            }
            else if (collision.CompareTag("NPC"))
            {
                CanGive = false;
                interactNPC = null;
            }
            else if (collision.CompareTag("Stranger"))
            {
                CanGive = false;
                interactNPC = null;
            }
        }

    }
}
