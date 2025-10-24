using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;

namespace CollectiveMind.Ladybug.Editor
{
  public class Builder
  {
    private static readonly string Eol = Environment.NewLine;

    private static readonly string[] Secrets = { "androidKeystorePass", "androidKeyaliasName", "androidKeyaliasPass" };

    public static void Build()
    {
      Dictionary<string, string> options = GetValidatedOptions();

      if (options.TryGetValue("buildVersion", out string buildVersion) && buildVersion != "none")
      {
        PlayerSettings.bundleVersion = buildVersion;
        PlayerSettings.macOS.buildNumber = buildVersion;
      }

      if (options.TryGetValue("androidVersionCode", out string versionCode) && versionCode != "0")
      {
        PlayerSettings.Android.bundleVersionCode = int.Parse(options["androidVersionCode"]);
      }

      var buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]);
      switch (buildTarget)
      {
        case BuildTarget.Android:
        {
          EditorUserBuildSettings.buildAppBundle = options["customBuildPath"].EndsWith(".aab");
          if (options.TryGetValue("androidKeystoreName", out string keystoreName) &&
            !string.IsNullOrEmpty(keystoreName))
          {
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keystoreName;
          }

          if (options.TryGetValue("androidKeystorePass", out string keystorePass) &&
            !string.IsNullOrEmpty(keystorePass))
            PlayerSettings.Android.keystorePass = keystorePass;
          if (options.TryGetValue("androidKeyaliasName", out string keyaliasName) &&
            !string.IsNullOrEmpty(keyaliasName))
            PlayerSettings.Android.keyaliasName = keyaliasName;
          if (options.TryGetValue("androidKeyaliasPass", out string keyaliasPass) &&
            !string.IsNullOrEmpty(keyaliasPass))
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
          if (options.TryGetValue("androidTargetSdkVersion", out string androidTargetSdkVersion) &&
            !string.IsNullOrEmpty(androidTargetSdkVersion))
          {
            var targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            try
            {
              targetSdkVersion =
                (AndroidSdkVersions)Enum.Parse(typeof(AndroidSdkVersions), androidTargetSdkVersion);
            }
            catch
            {
              UnityEngine.Debug.Log("Failed to parse androidTargetSdkVersion! Fallback to AndroidApiLevelAuto");
            }

            PlayerSettings.Android.targetSdkVersion = targetSdkVersion;
          }

          break;
        }
        case BuildTarget.StandaloneOSX:
          PlayerSettings.SetScriptingBackend(NamedBuildTarget.FromBuildTargetGroup(BuildTargetGroup.Standalone),
            ScriptingImplementation.Mono2x);
          break;
      }

      int buildSubtarget = 0;
      if (!options.TryGetValue("standaloneBuildSubtarget", out string subtargetValue)
        || !Enum.TryParse(subtargetValue, out StandaloneBuildSubtarget buildSubtargetValue))
      {
        buildSubtargetValue = default(StandaloneBuildSubtarget);
      }

      buildSubtarget = (int)buildSubtargetValue;

