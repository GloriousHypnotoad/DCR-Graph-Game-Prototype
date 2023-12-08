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
    private static string Button = "Button";
    private static string ButtonTransparent= "ButtonTransparent";
    public static string ButtonPath = ButtonObject+"/"+Button;
    public static string ButtonTransparentPath = ButtonObject+"/"+ButtonTransparent;
    private static string SceneryContainer = "SceneryContainer";
    private static string Scenery = "Scenery";
    private static string SceneryTransparent = "SceneryTransparent";
    public static string SceneryPath = SceneryContainer+"/"+Scenery;
    public static string SceneryTransparentPath = SceneryContainer+"/"+SceneryTransparent;
    private static string EffectsContainer = "EffectsContainer";
    private static string Disabled = "Disabled";
    private static string Executed = "Executed";
    private static string GlitterDisabledFalse = "GlitterDisabledFalse";
    private static string LightExecutedTrue = "LightExecutedTrue";
    private static string FogDisabledTrue = "FogDisabledTrue";
    public static string GlitterPath = EffectsContainer+"/"+Disabled+"/"+GlitterDisabledFalse;
    public static string LightPath = EffectsContainer+"/"+Executed+"/"+LightExecutedTrue;
    public static string FogPath = EffectsContainer+"/"+Disabled+"/"+FogDisabledTrue;
    private static string PushButton = "PushButton";
    public static string PushButtonPath = ButtonPath+"/"+PushButton;

    // RESOURCES
    private static string Environment = "Environment";
    private static string Materials = "Materials";
    private static string Common = "Common";
    private static string ButtonGreen = "ButtonGreen";
    private static string ButtonGreenEmission = "ButtonGreenEmission";
    public static string ButtonGreenPath = Materials+"/"+Common+"/"+ButtonGreen;
    public static string ButtonGreenEmissionPath = Materials+"/"+Common+"/"+ButtonGreenEmission;

    
    // ACTIVITY - BUTTONS CONTAINER OBJECTS
    public static string PendingTrueGlitter = "PendingTrueEffectsContainer/";
}