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

        public CheeseObject cheeseObj;

        public Cheese(Vector3 position, bool collected, CheeseObject cheeseObj)
        {
            this.position = position;
            this.collected = collected;
            this.cheeseObj = cheeseObj;
        }
    }

    public class Trap
    {
        public enum TrapType
        {
            MouseTrap,
            Laser,

        }
        public Vector3 position;
        public TrapType trapType;

        public Trap(Vector3 position, TrapType trapType)
        {
            this.position = position;
            this.trapType = trapType;
        }
    }

    public byte TeamNum;
    public string TeamName;
    public Cheese[] allCheese;
    public Trap[] allTraps;

    public GameTeamManager(byte teamNum, string teamName, Cheese[] allCheese, Trap[] allTraps)
    {
        TeamNum = teamNum;
        TeamName = teamName;
        this.allCheese = allCheese;
        this.allTraps = allTraps;
    }

    public GameTeamManager(byte teamNum, string teamName)
    {
        TeamNum = teamNum;
        TeamName = teamName;
    }

    internal void SetCheeseLocations(Vector3[] cheeseLocations, CheeseObject[] cheeseObjs)
    {
        allCheese = new Cheese[cheeseLocations.Length];
        for (int i = 0; i < allCheese.Length; i++)
        {
            allCheese[i] = new Cheese(cheeseLocations[i], false, cheeseObjs[i]);
        }
    }

    public void SetTrapLocations(Vector3[] trapLocations, Trap.TrapType[] trapType)
    {
        allTraps = new Trap[trapLocations.Length];
        for (int i = 0; i < allTraps.Length; i++)
        {
            allTraps[i] = new Trap(trapLocations[i], trapType[i]);
        }
    }

    public Cheese CollectCheese(byte cheeseId)
    {
        allCheese[cheeseId].collected = true;
        allCheese[cheeseId].cheeseObj.OnCollect();
        return allCheese[cheeseId];
    }
}
