

namespace Dal;
/// <summary>
/// The class advances the run variable
/// </summary>
internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTask1Id { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTask1Id"); }
    internal static int NextDependeencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependeencyId"); }
}

