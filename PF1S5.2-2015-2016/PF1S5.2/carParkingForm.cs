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
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\BMW850"), "BMW850.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Lamborgini1"), "Countach.3ds", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Lamborgini2"), "MURCIELAGO640.3ds", openGlWorld.Width, openGlWorld.Height);
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
                case Keys.F10: this.Close(); break;
                case Keys.W: if (m_world.RotationX == 360) { m_world.RotationX = 0; } m_world.RotationX += 5.0f; break;
                case Keys.S: if (m_world.RotationX == -360) { m_world.RotationX = 0; } m_world.RotationX -= 5.0f; break;
                case Keys.A: if (m_world.RotationY == 360) { m_world.RotationY = 0; } m_world.RotationY += 5.0f; break;
                case Keys.D: if (m_world.RotationY == 360) { m_world.RotationY = 0; } m_world.RotationY -= 5.0f; break;
            }

            openGlWorld.Refresh();
            m_world.Resize();
        }


    }
}