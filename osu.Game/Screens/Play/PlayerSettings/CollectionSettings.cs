﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays.Music;

namespace osu.Game.Screens.Play.PlayerSettings
{
    public class CollectionSettings : PlayerSettingsGroup
    {
        protected override string Title => @"收藏";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = @"将这首歌添加进",
                },
                new CollectionsDropdown<PlaylistCollection>
                {
                    RelativeSizeAxes = Axes.X,
                    Items = new[] { PlaylistCollection.All },
                },
            };
        }
    }
}
