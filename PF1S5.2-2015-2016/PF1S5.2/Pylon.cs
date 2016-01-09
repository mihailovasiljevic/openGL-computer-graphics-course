﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RacunarskaGrafika
{
    using Tao.OpenGl;

    /// <summary>
    ///  Klasa enkapsulira OpenGL kod za iscrtavanje stubica.
    /// </summary>
    public class Pylon
    {
        #region Atributi

        /// <summary>
        ///	 Gornja baza valjka.
        /// </summary>
        double m_topBase = 1.0;

        /// <summary>
        ///	 Donja baza valjka.
        /// </summary>
        double m_bottomBase = 1.0;

        /// <summary>
        ///	 Visina valjka
        /// </summary>
        float m_height = 1.0f;

        /// <summary>
        ///	 Unutrasnji precnik diska
        /// </summary>
        double m_innerRadius = 0.5;

        /// <summary>
        ///	 Spoljasnji precnik diska
        /// </summary>
        double m_outerRadius = 1.0;

        System.Drawing.Color color;
        bool turnOnLight;
        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Gornja baza valjka.
        /// </summary>
        public double TopBase
        {
            get { return m_topBase; }
            set { m_topBase = value; }
        }

        /// <summary>
        ///	 Donja baza valjka.
        /// </summary>
        public double BottomBase
        {
            get { return m_bottomBase; }
            set { m_bottomBase = value; }
        }


        /// <summary>
        ///	 Visina valjka
        /// </summary>
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        ///	 Unutrasnji precnik diska
        /// </summary>
        public double  InnerRadius
        {
            get { return m_innerRadius; }
            set { m_innerRadius = value; }
        }

        /// <summary>
        ///	 Spoljasnji precnik diska
        /// </summary>
        public double OuterRadius
        {
            get { return m_outerRadius; }
            set { m_outerRadius = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///		Konstruktor.
        /// </summary>
        public Pylon()
        {
        }

        /// <summary>
        ///		Konstruktor sa parametrima.
        /// </summary>
        /// <param name="width">Sirina kvadra.</param>
        /// <param name="height">Visina kvadra.</param>
        /// <param name="depth"></param>
        public Pylon(double topBase, double bottomBase, float height, double innerRadius, double outerRadius, System.Drawing.Color color, bool turnOnLight)
        {
            this.m_topBase = topBase;
            this.m_bottomBase = bottomBase;
            this.m_height = height;
            this.m_innerRadius = innerRadius;
            this.m_outerRadius = outerRadius;
            this.color = color;
            this.turnOnLight = turnOnLight;
        }

        #endregion Konstruktori

        public void Draw()
        {
            Glu.GLUquadric gluCylinderObject = Glu.gluNewQuadric();
            Glu.GLUquadric gluDiskObject = Glu.gluNewQuadric();
            Glu.GLUquadric gluLightSphere = Glu.gluNewQuadric();

            Gl.glPushMatrix();
            Glu.gluCylinder(gluCylinderObject, m_bottomBase, m_topBase, m_height, 128, 128);

            Gl.glTranslatef(0.0f, 0.0f,0.0f );                              //transliraj prsten za duzinu stubica prema posmatracu da vi mu stojao na vrhu
            Gl.glRotatef(180.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(0, 191, 255);
            Glu.gluDisk(gluDiskObject, m_innerRadius/4, m_topBase, 128, 128);
            Gl.glTranslatef(0.0f, 0.0f, 5.0f);
            float[] pos = { 0.0f, 0.0f, 5.0f, 1.0f };
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, pos);
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_POSITION, pos);
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_POSITION, pos);
            if (turnOnLight)
            {
                Gl.glEnable(Gl.GL_LIGHT2);
                Gl.glEnable(Gl.GL_LIGHT3);
                Gl.glEnable(Gl.GL_LIGHT4);
            }
            else
            {
                Gl.glDisable(Gl.GL_LIGHT2);
                Gl.glDisable(Gl.GL_LIGHT3);
                Gl.glDisable(Gl.GL_LIGHT4);
            }
            Gl.glColor3ub(color.R, color.G, color.B);

            Glu.gluSphere(gluLightSphere, m_innerRadius, 24, 24);
            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(gluCylinderObject);
            Glu.gluDeleteQuadric(gluDiskObject);
            Glu.gluDeleteQuadric(gluLightSphere);
        }
    }
}