      Build(buildTarget, buildSubtarget, options["customBuildPath"], CreateBuildOptions(options));
    }

    public static void BuildWithProfile()
    {
      Dictionary<string, string> options = GetValidatedOptions();

      var buildProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>(options["activeBuildProfile"]);

      BuildProfile.SetActiveBuildProfile(buildProfile);
      BuildOptions buildOptions = CreateBuildOptions(options);

      var buildPlayerWithProfileOptions = new BuildPlayerWithProfileOptions
      {
        buildProfile = buildProfile,
        locationPathName = options["customBuildPath"],
        options = buildOptions,
      };

      BuildSummary buildSummary = BuildPipeline.BuildPlayer(buildPlayerWithProfileOptions).summary;
      ReportSummary(buildSummary);
      ExitWithResult(buildSummary.result);
    }

    private static BuildOptions CreateBuildOptions(Dictionary<string, string> options)
    {
      var buildOptions = BuildOptions.None;
      foreach (string buildOptionString in Enum.GetNames(typeof(BuildOptions)))
      {
        if (options.ContainsKey(buildOptionString))
        {
          BuildOptions buildOptionEnum = (BuildOptions)Enum.Parse(typeof(BuildOptions), buildOptionString);
          buildOptions |= buildOptionEnum;
        }
      }

      return buildOptions;
    }

    private static Dictionary<string, string> GetValidatedOptions()
    {
      ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

      if (!validatedOptions.TryGetValue("projectPath", out string _))
      {
        Console.WriteLine("Missing argument -projectPath");
        EditorApplication.Exit(110);
      }

      if (validatedOptions.TryGetValue("buildTarget", out var buildTarget))
      {
        if (!Enum.IsDefined(typeof(BuildTarget), buildTarget ?? string.Empty))
        {
          Console.WriteLine($"{buildTarget} is not a defined {nameof(BuildTarget)}");
          EditorApplication.Exit(121);
        }
      }
      else if (!validatedOptions.TryGetValue("activeBuildProfile", out string _))
      {
        Console.WriteLine("Missing argument -buildTarget or -activeBuildProfile");
        EditorApplication.Exit(120);
      }

      if (!validatedOptions.TryGetValue("customBuildPath", out string _))
      {
        Console.WriteLine("Missing argument -customBuildPath");
        EditorApplication.Exit(130);
      }

      const string defaultCustomBuildName = "TestBuild";
      if (!validatedOptions.TryGetValue("customBuildName", out string customBuildName))
      {
        Console.WriteLine($"Missing argument -customBuildName, defaulting to {defaultCustomBuildName}.");
        validatedOptions.Add("customBuildName", defaultCustomBuildName);
      }
      else if (customBuildName == "")
      {
        Console.WriteLine($"Invalid argument -customBuildName, defaulting to {defaultCustomBuildName}.");
        validatedOptions.Add("customBuildName", defaultCustomBuildName);
      }

      return validatedOptions;
    }

    private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
    {
      providedArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      string[] args = Environment.GetCommandLineArgs();

      Console.WriteLine(
        $"{Eol}" +
        $"###########################{Eol}" +
        $"#    Parsing settings     #{Eol}" +
        $"###########################{Eol}" +
        $"{Eol}"
      );

      // Extract flags with optional values
      for (int current = 0, next = 1; current < args.Length; current++, next++)
      {
        // Parse flag
        bool isFlag = args[current].StartsWith("-");
        if (!isFlag) continue;
        string flag = args[current].TrimStart('-');

        // Parse optional value
        bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
        string value = flagHasValue ? args[next].TrimStart('-') : "";
        bool secret = Secrets.Contains(flag);
        string displayValue = secret ? "*HIDDEN*" : "\"" + value + "\"";

        // Assign
        Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
        providedArguments.Add(flag, value);
      }
    }

    private static void Build(BuildTarget buildTarget, int buildSubtarget, string filePath, BuildOptions buildOptions)
    {
      string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();
      var buildPlayerOptions = new BuildPlayerOptions
      {
        scenes = scenes,
        target = buildTarget,
        targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget),
        locationPathName = filePath,
        options = buildOptions,
        subtarget = buildSubtarget
      };

      BuildSummary buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
      ReportSummary(buildSummary);
      ExitWithResult(buildSummary.result);
    }

    private static void ReportSummary(BuildSummary summary)
    {
      Console.WriteLine(
        $"{Eol}" +
        $"###########################{Eol}" +
        $"#      Build results      #{Eol}" +
        $"###########################{Eol}" +
        $"{Eol}" +
        $"Duration: {summary.totalTime.ToString()}{Eol}" +
        $"Warnings: {summary.totalWarnings.ToString()}{Eol}" +
        $"Errors: {summary.totalErrors.ToString()}{Eol}" +
        $"Size: {summary.totalSize.ToString()} bytes{Eol}" +
        $"{Eol}"
      );
    }

    private static void ExitWithResult(BuildResult result)
    {
      switch (result)
      {
        case BuildResult.Succeeded:
          Console.WriteLine("Build succeeded!");
          EditorApplication.Exit(0);
          break;
        case BuildResult.Failed:
          Console.WriteLine("Build failed!");
          EditorApplication.Exit(101);
          break;
        case BuildResult.Cancelled:
          Console.WriteLine("Build cancelled!");
          EditorApplication.Exit(102);
          break;
        case BuildResult.Unknown:
        default:
          Console.WriteLine("Build result is unknown!");
          EditorApplication.Exit(103);
          break;
      }
    }
  }
}