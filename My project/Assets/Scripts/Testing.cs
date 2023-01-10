using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private UI_SkillTree uiSkillTree;

    void Start(){
        uiSkillTree.SetPlayerWeapons(player.GetPlayerWeapons());
    }
}
