using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles data serialisation/compute etc
public class DataController : MonoBehaviour
{
    class PlayerData
    {
        public int playerID { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int shots { get; set; }//optional
        public int accuracy { get; set; }//optional


    }

    private Dictionary<int, PlayerData> playerdictionary;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void initializePlayers(List<int> playerIDs)
    {
        playerdictionary = new Dictionary<int, PlayerData>();
        playerIDs.ForEach(id => {
            PlayerData player = new PlayerData
            {
                playerID = id
            };
            playerdictionary.Add(id, player);
        }) ;
    }
    public void registerKill(int killerId, int victimID)
    {
        playerdictionary[killerId].Kills++;
        playerdictionary[victimID].Deaths++;
    }

}
