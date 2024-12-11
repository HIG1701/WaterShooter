using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject moveRangeIndicatorPrefab;
    [SerializeField] private MapDateBase mapDate;
    [SerializeField] private MapManager mapManager;

    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    public List<GameObject> moveRangeIndicators = new List<GameObject>();

    public void MovementRangeRender(Vector2Int characterPosition, CharactersDates moveRange, Character characterSystem)
    {
        int i = 0;
        int j = 0;
        int k = 3;
        ClearMovementRangeIndicators();
        characterSystem.rangeTileList.Clear();
        characterSystem.tileIntNumber.Clear();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();

        queue.Enqueue(characterPosition);
        distances[characterPosition] = 0;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = distances[current];

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (distances.ContainsKey(neighbor))
                    continue;

                if (Mathf.Abs(neighbor.x - characterPosition.x) + Mathf.Abs(neighbor.y - characterPosition.y) <= moveRange.date.movementRange
                    && mapDate.tileList.Contains(neighbor))
                {
                    int newDistance = currentDistance + 1;
                    if (newDistance <= moveRange.date.movementRange)
                    {


                        queue.Enqueue(neighbor);
                        distances[neighbor] = newDistance;
                        Vector3 worldPosition = new Vector3(neighbor.x, neighbor.y, 0);
                        GameObject indicator = Instantiate(moveRangeIndicatorPrefab, worldPosition, Quaternion.identity);
                        moveRangeIndicators.Add(indicator);

                        if (characterSystem.rangeTileList.Count < moveRangeIndicators.Count)
                            if (j < k)
                            {

                                characterSystem.tileIntNumber.Add(i);
                                j++;
                            }
                            else if (j == k)
                            {
                                characterSystem.tileIntNumber.Add(i);
                                i++;
                                k += 4;
                                j = 0;
                            }
                        {
                            characterSystem.rangeTileList.Add(neighbor);
                        }
                    }
                }
                else
                {
                    if (characterSystem.rangeTileList.Count < moveRangeIndicators.Count)
                        if (j < k)
                        {

                            characterSystem.tileIntNumber.Add(-1);
                            j++;
                        }
                        else if (j == k)
                        {
                            characterSystem.tileIntNumber.Add(-1);
                            i++;
                            k += 4;
                            j = 0;
                        }
                }
            }
        }
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(position.x + 1, position.y),
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x, position.y + 1),
            new Vector2Int(position.x, position.y - 1)
        };

        return neighbors;
    }

    public void ClearMovementRangeIndicators()
    {
        foreach (var indicator in moveRangeIndicators)
        {
            Destroy(indicator);
        }
        moveRangeIndicators.Clear();
    }
}