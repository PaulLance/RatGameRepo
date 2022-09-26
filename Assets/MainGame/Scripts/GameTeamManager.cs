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
}
