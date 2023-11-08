using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : StaticInstance<WorldGenerateManager>
{
    public static event Action EatFruit;

    [SerializeField] private GameObject[] iconList;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private float HungryRate = 0.1f;

    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject throwFruitPrefab;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject EndUI;
    [SerializeField] private TMP_Text EndText;

    private Dictionary<int, Fruits> itemList;
    private Dictionary<Fruits, int> fruitNumList;

    private List<FruitPrefab> fruitList;

    public Slider HealthSlider { get => healthSlider; set => healthSlider = value; }

    private bool isMove = false;

    void Start()
    {
        Fullup.OnFullup += FullupHealthBar;



        HealthSlider.value = 1;

        PlayerController.StoreFruit += UpdateInventory;
        PlayerController.GiveFirstFruit += GiveFirstFruit;
        PlayerController.GiveSecondFruit += GiveSecondFruit;
        PlayerController.ThrowFirstFruit += ThrowFirstFruit;
        PlayerController.ThrowSecondFruit += ThrowSecondFruit;
        FruitPrefab.DecayFruit += RemoveInventory;
        foreach (GameObject gameObject in iconList)
        {
            gameObject.SetActive(false);
        }

        itemList = new Dictionary<int, Fruits>();
        fruitNumList = new Dictionary<Fruits, int>();
        fruitList = new List<FruitPrefab>();
    }


    void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            return;
        }
        scoreText.text = GameManager.Instance.KarmaScore.ToString();
        if (HealthSlider.value > 0)
        {
            HealthSlider.value -= Time.deltaTime * HungryRate;
        }
        else
        {
            HealthSlider.value = 0;
            // end
            EndUI.SetActive(true);
            EndText.text = $"Your Score is: {GameManager.Instance.KarmaScore}";
            GameManager.Instance.isPaulsed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (itemList.ContainsKey(0))
            {
                if (!player.CanGive)
                {
                    if (itemList[0].FruitName == "Banana")
                    {
                        itemList[0].SpecialAbility.DoAbility();
                        FruitPrefab fruit = null;
                        for (int i = 0; i < fruitList.Count; i++)
                        {
                            if (fruitList[i].Fruit.FruitName == itemList[0].FruitName)
                            {
                                fruit = fruitList[i];
                                fruitList.RemoveAt(i);
                                break;
                            }
                        }
                        EatFruit?.Invoke();
                        Destroy(fruit.gameObject);
                        RemoveInventoryFruit(itemList[0], false);
                    }
                }
            }        
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (itemList.ContainsKey(1))
            {
                if (!player.CanGive)
                {
                    if (itemList[1].FruitName == "Banana")
                    {
                        itemList[1].SpecialAbility.DoAbility();
                        FruitPrefab fruit = new FruitPrefab();
                        for (int i = 0; i < fruitList.Count; i++)
                        {
                            if (fruitList[i].Fruit.FruitName == itemList[1].FruitName)
                            {
                                fruit = fruitList[i];
                                fruitList.RemoveAt(i);
                                break;
                            }
                        }
                        EatFruit?.Invoke();
                        Destroy(fruit.gameObject);
                        RemoveInventoryFruit(itemList[1], false);
                    }
                }
            }
        }
    }


    private void OnDisable()
    {
        PlayerController.StoreFruit -= UpdateInventory;
        FruitPrefab.DecayFruit -= RemoveInventory;
        PlayerController.StoreFruit -= UpdateInventory;
        PlayerController.GiveFirstFruit -= GiveFirstFruit;
        Fullup.OnFullup -= FullupHealthBar;
        PlayerController.ThrowFirstFruit -= ThrowFirstFruit;
        PlayerController.ThrowSecondFruit -= ThrowSecondFruit;
    }

    private void UpdateInventory(FruitPrefab fruit)
    {

        for (int i = 0; i < iconList.Length; i++)
        {
            if (iconList[i].activeInHierarchy)
            {
                if (itemList[i].FruitName == fruit.Fruit.FruitName)
                //if (fruitList[i].Fruit.FruitName == fruit.Fruit.FruitName)
                {
                    // increase num
                    fruitNumList[fruit.Fruit] += 1;                    
                    iconList[i].GetComponentInChildren<TMP_Text>().text = fruitNumList[fruit.Fruit].ToString();
                    fruitList.Add(fruit);
                    break;
                }
                continue;
            }

            if (!fruitNumList.TryGetValue(fruit.Fruit, out int found))
            {
                iconList[i].SetActive(true);
                itemList.Add(i, fruit.Fruit);
                fruitList.Add(fruit);
                //fruitList.Add(i, fruit);
                fruitNumList.Add(fruit.Fruit, 1);
                iconList[i].GetComponent<Image>().sprite = fruit.Fruit.FruitImage;
            }
            else
            {
                fruitNumList[fruit.Fruit] += 1;
                fruitList.Add(fruit);
                var count = itemList.FirstOrDefault(x => x.Value == fruit.Fruit).Key;
                iconList[count].GetComponentInChildren<TMP_Text>().text = fruitNumList[fruit.Fruit].ToString();
            }
            break;                       
        }
    }


    private void RemoveInventory(FruitPrefab fruit)
    {
        fruitList.Remove(fruit);
        for (int i = 0; i < iconList.Length; i++)
        {
            if (iconList[i].activeInHierarchy)
            {
                if (itemList[i].FruitName == fruit.Fruit.FruitName)
                {
                    // decrease num
                    if (fruitNumList[fruit.Fruit] > 1)
                    {
                        fruitNumList[fruit.Fruit] -= 1;
                        iconList[i].GetComponentInChildren<TMP_Text>().text = fruitNumList[fruit.Fruit].ToString();
                        break;
                    }
                    else
                    {
                        itemList.Remove(i);
                        fruitNumList.Remove(fruit.Fruit);
                        iconList[i].GetComponent<Image>().sprite = null;
                        iconList[i].SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    private void RemoveInventoryFruit(Fruits fruit, bool give = true)
    {
        for (int i = 0; i < iconList.Length; i++)
        {
            if (iconList[i].activeInHierarchy)
            {
                if (itemList[i].FruitName == fruit.FruitName)
                {
                    // decrease num
                    if (fruitNumList[fruit] > 1)
                    {
                        fruitNumList[fruit] -= 1;
                        iconList[i].GetComponentInChildren<TMP_Text>().text = fruitNumList[fruit].ToString();
                        break;
                    }
                    else
                    {
                        itemList.Remove(i);
                        fruitNumList.Remove(fruit);
                        iconList[i].GetComponent<Image>().sprite = null;
                        iconList[i].SetActive(false);
                        break;
                    }
                }
            }
        }
    }



    private void FullupHealthBar()
    {
        HealthSlider.value = Mathf.Min(100, HealthSlider.value + Fullup.fullValue);
    }


    private void GiveFirstFruit(NPC interactNPC)
    {
        if (interactNPC != null)
        {
            if (itemList.ContainsKey(0))
            {
                FruitPrefab fruit = new FruitPrefab();
                for (int i = 0; i < fruitList.Count; i++)
                {
                    if (fruitList[i].Fruit.FruitName == itemList[0].FruitName)
                    {
                        fruit = fruitList[i];
                        fruitList.RemoveAt(i);
                        break;
                    }
                }

                if (fruit.FruitState == FruitPrefab.State.Green)
                {
                    GameManager.Instance.KarmaScore += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier;
                }
                else if (fruit.FruitState == FruitPrefab.State.Yellow)
                {
                    GameManager.Instance.KarmaScore += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier * 0.8f;
                }
                else
                {
                    GameManager.Instance.KarmaScore += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier * 0.6f;
                }
                Destroy(fruit.gameObject);
                RemoveInventoryFruit(itemList[0]);

                // npc smile
                StartCoroutine(MoveSmileFace(interactNPC));
            }
        }
    }



    private void GiveSecondFruit(NPC interactNPC)
    {
        if (interactNPC != null)
        {
            if (itemList.ContainsKey(1))
            {
                FruitPrefab fruit = new FruitPrefab();
                for (int i = 0; i < fruitList.Count; i++)
                {
                    if (fruitList[i].Fruit.FruitName == itemList[1].FruitName)
                    {
                        fruit = fruitList[i];
                        fruitList.RemoveAt(i);
                        break;
                    }
                }
                
                if (fruit.FruitState == FruitPrefab.State.Green)
                {
                    GameManager.Instance.KarmaScore += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier;
                }
                else if (fruit.FruitState == FruitPrefab.State.Yellow)
                {
                    GameManager.Instance.KarmaScore += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier * 0.8f;
                }
                else
                {
                    GameManager.Instance.KarmaScore += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier * 0.6f;
                }
                Destroy(fruit.gameObject);
                RemoveInventoryFruit(itemList[1]);

                StartCoroutine(MoveSmileFace(interactNPC));
            }

        }
    }


    private void ThrowFirstFruit(bool direction)
    {
        if (itemList.ContainsKey(0))
        {
            FruitPrefab fruit = new FruitPrefab();
            for (int i = 0; i < fruitList.Count; i++)
            {
                if (fruitList[i].Fruit.FruitName == itemList[0].FruitName)
                {
                    fruit = fruitList[i];
                    fruitList.RemoveAt(i);
                    break;
                }
            }
            
            // throw
            ThrowFruit(direction, itemList[0].FruitImage);
            Destroy(fruit.gameObject);
            RemoveInventoryFruit(itemList[0]);
        }
    }



    private void ThrowSecondFruit(bool direction)
    { 
        if (itemList.ContainsKey(1))
        {
            FruitPrefab fruit = new FruitPrefab();
            for (int i = 0; i < fruitList.Count; i++)
            {
                if (fruitList[i].Fruit.FruitName == itemList[1].FruitName)
                {
                    fruit = fruitList[i];
                    fruitList.RemoveAt(i);
                    break;
                }
            }            
            ThrowFruit(direction, itemList[1].FruitImage);
            Destroy(fruit.gameObject);
            RemoveInventoryFruit(itemList[1]);
        }
    }

    private void ThrowFruit(bool direction, Sprite sprite)
    {
        var fruit = Instantiate(throwFruitPrefab, player.transform.position, Quaternion.identity);
        fruit.GetComponent<SpriteRenderer>().sprite = sprite;
        //if (sprite.name == "Soppana_sprites_ex_0")
        //{
        //    fruit.transform.localScale = new Vector2(0.2f, 0.2f);
        //}
        if (direction)
        {
            fruit.GetComponent<ThrowFruit>().Direction = player.transform.position + new Vector3(10.0f, 0.0f, 0.0f);
        }
        else
        {
            fruit.GetComponent<ThrowFruit>().Direction = player.transform.position + new Vector3(-10.0f, 0.0f, 0.0f);
        }

        fruit.GetComponent<ThrowFruit>().CanMove = true;
    }




    IEnumerator MoveSmileFace(NPC interactNPC)
    {
        if (isMove == false)
        {
            var face = interactNPC.transform.GetChild(0);
            face.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            face.gameObject.SetActive(false);
        }
    }

}
