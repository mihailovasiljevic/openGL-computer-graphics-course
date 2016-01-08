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
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\RollsRoyce"), "RollsRoyce.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\LEG_CAR_B1"), "LEGO_CAR_B1.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\M5"), "Car BMW M5 N071210.3DS", openGlWorld.Width, openGlWorld.Height);
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

        /// <summary>
        /// Rukovalac dogadjaja: obrada tastera nad OpenGL kontrolom
        /// </summary>
        private void openGlWorld_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q: Dispose(); break;
                case Keys.I: if (e.Control) m_world.PositionZ -= 0.1f; else m_world.RotationX -= 5.0f; break;
                case Keys.K:  if (e.Control) m_world.PositionZ += 0.1f; else m_world.RotationX += 5.0f; break;
                case Keys.J: m_world.RotationY -= 5.0f; break;
                case Keys.L: m_world.RotationY += 5.0f; break;
                case Keys.Add: if (-m_world.SceneDistance < -100) m_world.SceneDistance -= 5; break;
                case Keys.Subtract: if (-m_world.SceneDistance > -2000) m_world.SceneDistance += 5; break;
            }

            if (m_world.PositionZ < -0.8f) m_world.PositionZ = -0.8f;
            else if (m_world.PositionZ > 3.0f) m_world.PositionZ = 3.0f;
            openGlWorld.Refresh();
            m_world.Resize();
        }

        private void tbVisinaStubica_Scroll(object sender, EventArgs e)
        {
            m_world.PylonHeight = 6 * (tbVisinaStubica.Value);
            openGlWorld.Refresh();
            m_world.Resize();
        }




    }
}