﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class MuiltSelectEditor : ISettingsEditor<IEnumerable<string>>
    {
        public PageItemType ItemType => PageItemType.Control;

        public PageEditorType EditorType => PageEditorType.SingleSelect;

        public PageItemFlags Flags => PageItemFlags.Default;

        private BComboBox<string> mCtrl;

        public IEnumerable<string> Value => mCtrl.Selecteds;

        public string Name { get; }

        public string Descrption { get; }

        public MuiltSelectEditor(IEnumerable<string> val, string name, string descrption)
        {
            Name = name;
            Descrption = descrption;

            mCtrl = new BComboBox<string>(host);
            mCtrl.Multiple = true;
            mCtrl.ForeColor = IDE.AppConfig.Skin.DarkForeColor;
            mCtrl.BackColor = Color.Transparent;
            mCtrl.BorderColor = IDE.AppConfig.Skin.TipsColor;

            mCtrl.AddItems(val);
            mCtrl.Selected = val.FirstOrDefault();

            var width = 0;
            var height = 0;
            foreach (var size in val.Select(x => TextRenderer.MeasureText(x, mCtrl.Host.Font)))
            {
                width = Math.Max(size.Width, width);
                height = Math.Max(size.Height, height);
            }

            mCtrl.Size = new Size(Math.Min(200, width + 28), height + 2);
        }

        public Size ComputeSize(Size maxSize)
        {
            return mCtrl.Size;
        }
    }
}