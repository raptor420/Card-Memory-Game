using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject RetryButton;
    public GridLayoutGroup grid;
    public TextMeshProUGUI Records;
    public TextMeshProUGUI Timer;
    public GameObject plus;
    public TextMeshProUGUI bonustext;
    Animator at;
    //  public GridLayout g ;

    private void Start()
    {
        at = plus.GetComponent<Animator>();
        plus.SetActive(false);
    }

    public void HideRetryButton()
    {
        RetryButton.SetActive(false);

    }
    public void ShowRetryButton()
    {
        RetryButton.SetActive(true);
    }
    public void ToggleGridLayout(bool boolean)
    {
        grid.enabled = boolean;    }
    public void UpdateTimer(float time)
    {
        if (time <=10)
        {
            Timer.color = Color.red;

        }
        else
        {
            Timer.color = Color.white;

        }
        Timer.text = time.ToString("F0");
    }
    public void UpdateRecords(int record )
    {
        Records.text ="WIN" + record.ToString();
    }
    IEnumerator DoANimForText()
    {
        
            plus.SetActive(true);
            at.SetTrigger("move");
        yield return new WaitForSeconds(.40f);
        bonustext.text = "";
        at.SetTrigger("start");
        yield return new WaitForSeconds(.40f);

        plus.SetActive(false);
       
          


       
     
    }

    public void PointUpdate(float i)
    {
        if (i > 0)
        {
            bonustext.color = Color.green;

            bonustext.text = "+" + i.ToString();
            
        }
        else
        {
            bonustext.color = Color.red;

            bonustext.text =  i.ToString();

        }
       StartCoroutine (DoANimForText());
        
    }
}
