// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Snowdrop.ext;
using Snowdrop.json;
using Snowdrop.util;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Snowdrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Timers.Timer aTimer;

        // MainWindow Method
        public MainWindow()
        {
            InitializeComponent();

            ConfigurationUtil.Load();
            InitializeLocalization();
            InitializeConfiguration();
            InitializeWebspaceJsonModel();
            InitializeApplication();
        }

        // InitializeLocalization Method
        private void InitializeLocalization()
        {
            if (ConfigurationUtil.ConfigurationJsonModel == null || ConfigurationUtil.ConfigurationJsonModel.Language == null || ConfigurationUtil.ConfigurationJsonModel.Language == String.Empty)
            {
                LoggingUtil.Warning("ConfigurationJsonModel was null, LANGUAGE was null or LANGUAGE was empty.");

                ConfigurationUtil.ConfigurationJsonModel = new ConfigurationJsonModel
                {
                    Language = Configuration.LANGUAGES[0] // Set the default language to english.
                };
                ConfigurationUtil.Save();
            }
            else
            {
                ConfigurationUtil.Load();

                int index = 0;
                foreach (var language in Configuration.LANGUAGES)
                {
                    if (ConfigurationUtil.ConfigurationJsonModel.Language.Equals(language))
                    {
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
                        ComboBox_Language.SelectedIndex = index;
                        break;
                    }
                    index++;
                }
            }
        }

        // InitializeConfiguration Method
        private void InitializeConfiguration()
        {
            if (ConfigurationUtil.ConfigurationJsonModel == null || ConfigurationUtil.ConfigurationJsonModel.BaseDirectoryPath == null || ConfigurationUtil.ConfigurationJsonModel.BaseDirectoryPath == String.Empty)
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
                        ConfigurationUtil.ConfigurationJsonModel.BaseDirectoryPath = directoryPath;
                        ConfigurationUtil.Save();
                    }
                }
            }
        }

        // InitializeWebspaceJsonModel Method
        private void InitializeWebspaceJsonModel()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(Configuration.APPLICATION_WEBSPACE_JSON_MODEL))
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            ApplicationManager.Webspace = JsonConvert.DeserializeObject<WebspaceJsonModel>(streamReader.ReadToEnd());
                        }

                    }

                }
            }
            catch (WebException e)
            {
                MessageBox.Show(ApplicationManager.Localization.GetString("INITIALIZE_WEBSPACE_JSON_MODEL"),
                    ApplicationManager.Localization.GetString("ERROR"), MessageBoxButton.OK, MessageBoxImage.Error);

                LoggingUtil.Exception(e.Message);
                Environment.Exit(0);
            }
        }

        // InitializeApplication Method
        private void InitializeApplication()
        {
            // set for each event the event information
            {
                var events = Grid_Event.Children.OfType<Label>().Where(c => c.Name.StartsWith("Label_Event_Body")).ToList();

                int i = 0;
                foreach (var item in events)
                {
                    try
                    {
                        item.Content = ApplicationManager.Webspace.Event[i].Truncate(25, "...");
                    }
                    catch (Exception ex)
                    {
                        LoggingUtil.Exception(ex.Message);
                        item.Content = String.Empty;
                    }
                    i++;
                }
            }

            // set for each changelog the changelog information
            {
                var changelog = Grid_Changelog.Children.OfType<Label>().Where(c => c.Name.StartsWith("Label_Changelog_Body")).ToList();

                int i = 0;
                foreach (var item in changelog)
                {
                    try
                    {
                        item.Content = ApplicationManager.Webspace.Changelog[i].Truncate(25, "...");
                    }
                    catch (Exception ex)
                    {
                        LoggingUtil.Exception(ex.Message);
                        item.Content = String.Empty;
                    }
                    i++;
                }
            }

            // set announcement
            Label_Announcement.Content = ApplicationManager.Webspace.Announcement.Truncate(115, "...");

            // set image
            var imageUriSource = new Uri(ApplicationManager.Webspace.Image[0], UriKind.Absolute);
            Image_Holder.Source = new BitmapImage(imageUriSource);
        }

        /*public void SetImage(int i)
        {
            Dispatcher.Invoke(new Action(() => {
                var imageUriSource = new Uri(ApplicationManager.Webspace.Image[i], UriKind.Absolute);
                Image_Holder.Source = new BitmapImage(imageUriSource);
            }), DispatcherPriority.Render);
        }*/

        private void ComboBox_Language_DropDownClosed(object sender, EventArgs e)
        {
            if (ConfigurationUtil.ConfigurationJsonModel.Language != Configuration.LANGUAGES[ComboBox_Language.SelectedIndex])
            {
                ConfigurationUtil.ConfigurationJsonModel.Language = Configuration.LANGUAGES[ComboBox_Language.SelectedIndex];
                ConfigurationUtil.Save();

                // release the mutex
                App.mutex.ReleaseMutex();

                // restart a new application and turn off the current
                System.Windows.Forms.Application.Restart();
                Application.Current.Shutdown();
            }
        }

        private void Button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
    }
}
