using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLROBS;
using ObsGw2Plugin.Extensions;
using ObsGw2Plugin.Scripting;
using ObsGw2Plugin.Scripting.Formatters;

namespace ObsGw2Plugin
{
    /// <summary>
    /// Interaction logic for Gw2PluginConfiguration.xaml
    /// </summary>
    public partial class Gw2PluginConfigurationDialog : Window, IDisposable
    {
        private XElement config;
        private TextImage textImage = new TextImage();
        private Timer previewUpdateTimer = new Timer(1d / 60);
        private string oldText = "";


        public Gw2PluginConfigurationDialog(XElement data)
        {
            InitializeComponent();

            Binding binding = new Binding("Image") { Source = this.textImage };
            this.imagePreview.SetBinding(Image.SourceProperty, binding);

            this.config = data;

            this.textBoxTextFormat.Text = this.config.GetString("textFormat", this.textBoxTextFormat.Text);
            this.UpdateToggleButtonTextFormatInsertContextMenu();
            this.checkBoxHideInactive.IsChecked = this.config.GetBoolean("hideWhenGw2IsInactive", this.checkBoxHideInactive.IsChecked.Value);

            this.comboBoxFontFamily.SelectedValue = new FontFamily(this.config.GetString("textFont", this.comboBoxFontFamily.SelectedValue.ToString()));
            this.integerUpDownFontSize.Value = this.config.GetInt("textFontSize", this.integerUpDownFontSize.Value.Value);
            this.colorPickerTextColor.SelectedColor = this.config.GetColor2("textColor", this.colorPickerTextColor.SelectedColor);
            this.colorPickerBackColor.SelectedColor = this.config.GetColor2("backColor", this.colorPickerBackColor.SelectedColor);
            this.checkBoxBold.IsChecked = this.config.GetBoolean("textBold", this.checkBoxBold.IsChecked.Value);
            this.checkBoxItalic.IsChecked = this.config.GetBoolean("textItalic", this.checkBoxItalic.IsChecked.Value);
            this.checkBoxUnderline.IsChecked = this.config.GetBoolean("textUnderline", this.checkBoxUnderline.IsChecked.Value);

            this.checkBoxOutline.IsChecked = this.config.GetBoolean("outline", this.checkBoxOutline.IsChecked.Value);
            this.colorPickerOutlineColor.SelectedColor = this.config.GetColor2("outlineColor", this.colorPickerOutlineColor.SelectedColor);
            this.doubleUpDownOutlineThickness.Value = this.config.GetFloat("outlineThickness", (float)this.doubleUpDownOutlineThickness.Value.Value);

            this.checkBoxScrolling.IsChecked = this.config.GetBoolean("scrolling", this.checkBoxScrolling.IsChecked.Value);
            this.integerUpDownScrollingSpeed.Value = this.config.GetInt("scrollingSpeed", this.integerUpDownScrollingSpeed.Value.Value);
            this.textBoxScrollingDelimiter.Text = this.config.GetString("scrollingDelimiter", this.textBoxScrollingDelimiter.Text);
            this.integerUpDownScrollingMaxWidth.Value = this.config.GetInt("scrollingMaxWidth", this.integerUpDownScrollingMaxWidth.Value.Value);
            this.comboBoxScrollingAlign.SelectedValue = this.config.GetString("scrollingAlign", this.comboBoxScrollingAlign.SelectedValue.ToString());
            this.checkBoxScrollingLargeOnly.IsChecked = this.config.GetBoolean("scrollingLargeOnly", this.checkBoxScrollingLargeOnly.IsChecked.Value);

            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            this.textBlockVersionInfo.Inlines.Clear();
            this.textBlockVersionInfo.Inlines.Add(new Run(string.Format("Version {0}", currentVersion.ToString(3))));
            Version liveVersion = Gw2Plugin.Instance.Configuration.LastVersionRelease;
            string liveVersionUrl = Gw2Plugin.Instance.Configuration.LastVersionReleaseUrl;
            if (liveVersion > currentVersion && !string.IsNullOrEmpty(liveVersionUrl))
            {
                string liveVersionString = liveVersion.Build > 0 ? liveVersion.ToString(3) : liveVersion.ToString(2);
                Hyperlink hyperlink = new Hyperlink(new Run(string.Format("Newer version {0} is available", liveVersionString)))
                {
                    NavigateUri = new Uri(liveVersionUrl)
                };
                hyperlink.RequestNavigate += (s_, e_) =>
                {
                    Process.Start(new ProcessStartInfo(e_.Uri.AbsoluteUri));
                    e_.Handled = true;
                };
                this.textBlockVersionInfo.Inlines.Add(new Run(" - "));
                this.textBlockVersionInfo.Inlines.Add(hyperlink);
            }

            this.previewUpdateTimer.Elapsed += previewUpdateTimer_Elapsed;
            this.previewUpdateTimer.Start();
        }

