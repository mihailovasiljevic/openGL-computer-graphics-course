

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

    public class World : IDisposable
    {
        #region Atributi
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

        #endregion

        #region Properties

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

            try
            {
                m_font = new BitmapFont("Verdana", 14, true, false, false, false);
            }
            catch (Exception)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL fonta", "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Korisnicka inicijalizacija OpenGL parametara
            this.Initialize();

            //Inicijalno podesavanje projekcije i viewport-a
            this.Resize();
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
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_ADD);
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
            Gl.glColor3ub(21, 29, 0); // neka braon boja zemljista
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(1000.0f, 0.5f, -1000.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glVertex3f(-1000.0f, 0.5f, -1000.0f);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glVertex3f(-1000.0f, 0.5f, 1000.0f);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glVertex3f(1000.0f, 0.5f, 1000.0f);

            Gl.glEnd();
            Gl.glPopMatrix();
        }

        private void DrawStreet()
        {
            Gl.glPushMatrix();
            Gl.glColor3ub(245, 245, 245); // siva boja
            Gl.glTranslatef(-250.0f, 0.0f, 0.0f); // transliraj u levo 250 da bi sa desne strane bilo mesta za parking
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(250.0f, 10f, -50.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena

            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glVertex3f(-250.0f, 10f, -50.0f);

            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glVertex3f(-250.0f, 10f, 1000.0f);

            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glVertex3f(250.0f, 10f, 1000.0f);
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        private void DrawParking()
        {
            Gl.glPushMatrix();
            Gl.glColor3ub(245, 245, 245); // siva boja
            Gl.glTranslatef(250.0f, 0.0f, 0.0f); //transliraj u desno 250 da bi izgledao kao da put skrece

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3f(250.0f, 10f, -50.0f); // velike vrednosti zato sto je projekciona povrsina dosta udaljena
            Gl.glVertex3f(-250.0f, 10f, -50.0f);
            Gl.glVertex3f(-250.0f, 10f, 500.0f);
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

            //iscrtaj BMW850
            Gl.glPushMatrix();
            Gl.glTranslatef(380f, 20.0f, 300.0f); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(45.0f, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(3f, 3f, 3f);
            m_bmw.Draw();
            Gl.glPopMatrix();

            //iscrtaj Lamborgini Countach
            Gl.glPushMatrix();
            Gl.glTranslatef(-400f, 60.0f, 350.0f); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(0.0f, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(1.8f, 1.8f, 1.8f);
            m_lamborgini1.Draw();
            Gl.glPopMatrix();

            //iscrtaj Lamborgini Murcielago 640
            Gl.glPushMatrix();
            Gl.glTranslatef(180f, 20f, 250f); // udalji objekat od kamere da bi se video ceo
            Gl.glRotatef(180.0f, 0.0f, 1.0f, 0.0f);
            Gl.glScalef(55f,55f, 55f);
            m_lamborgini2.Draw();
            Gl.glPopMatrix();
            
        }

        private void DrawPylons()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 60.0f, 0.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon leftPylon = new Pylon(10, 10, 60, 5, 60);
            leftPylon.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 60.0f, 250.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon centerPylon = new Pylon(10, 10, 60, 5, 60);
            centerPylon.Draw();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 60.0f, 460.0f);
            Gl.glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
            Gl.glColor3ub(255, 255, 0); // zuta boja
            Pylon rightPylon = new Pylon(10, 10, 60, 5, 60);
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

        #endregion IDisposable metode
    }
}
