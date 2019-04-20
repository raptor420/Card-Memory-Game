using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class CardPorperties : MonoBehaviour, IPointerClickHandler
{

    public Cards CardType;
    public Cards card
    {
        
        set
        {
            CardType = value;
            Initiate();
        }
        get
        {
            return CardType;

        }
    }
    public GameObject CardBack;
    Image CardImage;
    bool FlippedCard;
    public event Action<CardPorperties> OnClickCard;

    
    private void Initiate()
    {
        if (card != null)
        {
            CardImage = GetComponent<Image>();


            CardImage.sprite = card.CardSprite;
        }

        FlippedCard = false;

    }
    // Update is called once per frame
    void Update()
    {

    }


   


    public bool IsCardFlipped()
    {


        return FlippedCard;
    }
    public void flipcard()
    {
        FlippedCard = !FlippedCard;
        CardBack.SetActive(!FlippedCard);


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Left && card != null )
        {

            OnClickCard(this);
            Debug.Log("clicked");

        }

        else
        {
            Debug.Log("error");

        }
    }
    public void  sth (){

        }
}
