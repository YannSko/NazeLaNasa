using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
namespace NASA_API_Example
{
    partial class Form1
    {
        // les liens pour acceders aux api
        private const string API_Neo = "https://api.nasa.gov/neo/rest/v1/feed?start_date=2023-01-31&end_date=2023-01-31&api_key=wQ1JG1QCeewvuDMmRUU2ApQllPJBkNquLP1UCfLv";

        private const string DATA_API = "https://api.nasa.gov/planetary/apod?api_key=wQ1JG1QCeewvuDMmRUU2ApQllPJBkNquLP1UCfLv";

        
        // int const feed 
        private Form1 _form1;

        

        //._selectedAsteroidName;
            
        
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        /// 

        // fonctions permettant de recup le nom de chaque asteroide
        async Task<string> NameAsteroid()
        {
            string Result = "";

            string[] Neo_name2 = new string[] { "" };
            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            // suite à des requetes , on va boucler au sein de tout le content recuperer , on stock sous forme de string 
            if (response.IsSuccessStatusCode)
            {
                for (int i = 1; i < 20; i++)
                {
                    string AllData = await response.Content.ReadAsStringAsync();
                   // near earth object et un pivto dans la processus de split ( le name se trouve dans  le " bloc qui suit ")
                    string[] AllDatasplitted = AllData.Split("near_earth_objects");

                    string NEO_all = AllDatasplitted[1];
                    
                    string[] NEO_object = NEO_all.Split("link");
                    string Neo_names = NEO_object[i];
                    string[] Neo_name_tab = Neo_names.Split("name");
                    string Neo_name = Neo_name_tab[1];
                    Neo_name = Neo_name.Substring(3);

                    Neo_name2 = Neo_name.Split("\"");

                    Result = Result + Neo_name2[0] + ",";
                    // suite à un jeu de découpe on recupere toute la liste des astéroides
                }
            }
           
           
            return Result;
        }

        // On poursuit le meme raisonnement mais on utilise une autre technique de split
        
        async Task<string> MissDistance(string Object_Name)
        {
            string missDistance = "";
            Console.WriteLine("Poppers");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("\"name\":");
                foreach (string data in AllDatasplitted)
                {
                    if (data.Contains(Object_Name))
                    {
                        int startIndex = data.IndexOf("\"miss_distance_kilometers\":") + "\"miss_distance_kilometers\":".Length;
                        int endIndex = data.IndexOf(",", startIndex);
                        missDistance = data.Substring(startIndex, endIndex - startIndex);
                        break;
                    }
                }
            }
            return missDistance;
        }

        // Recupere tout les ids  des asteroides selectionnés, on reinjectera cette id lors de la combox afin dobtenir des informations supplementaires
        async Task<string> Id(string Object_Name)
        {
            string Ids = "";
            Console.WriteLine("Poppers");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("\"name\":");
                foreach (string data in AllDatasplitted)
                {
                    if (data.Contains(Object_Name))
                    {
                        
                        int startIndex = data.IndexOf("nasa_jpl_url\":\"http://ssd.jpl.nasa.gov/sbdb.cgi?sstr=") + "nasa_jpl_url\":\"http://ssd.jpl.nasa.gov/sbdb.cgi?sstr=".Length;
                        int endIndex = data.IndexOf("\"", startIndex);
                        Ids = data.Substring(startIndex, endIndex - startIndex);
                        break;
                    }
                }
            }
            return Ids;
        }
       
        //meme principe que le bloc precedent
        // cette fonction permet de récuperer le close approach date de l'api néo ( cette fonction est fonctionelle , mais elle ne repond pas à la consigne ) 
        // elle est laissée en temps que rappel à quelle point c'était long à faire :)
        async Task<string> Close_Approach_Fd_Obj(string Object_Name)
        {
            string fd = "";

            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("near_earth_objects");

                string NEO_all = AllDatasplitted[1];
                string[] NEO_object = NEO_all.Split(Object_Name);
                string NEO_object_name = NEO_object[1];
                string[] close_approach_data = NEO_object_name.Split("close_approach_date");
                //recupere la ligne full date pour lafficher
                string pfda = close_approach_data[1];
                // Recupere la date sous forme  yyyy-mm-dd
                string fda = pfda.Substring(2);
                
                fd = fda;
            }


            return fd;
        }
        //meme principe que le bloc precedent , inutile dans la réalisation finale , mais en terme de souvenir  
        async Task<string> Nom_Obj(string Object_Name)
        {
            string nm = "";

            Console.WriteLine(API_Neo);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(API_Neo);
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("near_earth_objects");

                string NEO_all = AllDatasplitted[1];
                string[] NEO_object = NEO_all.Split(Object_Name);
                string NEO_object_data = NEO_object[0];
                string[] neo_ref = NEO_object_data.Split("neo_reference_id");
                string post_neo = neo_ref[1];
                string[] pm = post_neo.Split("name");
                string fda = pm[1];
                string[] fpa = fda.Split("nasa_jpl_url");
                string fta = fpa[0];
                nm = fta;

            }
            // dans fta , je recupere le nom de lasteroide neo

            return nm;
        }

        // Fonction permettant de recuperer via une requete de la data api
        async Task<string> GlobalDataAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            var data = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string[] splitted = content.Split("url");
                string url = splitted[2];
                // on recupere le string par substring
                url = url.Substring(3, url.Length - 6);
                Console.WriteLine("Lien de l'image " + url);
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    //suite à la requete on place dans un tableau de byte
                    byte[] imageData = await resp.Content.ReadAsByteArrayAsync();
                    using (var stream = new MemoryStream(imageData))
                    {
                        // on le convertit en image afin de lafficher  en fond d'écran
                        var image = Image.FromStream(stream);
                        this.BackgroundImage = new Bitmap(image);
                        this.BackgroundImageLayout = ImageLayout.Zoom;

                    }
                }

                data = await response.Content.ReadAsStringAsync();

            }

            return data;
        }

        // recupérration de la description de lapod ,  split plutot court et simple en raison de la taille de lapod
        private async Task<string> GetDescriptionAsync()
        {
            string Description2 = "test";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(content);
                string[] splitted = content.Split("explanation");
                Description2 = splitted[1];

                Description2 = Description2.Substring(3, Description2.Length - 200);





            }
            return Description2;
        }
        // récuperation du titre de lapod
        private async Task<string> GetTitleAsync()
        {
            string Title = "";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(DATA_API);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(content);
                string[] splitted = content.Split("title");

                Title = splitted[1];

                Title = Title.Substring(3, Title.Length - 5);
                string[] tokens = Title.Split('"');
                Title = "Name of the picture : " + tokens[0];

            }
            return Title;
        }

        // instanciation de tout les elements du webform 
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Help;
            this.button1.Location = new System.Drawing.Point(1234, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 145);
            this.button1.TabIndex = 0;
            this.button1.Text = " Description";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.UseWaitCursor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1128, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(352, 449);
            this.textBox1.TabIndex = 1;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(668, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Titre :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(42, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 92);
            this.button2.TabIndex = 5;
            this.button2.Text = "autres  Asteroides";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1264, 690);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 7;
            this.label3.UseWaitCursor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox1.Location = new System.Drawing.Point(42, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(151, 28);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.Text = "Asteroides";
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.AccessibleName = "textBox2";
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox2.Location = new System.Drawing.Point(12, 307);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(228, 305);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "NasaApi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Button button2;
        private Label label3;
        private ComboBox comboBox1;
        private TextBox textBox2;
    }



    

}