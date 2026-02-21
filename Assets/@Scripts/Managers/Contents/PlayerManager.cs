using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public int Level;
    public double Exp;
    public double Atk = 10;
    public double Hp = 50;

    public void ExpUp()
    {
        Exp += float.Parse(CSVImporter.EXP[Level]["Get_EXP"].ToString());
        if (Exp >= float.Parse(CSVImporter.EXP[Level]["EXP"].ToString()))
        {
            Level++;
            UI_Main.instance.TextCheck();
        }
    }

    public float ExpPercentage()
    {
        float exp = float.Parse(CSVImporter.EXP[Level]["EXP"].ToString());
        double myExp = Exp;

        if (Level >= 1)
        {
            exp -= float.Parse(CSVImporter.EXP[Level - 1]["EXP"].ToString());
            myExp -= float.Parse(CSVImporter.EXP[Level - 1]["EXP"].ToString());
        }

        return (float)myExp / exp;
    }

    public float NextExp()
    {
        float exp = float.Parse(CSVImporter.EXP[Level]["EXP"].ToString());
        float myExp = float.Parse(CSVImporter.EXP[Level]["Get_EXP"].ToString());

        if (Level >= 1)
        {
            exp -= float.Parse(CSVImporter.EXP[Level - 1]["EXP"].ToString());
        }

        return (myExp / exp) * 100.0f;
    }

    public float NextAtk()
    {
        return float.Parse(CSVImporter.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    public double NextHp()
    {
        return float.Parse(CSVImporter.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
    }
}
