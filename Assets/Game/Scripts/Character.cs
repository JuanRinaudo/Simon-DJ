using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public Animator animator;
    public Mesh characterMesh;
    public Material characterMaterial;

    [HideInInspector]
    public int characterIndex;

    private void Awake()
    {
        Game.data.characters[Game.data.characterCount] = this;
        characterIndex = Game.data.characterCount;
        Game.data.characterCount++;
    }

    void Update()
    {
        
    }

    public void StartDance()
    {
        animator.SetBool(CharacterParameters.DANCING_BOOL, true);
        animator.SetInteger(CharacterParameters.DANCE_INT, Random.Range(0, 4));
    }

    public void StopDance()
    {
        animator.SetBool(CharacterParameters.DANCING_BOOL, false);
    }

    public void ChangeSpeed()
    {
        animator.speed = Random.Range(0.9f, 1.1f);
    }

    private void OnDestroy()
    {
        Game.data.characterCount--;
        if(Game.data.characterCount > 0)
        {
            Game.data.characters[characterIndex] = Game.data.characters[Game.data.characterCount];
        }
    }

}
