using FirClient.Component;
using FirClient.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AStarDebugger : MonoBehaviour
{
    private IAStar mInvoker = null;
    private GameObject debugTextPrefab = null;


    public void Initialize(IAStar aStar)
    {
        mInvoker = aStar;
        debugTextPrefab = Resources.Load<GameObject>("Prefabs/AStarDebugItem");
    }

    public void CreateTiles(Dictionary<Vector3Int, Node> allNode, Vector3Int[] finalPath, Vector3Int start, Vector3Int goal)
    {
        if (debugTextPrefab != null)
        {
            foreach (var item in allNode)
            {
                var gameObj = Instantiate<GameObject>(debugTextPrefab);
                gameObj.name = item.Value.Position.ToString();
                gameObj.transform.SetParent(transform);
                gameObj.transform.localScale = Vector3.one;
                gameObj.transform.position = mInvoker.GetCellPos(item.Key);

                var color = Color.blue;
                foreach(var pos in finalPath)
                {
                    if (pos == item.Key)
                    {
                        color = Color.green;
                    }
                }
                if (item.Key == start)
                {
                    color = Color.yellow;
                    gameObj.GetChild<TextMeshProUGUI>("T").text = "start";
                }
                else if (item.Key == goal)
                {
                    color = Color.red;
                    gameObj.GetChild<TextMeshProUGUI>("T").text = "goal";
                }
                gameObj.GetComponent<Image>().color = color;

                gameObj.GetChild<TextMeshProUGUI>("G").text = string.Format("G:{0}", item.Value.G);
                gameObj.GetChild<TextMeshProUGUI>("H").text = string.Format("H:{0}", item.Value.H);
                gameObj.GetChild<TextMeshProUGUI>("F").text = string.Format("F:{0}", item.Value.F);
                gameObj.GetChild<TextMeshProUGUI>("P").text = string.Format("P:{0},{1}", item.Value.Position.x, item.Value.Position.y);
            }
        }
    }

    public void Reset()
    {
        transform.ClearChild();
    }
}
