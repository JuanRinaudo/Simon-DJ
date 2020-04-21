using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{

    public Image healthbar;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI perfectsText;
    public TextMeshProUGUI missesText;

    void Update()
    {
        healthbar.fillAmount = Game.data.health;
        Color targetColor = Game.GetStepColor(Game.data.lastStep);
        targetColor.a = healthbar.color.a;
        healthbar.color = Color.Lerp(healthbar.color, targetColor, 0.98f);

        float score = Game.data.perfects * 5 - Game.data.misses;
        scoreText.text = score.ToString();

        perfectsText.text = Game.data.perfects.ToString();
        missesText.text = Game.data.misses.ToString();
    }

}
