using Sharpmake;
using System.Linq;

[module: Sharpmake.Include("config.sharpmake.cs")]

namespace Rex
{
  // Structure of the codebase
  // T:/<game_name>/source <-- folder with the source code
  // T:/<game_name>/include <-- all include files
  // T:/<game_name>/natvis <-- all natvis files
  // T:/<game_name>/src <-- all source files
  // T:/<game_name>/thirdparty <-- thirdparty files

  [Fragment, System.Flags]
  public enum Optimization
  {
    NoOpt = (1 << 0),
    FullOpRexithPdb = (1 << 1),
    FullOpt = (1 << 2)
  }

  public class GlobalDefinitions
  {
    public static readonly string GameName = "Game Name";
    public static readonly string VCInstallDir = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\";
    public static readonly string UnitTestInstallDir = VCInstallDir + @"\Auxiliary\VS\UnitTest\include\";
    public static readonly string RootDir = @"D:\\Rex\\game_name\\";

    public static readonly Options.Vc.General.WindowsTargetPlatformVersion WindowsSDKVersion = Options.Vc.General.WindowsTargetPlatformVersion.v10_0_18362_0;
  }

  public class RexTarget : ITarget
  {
    public DevEnv DevEnv;
    public Platform Platform;
    public Config Config;
    public RexTarget()
    { }
    public RexTarget(Platform platform, DevEnv devEnv, Config config)
    {
      DevEnv = devEnv;
      Platform = platform;
      Config = config;
    }

    // This is the display name of the configuration dropdown in visual studio
    public override string Name
    {
      get 
      {
        string config_str = string.Concat(Config.ToString().ToString().Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        return (config_str).ToLowerInvariant(); }
    }

    public Optimization Optimization
    {
      get { return ConfigManager.get_optimization_for_config(Config); }
    }
  }

  public class RexConfiguration : Sharpmake.Project.Configuration
  {
    public RexConfiguration()
    {
    }

    public void enable_exceptions()
    {
      Options.Add(Sharpmake.Options.Vc.Compiler.Exceptions.Enable);
      Defines.Add("_HAS_EXCEPTIONS=1");
    }

    public void disable_exceptions()
    {
      Options.Add(Sharpmake.Options.Vc.Compiler.Exceptions.Disable);
      Defines.Add("_HAS_EXCEPTIONS=0");
    }

    public void use_general_options()
    {
      Options.Add(Sharpmake.Options.Vc.General.JumboBuild.Enable);
      Options.Add(Sharpmake.Options.Vc.General.CharacterSet.MultiByte);
      Options.Add(Sharpmake.Options.Vc.General.PlatformToolset.v142);
      Options.Add(Sharpmake.Options.Vc.General.VCToolsVersion.v14_21_27702);
      Options.Add(Sharpmake.Options.Vc.General.WarningLevel.Level4);
      Options.Add(Sharpmake.Options.Vc.General.TreatWarningsAsErrors.Enable);
    }

    public void use_compiler_options()
    {
      Options.Add(Sharpmake.Options.Vc.Compiler.SupportJustMyCode.No); // this adds a call to __CheckForDebuggerJustMyCode into every function that slows down runtime significantly
      Options.Add(Sharpmake.Options.Vc.Compiler.CppLanguageStandard.Latest);
      Options.Add(Sharpmake.Options.Vc.Compiler.RTTI.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.RuntimeChecks.Default);
      Options.Add(Sharpmake.Options.Vc.Compiler.FloatingPointModel.Fast);
      Options.Add(Sharpmake.Options.Vc.Compiler.MultiProcessorCompilation.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.StringPooling.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.BufferSecurityCheck.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FunctionLevelLinking.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FloatingPointExceptions.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.OpenMP.Disable);
    }

    public void use_linker_options()
    {
      Options.Add(Sharpmake.Options.Vc.Linker.LargeAddress.SupportLargerThan2Gb);
      Options.Add(Sharpmake.Options.Vc.Linker.GenerateMapFile.Disable);
      Options.Add(Sharpmake.Options.Vc.Linker.TreatLinkerWarningAsErrors.Enable);
    }

