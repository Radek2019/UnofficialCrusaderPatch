﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UnofficialCrusaderPatch
{
    public class DefaultHeader : ChangeHeader
    {
        string descrIdent;
        public string DescrIdent => descrIdent;
        public string GetDescription() { return Localization.Get(descrIdent); }

        bool defaultIsEnabled;
        public bool DefaultIsEnabled => defaultIsEnabled;

        public DefaultHeader(string descrIdent, bool suggested = true)
        {
            this.descrIdent = descrIdent;
            this.defaultIsEnabled = suggested;
        }

        #region UI

        public event Action<DefaultHeader, bool> OnEnabledChange;

        CheckBox box;
        public CheckBox CheckBox => box;

        public FrameworkElement InitUI(bool createCheckBox)
        {
            FrameworkElement content = CreateUI();
            if (createCheckBox)
            {
                box = new CheckBox()
                {
                    IsChecked = this.isEnabled,
                    Content = content,
                    Height = content.Height,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };

                box.Checked += Box_Checked;
                box.Unchecked += Box_Unchecked;

                return box;
            }
            return content;
        }

        void Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.IsEnabled)
                SetEnabled(false);
        }

        void Box_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsEnabled)
                SetEnabled(true);
        }

        protected virtual FrameworkElement CreateUI()
        {
            return new TextBlock()
            {
                Text = this.GetDescription(),
                Height = 16,
            };
        }

        bool isEnabled;
        public virtual bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value)
                    return;

                isEnabled = value;
                if (box != null)
                    box.IsChecked = value;
            }
        }

        protected void SetEnabled(bool enabled)
        {
            if (IsEnabled == enabled)
                return;

            IsEnabled = enabled;
            OnEnabledChange?.Invoke(this, enabled);
        }

        #endregion

    }
}
