using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace RTS
{
    public class Player : BasePlayer
    {
        public int ColorID;
        public int BuildingID;
        public float PosX, PosY, PosZ;
    }

    [RoomType("WasteWar")]
    public class GameCode : Game<Player>
    {
        private int _currentColorID;
        private int _currentBuildingID;

        // This method is called when an instance of your the game is created
        public override void GameStarted()
        {
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started: " + RoomId);

        }

        // This method is called when the last player leaves the room, and it's closed down.
        public override void GameClosed()
        {
            Console.WriteLine("RoomId: " + RoomId);
        }

        // This method is called whenever a player joins the game
        public override void UserJoined(Player player)
        {
            player.ColorID = _currentColorID++;
            player.BuildingID = _currentBuildingID;
            _currentBuildingID += 3;

            foreach (Player pl in Players)
            {
                pl.Send("PlayerJoined", player.ConnectUserId, player.ColorID);

                if (pl.ConnectUserId != player.ConnectUserId)
                {
                    player.Send("PlayerJoined", pl.ConnectUserId, pl.ColorID);
                }
            }
        }

        // This method is called when a player leaves the game
        public override void UserLeft(Player player)
        {
            Broadcast("PlayerLeft", player.ConnectUserId);
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage(Player player, Message message)
        {
            switch (message.Type)
            {
                case "PlayerJoined":
                    Broadcast("PlayerJoined", player.ConnectUserId, player.ColorID);
                    break;
                // called when a player clicks on the ground
                case "CreateStartBuilding":
                    Broadcast("CreateBuilding", player.BuildingID);
                    break;
                case "CreateUnit":
                    Broadcast("UnitCreated", message.GetInt(0));
                    break;
                case "RemoveBuild":
                    Broadcast("BuildingRemoved", message.GetInt(0));
                    break;
                case "RemoveUnit":
                    Broadcast("UnitRemoved", message.GetInt(0));
                    break;
                case "PlayerHasLeft":
                    Broadcast("PlayerLeft", player.ConnectUserId);
                    break;
                default:
                    break;
            }
        }
    }
}