    public void enable_optimization()
    {
      Options.Add(Sharpmake.Options.Vc.Compiler.Optimization.MaximizeSpeed);
      Options.Add(Sharpmake.Options.Vc.Compiler.Intrinsic.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.RuntimeLibrary.MultiThreaded);
      Options.Add(Sharpmake.Options.Vc.Compiler.Inline.AnySuitable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FiberSafe.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.RuntimeChecks.Default);

      Options.Add(Sharpmake.Options.Vc.Compiler.MinimalRebuild.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FavorSizeOrSpeed.FastCode);
      Options.Add(Sharpmake.Options.Vc.Compiler.FunctionLevelLinking.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.OmitFramePointers.Enable);

      Options.Add(Sharpmake.Options.Vc.Linker.LinkTimeCodeGeneration.UseLinkTimeCodeGeneration);
      Options.Add(Sharpmake.Options.Vc.Linker.EnableCOMDATFolding.RemoveRedundantCOMDATs);
      Options.Add(Sharpmake.Options.Vc.Linker.Reference.EliminateUnreferencedData);
      //Options.Add(Sharpmake.Options.Vc.Linker.Incremental.Enable);
    }

    public void disable_optimization()
    {
      Defines.Add("USING_DEBUG_RUNTIME_LIBS");

      Options.Add(Sharpmake.Options.Vc.Compiler.Optimization.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.Intrinsic.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.RuntimeLibrary.MultiThreadedDebug);
      Options.Add(Sharpmake.Options.Vc.Compiler.Inline.Default);
      Options.Add(Sharpmake.Options.Vc.Compiler.FiberSafe.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.RuntimeChecks.Both);
      Options.Add(Sharpmake.Options.Vc.Compiler.MinimalRebuild.Enable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FavorSizeOrSpeed.Neither);
      Options.Add(Sharpmake.Options.Vc.Compiler.OmitFramePointers.Disable);
      Options.Add(Sharpmake.Options.Vc.Compiler.FunctionLevelLinking.Enable);

      Options.Add(Sharpmake.Options.Vc.Linker.LinkTimeCodeGeneration.Default);
      Options.Add(Sharpmake.Options.Vc.Linker.EnableCOMDATFolding.DoNotRemoveRedundantCOMDATs);
      Options.Add(Sharpmake.Options.Vc.Linker.CreateHotPatchableImage.Enable);
      Options.Add(Sharpmake.Options.Vc.Linker.Incremental.Enable);
      //Options.Add(Sharpmake.Options.Vc.Linker.GenerateDebugInformation.Enable);
    }

    public void add_dependency<TPROJECT>(ITarget target)
    {
      AddPublicDependency<TPROJECT>(target, DependencySetting.Default | DependencySetting.Defines);
    }

    public void add_public_define(string define)
    {
      Defines.Add(define);
      ExportDefines.Add(define);
    }

    public void set_precomp_header(string projectFolderName, string preCompHeaderName)
    {
      PrecompHeader = projectFolderName + @"/" + preCompHeaderName + @".h";
      PrecompSource = preCompHeaderName + @".cpp";
    }
  }

  abstract class BaseProject : Sharpmake.Project
  {
    protected string SolutionPath;
    protected string SourcePath;
    protected string ProjectFolderName;
    protected string SolutionFolderName;

