﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Framework.Graphics.Shapes;
//由于第18行有"Container",和翻译需要的System.ComponentModel冲突,故无法翻译
namespace osu.Game.Screens.Select
{
    public class BeatmapDetailAreaTabControl : Container
    {
        public static readonly float HEIGHT = 24;
        private readonly OsuTabControlCheckbox modsCheckbox;
        private readonly OsuTabControl<BeatmapDetailTab> tabs;
        private readonly Container tabsContainer;

        public Action<BeatmapDetailTab, bool> OnFilter; //passed the selected tab and if mods is checked

        private Bindable<BeatmapDetailTab> selectedTab;

        public BeatmapDetailAreaTabControl()
        {
            Height = HEIGHT;

            Children = new Drawable[]
            {
                new Box
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Height = 1,
                    Colour = Color4.White.Opacity(0.2f),
                },
                tabsContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = tabs = new OsuTabControl<BeatmapDetailTab>
                    {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        RelativeSizeAxes = Axes.Both,
                    },
                },
                modsCheckbox = new OsuTabControlCheckbox
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Text = @"已选择的Mod",
                    Alpha = 0,
                },
            };

            tabs.Current.ValueChanged += _ => invokeOnFilter();
            modsCheckbox.Current.ValueChanged += _ => invokeOnFilter();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, OsuConfigManager config)
        {
            modsCheckbox.AccentColour = tabs.AccentColour = colour.YellowLight;

            selectedTab = config.GetBindable<BeatmapDetailTab>(OsuSetting.BeatmapDetailTab);

            tabs.Current.BindTo(selectedTab);
            tabs.Current.TriggerChange();
        }

        private void invokeOnFilter()
        {
            OnFilter?.Invoke(tabs.Current.Value, modsCheckbox.Current.Value);

            modsCheckbox.FadeTo(tabs.Current.Value == BeatmapDetailTab.Details ? 0 : 1, 200, Easing.OutQuint);

            tabsContainer.Padding = new MarginPadding { Right = tabs.Current.Value == BeatmapDetailTab.Details ? 0 : 100 };
        }
    }

    public enum BeatmapDetailTab
    {
        //本人技术不佳,无法翻译此处
        //加Description会报'Container' is an ambiguous reference between 'osu.Framework.Graphics.Containers.Container' and 'System.ComponentModel.Container'
        Details,
        Local,
        Country,
        Global,
        Friends
    }
}
