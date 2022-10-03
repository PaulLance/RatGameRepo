using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhotonStatus 
{
   public string playerName { get;  private set; }
   public int status { get; private set; }
   public string message { get; private set; }

   public PhotonStatus(string name, int status1, string message1)
   {
        playerName = name;
        status = status1;
        message = message1;
   }

}
