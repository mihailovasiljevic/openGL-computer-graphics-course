

namespace RacunarskaGrafika
{
    using System;
    using Tao.OpenGl;//Imenski prostor za OpenGL/ GLU
    using Assimp;
    using System.IO;
    using System.Reflection;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    /// <summary>
    ///  Nabrojani tip OpenGL rezima filtriranja tekstura
    /// </summary>
    public enum TextureFilterMode
    {
        Nearest,
        Linear,
        NearestMipmapNearest,
        NearestMipmapLinear,
        LinearMipmapNearest,
        LinearMipmapLinear
    };

    public class World : IDisposable
    {
        #region Atributi
        //auto koji se parkira
        private float carPositionX;
        private float carPositionY;
        private float carPositionZ;
        private float carRotationAngle;

        /// <summary>
        ///	 Visina vertikalnih stubica.
        /// </summary>
        private int pylonHeight;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_bmw;
        private AssimpScene m_lamborgini1;
        private AssimpScene m_lamborgini2;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 1000.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///  Indikator stanja mehanizma sakrivanja nevidljivih povrsina.
        /// </summary>
        bool m_culling = true; // Ukljucivanje sakrivanja nevidljivih poligona (Ne iscratava se BACK strana poligiona)

        /// <summary>
        ///  Indikator stanja mehanizma za testiranje dubine.
        /// </summary>
        bool m_depthTesting = true; // Ukljucivanje testiranja dubine, tj. sakrivanja objekata koji su zaklonjeni nekim drugim objektom

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        /// <summary>
        ///	 Identifikator fonta.
        /// </summary>
        private BitmapFont m_font = null;

        /// <summary>
        ///	 Identifikatori tekstura za jednostavniji pristup teksturama
        /// </summary>
        private enum TextureObjects { Brick = 0, Road, Parking, Grass };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

        /// <summary>
        ///	 Identifikatori OpenGL tekstura
        /// </summary>
        private int[] m_textures = null;


