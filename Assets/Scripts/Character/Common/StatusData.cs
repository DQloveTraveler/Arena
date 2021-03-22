using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StatusData", menuName = "ScriptableObjects/StatusData", order = 1)]
public class StatusData : ScriptableObject
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int maxSP = 100;
    [SerializeField] private int maxEP = 100;


    //外部からアクセスする際は以下のプロパティを使う
    public int MaxHP { get => maxHP; }
    public int MaxSP { get => maxSP; }
    public int MaxEP { get => maxEP; }

}
