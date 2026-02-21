using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVImporter
{
    public static List<Dictionary<string, object>> EXP = new List<Dictionary<string, object>>(CSVReader.Read("EXP"));

}
