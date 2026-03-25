using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDeathObjectType
{
    NONE,
    RETRY,
    QUIT
}

public class DeathMenuObject : UIObject
{
    [SerializeField] private EDeathObjectType _type;

    public EDeathObjectType Type { get => _type; set => _type = value; }
}
