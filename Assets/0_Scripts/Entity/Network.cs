using PlayerIOClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour
{
    private static Network _instance;
    private Client PlayerIoClient;
    private Connection _connection;
    private List<Message> _msgList = new List<Message>();
    private Dictionary<string, PlayerData> _playersData = new Dictionary<string, PlayerData>();

    private string _localUserID;
    public PlayerData _localPlayer { get => _playersData[_localUserID]; }

    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private GameObject _playerPref;
    [SerializeField]
    private GameObject[] _spawnPoint;
    [SerializeField]
    private GameObject _loadingScreen;
    [SerializeField]
    private GameObject _redBuildingPref;
    [SerializeField]
    private GameObject _blueBuildingPref;
    [SerializeField]
    private UnitStorage _unitStorage;

    private int _playersInGame;

    [SerializeField]
    private List<GameObject> _redBuildingsSpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _blueBuildingsSpawns = new List<GameObject>();

    public class PlayerData
    {
        public string UserID { get; set; }
        public int ColorIndex { get; set; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _instance = this;
    }

    public static Network GetInstance()
    {
        return _instance;
    }


    void Start()
    {
        Application.runInBackground = true;

        // Create a random userid 
        System.Random random = new System.Random();
        _localUserID = "Commander" + random.Next(0, 4000);

        Debug.Log("Starting");

        PlayerIO.Authenticate(
    "rtsonline-vgx9br4yhui6g3tbpgm1q", // Copier/Coller le gameId présent sur le backoffice du jeu
    "public", // Voir les types de connexion, par défaut on laisse en "public" (pas safe)
    new Dictionary<string, string>  // arguments passés à la connexion (userId, mdp, etc...)
    {
        {"userId", _localUserID},
    },
    null, // Segments dans lequel placer le joueur pour PlayerInsight (analytics)
    delegate (Client client)
    {
        // Ce qu'il se passe quand la connexion est établie avec succès
        Debug.Log("Connexion à Player.io établie pour le client : " + client);

        // conseil : garder le paramètre "Client" en variable de classe, on va s'en servir souvent
        PlayerIoClient = client;

        // Commenter cette ligne pour un serveur en production !
        PlayerIoClient.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

        ConnectToRoom();
    },
    delegate (PlayerIOError error)
    {
        Debug.Log("Erreur à la connexion avec Player.io: " + error.ToString());
    }
);

    }

    private void ConnectToRoom()
    {
        PlayerIoClient.Multiplayer.CreateJoinRoom(
    "myRoom", // nom/id de la partie, pour le test on lui assigne un nom par défaut
    "WasteWar", // type de room, doit correspondre au GameCode du serveur !
    true, // la salle est elle visible par l'appel à "ListRooms" ?
    null, // opt: les données à passer à la room pour renseigner "ListRooms", par exemple le nom de la map choisie...
    null, // opt :les données du joueur qui vient d'entrer
    delegate (Connection connection)
    {
        Debug.Log("Vous avez rejoint une partie.");

        // comme pour le client, stocker cet objet car on va l'utiliser
        _connection = connection;
        // on ajoute une écoute sur l'événement "OnMessage" qui recevra les messages du serveur
        _connection.OnMessage += HandleMessage;
    },
    delegate (PlayerIOError error)
    {
        Debug.Log("Erreur à la connexion à une partie : " + error.Message + " (" + error.ErrorCode + ")");
    });

    }

    private void HandleMessage(object sender, Message e)
    {
        _msgList.Add(e);
    }

    private void PlayerJoined(Message m)
    {
        Debug.Log("PlayerJoined");
        _playersInGame++;
        // on recupère les données (dans le bon ordre !) du message
        var userId = m.GetString(0);
        var colorIndex = m.GetInt(1);

        // on créé nos données joueurs
        var playerData = new PlayerData
        {
            UserID = userId,
            ColorIndex = colorIndex,
        };
        // qu'on stock
        _playersData.Add(userId, playerData);

        if (_localUserID == userId)
        {
            _playerPref.transform.position.Equals(_spawnPoint[colorIndex].transform.position);
        }

        if (_playersInGame == 2)
        {
            DisableUI();
        }
    }

    private void OnApplicationQuit()
    {
        _connection.Send("PlayerHasLeft", _localUserID);
    }

    void FixedUpdate()
    {
        // traitement de la file de message
        foreach (Message m in _msgList)
        {
            // on traite chaque type de message séparément
            switch (m.Type)
            {
                case "PlayerJoined":
                    PlayerJoined(m);
                    break;
                case "PlayerLeft":
                    Disconnect(m);
                    break;
                case "CreateBuilding":
                    OnCreateBuilding(m);
                    break;
                case "BuildingRemoved":
                    OnRemoveBuild(m);
                    break;
                case "UnitRemoved":
                    OnRemoveUnit(m);
                    break;
                default:
                    break;
            }
        }
        // une fois tous les messages de la file traités, on la vide pour en attendre de nouveaux
        _msgList.Clear();
    }


    public void Disconnect(Message m)
    {
        _connection.Disconnect();
    }

    #region Buildings
    private void OnCreateBuilding(Message m)
    {
        if (_localPlayer.ColorIndex == 0)
        {
            for (int i = 0; i < _redBuildingsSpawns.Count; i++)
            {
                var _createdRedBuilding = Instantiate(_redBuildingPref, _redBuildingsSpawns[i].transform.position, Quaternion.identity);
                _createdRedBuilding.GetComponent<Entity>().UnitID = m.GetInt(0) + i;
                _unitStorage.AddBuilding(_createdRedBuilding.GetComponent<Entity>());
            }
        }
        else
        {
            for (int i = 0; i < _blueBuildingsSpawns.Count; i++)
            {
                var _createdBlueBuilding = Instantiate(_blueBuildingPref, _blueBuildingsSpawns[i].transform.position, Quaternion.identity);
                _createdBlueBuilding.GetComponent<Entity>().UnitID = m.GetInt(0) + i;
                _unitStorage.AddBuilding(_createdBlueBuilding.GetComponent<Entity>());
            }
        }
    }

    public void RemoveBuilding(int id)
    {
        _connection.Send("RemoveBuild", id);
    }
    private void OnRemoveBuild(Message m)
    {
        _unitStorage.RemoveBuilding(m.GetInt(0));
    }
    #endregion

    #region CreateUnit
    public void UnitOnServer(int id)
    {
        _connection.Send("CreateUnit", id);
    }
    #endregion

    #region RemoveUnit
    public void RemoveUnitFromServer(int id)
    {
        _connection.Send("RemoveUnit", id);
    }

    private void OnRemoveUnit(Message m)
    {
        _unitStorage.RemoveUnit(m.GetInt(0));
    }
    #endregion

    private void DisableUI()
    {
        _cameraController.enabled = true;
        _loadingScreen.SetActive(false);
        _connection.Send("CreateStartBuilding");
    }
}
