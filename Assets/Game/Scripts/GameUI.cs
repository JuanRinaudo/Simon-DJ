using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{

    public Image healthbar;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        healthbar.fillAmount = Game.data.health;

        float score = Game.data.perfects * 5 - Game.data.misses;
        scoreText.text = score.ToString();
    }
    
}
