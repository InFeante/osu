﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Overlays.Settings.Sections.Maintenance
{
    public class GeneralSettings : SettingsSubsection
    {
        protected override string Header => "总体";

        private TriangleButton importBeatmapsButton;
        private TriangleButton importScoresButton;
        private TriangleButton importSkinsButton;
        private TriangleButton deleteBeatmapsButton;
        private TriangleButton deleteScoresButton;
        private TriangleButton deleteSkinsButton;
        private TriangleButton restoreButton;
        private TriangleButton undeleteButton;

        [BackgroundDependencyLoader]
        private void load(BeatmapManager beatmaps, ScoreManager scores, SkinManager skins, DialogOverlay dialogOverlay)
        {
            if (beatmaps.SupportsImportFromStable)
            {
                Add(importBeatmapsButton = new SettingsButton
                {
                    Text = "从osu!stable中导入谱面",
                    Action = () =>
                    {
                        importBeatmapsButton.Enabled.Value = false;
                        beatmaps.ImportFromStableAsync().ContinueWith(t => Schedule(() => importBeatmapsButton.Enabled.Value = true));
                    }
                });
            }

            Add(deleteBeatmapsButton = new DangerousSettingsButton
            {
                Text = "删除所有谱面",
                Action = () =>
                {
                    dialogOverlay?.Push(new DeleteAllBeatmapsDialog(() =>
                    {
                        deleteBeatmapsButton.Enabled.Value = false;
                        Task.Run(() => beatmaps.Delete(beatmaps.GetAllUsableBeatmapSets())).ContinueWith(t => Schedule(() => deleteBeatmapsButton.Enabled.Value = true));
                    }));
                }
            });

            if (scores.SupportsImportFromStable)
            {
                Add(importScoresButton = new SettingsButton
                {
                    Text = "从osu!stable中导入分数",
                    Action = () =>
                    {
                        importScoresButton.Enabled.Value = false;
                        scores.ImportFromStableAsync().ContinueWith(t => Schedule(() => importScoresButton.Enabled.Value = true));
                    }
                });
            }

            Add(deleteScoresButton = new DangerousSettingsButton
            {
                Text = "删除所有分数",
                Action = () =>
                {
                    dialogOverlay?.Push(new DeleteAllBeatmapsDialog(() =>
                    {
                        deleteScoresButton.Enabled.Value = false;
                        Task.Run(() => scores.Delete(scores.GetAllUsableScores())).ContinueWith(t => Schedule(() => deleteScoresButton.Enabled.Value = true));
                    }));
                }
            });

            if (skins.SupportsImportFromStable)
            {
                Add(importSkinsButton = new SettingsButton
                {
                    Text = "从osu!stable中导入皮肤",
                    Action = () =>
                    {
                        importSkinsButton.Enabled.Value = false;
                        skins.ImportFromStableAsync().ContinueWith(t => Schedule(() => importSkinsButton.Enabled.Value = true));
                    }
                });
            }

            AddRange(new Drawable[]
            {
                deleteSkinsButton = new DangerousSettingsButton
                {
                    Text = "删除所有皮肤",
                    Action = () =>
                    {
                        dialogOverlay?.Push(new DeleteAllBeatmapsDialog(() =>
                        {
                            deleteSkinsButton.Enabled.Value = false;
                            Task.Run(() => skins.Delete(skins.GetAllUserSkins())).ContinueWith(t => Schedule(() => deleteSkinsButton.Enabled.Value = true));
                        }));
                    }
                },
                restoreButton = new SettingsButton
                {
                    Text = "恢复所有被隐藏的难度",
                    Action = () =>
                    {
                        restoreButton.Enabled.Value = false;
                        Task.Run(() =>
                        {
                            foreach (var b in beatmaps.QueryBeatmaps(b => b.Hidden).ToList())
                                beatmaps.Restore(b);
                        }).ContinueWith(t => Schedule(() => restoreButton.Enabled.Value = true));
                    }
                },
                undeleteButton = new SettingsButton
                {
                    Text = "恢复所有最近删除的谱面",
                    Action = () =>
                    {
                        undeleteButton.Enabled.Value = false;
                        Task.Run(() => beatmaps.Undelete(beatmaps.QueryBeatmapSets(b => b.DeletePending).ToList())).ContinueWith(t => Schedule(() => undeleteButton.Enabled.Value = true));
                    }
                },
            });
        }
    }
}
