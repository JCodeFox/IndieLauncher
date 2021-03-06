﻿using System;
using System.IO;
using System.Diagnostics;

namespace Dan200.Launcher.Main
{
    public static class GameLauncher
    {
        public static bool LaunchGame( string gameTitle, string gameVersion )
        {
            if( Installer.IsGameInstalled( gameTitle, gameVersion ) )
            {
                Logger.Log( "Determining path to game exe ({0} {1})", gameTitle, gameVersion );

                // Search for a suitable exe to run
                string gamePath = Installer.GetInstallPath( gameTitle, gameVersion );
                string launchPath = null;
                switch( Program.Platform )
                {
                    case Platform.Windows:
                    {
                        string exePath = Path.Combine( gamePath, gameTitle + ".exe" );
                        if( File.Exists( exePath ) )
                        {
                            launchPath = exePath;
                            break;
                        }
                        string batPath = Path.Combine( gamePath, gameTitle + ".bat" );
                        if( File.Exists( batPath ) )
                        {
                            launchPath = batPath;
                            break;
                        }
                        break;
                    }
                    case Platform.OSX:
                    {
                        string appPath = Path.Combine( gamePath, gameTitle + ".app" );
                        if( Directory.Exists( appPath ) )
                        {
                            launchPath = appPath;
                            break;
                        }
                        string shPath = Path.Combine( gamePath, gameTitle + ".sh" );
                        if( File.Exists( shPath ) )
                        {
                            launchPath = shPath;
                            break;
                        }
                        break;
                    }
                    case Platform.Linux:
                    default:
                    {
                        string shPath = Path.Combine( gamePath, gameTitle + ".sh" );
                        if( File.Exists( shPath ) )
                        {
                            launchPath = shPath;
                            break;
                        }
                        break;
                    }
                }

                if( launchPath != null )
                {
                    // Run the exe
                    Logger.Log( "Launching {0}", launchPath );
                    if( Path.GetExtension( launchPath ) == ".sh" )
                    {
                        var startInfo = new ProcessStartInfo();
                        startInfo.FileName = "/bin/sh";
                        startInfo.WorkingDirectory = gamePath;
                        startInfo.Arguments = Path.GetFileName( launchPath );
                        Process.Start( startInfo );
                        return true;
                    }
                    else
                    {
                        var startInfo = new ProcessStartInfo();
                        startInfo.FileName = launchPath;
                        startInfo.WorkingDirectory = gamePath;
                        Process.Start( startInfo );
                        return true;
                    }
                }
                else
                {
                    // If no exe was found, just open the folder
                    Logger.Log( "Opening {0}", gamePath );
                    Process.Start( gamePath );
                    return true;
                }
            }
            return false;
        }
    }
}

