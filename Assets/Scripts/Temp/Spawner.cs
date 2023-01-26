using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    bool horizontalSplit = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitGame()
    {
        for (int i = 0; i < MyInputController.PlayerConfigList.Count; i++)
        {
            Spawn(MyInputController.PlayerConfigList[i]);
        }
            UpdateSplitScreen();
    }


    public void Spawn(PlayerConfiguration playerConfig)
    {
        GameObject tmpPlayer = Instantiate(playerPrefab);
        GameManager.Instance.PlayerList.Add(tmpPlayer);
        PlayerController tmpPlayerController = tmpPlayer.GetComponent<PlayerController>();
        tmpPlayerController.Setup(playerConfig);
        UpdateSplitScreen();
    }

    public void UpdateSplitScreen()
    {
        int playerCount = GameManager.Instance.PlayerList.Count;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerController playerController = GameManager.Instance.PlayerList[i].GetComponent<PlayerController>();
            int maxColumn = Mathf.CeilToInt(Mathf.Sqrt(playerCount));
            int maxRow = Mathf.CeilToInt(playerCount / (maxColumn * 1.0f)); // 3/2 = 1.5f -> 2

            if (horizontalSplit)
            {
                int tmpSize = maxColumn;
                maxColumn = maxRow;
                maxRow = tmpSize;
            }

            float camWidth = 1.0f / maxColumn;
            float camHeight = 1.0f / maxRow;

            float xPos = (i % maxColumn) * camWidth;
            float yPos = (Mathf.FloorToInt(i / maxColumn)) * camHeight; // TODO: Start from the bottom

            Rect camRect = new Rect(xPos, yPos, camWidth, camHeight);

            playerController.UpdateScreen(camRect);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
