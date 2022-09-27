using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTeamManager 
{
    public class Cheese
    {
        public Vector3 position;
        public bool collected;

        public Cheese(Vector3 position, bool collected)
        {
            this.position = position;
            this.collected = collected;
        }
    }

    public byte TeamNum;
    public string TeamName;
    public Cheese[] allCheese;

    public GameTeamManager(byte teamNum, string teamName, Cheese[] allCheese)
    {
        TeamNum = teamNum;
        TeamName = teamName;
        this.allCheese = allCheese;
    }

    public GameTeamManager(byte teamNum, string teamName)
    {
        TeamNum = teamNum;
        TeamName = teamName;
    }

    internal void SetCheeseLocations(Vector3[] cheeseLocations)
    {
        allCheese = new Cheese[cheeseLocations.Length];
        for (int i = 0; i < allCheese.Length; i++)
        {
            allCheese[i] = new Cheese(cheeseLocations[i], false);
        }
    }
}
