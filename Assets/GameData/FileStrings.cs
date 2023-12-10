using System;
using System.IO;
using UnityEngine.SceneManagement;
static class FileStrings
{
    // SCENE
    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // GAMEDATA
    public static string GameDataPath = "GameData";
    public static string GraphFileExtension = ".xml";
    public static string JsonExtension = ".json";

    // ENVIRONMENT
    public static string SkyBoxesPath = "Environment/Skyboxes";
    public static string EnvironmentLightsPath = "Environment/Lights";
    public static string SkyBoxAllClearName = "Daytime";
    public static string SkyBoxPendingName = "Nighttime";
    public static string EnvironmentLightsAllClearName = "Sunlight";
    public static string EnvironmentLightsPendingName = "Moonlight";

    // ACTIVITY
    private static string ButtonObject = "ButtonObject";
    public static string ButtonOpaque = "ButtonOpaque";
    public static string ButtonTransparent= "ButtonTransparent";
    public static string ButtonPath = ButtonObject+"/"+ButtonOpaque;
    public static string ButtonTransparentPath = ButtonObject+"/"+ButtonTransparent;
    private static string SceneryContainer = "SceneryContainer";
    public static string SceneryOpaque = "SceneryOpaque";
    public static string SceneryTransparent = "SceneryTransparent";
    public static string SceneryPath = SceneryContainer+"/"+SceneryOpaque;
    public static string SceneryTransparentPath = SceneryContainer+"/"+SceneryTransparent;
    private static string PushButton = "PushButton";
    public static string PushButtonPath = ButtonPath+"/"+PushButton;

    // EFFECTSCONTAINER
    public static string Glitter = "Glitter";
    public static string GlitterBurst = "GlitterBurst";
    public static string SceneryLight = "SceneryLight";
    public static string PushButtonLight = "PushButtonLight";
    public static string Fog = "Fog";
    public static string Firework = "Firework";

    // RESOURCES
    private static string Environment = "Environment";
    private static string Materials = "Materials";
    private static string Common = "Common";
    private static string ButtonGreen = "ButtonGreen";
    private static string Transparent = "Transparent";
    private static string ButtonGreenEmission = "ButtonGreenEmission";
    public static string ButtonGreenPath = Materials+"/"+Common+"/"+ButtonGreen;
    public static string ButtonGreenEmissionPath = Materials+"/"+Common+"/"+ButtonGreenEmission;
    public static string TransparentPath = Materials+"/"+Common+"/"+Transparent;

    
    // ACTIVITY - BUTTONS CONTAINER OBJECTS
    public static string PendingTrueGlitter = "PendingTrueEffectsContainer/";
}