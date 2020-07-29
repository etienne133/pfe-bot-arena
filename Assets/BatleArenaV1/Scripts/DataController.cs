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
        playerdictionary = new Dictionary<int, PlayerData>();
    }

    public void initializePlayers(List<int> playerIDs)
    {
        playerIDs.ForEach(id => {
            playerdictionary.Add(id, new PlayerData());
        }) ;
    }
    public void registerKill(int killerId, int victimID)
    {
        playerdictionary[killerId].Kills++;
        playerdictionary[victimID].Deaths++;
    }

}
