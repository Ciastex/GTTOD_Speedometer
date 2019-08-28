using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Storage;
using System;
using UnityEngine;

namespace Speedometer
{
    [ModEntryPoint("com.github.ciastex/Speedometer")]
    public class Mod : MonoBehaviour
    {
        private Font _font;
        private GUIStyle _style;

        private float _margin = 16;

        private AssetBundle _bundle = new Assets("speedometer.bndl").Bundle;

        public void Initialize(IManager manager)
        {
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
        }

        public void OnGUI()
        {
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

                    var targetX = 0 + _margin;
                    var targetY = Screen.height - dims.y - _margin;

                    GUI.Label(
                        new Rect(targetX, targetY, dims.x, dims.y),
                        //new Rect(100, 100, dims.x, dims.y),
                        content,
                        _style
                    );
                }
            }
        }
    }
}