        private string grassPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "grass.png");
        private string roadPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "asphalt_texture_seamless_by_rfalworth-d6y71cv.jpg");
        private string parkingPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "Road-Parking Lot.png");
        private string brickPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "brick.jpg");
        /// <summary>
        ///	 Putanje do slika koje se koriste za teksture
        /// </summary>
        private string[] m_textureFiles = {
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "brick.jpg"),
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "asphalt_texture_seamless_by_rfalworth-d6y71cv.jpg"),
           Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "Road-Parking Lot.png"),
            Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images"), "ground_texture895.jpg") };

        /// <summary>
        ///  Izabrana OpenGL mehanizam za iscrtavanje.
        /// </summary>
        private TextureFilterMode m_selectedMode = TextureFilterMode.Nearest;

        /// <summary>
        ///  Pomeraj po Z osi
        /// </summary>
        private float m_zPosition = -60.0f;

        //koordinate uparkiranih automobila
        private float firstX = -380f;
        private float secondX = 330f;
        #endregion

        #region Properties

        public float CarPositionX
        {
            get { return carPositionX; }
            set { carPositionX = value; }
        }
        public float CarPositionY
        {
            get { return carPositionY; }
            set { carPositionY = value; }
        }
        public float CarPositionZ
        {
            get { return carPositionZ; }
            set { carPositionZ = value; }
        }
        public float CarRotationAngle
        {
            get { return carRotationAngle; }
            set { carRotationAngle = value; }
        }

        public float FirstX
        {
            get { return firstX; }
            set { firstX = value; }
        }

        public float SecondX
        {
            get { return secondX; }
            set { secondX = value; }
        }

        /// <summary>
        ///	 Visina vertikalnih stubica.
        /// </summary>
        public int PylonHeight
        {
            get { return pylonHeight; }
            set { pylonHeight = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        ///  Indikator stanja mehanizma sakrivanja nevidljivih povrsina.
        /// </summary>
        public bool Culling
        {
            get { return m_culling; }
            set { m_culling = value; }
        }

        /// <summary>
        ///  Indikator stanja mehanizma za testiranje dubine.
        /// </summary>
        public bool DepthTesting
        {
            get { return m_depthTesting; }
            set { m_depthTesting = value; }
        }

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Bmw
        {
            get { return m_bmw; }
            set { m_bmw = value; }
        }

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Lamborgini1
        {
            get { return m_lamborgini1; }
            set { m_lamborgini1 = value; }
        }

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Lamborgini2
        {
            get { return m_lamborgini2; }
            set { m_lamborgini2 = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }


        /// <summary>
        ///  Izabrani OpenGL rezim stapanja teksture sa materijalom
        /// </summary>
        public TextureFilterMode SelectedMode
        {
            get { return m_selectedMode; }
            set
            {
                m_selectedMode = value;

                foreach (int textureId in m_textures)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureId);

                    switch (m_selectedMode)
                    {
                        case TextureFilterMode.Nearest:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
                            break;

                        case TextureFilterMode.Linear:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                            break;

                        case TextureFilterMode.NearestMipmapNearest:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_NEAREST);
                            break;

                        case TextureFilterMode.NearestMipmapLinear:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR);
                            break;

                        case TextureFilterMode.LinearMipmapNearest:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
                            break;

                        case TextureFilterMode.LinearMipmapLinear:
                            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
                            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float PositionZ
        {
            get { return m_zPosition; }
            set { m_zPosition = value; }
        }
        #endregion

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        /// <param name="width">Visina OpenGL kontrole u pikselima.</param>
        /// <param name="height">Sirina OpenGL kontrole u pikselima.</param>
        public World(String bmwPath, String bmwFileName, String lamborgini1Path, String lamborgini1FileName, String lamborgini2Path, String lamborgini2FileName, int width, int height)
        {
            this.m_bmw = new AssimpScene(bmwPath, bmwFileName);
          this.m_lamborgini1 = new AssimpScene(lamborgini1Path, lamborgini1FileName);
            this.m_lamborgini2 = new AssimpScene(lamborgini2Path, lamborgini2FileName);
            this.m_height = height;
            this.m_width = width;
            this.pylonHeight = 0;
            try
            {
                m_font = new BitmapFont("Verdana", 14, true, false, false, false);
                m_textures = new int[m_textureCount];
            }
            catch (Exception)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL fonta", "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.carPositionX = -330.0f;
            this.carPositionY = 60.0f;
            this.carPositionZ = 750.0f;
            this.carRotationAngle = 180.0f;

            //Korisnicka inicijalizacija OpenGL parametara
            Initialize();

            //Inicijalno podesavanje projekcije i viewport-a
            Resize();
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false); //Dispoze metod dolazi od filnaizer-a ne iz dispose metode.
        }

        #endregion

        #region Metode

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw() 
        {

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            // Pomeraj objekat po z-osi
            Gl.glPushMatrix();
            // Kamera
            Glu.gluLookAt(m_sceneDistance / 3, m_sceneDistance/3, -m_sceneDistance*1.5, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f);

            Gl.glTranslatef(0.0f, 0.0f, -m_sceneDistance);
            Gl.glRotatef(m_xRotation+15, 1.0f, 0.0f, 0.0f); //+5
            Gl.glRotatef(m_yRotation+20, 0.0f, 1.0f, 0.0f); //+20

            // TODO 4: Modelovati podlogu podlogu koristeći GL_QUADS primitivu
            DrawBase(); // iscrtaj bazu 
            // TODO 5: Modelovati ulicu, koja ide pored i potom skreće na parking, koristeći GL_QUADS primitive
            DrawStreet(); // iscrtaj ulicu
            DrawParking(); // iscrtaj parking
            // TODO 6: Modelovati zidove oko parkinga, koristeći instance Box klase
            DrawWalls(); //iscrtaj zidove
            DrawModels(); //iscrtaj modele automobila
            
            // TODO 7: tri vertikalna stubića na ulasku na parking, koristeći gluCylinder i gluDisk objekte
            DrawPylons();


            drawLightBulb();

            drawParkingLightSource();

            // TODO 8: Ispisati bitmap tekst zelenom bojom u donjem desnom uglu prozora (redefinisati
            //         projekciju korišćenjem gluOrtho2D metode). Font je Verdana, 14pt, bold.
            DrawText();
            Gl.glPopMatrix();
            // kraj iscrtavanja
            Gl.glFlush();
            
        
        }


        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        private void Initialize()
        {

            // Boja pozadine je bela
            // Boja pozadine je teget, a boja ispisa je crna
            Gl.glClearColor(0.0f, 0.0f, 0.2588f, 1.0f); // teget boja
            Gl.glColor3ub(0, 145, 45); // poja podloge - zelena

            //TODO 1: Uključiti testiranje dubine i sakrivanje nevidljivih površina

            // podesi testiranje dubine ukoliko je potrebno
            if (m_depthTesting == true)
                Gl.glEnable(Gl.GL_DEPTH_TEST);
            else
                Gl.glDisable(Gl.GL_DEPTH_TEST);

            // podesi sakrivanje poligina ukoliko je potrebno
            if (m_culling == true)
                Gl.glEnable(Gl.GL_CULL_FACE);
            else
                Gl.glDisable(Gl.GL_CULL_FACE);

            //TODO 2.1: Uključiti color tracking mehanizam i podesiti da se pozivom metode glColor definiše
            //           ambijentalna i difuzna komponenta materijala.

            // Ukljuci color tracking
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            // Podesi na koje parametre materijala se odnose pozivi glColor funkcije
            Gl.glColorMaterial(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE);

            //TODO 2.2: Definisati tačkasti svetlosni izvor bele boje i pozicionirati ga gore-desno u odnosu na
            //          centar scene (na pozitivnom delu vertikalne i horizontalne ose). Svetlosni izvor treba da
            //          bude stacionaran (tj.transformacije nad modelom ne utiču na njega). Definisati normale
            //          za podlogu, ulicu i garažu. Uključiti normalizaciju.

            //TODO 2.2.1: podesavanje tackastog izvora svetlosti gore desno u odnosu na scenu, izvor je stacionaran
            float[] ambient = { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] ambiental = { 0.2f, 0.2f, 0.2f, 1.0f };

            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, ambiental);

            // Podesi parametre LIGHT0 svetlosnog izvora (ambijentalnu i difuznu komponentu)
            //         Podesi svetlosni izvor da bude tackasti
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambient);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuse);
            Gl.glLightf(Gl.GL_LIGHT0, Gl.GL_SPOT_CUTOFF, 180.0f);

            //Kreiraj objekat koji reprezentuje tackasti izvor svetlosti
            Glu.GLUquadric m_gluObj = Glu.gluNewQuadric();
            Glu.gluQuadricNormals(m_gluObj, Glu.GLU_SMOOTH);

            //Ukljuci proracun osvetljenja i svetlosni LIGHT0, kao i normalizaciju
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            //TODO 2.2.2: podesavanje normala na podlogu, ulicu i garazu, ukljucivanje normalizacije
            Gl.glEnable(Gl.GL_NORMALIZE);

            //TODO 2.3: Za teksture podesiti wrapping da bude GL_REPEAT po obema osama. Podesiti filtere za
            //          teksture da budu linearno filtriranje.Način stapanja teksture sa materijalom postaviti da
            //          bude GL_ADD.
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_ADD);

            //TODO 2.4:Zidovima oko parkinga pridružiti teksturu zida od cigle. Ulici pridružiti teksturu asfalta.
            //         Definisati koordinate tekstura.
            //TODO 2.5:Parkingu pridružiti teksturu parkinga sa izvučenim linijama (slika koja se koristi je jedan
            //         segment parkinga). Pritom obavezno skalirati teksture (shodno potrebi). Skalirati teksture
            //         korišćenjem Texture matrice.Definisati koordinate teksture.

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

            //TODO 2.9 Definisati reflektorski svetlosni izvor (cut-off=35º) žute boje iznad uparkiranih automobila
            float[] smer = { 0.0f, 0.0f, -1.0f };
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPOT_DIRECTION, smer);
            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_CUTOFF, 35.0f);
            Gl.glEnable(Gl.GL_LIGHT1);
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize()
        {
            //TODO 2: Definisati projekciju u perspektivi (fov=60, near=1, a vrednost far zadati po potrebi) i viewport-om preko celog prozora unutar Resize metode

            // Kreiraj Viewport preko celog prozora 
            // mapiranje 2D projekcije scene u prozor na koji se projektuje
            // (0,0) <=> donji levi cosak ekrana
            // (m_width, m_heigt_ <=> sirina i duzina ekrana
            Gl.glViewport(0, 0, m_width, m_height);

            // Selektuj Projection Matrix
            // Projection matrica definise vidljivi volumen kao i tip rojekcije scene na ekran
            Gl.glMatrixMode(Gl.GL_PROJECTION);

            // Resetuj projection matricu
            Gl.glLoadIdentity();

            // Postavi da tip projekcije bude perspektiva
            // prvi parametar je dubina vidnog polja, tj. fov
            // drugi parametar je odnos sirine i visine
            // treci i cetvrti parametar zNear i zFar ili dubina po z-osi
            Glu.gluPerspective(60, (double)m_width / (double)m_height, 1.0, 7000.0);

            //
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

        }

        private void DrawBase()
        {

            Gl.glPushMatrix();
            
            // Gl.glColor3ub(21, 29, 0); // neka braon boja zemljista
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Grass]);
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3f(1000.0f, 0.5f, -1000.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3f(-1000.0f, 0.5f, -1000.0f);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3f(-1000.0f, 0.5f, 1000.0f);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f(1000.0f, 0.5f, 1000.0f);

            Gl.glEnd();
            Gl.glPopMatrix();
        }

        private void DrawStreet()
        {



            Gl.glPushMatrix();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Road]);
            Gl.glTranslatef(-250.0f, 0.0f, 0.0f); // transliraj u levo 250 da bi sa desne strane bilo mesta za parking
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3f(250.0f, 10f, -50.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3f(-250.0f, 10f, -50.0f);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3f(-250.0f, 10f, 1000.0f);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f(250.0f, 10f, 1000.0f);
            Gl.glEnd();
            Gl.glPopMatrix();


        }

        private void DrawParking()
        {
            Gl.glPushMatrix();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Parking]);

            //SKALIRANJE
            /*Gl.glPushAttrib(Gl.GL_CURRENT_BIT);
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glPushMatrix();
            Gl.glScalef(1.0f, 1.0f, 6f);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopAttrib();
            */
            Gl.glTranslatef(250.0f, 0.0f, 0.0f); //transliraj u desno 250 da bi izgledao kao da put skrece

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(2.0f, 0.0f);
            Gl.glVertex3f(250.0f, 10f, -50.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena
            Gl.glTexCoord2f(2.0f, 1.0f);
            Gl.glVertex3f(-250.0f, 10f, -50.0f);
            Gl.glTexCoord2f(0.0f,1.0f);
            Gl.glVertex3f(-250.0f, 10f, 500.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f(250.0f, 10f, 500.0f);
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        private void DrawWalls()
        {
            Gl.glPushMatrix();
            Gl.glColor3ub(204, 255, 247); // svetlo plava boja
            Gl.glTranslatef(250.0f, 20.0f, -50.0f); //pomeri se na kraj parkinga i iscrtaj levi zid kada se na parking ulazi s puta
            Gl.glTranslatef(0.0f, 140.0f, 0.0f); //izdigni zid da bude u nivou parkinga
            Box leftSideWall = new Box(500.0, 300, 10);
            leftSideWall.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glColor3ub(204, 255, 247);
            Gl.glTranslatef(500.0f, 20.0f, 220.0f);
            Gl.glRotatef(90.0f , 0.0f, 1.0f, 0.0f);
            Gl.glTranslatef(0.0f, 140.0f, 0.0f);
            Box rightSideWall = new Box(550.0, 300, 10);
            rightSideWall.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glColor3ub(204, 255, 247);
            Gl.glTranslatef(250.0f, 20.0f, 500.0f);
            Gl.glRotatef(180.0f, 0.0f, 1.0f, 0.0f);
            Gl.glTranslatef(0.0f, 140.0f, 0.0f);
            Box lastWall = new Box(500.0, 300, 10);
            lastWall.Draw();
            Gl.glPopMatrix();

        }

        private void DrawModels()
        {
            //TODO 3: Koristeći AssimpNet bibloteku i klasu AssimpScene, importovati 3 različita modela
            //        automobila. Ukoliko je model podeljen u nekoliko fajlova, potrebno ih je sve učitati i
            //        iscrtati. Skalirati modele, ukoliko je neophodno, tako da u celosti budu vidljivi.
            // Konstruktor World klase prima po 2 parametra kojim se ucitavaju modeli
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            //iscrtaj BMW850
            Gl.glPushMatrix();
            Gl.glTranslatef(firstX, 20.0f, 10.0f); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(45f, 45f,45f);
            m_bmw.Draw();
            Gl.glPopMatrix();

            //iscrtaj Lamborgini Countach
            Gl.glPushMatrix();
            Gl.glTranslatef(secondX, 20.0f, 220.0f); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(0.0f, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(0.6f, 0.6f, 0.6f);
            m_lamborgini1.Draw();
            Gl.glPopMatrix();

            //iscrtaj Lamborgini Murcielago 640
            Gl.glPushMatrix();
            Gl.glTranslatef(carPositionX, carPositionY, carPositionZ); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(carRotationAngle, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(60f,60f, 60f);
            m_lamborgini2.Draw();
            Gl.glPopMatrix();
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_ADD);
        }

        private void DrawPylons()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, pylonHeight + 12, 70.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon leftPylon = new Pylon(10, 10, pylonHeight, 5, 60);
            leftPylon.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, pylonHeight +12, 240.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon centerPylon = new Pylon(10, 10, pylonHeight, 5, 60);
            centerPylon.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, pylonHeight + 12, 410.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon rightPylon = new Pylon(10, 10, pylonHeight, 5, 60);
            rightPylon.Draw();
            Gl.glPopMatrix();
        }

        private void DrawText()
        {
            // Postavi projekciju da bude ortogonalna i vrati se na modelview matricu
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(-m_width / 2.0, m_width / 2.0, -m_height / 2.0, m_height / 2.0, -1.0, 1.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glPushMatrix();
            Gl.glColor3ub(0, 255, 34);
            Gl.glTranslatef(m_width/3f, -m_height/3, 0.0f);

            Gl.glRasterPos2d(-0.5 * m_font.CalculateTextWidth("Predmet: Racunarska grafika"), 0);
            m_font.DrawText("Predmet: Racunarska grafika");

            Gl.glTranslatef(-55.0f, -18.0f, 0.0f);
            Gl.glRasterPos2d(-0.5 * m_font.CalculateTextWidth("Sk. god: 2015/ 16."), 0);
            m_font.DrawText("Sk. god: 2015/ 16.");

            Gl.glTranslatef(-35.0f, -20.0f, 0.0f);
            Gl.glRasterPos2d(-0.5 * m_font.CalculateTextWidth("Ime: Mihailo"), 0);
            m_font.DrawText("Ime: Mihailo");

            Gl.glTranslatef(33.0f, -20.0f, 0.0f);
            Gl.glRasterPos2d(-0.5 * m_font.CalculateTextWidth("Prezime: Vasiljevic"), 0);
            m_font.DrawText("Prezime: Vasiljevic");

            Gl.glTranslatef(-30.0f, -20.0f, 0.0f);
            Gl.glRasterPos2d(-0.5 * m_font.CalculateTextWidth("Sifra zad: 5.2"), 0);
            m_font.DrawText("Sifra zad: 5.2");


            Gl.glPopMatrix();

        }
        private static float[] lightBulbPosition = {1000.0f, 800.0f, -1000.0f, 1.0f };
        private static float[] emission = new float[4];
        private static float[] lightBulbColor = { 1.0f, 1.0f, 1.0f, 1.0f }; // bela boja
        private void drawLightBulb()
        {

            // Postavi svetlosni izvor na poziciju lightBulbPoistion
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightBulbPosition);

            Gl.glGetMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, emission);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, lightBulbColor);

            // Iskljuci osvetljenje samo za izvor svetlosti
            Gl.glDisable(Gl.GL_LIGHTING);
            Glu.GLUquadric gluSphere = Glu.gluNewQuadric();
            Glu.gluQuadricNormals(gluSphere, Glu.GLU_SMOOTH);
            Gl.glPushMatrix();

            Gl.glTranslatef(lightBulbPosition[0], lightBulbPosition[1], lightBulbPosition[2]);

            Gl.glColor3ub(255, 255, 255);
            Glu.gluSphere(gluSphere, 60.0f, 24, 24);

            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, emission);
            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(gluSphere);

        }
        public Color ambientColor = Color.FromArgb(1, 255, 255, 0);
        private static float[] parkingLightSourcePosition = { 300.0f, 400.0f, 200.0f, 1.0f };
        public static float[] parkingLightSourceColor = { 1.0f, 1.0f, 0.0f, 1.0f }; // zuta boja
        private void drawParkingLightSource()
        {
            // Postavi svetlosni izvor na poziciju lightBulbPoistion
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, parkingLightSourcePosition);

            Gl.glGetMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, emission);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, parkingLightSourceColor);

            // Iskljuci osvetljenje samo za izvor svetlosti
            Gl.glDisable(Gl.GL_LIGHTING);
            Glu.GLUquadric gluSphere = Glu.gluNewQuadric();
            Glu.gluQuadricNormals(gluSphere, Glu.GLU_SMOOTH);
            Gl.glPushMatrix();

            Gl.glTranslatef(parkingLightSourcePosition[0], parkingLightSourcePosition[1], parkingLightSourcePosition[2]);

            Gl.glColor3f(parkingLightSourceColor[0], parkingLightSourceColor[1], parkingLightSourceColor[2]);
            Glu.gluSphere(gluSphere, 20.0f, 24, 24);

            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, emission);
            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(gluSphere);
        }
        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Oslodi managed resurse
            }

            // Oslobodi unmanaged resurse
            m_bmw.Dispose();
           m_lamborgini1.Dispose();
           m_lamborgini2.Dispose();
            m_font.Dispose();
            Terminate();
        }


        /// <summary>
        ///  Metoda azurira pozicije objekata.
        /// </summary>
        /// <param name="value">Vrednost koja predstavlja preostali broj frejmova animacije</param>
        public void Update(long value)
        {
            /*
            // Animacija sletanja broda
            if (value > 20)
            {
                m_ship.PositionZ += 0.075f;
                m_ship.PositionY -= 0.005f;
                m_ship.PositionX += 0.03f;

                m_ship.RotationZ += 0.75f;
                m_ship.Scale += 0.2f;
            }
            else
            {
                if (value > 0)
                {
                    m_ship.PositionY -= 0.05f; // spustamo brod ispred kuce
                }
            }

            // Animacija promene svetla prozora
            m_deltaTexture -= 0.05f;

            if (m_deltaColor == (byte)0)
            {
                m_deltaColor = (byte)100;
            }
            else
            {
                m_deltaColor = (byte)0;
            }
            */
        }



        #endregion

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Korisnicko oslobadjanje OpenGL resursa.
        /// </summary>
        private void Terminate()
        {
            Gl.glDeleteTextures(m_textureCount, m_textures);
        }
        #endregion IDisposable metode

    }
}
