using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Hearts
    public List<Image> hearts;
    public Color heartDeadColor;
    public Sprite aliveHeart;
    public Sprite deadHeart;

    //Score
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI hiScoreText;

    //Rewind
    public Image rewindCircle;
    public Image rewindStrikethrough;

    // Game over
    public Animator deathOverlayAnim;

    public void SetUpUI(int maxHealth)
    {
        if(maxHealth == 1)
        {
            hearts[2].enabled = hearts[0].enabled = false;
            hearts.RemoveAt(2);
            hearts.RemoveAt(0);
        }
        foreach(Image h in hearts)
        {
            h.sprite = aliveHeart;
            h.color = Color.red;
        }

        UpdateRewind(true);

        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealth(int health)
    {
        for(int i = health; i < hearts.Count; i++)
        {
            hearts[i].sprite = deadHeart;
            hearts[i].color = heartDeadColor;
        }
    }

    public void UpdateRewind(bool canRewind)
    {
        rewindCircle.color = canRewind ? Color.green : Color.red;
        rewindStrikethrough.enabled = !canRewind;
    }

    public void GameOverUI(int score)
    {
        deathOverlayAnim.SetTrigger("GameOver");
        finalScoreText.text = score.ToString();
        hiScoreText.text = score.ToString(); // Get high score
    }
}
