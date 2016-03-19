﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Floodlight.Client.Managers;
using Floodlight.Client.Models;

namespace Floodlight.Client.Common
{
    public static class Changer
    {
        public static async Task Execute()
        {
            var backgroundCache = SettingsManager.Internal.GetBackgroundCache();
            Background wallpaperBackground;
            Background lockScreenBackground;

            if (SettingsManager.UserDefined.UseSameImage)
            {
                var sharedBackground = GetRandomBackground(backgroundCache);
                wallpaperBackground = sharedBackground;
                lockScreenBackground = sharedBackground;
            }
            else
            {
                wallpaperBackground = GetRandomBackground(backgroundCache);
                lockScreenBackground = GetRandomBackground(backgroundCache);
            }
            

            if (SettingsManager.UserDefined.UpdateWallpaper)
            {
                await ChangeWallpaper(wallpaperBackground);
            }

            if (SettingsManager.UserDefined.UpdateLockScreen)
            {
                await ChangeLockScreen(lockScreenBackground);
            }

            SettingsManager.Internal.LastUpdatedDate = DateTime.Now;
        }

        private static Background GetRandomBackground(Dictionary<string, Background> backgroundCache)
        {
            var bgIndex = new Random().Next(backgroundCache.Count - 1);
            return backgroundCache.Values.ElementAt(bgIndex);
        }

        private static async Task ChangeWallpaper(Background background)
        {
            var backgroundFile = await FileManager.GetBackgroundFromLocalFolder(background);
            await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(backgroundFile);

            SettingsManager.Internal.CurrentWallpaper = background;
        }

        private static async Task ChangeLockScreen(Background background)
        {
            
            var backgroundFile = await FileManager.GetBackgroundFromLocalFolder(background);
            await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(backgroundFile);

            SettingsManager.Internal.CurrentLockScreen = background;
        }

    }
}
