// -----------------------------------------------------------------------
// <file>MainForm.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Zoran Milicevic</author>
// <summary>Demonstracija ucitavanja modela pomocu AssimpNet biblioteke i koriscenja u OpenGL-u.</summary>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assimp;
using System.IO;
using System.Reflection;

namespace RacunarskaGrafika
{
    public partial class carParkingForm : Form
    {
        private float oldValueFirst;
        private float oldValueSecond;
        private float oldValueX;
        private float oldValueAngle;
        private float oldValueZ;
        private bool animation;
        /// <summary>
        /// Trajanje animacije parkiranja
        /// </summary>
        private long m_duration = 120;
        #region Atributi

        /// <summary>
        ///  Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion

        #region Konstruktori

        /// <summary>
        ///  Konstruktor forme.
        /// </summary>
        public carParkingForm()
        {
            // Inicijalizj komponente forme
            InitializeComponent();

            // Inicijalizuj OpenGL konteksta
            openGlWorld.InitializeContexts();

            //Kreiraj OpenGl svet
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\RollsRoyce"), "RollsRoyce.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\LEG_CAR_B1"), "LEGO_CAR_B1.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\N17"), "Car N170814.3DS", openGlWorld.Width, openGlWorld.Height);
                oldValueFirst = m_world.FirstX;
                oldValueSecond = m_world.SecondX;
                this.oldValueX = m_world.CarPositionX;
                this.oldValueAngle = m_world.CarRotationAngle;
                this.oldValueZ = m_world.CarPositionZ;
                animation = false;
                tbVisinaStubica.Value = 10;
                tbVisinaStubica_Scroll(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neuspesno kreiranje instance OpenGL sveta", "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #endregion

        #region Rukovaoci dogadjajima OpenGL kontrola

        /// <summary>
        /// Rukovalac dogadjaja: izmena dimenzije OpenGL kontrole
        /// </summary>

        private void OpenGlControlResize(object sender, EventArgs e)
        {
            base.OnResize(e);
            m_world.Height = this.Height;
            m_world.Width = this.Width;
            m_world.Resize();
        }

        /// <summary>
        /// Rukovalac dogadjaja: iscrtavanje OpenGL kontrole
        /// </summary>

        private void OpenGlControlPaint(object sender, PaintEventArgs e)
        {
            // Iscrtaj svet
            m_world.Draw();
        }

        #endregion



        private void tbVisinaStubica_Scroll(object sender, EventArgs e)
        {
            m_world.PylonHeight = 6 * (tbVisinaStubica.Value);
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void btnIzborBoje_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog.ShowDialog();
            if(dr == DialogResult.OK)
            {
                World.parkingLightSourceColor[0] = colorDialog.Color.R/255;
                World.parkingLightSourceColor[1] = colorDialog.Color.G / 255;
                World.parkingLightSourceColor[2] = colorDialog.Color.B / 255;
                pnlIzborBOje.BackColor = colorDialog.Color;
                openGlWorld.Refresh();
                m_world.Resize();
            }
        }

        private void tbPomeriPrvi_Scroll(object sender, EventArgs e)
        {

            m_world.FirstX = oldValueFirst + (tbPomeriPrvi.Value)*(-50);
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void tbPomeriDrugi_Scroll(object sender, EventArgs e)
        {
            m_world.SecondX = oldValueSecond + (tbPomeriDrugi.Value) * (-50);
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void openGlWorld_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q: if(animation == true) return; Dispose(); break;
                case Keys.I: if (animation == true) return; if (e.Control) m_world.PositionZ -= 0.1f; else m_world.RotationX -= 5.0f; break;
                case Keys.K: if (animation == true) return; if (e.Control) m_world.PositionZ += 0.1f; else m_world.RotationX += 5.0f; break;
                case Keys.J: if (animation == true) return; m_world.RotationY -= 5.0f; break;
                case Keys.L: if (animation == true) return; m_world.RotationY += 5.0f; break;
                case Keys.Add: if (animation == true) return; if (-m_world.SceneDistance < -100) m_world.SceneDistance -= 50; break;
                case Keys.Subtract: if (animation == true) return; if (-m_world.SceneDistance > -25000) m_world.SceneDistance += 50; break;
                case Keys.P: animation = true; allControlsChange(false); carParkingTimer.Start();
                    TimeSpan timeout = TimeSpan.FromSeconds(1);
                    DateTime start_time = DateTime.Now;

                    while (DateTime.Now - start_time < timeout)
                    {
                        //something is happening, just loop waiting
                    }
                    allControlsChange(true); animation = false;

                    break;
            }

            if (m_world.PositionZ < -0.8f) m_world.PositionZ = -0.8f;
            else if (m_world.PositionZ > 3.0f) m_world.PositionZ = 3.0f;
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Azuriraj svet i smanji trajanje
            m_world.Update(--m_duration);

            // Ponovo iscrtaj kontrolu
            openGlWorld.Refresh();
            m_world.Resize();
        }
        private void allControlsChange(bool enabled)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = enabled;
                c.Visible = enabled;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_world.CarPositionX = oldValueX + (trackBar1.Value) * (20);
            Console.WriteLine("PositionX: " + m_world.CarPositionX);
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            m_world.CarRotationAngle = oldValueAngle + (trackBar2.Value) * (-5);
            Console.WriteLine("Angle: " + m_world.CarRotationAngle);
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            m_world.CarPositionZ = oldValueZ + (trackBar3.Value) * (-20);
            Console.WriteLine("PositionZ: " + m_world.CarPositionZ);
            openGlWorld.Refresh();
            m_world.Resize();
        }
    }
}