        protected void UpdateToggleButtonTextFormatInsertContextMenu()
        {
            this.toggleButtonTextFormatInsert.ContextMenu = new ContextMenu();
            foreach (IScriptFormatter formatter in Gw2Plugin.Instance.ScriptsManager.GetListOfScriptFormatters())
            {
                ItemCollection currentMenuItems = this.toggleButtonTextFormatInsert.ContextMenu.Items;
                foreach (string submenu in formatter.Category)
                {
                    bool submenuFound = false;
                    foreach (object item in currentMenuItems)
                    {
                        MenuItem menuItem = item as MenuItem;
                        if (item != null && menuItem.Header.ToString() == submenu)
                        {
                            submenuFound = true;
                            currentMenuItems = menuItem.Items;
                            break;
                        }
                    }

                    if (!submenuFound)
                    {
                        MenuItem submenuItem = new MenuItem() { Header = submenu };
                        currentMenuItems.Add(submenuItem);
                        currentMenuItems = submenuItem.Items;
                    }
                }

                MenuItem newMenuItem = new MenuItem() { Header = formatter.Name, InputGestureText = "%" + formatter.Id + "%" };
                newMenuItem.Click += toggleButtonTextFormatInsertMenuItem_Click;
                currentMenuItems.Add(newMenuItem);
            }

            this.ApplySortDescriptionRecursively(this.toggleButtonTextFormatInsert.ContextMenu.Items,
                new SortDescription("Header", ListSortDirection.Ascending));
        }

        protected void ApplySortDescriptionRecursively(ItemCollection itemCollection, SortDescription sortDescription)
        {
            itemCollection.SortDescriptions.Clear();
            itemCollection.SortDescriptions.Add(sortDescription);
            foreach (object item in itemCollection)
            {
                ItemsControl control = item as ItemsControl;
                if (control != null)
                    this.ApplySortDescriptionRecursively(control.Items, sortDescription);
            }
        }


        private void previewUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                string text = Gw2Plugin.Instance.ScriptsManager.FormatString(textBoxTextFormat.Text);
                if (text != this.oldText)
                {
                    this.oldText = text;
                    this.textImage.Text = text;
                }

