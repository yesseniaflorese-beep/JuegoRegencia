using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChoiceData
{
    public string id;
    public string text;

    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public string gotoLabel;
}