using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    public Dictionary<string, UnityEngine.Color> GenerateColors(HashSet<string> activities)
    {
        Dictionary<string, UnityEngine.Color> colorMap = new Dictionary<string, UnityEngine.Color>();
        int counter = UnityEngine.Random.Range(0, 4); // Randomized starting value

        foreach (var activity in activities)
        {
            UnityEngine.Color color;
            switch (counter)
            {
                case 0:
                    color = new UnityEngine.Color(0, 1, RandomRange(0.5f, 1));
                    break;
                case 1:
                    color = new UnityEngine.Color(0, RandomRange(0, 0.5f), 1);
                    break;
                case 2:
                    color = new UnityEngine.Color(0, RandomRange(0.5f, 1), 1);
                    break;
                case 3:
                    color = new UnityEngine.Color(RandomRange(0, 0.5f), 0, 1);
                    break;
                default:
                    throw new InvalidOperationException("Invalid counter state");
            }

            colorMap.Add(activity, color);
            counter = (counter + 1) % 4;
        }

        return colorMap;
    }

    private float RandomRange(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

/*
    public Dictionary<string, Color> GenerateColors(HashSet<string> activities)
    {
        Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>();
        foreach (var id in activities)
        {
            Color newColor = GenerateColor();
            AdjustColor(ref newColor); // Adjust the color as per new conditions

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
            b = (g < 1f) ? Random.Range(0, 256) / 255f : Random.Range(1, 256) / 255f; // Ensure B is not 0 when G is 255
            break;
        case 1: // G is 0
            r = alternationCounter % 2 == 0 ? 1f : Random.Range(0, 256) / 255f;
            b = (r < 1f) ? Random.Range(0, 256) / 255f : Random.Range(1, 256) / 255f; // Ensure B is not 0 when R is 255
            break;
        case 2: // B is 0
            r = alternationCounter % 2 == 0 ? 1f : Random.Range(0, 256) / 255f;
            g = alternationCounter % 2 == 1 ? 1f : Random.Range(0, 256) / 255f;
            if (r == 1f || g == 1f) b = Random.Range(1, 256) / 255f; // Ensure B is not 0 when R or G is 255
            break;
    }

    return new Color(r, g, b);
}


    private void AdjustColor(ref Color color)
    {
        if (color.r == 1f && color.g == 0f) // R is 255 and G is 0
            color.b = Mathf.Max(color.b, 128f / 255f);
        if (color.g == 1f && color.r == 0f) // G is 255 and R is 0
            color.b = Mathf.Max(color.b, 128f / 255f);
        if (color.b == 1f && color.r == 0f) // B is 255 and R is 0
            color.g = Mathf.Max(color.g, 0f / 255f);
        if (color.b == 1f && color.g == 0f) // B is 255 and G is 0
            color.r = Mathf.Max(color.r, 0f / 255f); 
    }
    // B is 255 and G is 0
*/
/*
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
    }*/
}
