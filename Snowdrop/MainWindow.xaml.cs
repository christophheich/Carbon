// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.json;
using Snowdrop.util;
using System;
using System.Windows;
using System.Globalization;
using System.Threading;

namespace Snowdrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // MainWindow Method
        public MainWindow()
        {
            InitializeComponent();

            ConfigurationUtil.Load();
            InitializeLocalization();
            InitializeConfiguration();

            // If the host is not available we turn off the process.
            /*if (HttpUtil.PingHost(Configuration.UPDATE_BASE, 80))
            {
                MessageBox.Show("Could not reach the network", "ERROR!");
                LoggingUtil.Warning("Test");
                Environment.Exit(0);
            }*/

            
        }

        // InitializeLocalization Method
        private void InitializeLocalization()
        {
            if (ConfigurationUtil.ConfigurationJsonModel == null || ConfigurationUtil.ConfigurationJsonModel.LANGUAGE == null || ConfigurationUtil.ConfigurationJsonModel.LANGUAGE == String.Empty)
            {
                LoggingUtil.Warning("ConfigurationJsonModel was null, LANGUAGE was null or LANGUAGE was empty.");

                ConfigurationUtil.ConfigurationJsonModel = new ConfigurationJsonModel
                {
                    LANGUAGE = Configuration.LANGUAGES[0] // Set the default language to english.
                };
                ConfigurationUtil.Save();
            }
            else
            {
                ConfigurationUtil.Load();

                foreach(var language in Configuration.LANGUAGES)
                {
                    if (ConfigurationUtil.ConfigurationJsonModel.LANGUAGE.Equals(language))
                    {
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
                        break;
                    }
                }
            }
        }

        // InitializeConfiguration Method
        private void InitializeConfiguration()
        {
            if (ConfigurationUtil.ConfigurationJsonModel == null || ConfigurationUtil.ConfigurationJsonModel.BASE_DIRECTORY_PATH == null || ConfigurationUtil.ConfigurationJsonModel.BASE_DIRECTORY_PATH == String.Empty)
            {
                LoggingUtil.Warning("ConfigurationJsonModel was null, BASE_DIRECTORY_PATH was null or BASE_DIRECTORY_PATH was empty.");

                using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = ApplicationManager.Localization.GetString("FOLDER_BROWSER_DIALOG_DESC");
                    System.Windows.Forms.DialogResult dialogResult = folderBrowserDialog.ShowDialog();

                    string directoryPath = null;

                    if (dialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        directoryPath = folderBrowserDialog.SelectedPath;
                    }
                    else
                    {
                        Environment.Exit(0);
                    }

                    MessageBoxResult messageBoxResult = MessageBox.Show(directoryPath + "\n\n" + ApplicationManager.Localization.GetString("FOLDER_BROWSER_RESULT_BODY"), 
                        ApplicationManager.Localization.GetString("FOLDER_BROWSER_RESULT_HEADER"), MessageBoxButton.YesNoCancel, MessageBoxImage.None, 
                        MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);

                    if (messageBoxResult == MessageBoxResult.No)
                    {
                        InitializeConfiguration();
                    }
                    else if (messageBoxResult == MessageBoxResult.Cancel)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        LoggingUtil.Info("The BASE_DIRECTORY_PATH has now a valid value.");
                        ConfigurationUtil.ConfigurationJsonModel.BASE_DIRECTORY_PATH = directoryPath;
                        ConfigurationUtil.Save();
                    }
                }
            }
        }

    }
}
