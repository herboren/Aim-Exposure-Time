using System;
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
        ExposureFormulas formulas = new ExposureFormulas();
        public Form1()
        {
            InitializeComponent();


            // Fill camera db listview object
            foreach (var camera in cameraDb.Camera_Data)
            {
                ListViewItem lvi = lstCameraSensors.Items.Add(camera.Item1);
                lvi.SubItems.Add(camera.Item2);
                lvi.SubItems.Add(camera.Item3);
                lvi.SubItems.Add(camera.Item4);
                lvi.SubItems.Add(camera.Item5);
                lvi.SubItems.Add(camera.Item6);
            }

            // Resize listview column widths to fit
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

        // Update label(s)
        private void tbxPixelPitch_TextChanged(object sender, EventArgs e)
        {
            formulas.Pitch = formulas.IsFloat(tbxPixelPitch.Text);

            if (tbxFocalLength.Text != string.Empty)
                lblPlate.Text = $"{formulas.PlateExposureTime().ToString("#.####")}/s";
        }

        // Update label(s)
        private void tbxFocalLength_TextChanged(object sender, EventArgs e)
        {
            formulas.FocalLength = formulas.IsNumeric(tbxFocalLength.Text);

            if (tbxPixelPitch.Text != string.Empty)
            {
                lblPlate.Text = $"{formulas.PlateExposureTime().ToString("#.####")}/s";
                lblSimpNpf.Text = $"{formulas.SimplifiedAperturePixelFocal().ToString("#.####")}/s";
                lblFourCrop.Text = $"{formulas.FourCropExposureTime().ToString("#.####")}/s";
            }
        }

        // Update label(s)
        private void tbxDeclination_TextChanged(object sender, EventArgs e)
        {
            formulas.Declination = formulas.IsNumeric(tbxDeclination.Text);

            if (tbxPixelPitch.Text != string.Empty && tbxFocalLength.Text != string.Empty)
                lblPlatePlus.Text = $"{formulas.PlatePlusExposureTime().ToString("#.####")}/s";
        }

        private void lbPitchTooltip_MouseEnter(object sender, EventArgs e)
        {
            ttDescriptions.SetToolTip(this.lbPitchTooltip, "Pixel size/pitch of an individual pixel on the camera sensor.\n\n" +
                "To calculate pixel pitch:\nPixel Pitch = Sensor Width / Max Image Width * 1000");
        }

        private void btnlblPlate_MouseEnter(object sender, EventArgs e)
        {
            ttDescriptions.SetToolTip(this.btnlblPlate, "The plate scale of a telescope connects the angular separation of an\n" +
                "object with the linear separation of its image at the focal plane. The plate\n" +
                "scale of a telescope can be described as the number of degrees or arcminutes\n" +
                "or arcseconds, corresponding to a number of inches, or centimeters, or\n" +
                "millimeters (etc.) at the focal plane (where an image of an object is \"seen\")\n" +
                "of a telescope. Each telescope has its own plate scale");
        }

        // Update label(s)
        private void lsbRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            formulas.RuleInt = lsbRule.SelectedIndex+1;
            formulas.Rule = formulas.IsNumeric(lsbRule.SelectedItem.ToString());
            
            if (tbxPixelPitch.Text != string.Empty &&
                tbxFocalLength.Text != string.Empty &&
                tbxFSstop.Text != string.Empty &&
                tbxDeclination.Text != string.Empty &&
                lsbSensor.SelectedIndex != -1)
            {
                lblNpf.Text = $"{formulas.AperturePixelFocal().ToString("#.####")}/s";
                lblSimpNpf.Text = $"{formulas.SimplifiedAperturePixelFocal().ToString("#.####")}/s";
                lblRule.Text = $"{formulas.RuleExposureTime().ToString("#.####")}/s";
                lblFourCrop.Text = $"{formulas.FourCropExposureTime().ToString("#.####")}/s";
            }

        }

        // Update label(s)
        private void tbxFSstop_TextChanged(object sender, EventArgs e)
        {
            formulas.Aperture = formulas.IsFloat(tbxFSstop.Text);

            if (tbxPixelPitch.Text != string.Empty &&
                tbxFocalLength.Text != string.Empty &&
                lsbRule.SelectedIndex != -1 &&
                tbxDeclination.Text != string.Empty)
            {
                lblNpf.Text = $"{formulas.AperturePixelFocal().ToString("#.####")}/s";
                lblSimpNpf.Text = $"{formulas.SimplifiedAperturePixelFocal().ToString("#.####")}/s";
            }
        }


        // Close program
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // Update label(s)
        private void lsbSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            formulas.SensorInt = lsbSensor.SelectedIndex;

            if (tbxFocalLength.Text != string.Empty &&
                lsbRule.SelectedIndex != -1)
            {
                lblRule.Text = $"{formulas.RuleExposureTime().ToString("#.####")}/s";
                lblFourCrop.Text = $"{formulas.FourCropExposureTime().ToString("#.####")}/s";
            }
        }

        private void tbxFilter_TextChanged(object sender, EventArgs e)
        {
            // Create new list of filtered data
            List<(string, string, string, string, string, string)> newList =
                   new List<(string, string, string, string, string, string)>();

            // Store data first, draw later, improves performance.
            foreach (var camera in cameraDb.Camera_Data)
            {
                if (camera.Item1.ToLower().Contains(tbxFilter.Text.ToLower()) || camera.Item2.ToLower().Contains(tbxFilter.Text.ToLower()))
                {
                    newList.Add(camera);
                }
            }

            // Clear list before redrawing
            lstCameraSensors.Items.Clear();

            foreach (var camera in newList)
            {
                ListViewItem li = lstCameraSensors.Items.Add(camera.Item1);
                li.SubItems.Add(camera.Item2);
                li.SubItems.Add(camera.Item3);
                li.SubItems.Add(camera.Item4);
                li.SubItems.Add(camera.Item5);
                li.SubItems.Add(camera.Item6);
            }
        }

        /// <summary>
        /// Highlights text when tabchanged. Easy editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbxPixelPitch_Enter(object sender, EventArgs e)
        {
            tbxPixelPitch.SelectionStart = 0;
            tbxPixelPitch.SelectionLength = tbxPixelPitch.Text.Length;
        }

        private void tbxFocalLength_Enter(object sender, EventArgs e)
        {
            tbxFocalLength.SelectionStart = 0;
            tbxFocalLength.SelectionLength = tbxFocalLength.Text.Length;
        }

        private void tbxFSstop_Enter(object sender, EventArgs e)
        {
            tbxFSstop.SelectionStart = 0;
            tbxFSstop.SelectionLength = tbxFSstop.Text.Length;
        }

        private void tbxDeclination_Enter(object sender, EventArgs e)
        {
            tbxDeclination.SelectionStart = 0;
            tbxDeclination.SelectionLength = tbxDeclination.Text.Length;
        }

        private void tbxFilter_Enter(object sender, EventArgs e)
        {
            tbxFilter.SelectionStart = 0;
            tbxFilter.SelectionLength = tbxFilter.Text.Length;
        }
    }
}
