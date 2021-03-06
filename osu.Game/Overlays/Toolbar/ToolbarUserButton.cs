﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Online.API;
using osu.Game.Users;
using osu.Game.Users.Drawables;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Overlays.Toolbar
{
    public class ToolbarUserButton : ToolbarOverlayToggleButton, IOnlineComponent
    {
        private readonly UpdateableAvatar avatar;

        public ToolbarUserButton()
        {
            TooltipMain = "我";
            TooltipSub = $"在这里查看个人信息";

            AutoSizeAxes = Axes.X;

            DrawableText.Font = OsuFont.GetFont(size:18, italics: true);

            Add(new OpaqueBackground { Depth = 1 });

            Flow.Add(avatar = new UpdateableAvatar
            {
                Masking = true,
                Size = new Vector2(32),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                CornerRadius = 4,
                OpenOnClick = { Value = false },
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 4,
                    Colour = Color4.Black.Opacity(0.1f),
                }
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load(IAPIProvider api, LoginOverlay login)
        {
            api.Register(this);

            StateContainer = login;
        }

        public void APIStateChanged(IAPIProvider api, APIState state)
        {
            switch (state)
            {
                default:
                    Text = @"游客";
                    avatar.User = new User();
                    break;

                case APIState.Online:
                    Text = $"别来无恙, {api.LocalUser.Value.Username} !";
                    avatar.User = api.LocalUser.Value;
                    break;
            }
        }
    }
}
