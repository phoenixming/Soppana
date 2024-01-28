using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : StaticInstance<UIManager>
{
    public static event Action EatFruit;

    [SerializeField] private GameObject[] iconList;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private float HungryRate = 0.05f;

    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject throwFruitPrefab;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject EndUI;
    [SerializeField] private TMP_Text EndText;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button instructionButton;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject instructions;

    private Dictionary<int, Fruits> itemList;
    private Dictionary<Fruits, int> fruitNumList;

    private List<FruitPrefab> fruitList;

    [SerializeField] private List<GameObject> playDataList = new(5);
    private GameObject playData = null;

    public Slider HealthSlider { get => healthSlider; set => healthSlider = value; }
    public List<FruitPrefab> FruitList { get => fruitList; set => fruitList = value; }

    private bool isMove = false;

    void Start()
    {
        Fullup.OnFullup += FullupHealthBar;

        replayButton.onClick.AddListener(GameManager.Instance.RePlay);
        pauseButton.onClick.AddListener(GameManager.Instance.Pause);
        pauseButton.onClick.AddListener(ShowPauseMenu);
        resumeButton.onClick.AddListener(GameManager.Instance.Resume);
        resumeButton.onClick.AddListener(HidePauseMenu);
        quitButton.onClick.AddListener(GameManager.Instance.Quit);
        instructionButton.onClick.AddListener(ShowInstruction);

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
        FruitList = new List<FruitPrefab>();
    }


    void Update()
    {

        if (GameManager.Instance.isPaulsed)
        {
            return;
        }
        else
        {
            if (!pauseButton.gameObject.activeInHierarchy)
            {
                pauseButton.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
            ShowPauseMenu();
        }


        scoreText.text = GameManager.Instance.karmaScore[GameManager.Instance.playTime-1].ToString();
        if (HealthSlider.value > 0)
        {
            HealthSlider.value -= Time.deltaTime * HungryRate;
        }
        else
        {
            HealthSlider.value = 0;
            // end
            EndUI.SetActive(true);
            EndText.text = $"Your Score is: {GameManager.Instance.karmaScore[GameManager.Instance.playTime-1]}";
            UpdateChart();
            

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
                        for (int i = 0; i < FruitList.Count; i++)
                        {
                            if (FruitList[i].Fruit.FruitName == itemList[0].FruitName)
                            {
                                fruit = FruitList[i];
                                FruitList.RemoveAt(i);
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
                        FruitPrefab fruit = null;
                        for (int i = 0; i < FruitList.Count; i++)
                        {
                            if (FruitList[i].Fruit.FruitName == itemList[1].FruitName)
                            {
                                fruit = FruitList[i];
                                FruitList.RemoveAt(i);
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
                    FruitList.Add(fruit);
                    break;
                }
                continue;
            }

            if (!fruitNumList.TryGetValue(fruit.Fruit, out int found))
            {
                iconList[i].SetActive(true);
                itemList.Add(i, fruit.Fruit);
                FruitList.Add(fruit);
                //fruitList.Add(i, fruit);
                fruitNumList.Add(fruit.Fruit, 1);
                iconList[i].GetComponent<Image>().sprite = fruit.Fruit.FruitImage;
            }
            else
            {
                fruitNumList[fruit.Fruit] += 1;
                FruitList.Add(fruit);
                var count = itemList.FirstOrDefault(x => x.Value == fruit.Fruit).Key;
                iconList[count].GetComponentInChildren<TMP_Text>().text = fruitNumList[fruit.Fruit].ToString();
            }
            break;                       
        }
    }


    public void RemoveInventory(FruitPrefab fruit)
    {
        FruitList.Remove(fruit);
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
                FruitPrefab fruit = null;
                for (int i = 0; i < FruitList.Count; i++)
                {
                    if (FruitList[i].Fruit.FruitName == itemList[0].FruitName)
                    {
                        fruit = FruitList[i];
                        FruitList.RemoveAt(i);
                        break;
                    }
                }

                if (fruit.FruitState == FruitPrefab.State.Green)
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier;
                }
                else if (fruit.FruitState == FruitPrefab.State.Yellow)
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier * 0.8f;
                }
                else
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[0].FriendPoint * interactNPC.FriendPointMulitiplier * 0.6f;
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
                FruitPrefab fruit = null;
                for (int i = 0; i < FruitList.Count; i++)
                {
                    if (FruitList[i].Fruit.FruitName == itemList[1].FruitName)
                    {
                        fruit = FruitList[i];
                        FruitList.RemoveAt(i);
                        break;
                    }
                }
                
                if (fruit.FruitState == FruitPrefab.State.Green)
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier;
                }
                else if (fruit.FruitState == FruitPrefab.State.Yellow)
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier * 0.8f;
                }
                else
                {
                    GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1] += itemList[1].FriendPoint * interactNPC.FriendPointMulitiplier * 0.6f;
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
            FruitPrefab fruit = null;
            for (int i = 0; i < FruitList.Count; i++)
            {
                if (FruitList[i].Fruit.FruitName == itemList[0].FruitName)
                {
                    fruit = FruitList[i];
                    FruitList.RemoveAt(i);
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
            FruitPrefab fruit = null;
            for (int i = 0; i < FruitList.Count; i++)
            {
                if (FruitList[i].Fruit.FruitName == itemList[1].FruitName)
                {
                    fruit = FruitList[i];
                    FruitList.RemoveAt(i);
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


    public void UpdateChart()
    {
        GameManager.Instance.isEnd = true;
        for (int i = 0; i < GameManager.Instance.playTime; i++)
        {
            playData = playDataList[i];
            if (playData != null)
            {
                foreach (Transform child in playData.transform)
                {
                    if (child.gameObject != null)
                    {
                        if (child.name == "Header2")
                        {
                            child.GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.collectCount[i].ToString();
                        }
                        else if (child.name == "Header3")
                        {
                            child.GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.giveCount[i].ToString();
                        }
                        else if (child.name == "Header4")
                        {
                            child.GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.throwCount[i].ToString();
                        }
                        else if (child.name == "Header5")
                        {
                            child.GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1].ToString();
                        }
                    }
                }
            }
        }
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

    public void ShowPauseMenu()
    {
        if (!GameManager.Instance.isEnd)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ShowInstruction()
    {
        
        if (instructions.activeInHierarchy)
        {
            instructions.SetActive(false);
        }
        else
        {
            instructions.SetActive(true);
        }

    }

}