                this.textImage.RenderNextFrame();
            });
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.previewUpdateTimer.Stop();

            this.config.SetString("textFormat", this.textBoxTextFormat.Text);
            this.config.SetBoolean("hideWhenGw2IsInactive", this.checkBoxHideInactive.IsChecked.Value);

            this.config.SetString("textFont", this.comboBoxFontFamily.SelectedValue.ToString());
            this.config.SetInt("textFontSize", this.integerUpDownFontSize.Value.Value);
            this.config.SetColor2("textColor", this.colorPickerTextColor.SelectedColor);
            this.config.SetColor2("backColor", this.colorPickerBackColor.SelectedColor);
            this.config.SetBoolean("textBold", this.checkBoxBold.IsChecked.Value);
            this.config.SetBoolean("textItalic", this.checkBoxItalic.IsChecked.Value);
            this.config.SetBoolean("textUnderline", this.checkBoxUnderline.IsChecked.Value);

            this.config.SetBoolean("outline", this.checkBoxOutline.IsChecked.Value);
            this.config.SetColor2("outlineColor", this.colorPickerOutlineColor.SelectedColor);
            this.config.SetFloat("outlineThickness", (float)this.doubleUpDownOutlineThickness.Value.Value);

            this.config.SetBoolean("scrolling", this.checkBoxScrolling.IsChecked.Value);
            this.config.SetInt("scrollingSpeed", this.integerUpDownScrollingSpeed.Value.Value);
            this.config.SetString("scrollingDelimiter", this.textBoxScrollingDelimiter.Text);
            this.config.SetInt("scrollingMaxWidth", this.integerUpDownScrollingMaxWidth.Value.Value);
            this.config.SetString("scrollingAlign", this.comboBoxScrollingAlign.SelectedValue.ToString());
            this.config.SetBoolean("scrollingLargeOnly", this.checkBoxScrollingLargeOnly.IsChecked.Value);

            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.previewUpdateTimer.Stop();

            this.DialogResult = false;
            this.Close();
        }


        private void toggleButtonTextFormatInsert_CheckedChanged(object sender, RoutedEventArgs e)
        {
            toggleButtonTextFormatInsert.ContextMenu.PlacementTarget = toggleButtonTextFormatInsert;
            toggleButtonTextFormatInsert.ContextMenu.Placement = PlacementMode.Right;
            RoutedEventHandler handler = null;
            handler = (s_, e_) =>
            {
                toggleButtonTextFormatInsert.IsChecked = false;
                toggleButtonTextFormatInsert.ContextMenu.Closed -= handler;
            };
            toggleButtonTextFormatInsert.ContextMenu.Closed += handler;
            toggleButtonTextFormatInsert.ContextMenu.IsOpen = toggleButtonTextFormatInsert.IsChecked == true;
        }

        private void toggleButtonTextFormatInsert_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void toggleButtonTextFormatInsert_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private void InsertPredefinedTextFormat(string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                int currentIndex = textBoxTextFormat.SelectionStart;
                textBoxTextFormat.SelectedText = format;
                textBoxTextFormat.SelectionStart = currentIndex + format.Length;
                textBoxTextFormat.SelectionLength = 0;
            }
        }

        private void toggleButtonTextFormatInsertMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                InsertPredefinedTextFormat(menuItem.InputGestureText as string);
                textBoxTextFormat.Focus();
            }
        }


        private void textBoxTextFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textImage.Text = Gw2Plugin.Instance.ScriptsManager.FormatString(this.textBoxTextFormat.Text);
        }

        private void comboBoxFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Working with added items instead of the selection directly,
            // since the comboBox can work with multiple selections internally.
            // If the index of the new selection is higher than the previous one,
            // this will fuck up the selection in this event handler.
            if (e.AddedItems.Count > 0)
                this.textImage.FontFamily = new FontFamily(e.AddedItems[0].ToString());
        }

        private void integerUpDownFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.integerUpDownFontSize.Value.HasValue)
                this.textImage.FontSize = this.integerUpDownFontSize.Value.Value;
        }

        private void colorPickerTextColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            this.textImage.TextColor = this.colorPickerTextColor.SelectedColor;
        }

        private void colorPickerBackColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            this.textImage.BackColor = this.colorPickerBackColor.SelectedColor;
        }

        private void checkBoxBold_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.textImage.EffectBold = this.checkBoxBold.IsChecked == true;
        }

        private void checkBoxItalic_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.textImage.EffectItalic = this.checkBoxItalic.IsChecked == true;
        }

        private void checkBoxUnderline_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.textImage.EffectUnderline = this.checkBoxUnderline.IsChecked == true;
        }


        private void checkBoxOutline_Checked(object sender, RoutedEventArgs e)
        {
            if (this.colorPickerOutlineColor != null && this.colorPickerOutlineColor.SelectedColor != null
                && this.doubleUpDownOutlineThickness != null && this.doubleUpDownOutlineThickness.Value.HasValue)
            {
                this.textImage.OutlineColor = this.colorPickerOutlineColor.SelectedColor;
                if (this.doubleUpDownOutlineThickness.Value.HasValue)
                    this.textImage.OutlineThickness = this.doubleUpDownOutlineThickness.Value.Value;
            }
        }

        private void checkBoxOutline_Unchecked(object sender, RoutedEventArgs e)
        {
            this.textImage.OutlineColor = Colors.Transparent;
            this.textImage.OutlineThickness = 0;
        }

        private void colorPickerOutlineColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            this.textImage.OutlineColor = this.colorPickerOutlineColor.SelectedColor;
        }

        private void doubleUpDownOutlineThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.doubleUpDownOutlineThickness.Value.HasValue)
                this.textImage.OutlineThickness = this.doubleUpDownOutlineThickness.Value.Value;
        }


        private void checkBoxScrolling_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.textImage.EnableScrolling = this.checkBoxScrolling.IsChecked == true;
        }

        private void integerUpDownScrollingSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.integerUpDownScrollingSpeed.Value.HasValue)
                this.textImage.ScrollingSpeed = this.integerUpDownScrollingSpeed.Value.Value;
        }

        private void textBoxScrollingDelimiter_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textImage.ScrollingDelimiter = this.textBoxScrollingDelimiter.Text;
        }

        private void integerUpDownScrollingMaxWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.integerUpDownScrollingMaxWidth.Value.HasValue)
                this.textImage.ScrollingMaxWidth = this.integerUpDownScrollingMaxWidth.Value.Value;
        }

        private void comboBoxScrollingAlign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.comboBoxScrollingAlign.SelectedValue.ToString())
            {
                case "Left":
                    this.textImage.ScrollingAlign = TextImage.ScrollingAligns.Left;
                    break;
                case "Center":
                    this.textImage.ScrollingAlign = TextImage.ScrollingAligns.Center;
                    break;
                case "Right":
                    this.textImage.ScrollingAlign = TextImage.ScrollingAligns.Right;
                    break;
            }
        }

        private void checkBoxScrollingLargeOnly_CheckedChanged(object sender, RoutedEventArgs e)
        {
            this.textImage.ScrollingLargeOnly = this.checkBoxScrollingLargeOnly.IsChecked == true;
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.previewUpdateTimer.Dispose();
            }
        }

    }
}
