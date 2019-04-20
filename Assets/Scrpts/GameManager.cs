

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Range(1,4)]
    [SerializeField]
   public  const int version = 5; //version 1 where you cant turn an already turned card, version 2 fixes that, and version 3 SHOWS ALL cards at first then hides., version 4 will have timer
    //public GridLayout Girdlayout; // for v5 maybe a penalty system for mismatched card and reward player for matches// we have a version 6 now kek version 6 is where ttotal time is limited you go to next stage with your saved time
    public float lookingTime;
    public float GameTime;
    public float timer;
    public float timer2;
    public float GiveOrRemoveTimeValue; // this is penalty or reward;
    public bool GameStart;
    public bool cardsready;
    [Range(3, 9)]
    public int CardPairs;
    public int TotalCardtoSpawn;
    public AudioManager AM;
    public UIManager UI;
    public CardPorperties CardPrefab;
    public Transform CardZone;
    [SerializeField]
    List<Cards> Cards = new List<Cards>();
    [SerializeField]
    List<CardPorperties> CardList = new List<CardPorperties>();
    public CardPorperties OldClickedCard;
    public CardPorperties NewClickedCard;
    int paircounter;
    public int PlayRecord =0;
    

    /// <summary>
    /// Start mehtod
    /// </summary>
    /// v
    void  Start()
    {
        timer = GameTime;
        cardsready = false;
        GameStart = true;
        paircounter = 0;
        //if (version >= 4)
        //{
        //    UpdateTime(timer);

        //}
        UpdateTime(GameTime);
        StartCoroutine (GenerateCards());
       // UI.ToggleGridLayout(false);


    }
    private void Update()
    {
        if (GameStart && version >=4 )
        {
            timer2 += Time.deltaTime; // this is for fixing before the player can play
            if (timer2 >= lookingTime+1f)
            {

                GameTime -= Time.deltaTime;

                // timer = timer + Time.deltaTime;
                UpdateTime(GameTime);
                Debug.Log(timer);
                if (GameTime <= 0)
                {

                    GameOver();
                }
            }


           
            //if (Input.GetKeyDown(KeyCode.E))
            //{

            //    if (version >= 4)
            //    {
            //        for (int i = 0; i < CardList.Count; i++)
            //        {
            //            Destroy(CardList[i].gameObject);

            //            //CardList.Remove(CardList[i]);
            //        }
            //        CardList.Clear();
                  
                    
            //    }

            //}
            
        }

     
    }
    /// <summary>
    /// OnclickCard subscribed to event;
    /// </summary>


    private void GameManager_OnClickCard(CardPorperties obj)
    {
        if (cardsready)
        {

            Cards ReceievedCard = obj.card;
            // CardClicked(obj);
            Debug.Log(ReceievedCard.name);

            if (version < 2)
            {
                if (!obj.IsCardFlipped())
                {
                    CardClicked(obj);
                    if (OldClickedCard == null)
                    {
                        OldClickedCard = obj;

                    }
                    else
                    {

                        NewClickedCard = obj;

                        StartCoroutine(CheckMatch());

                    }
                }
                else
                {

                    Debug.Log("Card Is ALready Flipped");

                }
            }
            else if (version >= 2)
            {


                if (OldClickedCard == null && !obj.IsCardFlipped())
                {
                    CardClicked(obj);
                    OldClickedCard = obj;


                }
                else if (OldClickedCard == obj && obj.IsCardFlipped())
                {

                    OldClickedCard = null;
                    CardClicked(obj);
                }


                else
                {
                    CardClicked(obj);
                    NewClickedCard = obj;
                    StartCoroutine(CheckMatch());

                }



            }
        }
        else
        {

            Debug.Log("Cards Are no ready");
        }



    }
    /// <summary>
    /// ///flipcard
    /// </summary>
    /// <param name="cardproperties"></param>
    public void CardClicked(CardPorperties cardproperties)
    {
        int x = Random.Range(0, AM.draw.Length);
        AM.PlayAudio(AM.draw[x]);
        cardproperties.flipcard();


    }

    /// <summary>
    /// //checkmatch
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckMatch()
    {
        if (OldClickedCard.card == NewClickedCard.card)
        {
            AM.PlayAudio(AM.Match);
            yield return new WaitForSeconds(.25f);
            AM.PlayAudio(AM.Match);
            DestroyingCards(OldClickedCard);
            DestroyingCards(NewClickedCard);
            OldClickedCard = null;
            NewClickedCard = null;

            if(version >= 5)
            {
                AddOrRemoveTime(GiveOrRemoveTimeValue);

            }
            if (CheckEmptyCardList())
            {
                UpdateGameRecord(PlayRecord);
                GameOver();

               
            }
        }
        else
        {
            yield return new WaitForSeconds(.25f);
            OldClickedCard.flipcard();
            NewClickedCard.flipcard();

            OldClickedCard = null;
            NewClickedCard = null;
            if(version>= 5)
            {
                AddOrRemoveTime(-GiveOrRemoveTimeValue);

            }

        }
    }
    /// <summary>
    /// ///////generate cards
    /// </summary>
    IEnumerator  GenerateCards()
    {
       

        int CardsIndex = 0; ;
        for (int i = 0; i < TotalCardtoSpawn; i++)
        {
            if (paircounter == 0)
            {
                CardsIndex = Random.Range(0, Cards.Count);
            }

            if (paircounter < 2)
            {
               //  StartCoroutine (InstantaniatingCardOnBoard(CardsIndex));
                yield return new WaitForSeconds(.05f);
                InstantaniatingCardOnBoard(CardsIndex);
                paircounter++;


            }
            else
            {
                yield return new WaitForSeconds(.05f);
                paircounter = 0;
                CardsIndex = Random.Range(0, Cards.Count);
                InstantaniatingCardOnBoard(CardsIndex);
                //  StartCoroutine(InstantaniatingCardOnBoard(CardsIndex));

                paircounter++;


                


            }

          
           // yield return null;
        }
        ShuffleCards();
      
        if (version >= 3)
        {
            yield return StartCoroutine(FlipAllcards());
        }
        
         UI.ToggleGridLayout(false);
        cardsready = true;
        
    }
    void  togglelayout(bool condition)
    {
        
        UI.ToggleGridLayout(condition);
       // yield return null;
    }

    IEnumerator FlipAllcards()
    {

        yield return new WaitForSeconds(lookingTime);
        for (int i = 0; i< CardList.Count; i++)
        {

            CardList[i].flipcard();
        }
     
        // UI.ToggleGridLayout(false);
    }
    /// <summary>
    /// ////instantaniating the prefab and adding it to card list
    /// </summary>
    /// <param name="CardsIndex"></param>

    void InstantaniatingCardOnBoard(int CardsIndex)
    {
        // not working
            CardPorperties CardObject = Instantiate(CardPrefab, CardZone) as CardPorperties;
            CardObject.card = Cards[CardsIndex];

            // CardObject.gameObject.SetActive(false);
            CardList.Add(CardObject);
        CardObject.OnClickCard += GameManager_OnClickCard;
        if(version >= 3)
        {

            CardObject.flipcard();
            CardObject.gameObject.SetActive(false);

        }
        // yield return new WaitForSeconds(1f);



    }
    /// <summary>
    /// ///////shuffle dem, cards
    /// </summary>
    private void ShuffleCards()
    {
        // CardPorperties[] ArrayOfCards = CardList.ToArray();
        AM.PlayAudio(AM.Shuffle);
        for (int i = 0; i < CardList.Count; i++)
        {
            CardPorperties temp = CardList[i];
            int j = Random.Range(i, CardList.Count);
            CardList[i] = CardList[j];
            CardList[j] = temp;
           

        }

        ShuffleInUi();
    }

    /// <summary>
    /// ///then shuffle the cards according to the list to the UI
    /// </summary>

    void ShuffleInUi()
    {
      
            for (int i = 0; i < CardList.Count; i++)
            {
                CardList[i].transform.SetSiblingIndex(i);


            }
            if (version >= 3)
        {
            for (int i = 0; i < CardList.Count; i++)
            {

                CardList[i].gameObject.SetActive(true);

            }

        }
        

    }
    /// <summary>
    /// /////destroying the cards after the cards have been matched
    /// </summary>
    /// <param name="card"></param>
    void DestroyingCards(CardPorperties card)
    {

        card.OnClickCard -= GameManager_OnClickCard;
        CardList.Remove(card);
        Destroy(card.gameObject);

    }
    /// <summary>
    /// //check if our cardlist is empty for winning condition
    /// </summary>
    /// <returns></returns>

    bool CheckEmptyCardList()
    {
        if (CardList.Count == 0)
        {
            return true;


        }

        else return false;
    }

    

    /// <summary>
    /// //only will work on the editor set a number of total cards from getting the pairs
    /// </summary>
    private void OnValidate()
    {
        TotalCardtoSpawn = CardPairs * 2;
    }
    /// <summary>
    /// ////call gameover when time is over or challenge is finished
    /// </summary>
    void GameOver()
    {
        GameStart = false;
        if (version >= 4 && version <6)
        {
            GameTime = timer;
            timer2 = 0;
        }

        else if(version >= 6)
        {
            timer2 = 0;

        }
        UI.ShowRetryButton();
        UI.ToggleGridLayout(true);
        NewClickedCard = null;
        OldClickedCard = null;
        for (int i = 0; i < CardList.Count; i++)
        {
            if (!CardList[i].IsCardFlipped())
            {
                CardList[i].flipcard();
            }
        }

        // StopAllCoroutines();
        // we will show a button to restart our game, or a prompt of pressing any buttons we will animate animate 
        //RestartGame();
    }
    /// <summary>
    /// /// restart our game if our game is over or gamestart is false
    /// here we call, our startt method once again
    /// </summary>
   public void RestartGame()
    {
        UI.HideRetryButton();
        AM.PlayAudio(AM.draw[1]);

        
        //GenerateCards();

        if (version >= 4 )
        {
            
            
                for (int i = 0; i < CardList.Count; i++)
                {
                    Destroy(CardList[i].gameObject);

                    //CardList.Remove(CardList[i]);
                }
                CardList.Clear();

            if (version < 6)
            {
                GameTime = timer;
            }
            
        }

        Start();

    }
    public void UpdateGameRecord(int rec)
    {
        PlayRecord = rec +1;
        UI.UpdateRecords(PlayRecord);

    }
    public void UpdateTime(float timer)
    {

        UI.UpdateTimer(timer);
    }
    void AddOrRemoveTime(float time)
    {
        UI.PointUpdate(time);
        GameTime += time;
    }

    void ResetScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
