// -----------------------------------------------------------------------
// <file>Box.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2012.</copyright>
// <author>Srdjan Mihic</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod za iscrtavanje kvadra sa tezistem u koord.pocetku.</summary>
// -----------------------------------------------------------------------
namespace RacunarskaGrafika
{
    using Tao.OpenGl;
    using System.IO;
    using System.Reflection;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System;
    using System.Windows.Forms;/// <summary>
                               ///  Klasa enkapsulira OpenGL kod za iscrtavanje kvadra.
                               /// </summary>
    public class Box
    {
        #region Atributi

        /// <summary>
        ///	 Visina kvadra.
        /// </summary>
        double m_height = 1.0;

        /// <summary>
        ///	 Sirina kvadra.
        /// </summary>
        double m_width = 1.0;

        /// <summary>
        ///	 Dubina kvadra.
        /// </summary>
        double m_depth = 1.0;

        /// <summary>
        ///	 Identifikatori tekstura za jednostavniji pristup teksturama
        /// </summary>
        private enum TextureObjects { Brick = 0};
        private readonly int m_textureCount = System.Enum.GetNames(typeof(TextureObjects)).Length;

        /// <summary>
        ///	 Identifikatori OpenGL tekstura
        /// </summary>
        private int[] m_textures = null;

        /// <summary>
        ///	 Putanje do slika koje se koriste za teksture
        /// </summary>
        private string[] m_textureFiles = {
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "brick.jpg")};

        /// <summary>
        ///  Izabrana OpenGL mehanizam za iscrtavanje.
        /// </summary>
        private TextureFilterMode m_selectedMode = TextureFilterMode.Nearest;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Visina kvadra.
        /// </summary>
        public double Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        ///	 Sirina kvadra.
        /// </summary>
        public double Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Dubina kvadra.
        /// </summary>
        public double Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///		Konstruktor.
        /// </summary>
        public Box()
        {
        }

        /// <summary>
        ///		Konstruktor sa parametrima.
        /// </summary>
        /// <param name="width">Sirina kvadra.</param>
        /// <param name="height">Visina kvadra.</param>
        /// <param name="depth"></param>
        public Box(double width, double height, double depth)
        {
            this.m_width = width;
            this.m_height = height;
            this.m_depth = depth;
            try
            {
                m_textures = new int[m_textureCount];
            }
            catch (Exception)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL fonta", "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Konstruktori

        #region Metode

        public void Draw()
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            // Ucitaj slike i kreiraj teksture
            Gl.glGenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, (int)Gl.GL_RGBA8, image.Width, image.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, imageData.Scan0);
                Gl.glTexParameteri((int)Gl.GL_TEXTURE_2D, (int)Gl.GL_TEXTURE_MIN_FILTER, (int)Gl.GL_LINEAR);		// Linear Filtering
                Gl.glTexParameteri((int)Gl.GL_TEXTURE_2D, (int)Gl.GL_TEXTURE_MAG_FILTER, (int)Gl.GL_LINEAR_MIPMAP_LINEAR);		// Linear Filtering

                image.UnlockBits(imageData);
                image.Dispose();
            }

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Brick]);
            Gl.glBegin(Gl.GL_QUADS);
            // Zadnja
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, -m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f,1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, -m_depth / 2);

            // Desna
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, -m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, m_depth / 2);

            // Prednja
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, m_depth / 2);

            // Leva
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, -m_depth / 2);

            // Donja
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, -m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, -m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, m_depth / 2);

            // Gornja
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glEnd();
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_ADD);
        }

        public void SetSize(double width, double height, double depth)
        {
            m_depth = depth;
            m_height = height;
            m_width = width;
        }

        #endregion Metode
    }
}
