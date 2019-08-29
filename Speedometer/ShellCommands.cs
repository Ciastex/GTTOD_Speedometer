using System.Collections.Generic;

namespace Speedometer
{
    public class ShellCommands
    {
        private static readonly List<string> _corners = new List<string> { "BL", "BR", "TL", "TR" };

        public static void ToggleSpeedometer(CommandTerminal.CommandArg[] args)
        {
            Mod.Settings[Mod.ShowMeterSettingsKey] = !Mod.Settings.GetItem<bool>(Mod.ShowMeterSettingsKey);
            Mod.Settings.Save();
        }

        public static void SetCorner(CommandTerminal.CommandArg[] args)
        {
            var corner = args[0].String;

            if (!_corners.Contains(corner))
            {
                CommandTerminal.Terminal.Log("Valid corner descriptors are BL, BR, TL and TR");
                return;
            }

            Mod.Settings[Mod.CornerSettingsKey] = corner;
            Mod.Settings.Save();
        }

        public static void Margins(CommandTerminal.CommandArg[] args)
        {
            var x = args[0].Float;
            var y = args[1].Float;

            Mod.Settings[Mod.MarginXSettingsKey] = x;
            Mod.Settings[Mod.MarginYSettingsKey] = y;

            Mod.Settings.Save();
        }
    }
}
