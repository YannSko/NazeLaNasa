using System;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Xml.Linq;
using NASA_API_Example;
using Newtonsoft.Json;
// Ce projet aurait était plus simple d'être abordé avec le newtonsoft.json afin de jongler et acceder à l'Api 
// Pour ma part , une fois impliquée et avoir découvert tardivement , j'ai décidé de passer par le split  : les répercussions se verront dans la suite du programme
namespace NASA_API_Example
{
    public partial class Form1 : Form
    {
        // appel des fonctions majeurs pour la réalisation
        public Form1()
        {

            GlobalDataAsync();
            NameAsteroid();
            InitializeComponent();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        // Script pour afficcher la text box si elle n'est pas visible et la remplir de la description
        {
            if (textBox1.Visible)
            {
                textBox1.Visible = false;
            }
            else
            {
                textBox1.Visible = true;
            }
            // affichage description apod
            var d = await GetDescriptionAsync();
            textBox1.Text = d;

            // Affichage du Titre de l'image 

            var Title = await GetTitleAsync();
            label1.Text = Title;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private async void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // temps d'attente ( asynchrone) pour recolter la liste d'asteroide
            var data = await NameAsteroid();
            
            string[] data_tri = data.Split(",");
            // Effacement des données puis re importation
            comboBox1.Items.Clear();
            // click ou non sur le boutton , permet d'afficher ou non la combox
            foreach (string item in data_tri)
            {
                comboBox1.Items.Add(item);
            }


            if (comboBox1.Visible)
            {
                comboBox1.Visible = false;
            }
            else
            {
                comboBox1.Visible = true;
            }
        }


        // Creation dun combo box afin dafficher la liste des asteroides
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;

            // Get le nom de lasteroide selectionné
            string _selectedAsteroidName = senderComboBox.SelectedItem.ToString();
            //nettoie la zone texte entre les selections
            textBox2.Clear();
            MessageBox.Show("Data is cumming");
            // Recup l'id 
            var id = await Id(_selectedAsteroidName);
            HttpClient client = new HttpClient();
            // injection pas très propre de l'id dans un link pour recup de la data ( distance
            HttpResponseMessage response = await client.GetAsync("https://api.nasa.gov/neo/rest/v1/neo/" + id + "?api_key=wQ1JG1QCeewvuDMmRUU2ApQllPJBkNquLP1UCfLv");
            string[]Daplot = new string[] { };
            string DataApproache = "";
            if (response.IsSuccessStatusCode)
            {
                string AllData = await response.Content.ReadAsStringAsync();
                string[] AllDatasplitted = AllData.Split("\"close_approach_date\":");
                string AllDataSplit = AllDatasplitted[0];
                string[] Dasplit = AllData.Split("close_approach_data");

                string Daplit = Dasplit[1];
                 Daplot = Daplit.Split(",");

                Daplot[0] = Daplot[0].Substring(4);



                //DataApproach = DataApproach + data;



                MessageBox.Show(Daplot[0]);

            }

            // le code est incomplet dans mes splits , la découpe n'est pas bonne , je ne recupere pas la bonne partie
            textBox2.Text = "The id of the asteroid is " + await Id(_selectedAsteroidName)
                + " And the distance is " + await MissDistance(_selectedAsteroidName)
                + " les dates des approches passées ou futures sont :" + Daplot[0];


        }
        // le code est incomplet dans mes splits , la découpe n'est pas bonne , je ne recupere pas la bonne partie
       /* textBox2.Text = "The id of the asteroid is " + await Id(_selectedAsteroidName)
                + " And the distance is " + await MissDistance(_selectedAsteroidName)
                + " les dates des approches passées ou futures sont :" + DataApproach;


        }

*/
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

        

