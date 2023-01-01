using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string wName;
    public Sprite wSprite;
    [TextArea(1, 3)]
    public string wDescription;
    public bool isUpgraded;
    public Weapon[] previousWeapons;
}
