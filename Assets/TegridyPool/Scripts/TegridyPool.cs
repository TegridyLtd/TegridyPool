using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Tegridy.AudioTools;
using Tegridy.UITools;
using Tegridy.Tools;
namespace Tegridy.PoolGame
{
    public class ActivePlayer
    {
        public int playerCharacter;
        public string playerName;
        public float timeRemaining;
        public int turnsRemaining;
        public int shotsTaken;
        public int foulsTotal;
        public int currentColour;
        public Image playerPowerBar;
        public int ballsLeft;
        public List<TegridyPoolBall> remainingBalls;
        public TegridyPoolCue playercue;
    }
    public class TegridyPool : MonoBehaviour
    {
        public bool configUsingStats = false;

        //////////////////////////////////////////////////////////////////////////////////////
        ///UI Stuff
        public Transform cueHolder; //parent game object containing aviable cues
        public Transform tableHolder; //parent gameobject containing available tables
        public Transform characterHolder; //parent gameobject containing available characters
        TegridyPoolCue[] cues;
        TegridyPoolTable[] tables;
        TegridyPoolCharacter[] characters;

        UITegridyPoolMenu guiMenu; //user interface
        GameObject[] prefabHolder; //for keeping track of the prefabs
        GameObject hostMenu; //menu to return to when we are finished

        //used to keep track of the current sellected config items
        int[] curChar;
        int[] curCue;

        //for working out our display, can be moved to config file
        readonly float maxForce = 4;
        readonly float maxAccuracy = 1;

        //////////////////////////////////////////////////////////////////////////////////////
        ///Match Stuff
        ActivePlayer[] players; //for keeping track of the current players
        int playerActive; //array identifier for current active player
        int currentTable;

        bool endShot; //used while waiting for player to take shot
        bool ballStop; //used while balls are moving
        bool colourSelected = false; //when true check player hit right color
        bool matchOver; //while false wait for players to do there thing

        [HideInInspector] public List<int> pottedlist; //list of balls potted for turn
        [HideInInspector] public List<int> touchedList; //list of balls touched this shot

        GameObject ballHolder; //balls for this round
        TegridyPoolBall[] balls;
        [HideInInspector] public GameObject cueBall;
        Vector3 cueBallHome;
        int finalBall;

