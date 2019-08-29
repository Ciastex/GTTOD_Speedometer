using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Storage;
using System;
using UnityEngine;

namespace Speedometer
{
    [ModEntryPoint(ModID)]
    public class Mod : MonoBehaviour
    {
        private const string ModID = "com.github.ciastex/Speedometer";

        internal const string ShowMeterSettingsKey = "ShowMeter";
        internal const string MarginXSettingsKey = "MarginX";
        internal const string MarginYSettingsKey = "MarginY";
        internal const string CornerSettingsKey = "Corner";

        private Font _font;
        private GUIStyle _style;
        private readonly AssetBundle _bundle = new Assets("speedometer.bndl").Bundle;

        internal static Settings Settings;

        public void Initialize(IManager manager)
        {
            SetUpSettings();

            _font = _bundle.LoadAsset<Font>("sunspire");

            _style = new GUIStyle()
            {
                fontStyle = FontStyle.Normal,
                normal = new GUIStyleState
                {
                    textColor = Color.white
                },
                font = _font,
                fontSize = 36
            };

            CommandTerminal.Terminal.Shell.AddCommand(
                "speedometer.toggle",
                ShellCommands.ToggleSpeedometer,
                0, 0,
                "Toggles speedometer on/off."
            );

            CommandTerminal.Terminal.Shell.AddCommand(
                "speedometer.corner",
                ShellCommands.SetCorner,
                1, 1,
                "Set preferred display corner (TL/TR/BL/BR)."
            );
            CommandTerminal.Terminal.Shell.AddCommand(
                "speedometer.margins",
                ShellCommands.Margins,
                2, 2,
                "Set spacing between display corner and speedometer text."
            );
        }

        public void OnGUI()
        {
            if (!Settings.GetItem<bool>(ShowMeterSettingsKey))
                return;

            var comp = FindObjectOfType<GTTODManager>();

            if (comp)
            {
                if (comp.Player.gameObject.activeInHierarchy && !comp.Player.isFrozen)
                {
                    var speed = comp.Player.PlayerPhysics.velocity.magnitude;

                    if (comp.Player.ParkourPlayer && comp.Player.ParkourPlayer.WallRunning)
                    {
                        speed = comp.Player.ParkourPlayer.PlayerPhysics.velocity.magnitude;
                    }

                    var content = new GUIContent(Math.Round(speed, 2).ToString());
                    var dims = _style.CalcSize(content);

                    float targetX, targetY;
                    switch (Settings.GetItem<string>(CornerSettingsKey))
                    {
                        case "TL":
                            targetX = Settings.GetItem<int>(MarginXSettingsKey);
                            targetY = Settings.GetItem<int>(MarginYSettingsKey);
                            break;

                        case "TR":
                            targetX = Screen.width - dims.x - Settings.GetItem<int>(MarginXSettingsKey);
                            targetY = Settings.GetItem<int>(MarginYSettingsKey);
                            break;

                        case "BR":
                            targetX = Screen.width - dims.x - Settings.GetItem<int>(MarginXSettingsKey);
                            targetY = Screen.height - dims.y - Settings.GetItem<int>(MarginYSettingsKey);
                            break;

                        case "BL":
                        default:
                            targetX = Settings.GetItem<int>(MarginXSettingsKey);
                            targetY = Screen.height - dims.y - Settings.GetItem<int>(MarginYSettingsKey);
                            break;
                    }

                    GUI.Label(
                        new Rect(targetX, targetY, dims.x, dims.y),
                        content,
                        _style
                    );
                }
            }
        }

        private void SetUpSettings()
        {
            Settings = new Settings("hud_settings");

            Settings.GetOrCreate(ShowMeterSettingsKey, true);
            Settings.GetOrCreate(MarginXSettingsKey, 16);
            Settings.GetOrCreate(MarginYSettingsKey, 16);
            Settings.GetOrCreate(CornerSettingsKey, "BL");

            if (Settings.Dirty)
                Settings.Save();
        }
    }
}
