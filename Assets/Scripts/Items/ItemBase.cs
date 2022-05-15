using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    public Sprite ItemIcon;
    public string ItemName;
    public string ShortDescription;
    public string LongDescription;
    public int MaxStackSize = 1;
}