        AudioSource source;
        public void StartUp()
        {
            //find our game objects
            source = gameObject.AddComponent<AudioSource>();
            cues = cueHolder.GetComponentsInChildren<TegridyPoolCue>();
            tables = tableHolder.GetComponentsInChildren<TegridyPoolTable>();
            characters = characterHolder.GetComponentsInChildren<TegridyPoolCharacter>();

            //Update the cue names and descriptions if the strings length matches the amount of cues found
            if (TegridyPoolLanguage.cueNames.Length == cues.Length)
                for (int i = 0; i < cues.Length; i++)
                    cues[i].cueName = TegridyPoolLanguage.cueNames[i];
            if (TegridyPoolLanguage.cueDescritpions.Length == cues.Length)
                for (int i = 0; i < cues.Length; i++)
                    cues[i].cueDescription = TegridyPoolLanguage.cueDescritpions[i];

            //do the same for the characters
            if (TegridyPoolLanguage.playerNames.Length == characters.Length)
                for (int i = 0; i < characters.Length; i++)
                    characters[i].characterName = TegridyPoolLanguage.playerNames[i];
            if (TegridyPoolLanguage.playerDescriptions.Length == characters.Length)
                for (int i = 0; i < characters.Length; i++)
                    characters[i].characterDescription = TegridyPoolLanguage.playerDescriptions[i];

            //and finally do the same for our levels
            if (TegridyPoolLanguage.levelNames.Length == tables.Length)
                for (int i = 0; i < tables.Length; i++)
                    tables[i].tableName = TegridyPoolLanguage.levelNames[i];
            if (TegridyPoolLanguage.levelDescriptions.Length == tables.Length)
                for (int i = 0; i < tables.Length; i++)
                    tables[i].tableDescription = TegridyPoolLanguage.levelDescriptions[i];

            //make sure the objects we aren't using yet are disabled for now
            foreach (TegridyPoolCue cue in cues)
            {
                cue.SetActive(false);
            }
            foreach (TegridyPoolTable table in tables)
            {
                table.SetActive(false);
            }

            //set the index for the character and table dispay so we start on player and table one
            curCue = new int[cues.Length];
            curChar = new int[characters.Length];
        }
        #region Menu
        public void OpenMainMenu(UITegridyPoolMenu gui, GameObject host)
        {
            guiMenu = gui;

            TegridyUITools.SetText(guiMenu.title, TegridyPoolLanguage.poolMainTitle);

            //Add the listeners and update any text fields with the desired language
            guiMenu.back.onClick.AddListener(() => CloseMenu());
            TegridyUITools.SetButtonText(guiMenu.back, TegridyPoolLanguage.poolMainBack);
            guiMenu.startLocal.onClick.AddListener(() => OpenSinglePlayer());
            TegridyUITools.SetButtonText(guiMenu.startLocal, TegridyPoolLanguage.poolMainLocal);

            //disable the old menu if we were given one
            if (host != null)
            {
                hostMenu = host;
                hostMenu.SetActive(false);
            }

            //enable the pool table/character select menu
            guiMenu.SetActive(true);
            guiMenu.mainMenuScreen.SetActive(true);
        }
        private void CloseMenu()
        {
            //remove the scripts we added to GUI
            guiMenu.back.onClick.RemoveAllListeners();
            guiMenu.startLocal.onClick.RemoveAllListeners();
            guiMenu.mainMenuScreen.SetActive(false);
            guiMenu.SetActive(false);

            //if we had a host menu open it
            if (hostMenu != null) hostMenu.SetActive(true);
            else { Debug.Log("NO host menu"); Application.Quit(); }
        }
        private void OpenSinglePlayer()
        {
            if (prefabHolder != null) TegridyUITools.DestoryOld(prefabHolder);
            prefabHolder = TegridyUITools.DrawStraight(guiMenu.tablePrefab.gameObject, tables.Length, guiMenu.prefabSpacing, false, true, guiMenu.scrollContent);
            for (int i = 0; i < prefabHolder.Length; i++)
            {
                int newInt = i;
                UITegridyPoolTablePrefab thisPrefab = prefabHolder[i].GetComponent<UITegridyPoolTablePrefab>();
                TegridyUITools.SetText(thisPrefab.tableName, tables[i].tableName);
                TegridyUITools.SetText(thisPrefab.tableDescription, tables[i].tableDescription);

                thisPrefab.tablePic.sprite = tables[i].tablePic;
                thisPrefab.play.onClick.AddListener(() => SetupLocalMatch(newInt));
                TegridyUITools.SetButtonText(thisPrefab.play, TegridyPoolLanguage.poolStartMatch);
            }
            guiMenu.tableSelectback.onClick.AddListener(() => CloseSinglePlayer());
            TegridyUITools.SetButtonText(guiMenu.tableSelectback, TegridyPoolLanguage.poolMainBack);
            TegridyUITools.SetText(guiMenu.tableSelectTitle, TegridyPoolLanguage.poolTableSelect);
            guiMenu.tableSelectScreen.SetActive(true);
        }
        private void CloseSinglePlayer()
        {
            TegridyUITools.DestoryOld(prefabHolder);
            guiMenu.tableSelectback.onClick.RemoveAllListeners();
            guiMenu.tableSelectScreen.SetActive(false);
            guiMenu.mainMenuScreen.SetActive(true);
        }
        private void SetupLocalMatch(int table)
        {
            //get rid of the previous screen
            CloseSinglePlayer();
            guiMenu.mainMenuScreen.SetActive(false);

            //setup the listeners and update the text fields if we have any
            TegridyUITools.SetText(guiMenu.configTitle, TegridyPoolLanguage.poolConfigTitle);

            guiMenu.configBack.onClick.AddListener(() => CancelSetup());
            TegridyUITools.SetButtonText(guiMenu.configBack, TegridyPoolLanguage.poolMainBack);

            guiMenu.configStart.onClick.AddListener(() => StartLocalMatch(table));
            TegridyUITools.SetButtonText(guiMenu.configStart, TegridyPoolLanguage.poolStartMatch);

            //draw the prefabs for the character select screen
            prefabHolder = TegridyUITools.DrawStraight(guiMenu.configPreb.gameObject, tables[table].players, guiMenu.configPrefabSpacing, false, true, guiMenu.configScrollContent);
            for (int i = 0; i < prefabHolder.Length; i++)
            {
                int newInt = i;
                UITegridyPoolPlayerConfigPrefab thisPrefab = prefabHolder[i].GetComponent<UITegridyPoolPlayerConfigPrefab>();

                thisPrefab.cueChange.onClick.AddListener(() => ChangeCue(newInt));
                TegridyUITools.SetButtonText(thisPrefab.cueChange, TegridyPoolLanguage.poolChangeCue);

                thisPrefab.playerChange.onClick.AddListener(() => ChangeCharacter(newInt));
                TegridyUITools.SetButtonText(thisPrefab.playerChange, TegridyPoolLanguage.poolChangeChar);

                ChangeCue(newInt);
                ChangeCharacter(newInt);
            }

            guiMenu.configScreen.SetActive(true);
        }
        private void ChangeCue(int prefab)
        {
            //Check its valid
            curCue[prefab]++;
            if (curCue[prefab] >= cues.Length) curCue[prefab] = 0;

            //draw the ui
            UITegridyPoolPlayerConfigPrefab thisPrefab = prefabHolder[prefab].GetComponent<UITegridyPoolPlayerConfigPrefab>();
            thisPrefab.cue.sprite = cues[curCue[prefab]].cuePic;

            TegridyUITools.SetText(thisPrefab.cueName, cues[curCue[prefab]].cueName);
            TegridyUITools.SetText(thisPrefab.cueDescription, cues[curCue[prefab]].cueDescription);

            //work out the fill amount from our stats
            float value = cues[curCue[prefab]].forceMultiplier / (maxForce / 100);
            thisPrefab.cueForce.fillAmount = value / 100;
            TegridyUITools.SetChildText(thisPrefab.cueForce.gameObject, TegridyPoolLanguage.poolPower);

            value = cues[curCue[prefab]].accuracy / (maxAccuracy / 100);
            thisPrefab.cueAccuracy.fillAmount = value / 100;
            TegridyUITools.SetChildText(thisPrefab.cueAccuracy.gameObject, TegridyPoolLanguage.poolAccuracy);

        }
        private void ChangeCharacter(int prefab)
        {
            //make sure we haven't gone past the last character
            if (curChar[prefab] >= characters.Length) curChar[prefab] = 0;

            //Setup the gui and any text fields we find
            UITegridyPoolPlayerConfigPrefab thisPrefab = prefabHolder[prefab].GetComponent<UITegridyPoolPlayerConfigPrefab>();
            thisPrefab.playerCharacter.sprite = characters[curChar[prefab]].characterPic;

            TegridyUITools.SetText(thisPrefab.playerName, characters[curChar[prefab]].characterName);
            TegridyUITools.SetText(thisPrefab.playerDescription, characters[curChar[prefab]].characterDescription);

            //set the image fills to match the stats
            thisPrefab.playerDexterity.fillAmount = characters[curChar[prefab]].dexterity;
            TegridyUITools.SetChildText(thisPrefab.playerDexterity.gameObject, TegridyPoolLanguage.poolDexterity);
            thisPrefab.playerLuck.fillAmount = characters[curChar[prefab]].luck;
            TegridyUITools.SetChildText(thisPrefab.playerLuck.gameObject, TegridyPoolLanguage.poolLuck);
            thisPrefab.playerStrength.fillAmount = characters[curChar[prefab]].strength;
            TegridyUITools.SetChildText(thisPrefab.playerStrength.gameObject, TegridyPoolLanguage.poolStrength);

            //get ready for the next character
            curChar[prefab]++;
        }
        private void CancelSetup()
        {
            //go back to the menu
            TegridyUITools.DestoryOld(prefabHolder);
            guiMenu.configBack.onClick.RemoveAllListeners();
            guiMenu.configStart.onClick.RemoveAllListeners();
            tables[currentTable].tableUI.exit.onClick.RemoveAllListeners();
            guiMenu.configScreen.SetActive(false);
            OpenSinglePlayer();
        }
        #endregion
        #region MatchControl
        private void StartLocalMatch(int table)
        {
            currentTable = table;
            tables[currentTable].tableUI.exit.onClick.AddListener(() => EndMatch(0));
            tables[table].SetActive(true);

            //Create some balls and setup the audio
            ballHolder = Instantiate(tables[table].ballHolder);
            ballHolder.transform.parent = tables[table].ballHolder.transform.parent;
            ballHolder.SetActive(true);
            tables[table].ballHolder.SetActive(false);
            balls = ballHolder.GetComponentsInChildren<TegridyPoolBall>();
            colourSelected = false;

            foreach (TegridyPoolBall ball in balls)
            {
                ball.sfx = tables[table].ballHit;
            }

            finalBall = tables[table].players;
            cueBall = ballHolder.GetComponentInChildren<TegridyPoolCueBall>().gameObject;
            cueBallHome = cueBall.transform.position;

            //setup the pocket audio
            TegridyPoolPocket[] pockets = tables[currentTable].GetComponentsInChildren<TegridyPoolPocket>();
            foreach (TegridyPoolPocket pocket in pockets)
            {
                pocket.sfxCue = tables[currentTable].foul;
                pocket.sfxPotted = tables[currentTable].ballPocket;
            }
            tables[currentTable].GetComponentInChildren<TegridyPoolWall>().sfx = tables[currentTable].ballWall;

            //delete any old data and setup the players
            if (players != null) foreach (ActivePlayer player in players)
            {
                if (player.playercue != null) Destroy(player.playercue.gameObject);
            }
            
            players = new ActivePlayer[prefabHolder.Length];
            for (int i = 0; i < prefabHolder.Length; i++)
            {
                players[i] = new ActivePlayer();
                //setup the character
                int chosen = curChar[i] - 1;

                players[i].playerCharacter = chosen;
                players[i].playerName = characters[chosen].characterName;

                tables[currentTable].tableUI.playerOverlays[i].charPic.sprite = characters[chosen].characterPic;
                tables[currentTable].tableUI.playerOverlays[i].playerName.text = characters[chosen].characterName;

                //setup the cue
                TegridyPoolCue newCue = Instantiate(cues[curCue[i]]);
                newCue.transform.parent = cues[curCue[i]].transform.parent;
                int thisInt = i;
                newCue.playerID = thisInt;
                newCue.powerBar = tables[currentTable].tableUI.playerOverlays[i].playerPowerBar;
                newCue.dexterity = characters[chosen].dexterity;
                newCue.strength = characters[chosen].strength;
                newCue.luck = characters[chosen].luck;

                players[i].playercue = newCue;
                players[i].playercue.StartUp(this);
                players[i].playercue.SetActive(false);
            }

            TegridyUITools.SetText(tables[currentTable].tableUI.title, tables[table].tableName);
            guiMenu.configScreen.SetActive(false);
            tables[currentTable].tableUI.SetActive(true);

            //start with a random player
            playerActive = Random.Range(0, players.Length);
            source.loop = true;
            TegridyAudioTools.PlayRandomClip(tables[currentTable].tableMusic, source);
            StartCoroutine(WaitForMatchEnd());
        }
        public void TakeShot(int playerID)
        {
            players[playerID].turnsRemaining--;
            players[playerID].shotsTaken++;

            for (int i = 0; i < players.Length; i++)
            {
                players[i].playercue.active = false;
                players[i].playercue.SetActive(false);
            }
            ballStop = false;
            endShot = true;
        }
        public void ResetCueBall()
        {
            cueBall.transform.position = cueBallHome;
            cueBall.SetActive(true);
            Debug.Log("Resetting CueBall");
        }
        IEnumerator WaitForMatchEnd()
        {
            matchOver = false;
            StartCoroutine(PlayerTurn());
            yield return new WaitUntil(() => matchOver);
            StopCoroutine(PlayerTurn());
            StopCoroutine(WaitForBallsToStop());
            tables[currentTable].tableUI.foulScreen.SetActive(true);
            yield return new WaitForSeconds(4f);
            tables[currentTable].tableUI.foulScreen.SetActive(false);
            tables[currentTable].tableUI.SetActive(false);
            //return to table select menu
            CancelSetup();
        }
        IEnumerator PlayerTurn()
        {
            if (players[playerActive].turnsRemaining <= 0)
            {
                playerActive++;
                if (playerActive >= players.Length) playerActive = 0;
                players[playerActive].turnsRemaining++;
                TegridyAudioTools.PlayOneShot(tables[currentTable].playerChanged, source);
            }
            UpdateBallsLeft();
            endShot = false;
            touchedList = new List<int>();
            pottedlist = new List<int>();
            players[playerActive].playercue.active = true;
            players[playerActive].playercue.SetActive(true);
            yield return new WaitUntil(() => endShot);

            StartCoroutine(WaitForBallsToStop());
        }
        IEnumerator WaitForBallsToStop()
        {
            tables[currentTable].tableUI.playerOverlays[playerActive].playerTurn.SetActive(false);
            tables[currentTable].tableUI.takingShotText.text = TegridyPoolLanguage.poolTakingShot;
            tables[currentTable].tableUI.takingShot.SetActive(true);

            while (!ballStop)
            {
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < balls.Length; i++)
                {
                    if (balls[i] != null)
                    {
                        if (balls[i].GetComponent<Rigidbody2D>().velocity.magnitude < 0.005f)
                        {
                            ballStop = true;
                        }
                        else
                        {
                            ballStop = false;
                            break;
                        }
                    }
                }
                if (cueBall.GetComponent<Rigidbody2D>().velocity.magnitude > 0.005f)
                {
                    ballStop = false;
                }
            }
            tables[currentTable].tableUI.takingShot.SetActive(false);
            List<int> fouls = GetFouls();
            players[playerActive].foulsTotal += fouls.Count;
            bool blackDown = false;
            if (fouls.Count > 0)
            {
                string foulText = "";
                //check if black potted
                for (int i = 0; i < fouls.Count; i++)
                {
                    foulText += TegridyPoolLanguage.poolFouls[fouls[i]] + "<br>";
                    if (fouls[i] == 4) blackDown = true;
                }
                tables[currentTable].tableUI.foulTitle.text = TegridyPoolLanguage.poolFoulTitle;
                tables[currentTable].tableUI.foulDescription.text = foulText;
                tables[currentTable].tableUI.foulScreen.SetActive(true);
                yield return new WaitForSeconds(3f);
                tables[currentTable].tableUI.foulScreen.SetActive(false);
                if (blackDown)
                {
                    EndMatch(1);
                }
                else AddFreeShot();
            }
            if(!blackDown)StartCoroutine(PlayerTurn());
        }
        private void AddFreeShot()
        {
            int nextPlayer = playerActive;
            nextPlayer++;
            if (nextPlayer == players.Length) nextPlayer = 0;
            players[playerActive].turnsRemaining = 0;
            players[nextPlayer].turnsRemaining++;
        }
        private List<int> GetFouls()
        {
            //foul 0 no foul
            //foul 1 no touch
            //foul 2 touched wrong colour
            //foul 3 potted wrong color
            //foul 4 potted final ball
            //foul 5 potted cue ball

            List<int> fouls = new List<int>();

            //check first touch
            if (touchedList.Count <= 0)
            {
                fouls.Add(1);
            }
            else if (colourSelected && players[playerActive].currentColour != touchedList[0] && players[playerActive].ballsLeft != 0)
            {
                fouls.Add(2);
            }
            //check potted balls for foul
            if (pottedlist.Count > 0 && colourSelected)
            {
                fouls = CheckPotted(fouls);
            }
            else if (pottedlist.Count > 0 && !colourSelected)
            {
                fouls = AssignColor(fouls);
            }
            return fouls;
        }
        private List<int> CheckPotted(List<int> currentFouls)
        {
            for (int i = 0; i < pottedlist.Count; i++)
            {
                if (pottedlist[i] == 99)
                {
                    currentFouls.Add(5);
                    ResetCueBall();
                }
                else if (pottedlist[i] == players[playerActive].currentColour)
                {
                    players[playerActive].ballsLeft--;
                    players[playerActive].turnsRemaining++;
                }
                else if (players[playerActive].ballsLeft <= 0 && pottedlist[i] == finalBall && i == pottedlist.Count - 1)
                {
                    EndMatch(2);
                }
                else if (players[playerActive].ballsLeft <= 0 && pottedlist[i] == finalBall)
                {
                    EndMatch(1);
                }

                else if (pottedlist[i] != players[playerActive].currentColour)
                {
                    if (finalBall == pottedlist[i]) currentFouls.Add(4);
                    else currentFouls.Add(3);
                }
            }
            return currentFouls;
        }
        private List<int> AssignColor(List<int> currentFouls)
        {
            for (int i = 0; i < pottedlist.Count; i++)
            {
                int ballCount = 1; //incase they get all balls on the first go 

                if (colourSelected)
                {
                    ballCount = 0;
                    foreach (TegridyPoolBall ball in balls)
                    {
                        if (ball != null && ball.ballID == players[playerActive].currentColour)
                        {
                            ballCount++;
                        }
                    }
                }

                if (pottedlist[i] == 99) { currentFouls.Add(5); ResetCueBall(); }
                else if (pottedlist[i] == finalBall && ballCount != 0) currentFouls.Add(4);
                else if (pottedlist[i] == finalBall && ballCount == 0 && i == pottedlist.Count - 1) EndMatch(2);
                else if (colourSelected == false)
                {
                    int ballgroup = 0;
                    for (int playerCount = 0; playerCount < players.Length; playerCount++)
                    {
                        if (playerCount == playerActive)
                        {
                            players[playerCount].currentColour = pottedlist[i];
                            players[playerCount].turnsRemaining++;
                        }
                        else
                        {
                            if (ballgroup == pottedlist[i]) ballgroup++;
                            if (ballgroup == players.Length) ballgroup = 0;
                            players[playerCount].currentColour = ballgroup;
                            ballgroup++;
                        }
                    }
                    colourSelected = true;
                }
            }
            return currentFouls;
        }
        private void UpdateBallsLeft()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].ballsLeft = 0;
                int count = 0;
                //set the images for reamining balls
                if (colourSelected)
                {
                    foreach (TegridyPoolBall ball in balls)
                    {
                        if (ball != null && players[i].currentColour == ball.ballID)
                        {

                            players[i].ballsLeft++;
                            tables[currentTable].tableUI.playerOverlays[i].remainingBalls[count].sprite = ball.GetComponent<SpriteRenderer>().sprite;
                            tables[currentTable].tableUI.playerOverlays[i].remainingBalls[count].color = ball.GetComponent<SpriteRenderer>().color;
                            tables[currentTable].tableUI.playerOverlays[i].remainingBalls[count].SetActive(true);
                            count++;
                        }
                    }
                    //disable whatever ball images are left over
                    for (int i2 = count; i2 < tables[currentTable].tableUI.playerOverlays[i].remainingBalls.Count; i2++)
                    {
                        tables[currentTable].tableUI.playerOverlays[i].remainingBalls[i2].SetActive(false);
                    }
                }
                else
                {
                    foreach (Image ball in tables[currentTable].tableUI.playerOverlays[i].remainingBalls)
                    {
                        ball.sprite = cueBall.GetComponent<SpriteRenderer>().sprite;
                        ball.color = cueBall.GetComponent<SpriteRenderer>().color;
                        ball.SetActive(true);
                    }
                }

                //update the gui
                tables[currentTable].tableUI.playerOverlays[i].playerName.text = players[i].playerName;
                tables[currentTable].tableUI.playerOverlays[i].turnsTotal.text = TegridyPoolLanguage.poolTurnsTaken + players[i].shotsTaken;
                tables[currentTable].tableUI.playerOverlays[i].turnsLeft.text = TegridyPoolLanguage.poolTurnsLeft + players[i].turnsRemaining;
                tables[currentTable].tableUI.playerOverlays[i].fouls.text = TegridyPoolLanguage.poolFoulsTotal + players[i].foulsTotal;
                tables[currentTable].tableUI.playerOverlays[i].ballsRemain.text = TegridyPoolLanguage.poolRemainingBalls + players[i].ballsLeft;

                tables[currentTable].tableUI.playerOverlays[i].playerTurnText.text = TegridyPoolLanguage.poolPlayerTurn;
                if (playerActive == i) tables[currentTable].tableUI.playerOverlays[i].playerTurn.SetActive(true);
                else tables[currentTable].tableUI.playerOverlays[i].playerTurn.SetActive(false);
            }
        }
        private void EndMatch(int reason)
        {
            Debug.Log("Add Player won stuff here - logs, prizes, etc. Reason #" + reason);

            string reasonText = players[playerActive].playerName + TegridyPoolLanguage.poolMatchOverReasons[reason];
            tables[currentTable].tableUI.foulTitle.text = TegridyPoolLanguage.poolMatchOverTitle;
            tables[currentTable].tableUI.foulDescription.text = reasonText;

            switch (reason)
            {
                case 0:
                    TegridyAudioTools.PlayOneShot(tables[currentTable].lost, source);
                    break;
                case 1:
                    TegridyAudioTools.PlayOneShot(tables[currentTable].lost, source);
                    break;
                case 2:
                    TegridyAudioTools.PlayOneShot(tables[currentTable].won, source);
                    break;
            }

            foreach (ActivePlayer player in players)
            {
                Destroy(player.playercue.gameObject);
            }
            players = null;
            Destroy(ballHolder);
            tables[currentTable].SetActive(false);

            //tell the coreroutine to end the match
            matchOver = true;
        }
        #endregion
    }
}
