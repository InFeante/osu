﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Configuration;

namespace osu.Game.Screens.Play.PlayerSettings
{
    public class InputSettings : PlayerSettingsGroup
    {
        protected override string Title => "输入设置";

        private readonly PlayerCheckbox mouseButtonsCheckbox;

        public InputSettings()
        {
            Children = new Drawable[]
            {
                mouseButtonsCheckbox = new PlayerCheckbox
                {
                    LabelText = "禁用鼠标按键"
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config) => mouseButtonsCheckbox.Current = config.GetBindable<bool>(OsuSetting.MouseDisableButtons);
    }
}
