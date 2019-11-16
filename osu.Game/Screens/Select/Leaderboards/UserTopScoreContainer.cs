﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Threading;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Online.Leaderboards;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Screens.Select.Leaderboards
{
    public class UserTopScoreContainer : VisibilityContainer
    {
        private const int duration = 500;

        private readonly Container scoreContainer;

        public Bindable<APILegacyUserTopScoreInfo> Score = new Bindable<APILegacyUserTopScoreInfo>();

        public Action<ScoreInfo> ScoreSelected;

        protected override bool StartHidden => true;

        public UserTopScoreContainer()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            Margin = new MarginPadding { Vertical = 5 };

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = @"个人最佳成绩".ToUpper(),
                            Font = OsuFont.GetFont(size: 20, weight: FontWeight.Bold)
                        },
                        scoreContainer = new Container
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                        }
                    }
                }
            };

            Score.BindValueChanged(onScoreChanged);
        }

        private CancellationTokenSource loadScoreCancellation;

        private void onScoreChanged(ValueChangedEvent<APILegacyUserTopScoreInfo> score)
        {
            var newScore = score.NewValue;

            scoreContainer.Clear();
            loadScoreCancellation?.Cancel();

            if (newScore == null)
                return;

            LoadComponentAsync(new LeaderboardScore(newScore.Score, newScore.Position, false)
            {
                Action = () => ScoreSelected?.Invoke(newScore.Score)
            }, drawableScore =>
            {
                scoreContainer.Child = drawableScore;
                drawableScore.FadeInFromZero(duration, Easing.OutQuint);
            }, (loadScoreCancellation = new CancellationTokenSource()).Token);
        }

        protected override void PopIn() => this.FadeIn(duration, Easing.OutQuint);

        protected override void PopOut() => this.FadeOut(duration, Easing.OutQuint);
    }
}
