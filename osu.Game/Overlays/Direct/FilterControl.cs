﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Online.API.Requests;
using osu.Game.Overlays.SearchableList;
using osu.Game.Rulesets;
using osuTK.Graphics;

namespace osu.Game.Overlays.Direct
{
    public class FilterControl : SearchableListFilterControl<DirectSortCriteria, BeatmapSearchCategory>
    {
        private DirectRulesetSelector rulesetSelector;

        protected override Color4 BackgroundColour => OsuColour.FromHex(@"384552");
        protected override DirectSortCriteria DefaultTab => DirectSortCriteria.Ranked;
        protected override BeatmapSearchCategory DefaultCategory => BeatmapSearchCategory.Leaderboard;

        protected override Drawable CreateSupplementaryControls() => rulesetSelector = new DirectRulesetSelector();

        public Bindable<RulesetInfo> Ruleset => rulesetSelector.Current;

        [BackgroundDependencyLoader(true)]
        private void load(OsuColour colours, Bindable<RulesetInfo> ruleset)
        {
            DisplayStyleControl.Dropdown.AccentColour = colours.BlueDark;
            rulesetSelector.Current.BindTo(ruleset);
        }
    }

    public enum DirectSortCriteria
    {
        [Description("相关性")]
        Relevance,
        [Description("标题")]
        Title,
        [Description("艺术家")]
        Artist,
        [Description("作图者")]
        Creator,
        [Description("难度")]
        Difficulty,
        [Description("Ranked 时间")]
        Ranked,
        [Description("评分")]
        Rating,
        [Description("游玩次数")]
        Plays,
        [Description("收藏量")]
        Favourites,
    }
}
