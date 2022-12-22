﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aimless_et
{

    public partial class Form1 : Form
    {
        /// <summary>
        /// Used for custom form template to drag form on mouse down
        /// </summary>
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        /// <summary>
        /// Access camera specs DB to use in listview
        /// Tooltips DB for helpful hints.
        /// </summary>
        CameraDb cameraDb = new CameraDb();
        Descriptions descriptions = new Descriptions();
        public Form1()
        {
            InitializeComponent();

            foreach (var camera in cameraDb.Camera_Data)
            {
                ListViewItem lvi = lstCameraSensors.Items.Add(camera.Item1);
                lvi.SubItems.Add(camera.Item2);
                lvi.SubItems.Add(camera.Item3);
                lvi.SubItems.Add(camera.Item4);
                lvi.SubItems.Add(camera.Item5);
                lvi.SubItems.Add(camera.Item6);
            }

            foreach (ColumnHeader column in lstCameraSensors.Columns)
            {
                column.Width = -2;
            }
        }
        /// <summary>
        /// Drag form on mouse down from titlebar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        /// <summary>
        /// Move custom title bar within panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<(string, string, string, string, string, string)> Camera_Data_Filter;

        /// <summary>
        /// Filter Camera spec data in listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbFilter_TextChanged(object sender, EventArgs e)
        {
            lstCameraSensors.Items.Clear();

            foreach (var camera in cameraDb.Camera_Data)
            {
                if (camera.Item2.ToLower().Contains(txbFilter.Text.ToLower()))
                {
                    ListViewItem lvi = lstCameraSensors.Items.Add(camera.Item1);
                    lvi.SubItems.Add(camera.Item2);
                    lvi.SubItems.Add(camera.Item3);
                    lvi.SubItems.Add(camera.Item4);
                    lvi.SubItems.Add(camera.Item5);
                    lvi.SubItems.Add(camera.Item6);
                }
            }

        }

        /// <summary>
        /// Show tooltips in respect to help button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tipRule_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[0].Item1;
            lblDescription.Text = descriptions.Tooltips[0].Item2;
        }

        private void tipSensor_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[1].Item1;
            lblDescription.Text = descriptions.Tooltips[1].Item2;
        }

        private void tipFstop_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[2].Item1;
            lblDescription.Text = descriptions.Tooltips[2].Item2;
        }

        private void tipPitch_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[3].Item1;
            lblDescription.Text = descriptions.Tooltips[3].Item2;
        }

        private void tipFocal_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[4].Item1;
            lblDescription.Text = descriptions.Tooltips[4].Item2;
        }

        private void tipDeclination_Click(object sender, EventArgs e)
        {
            lblDescriptionTitle.Text = descriptions.Tooltips[5].Item1;
            lblDescription.Text = descriptions.Tooltips[5].Item2;
        }

        /// <summary>
        /// Calculate pixel pitch
        /// Sensor Width / Mac Image Width * 1000 = Pixel Pitch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxmiAddPitch_Click(object sender, EventArgs e)
        {
            float sWidth = float.Parse(lstCameraSensors.SelectedItems[0].SubItems[2].Text);
            int mWidth = int.Parse(lstCameraSensors.SelectedItems[0].SubItems[4].Text);
            tbxPixelPitch.Text = GetPixelPitch(sWidth, mWidth).ToString("#.##");
        }

        /// <summary>
        /// Formula to calculate pixel pitch
        /// </summary>
        /// <param name="_sWidth"></param>
        /// <param name="_mWidth"></param>
        /// <returns></returns>
        public float GetPixelPitch(float _sWidth, int _mWidth)
        {
            return (_sWidth / _mWidth) * 1000;
        }

        /// <summary>
        /// Data validation, is it present? Is returned datatype valid?
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsValuePresent(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            return false;
        }

        public bool IsDigit(string text)
        {
            if (!string.IsNullOrEmpty(text))
                return int.TryParse(text, out int i);

            return false;
        }

        public bool IsDecimal(string text)
        {
            if (!string.IsNullOrEmpty(text))
                return float.TryParse(text, out float fl);
            
            return false;
        }
    }
}
