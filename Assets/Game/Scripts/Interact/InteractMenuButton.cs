using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MenuButtonType
{
    PLAY,
    STOP,
    FREEPLAY,
    EXIT
}

public class InteractMenuButton : AbstractInteractable
{

    private string initialText;
    public TextMeshPro text;
    public MenuButtonType type;

    private void Awake()
    {
        initialText = text.text;

        Game.instance.menuButtons[(int)type] = this;
    }

    public override void Interact(InteractionType interactionType, GameObject interactionParent = null)
    {
        for(int buttonIndex = 0; buttonIndex < Game.instance.menuButtons.Length; ++buttonIndex)
        {
            if(buttonIndex != (int)type)
            {
                Game.instance.menuButtons[buttonIndex].ResetState();
            }
        }

        switch (type)
        {
            case MenuButtonType.PLAY:
                if (text.text == "PLAY")
                {
                    text.text = "PLAYING!";
                    Game.StartNewGame();
                    Game.instance.stepPlayer.PlaySteps(0);
                }
                else if(text.text == "RESET?")
                {
                    text.text = "PLAYING!";
                    Game.instance.stepPlayer.StopSteps();
                    Game.StartNewGame();
                    Game.instance.stepPlayer.PlaySteps(0);
                }
                else if(text.text == "PLAYING!")
                {
                    text.text = "RESET?";
                }
                break;
            case MenuButtonType.STOP:
                text.text = "STOPPED";
                Game.instance.stepPlayer.StopSteps();
                break;
            case MenuButtonType.FREEPLAY:
                Game.instance.stepPlayer.Freeplay();
                text.text = "> FREEPLAY <";
                break;
            case MenuButtonType.EXIT:
                if(text.text == "SURE?")
                {
                    Application.Quit();
                }
                else
                {
                    text.text = "SURE?";
                }
                break;
        }
    }

    public void ResetState()
    {
        text.text = initialText;

        switch (type)
        {
            case MenuButtonType.PLAY:

                break;
            case MenuButtonType.STOP:

                break;
            case MenuButtonType.FREEPLAY:

                break;
            case MenuButtonType.EXIT:

                break;
        }
    }

}
