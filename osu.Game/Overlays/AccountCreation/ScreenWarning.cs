// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Online.API;
using osu.Game.Overlays.Settings;
using osu.Game.Screens.Menu;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Overlays.AccountCreation
{
    public class ScreenWarning : AccountCreationScreen
    {
        private OsuTextFlowContainer multiAccountExplanationText;
        private LinkFlowContainer furtherAssistance;
        private IAPIProvider api;

        private const string help_centre_url = "/help/wiki/Help_Centre#login";

        public override void OnEntering(IScreen last)
        {
            if (string.IsNullOrEmpty(api.ProvidedUsername))
            {
                this.FadeOut();
                this.Push(new ScreenEntry());
                return;
            }

            base.OnEntering(last);
        }

        [BackgroundDependencyLoader(true)]
        private void load(OsuColour colours, IAPIProvider api, OsuGame game, TextureStore textures)
        {
            this.api = api;

            if (string.IsNullOrEmpty(api.ProvidedUsername))
                return;

            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Texture = textures.Get(@"Menu/dev-build-footer"),
                },
                new Sprite
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Texture = textures.Get(@"Menu/dev-build-footer"),
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Padding = new MarginPadding(20),
                    Spacing = new Vector2(0, 5),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 150,
                            Child = new OsuLogo
                            {
                                Scale = new Vector2(0.1f),
                                Anchor = Anchor.Centre,
                                Triangles = false,
                            },
                        },
                        new OsuSpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Colour = Color4.Red,
                            Font = OsuFont.GetFont(size: 28, weight: FontWeight.Light),
                            Text = "Warning! 注意！",
                        },
                        multiAccountExplanationText = new OsuTextFlowContainer(cp => cp.Font = cp.Font.With(size: 16))
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        },
                        new SettingsButton
                        {
                            Text = "我没法登录我的账号!",
                            Margin = new MarginPadding { Top = 50 },
                            Action = () => game?.OpenUrlExternally(help_centre_url)
                        },
                        new DangerousSettingsButton
                        {
                            Text = "我了解. 但我是为别人创建账号的.",
                            Action = () => this.Push(new ScreenEntry())
                        },
                        furtherAssistance = new LinkFlowContainer(cp => cp.Font = cp.Font.With(size: 16))
                        {
                            Margin = new MarginPadding { Top = 20 },
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            AutoSizeAxes = Axes.Both
                        },
                    }
                }
            };

            multiAccountExplanationText.AddText("你是 ");
            multiAccountExplanationText.AddText(api.ProvidedUsername, cp => cp.Colour = colours.BlueLight);
            multiAccountExplanationText.AddText("吗? 如果是,osu! 不允许 ");
            multiAccountExplanationText.AddText("多开账号!", cp => cp.Colour = colours.Yellow);
            multiAccountExplanationText.AddText("请注意,多开账号行为将会导致您的osu!账号被");
            multiAccountExplanationText.AddText("一起封禁", cp => cp.Colour = colours.Yellow);
            multiAccountExplanationText.AddText(".");

            furtherAssistance.AddText("需要进一步的帮助?请联系我们的");
            furtherAssistance.AddLink("帮助中心", help_centre_url);
            furtherAssistance.AddText(".");
        }
    }
}
