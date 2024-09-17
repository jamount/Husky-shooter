using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;

    public TileHelper tileHelper;
    public int startingLength = 8;
    private int length = 8;
    private float angle = 90;
    private bool waitingForTheRoad = false;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        CreatLevel();
    }

    public void CreatLevel()
    {
        length = startingLength;
        tileHelper.Reset();
        var sequence = lsystem.GenerateSentence();
        Debug.Log(sequence);
        StartCoroutine(VisualizeSequence(sequence));
    }

    private IEnumerator VisualizeSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        var startingPositions = tileHelper.PlaceMiddleTile(Vector3Int.zero);

        var currentPosition = (Vector3)startingPositions.Item1;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;


        foreach (var letter in sequence)
        {
            if (waitingForTheRoad)
            {
                yield return new WaitForEndOfFrame();
            }
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("Dont have saved point in our stack");
                    }
                    break;
                case EncodingLetters.draw_static:
                case EncodingLetters.draw_dynamic:
                    tempPosition = currentPosition;
                    currentPosition += direction * length * 2;
                    tileHelper.PlaceTilePositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                    tileHelper.PlaceTilePositions(tempPosition * -1, Vector3Int.RoundToInt(direction) * -1, length);

                    waitingForTheRoad = true;

                    //Length -= 2;
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
        tileHelper.FixRoad();
    }
}