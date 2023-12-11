using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private List<Color> generatedColors = new List<Color>();
    private int runCounter = 0;
    private int alternationCounter = 0;
    private float minColorDistance = 0.7f;

    public Dictionary<string, Color> GenerateColors(HashSet<string> activities)
    {
        Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>();
        foreach (var id in activities)
        {
            /*
            Color newColor;
            do
            {
                newColor = GenerateColor();
            } while (!IsColorSufficientlyDistant(newColor));
            */
            Color newColor = GenerateColor();

            colorDictionary[id] = newColor;
            generatedColors.Add(newColor);

            runCounter++;
            if (runCounter % 3 == 0)
            {
                alternationCounter++;
            }
        }

        return colorDictionary;        
    }

    private Color GenerateColor()
    {
        float r, g, b;
        r = g = b = 0;

        switch (runCounter % 3)
        {
            case 0: // R is 0
                g = alternationCounter % 2 == 0 ? 1f : Random.Range(0, 256) / 255f;
                b = alternationCounter % 2 == 1 ? 1f : Random.Range(0, 256) / 255f;
                break;
            case 1: // G is 0
                r = alternationCounter % 2 == 0 ? 1f : Random.Range(0, 256) / 255f;
                b = alternationCounter % 2 == 1 ? 1f : Random.Range(0, 256) / 255f;
                break;
            case 2: // B is 0
                r = alternationCounter % 2 == 0 ? 1f : Random.Range(0, 256) / 255f;
                g = alternationCounter % 2 == 1 ? 1f : Random.Range(0, 256) / 255f;
                break;
        }

        return new Color(r, g, b);
    }

    private bool IsColorSufficientlyDistant(Color color)
    {
        foreach (Color existingColor in generatedColors)
        {
            if (Vector3.Distance(new Vector3(color.r, color.g, color.b),
                                 new Vector3(existingColor.r, existingColor.g, existingColor.b)) < minColorDistance)
            {
                return false;
            }
        }
        return true;
    }
}