    public string IncludeFolderWithoutProject() { return SourcePath + @"include\" + SolutionFolderName + @"\"; }
    public string IncludeFolder() { return IncludeFolderWithoutProject() + ProjectFolderName; }
    public string SourceFolder() { return SourcePath + @"src\" + SolutionFolderName + @"\" + ProjectFolderName; }
    public string TempOutputFolder() { return SolutionPath + @"_temp\"; }

    public BaseProject() : base(typeof(RexTarget), typeof(RexConfiguration))
    {
      SolutionPath = @"[project.SharpmakeCsPath]\..\..\";
      SourcePath = SolutionPath + @"\source\";

      Platform platforms = Platform.win64;
      AddTargets(new RexTarget(platforms, DevEnv.vs2019, ConfigManager.get_all_configs()));
      //this.CustomProperties.Add("ShowAllFiles", "True");
    }

    public override bool ResolveFilterPathForFile(string relativeFilePath, out string filterPath)
    {
      string absolute_file_path = this.SourceRootPath + @"\" + relativeFilePath;
      absolute_file_path = System.IO.Path.GetFullPath(absolute_file_path);

      if (relativeFilePath.EndsWith(".h"))
      {
        filterPath = create_filter_path(absolute_file_path, "include");
        return true;
      }
      else if (relativeFilePath.EndsWith(".cpp"))
      {
        filterPath = create_filter_path(absolute_file_path, "src");
        return true;
      }

      filterPath = null;
      return false;
    }

    protected virtual void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      conf.SolutionFolder = SolutionFolderName; // eg. 1_common

      // Output folders:
      // SolutionDir/_Temp/ProjectName/Platform/Config/intermediate/
      // SolutionDir/_Temp/ProjectName/Platform/Config/bin/
      conf.IntermediatePath = TempOutputFolder() + SolutionFolderName + @"\" + ProjectFolderName + @"\[project.Name]\[target.Platform]\[target.Config]\intermediate\";
      conf.TargetPath = TempOutputFolder() + SolutionFolderName + @"\" + ProjectFolderName + @"\[target.Platform]\[target.Config]\bin\";

      conf.ProjectPath = ProjectPath;
      conf.ProjectName = Name;
      conf.ProjectFileName = conf.ProjectName;

      conf.VcxprojUserFile = new Configuration.VcxprojUserFileSettings();
      conf.VcxprojUserFile.LocalDebuggerWorkingDirectory = SolutionPath + @"data\";

      string pdb_file_name = Name + "_" + target.Config + "_" + target.Platform + ".pdb";

      conf.CompilerPdbFilePath = conf.TargetPath + "compiler_" + pdb_file_name;
      conf.LinkerPdbFilePath = conf.TargetPath + "linker_" + pdb_file_name;
      conf.TargetFileName = Name + "_" + target.Config + "_" + target.Platform;

      conf.IncludePaths.Add(IncludeFolderWithoutProject());

      conf.set_precomp_header(ProjectFolderName, Name.ToLowerInvariant() + "_pch");

      // Add Target to defines
      conf.Defines.Add("CA_" + target.Config.ToString().ToUpperInvariant() + "_CONFIG");
      conf.Defines.Add("CA_TARGETING_" + target.Platform.ToString().ToUpperInvariant());
      conf.Defines.Add("CONFIGURATION=" + target.Config.ToString());
      conf.Defines.Add("PLATFORM=" + target.Platform.ToString());
      conf.Defines.Add("SHORT_FILENAME_ROOT=" + GlobalDefinitions.RootDir);

      string file_path = GlobalDefinitions.RootDir + @"data\tools_data\config\";
      file_path += target.Config.ToString();
      file_path += ".macros";

      string macros;
      if (System.IO.File.Exists(file_path))
      {
        byte[] bytes = System.IO.File.ReadAllBytes(file_path);
        int start_offset = sizeof(Optimization);
        macros = System.Text.Encoding.Default.GetString(bytes);
        macros = macros.Remove(0, start_offset);
        System.Collections.Generic.List<string> splitted = macros.Split('\n').ToList();

        int global_index = splitted.FindIndex(x => x == "Global");
        int index = splitted.FindIndex(x => x == Name);

        add_macros(conf, splitted, global_index);
        add_macros(conf, splitted, index);

      }

      conf.AdditionalLinkerOptions.Add("/NODEFAULTLIB:library");

      conf.Options.Add(Options.Vc.Compiler.ConformanceMode.Enable);
      conf.MaxFilesPerUnityFile = 0; // all files added to 1 unity file by default, this could crash the compiler so be careful


      // This can't be used right now as we'll get warning emitted
      // because we use std in Rex
      // This should be enabled in the future though..
      conf.disable_exceptions();
      //conf.enable_exceptions();

      conf.use_general_options();
      conf.use_compiler_options();
      conf.use_linker_options();

      // Disable integer overflow
      // it's caused by the hash algorithm we use
      // conf.Options.Add(new Sharpmake.Options.Vc.Compiler.DisableSpecificWarnings("4307"));

      // Disable "symbol already defined in object; second definition ignored"
      // It's needed to set up project dependencies, and sharpmake "build order only" flag
      // doesn't work yet, so we have to do it like this.
      conf.Options.Add(new Sharpmake.Options.Vc.Linker.DisableSpecificWarnings("4006"));

      switch (target.Platform)
      {
        case Platform.win32:
          conf.Defines.Add("CA_TARGET_X86"); conf.Defines.Add("CA_TARGETING_WINDOWS"); break; // for backwards compatibility

        case Platform.win64:
          conf.Defines.Add("CA_TARGET_X64"); conf.Defines.Add("CA_TARGETING_WINDOWS"); break; // for backwards compatibility
      }

      switch(target.Optimization)
      {
        case Optimization.NoOpt:
          conf.Options.Add(Sharpmake.Options.Vc.General.DebugInformation.ProgramDatabase);
          conf.disable_optimization();
          break;
        case Optimization.FullOpRexithPdb:
          conf.Options.Add(Sharpmake.Options.Vc.General.DebugInformation.ProgramDatabase);
          conf.enable_optimization();
          conf.Options.Add(Sharpmake.Options.Vc.Linker.LinkTimeCodeGeneration.Default);      // To fix linker warning
          conf.Options.Add(Sharpmake.Options.Vc.Compiler.OmitFramePointers.Disable);         // Disable so we can have a stack trace
          break;
        case Optimization.FullOpt:
          conf.Options.Add(Sharpmake.Options.Vc.General.DebugInformation.Disable);
          conf.Options.Add(Sharpmake.Options.Vc.General.WholeProgramOptimization.Optimize);
          conf.enable_optimization();
          break;
      }

    }

    protected string ProjectPath
    {
      get { return SolutionPath + @"sharpmake/output"; }
    }

    private void add_macros(RexConfiguration conf, System.Collections.Generic.List<string> splitted, int start_index)
    {
      if (start_index == -1)
        return;

      for (int i = start_index + 1; i < splitted.Count; ++i)
      {
        string macro = splitted[i];
        if (macro.Length == 0)
          continue;

        if (macro[0] != '\t')
          break;

        macro = macro.Trim();
        conf.add_public_define(macro);
      }
    }

    private string create_filter_path(string absoluteFilePath, string prefix)
    {
      string directory_name = System.IO.Path.GetDirectoryName(absoluteFilePath);
      string solution_and_project_folder = (SolutionFolderName + @"\" + ProjectFolderName).ToLower();
      int pos = directory_name.ToLower().IndexOf(solution_and_project_folder) + solution_and_project_folder.Length + 1;
      pos = System.Math.Min(pos, directory_name.Length);
      return prefix + @"\" + directory_name.Substring(pos);
    }
  }

  class ThirdPartyProject : BaseProject
  {
    public ThirdPartyProject() : base()
    {
      SolutionFolderName = "0_thirdparty";
    }

    public override bool ResolveFilterPathForFile(string relativeFilePath, out string filterPath)
    {
      filterPath = null;
      return false;
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);

      conf.Options.Add(Sharpmake.Options.Vc.General.WarningLevel.Level3);
      conf.Options.Add(Sharpmake.Options.Vc.General.TreatWarningsAsErrors.Disable);

      conf.IncludePaths.Add(SourcePath + @"thirdparty\" + ProjectFolderName);

      conf.Output = Configuration.OutputType.Lib;
    }
  }

  class CommonProject : BaseProject
  {
    public CommonProject() : base()
    {
      SolutionFolderName = "1_core";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);

      conf.Output = Configuration.OutputType.Lib;
    }

  }
  class CoreProject : BaseProject
  {
    public CoreProject() : base()
    {
      SolutionFolderName = "2_engine";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);

      conf.Output = Configuration.OutputType.Lib;
    }
  }
  class PlatformProject : BaseProject
  {
    public PlatformProject() : base()
    {
      SolutionFolderName = "3_platform";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);

      conf.Output = Configuration.OutputType.Lib;
    }
  }
  class PlatformAppsProject : BaseProject
  {
    public PlatformAppsProject() : base()
    {
      SolutionFolderName = "4_platform_apps";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);

      conf.Output = Configuration.OutputType.Lib;
    }
  }
  class GameProjectBase : BaseProject
  {
    public GameProjectBase() : base()
    {
      SolutionFolderName = "5_game";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);
    }
  }

  class ToolsProject : BaseProject
  {
    public ToolsProject() : base()
    {
      SolutionFolderName = "6_tools";
    }

    protected override void configure_defaults(RexConfiguration conf, RexTarget target)
    {
      base.configure_defaults(conf, target);
    }
  }
}