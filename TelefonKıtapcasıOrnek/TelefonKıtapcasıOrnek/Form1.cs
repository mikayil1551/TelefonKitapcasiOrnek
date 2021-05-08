using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TelefonKıtapcasıOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Server=localhost;DataBase=TelefonKıtapcasıOrnek;
             Integrated Security=true;");
        private void Form1_Load(object sender, EventArgs e)
        {
            UserEkleme();
        }

        private void UserEkleme()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select*from UserName", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtAdi.Text == "" || txtSoyadi.Text == "" || mskdTelefon.Text == "" || mskdMailAdress.Text ==""||txtAdress.Text=="")
            {
                MessageBox.Show("Lutfen gerekli alanlari doldurunuz");
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format(@"insert Username(Name,Surname,Phone,Adress,MailAdress)
            values ('{0}','{1}','{2}','{3}','{4}')",txtAdi.Text,txtSoyadi.Text,mskdTelefon.Text,
            txtAdress.Text,mskdMailAdress.Text);

            cmd.Connection = baglanti;
            baglanti.Open();
            int ekl = cmd.ExecuteNonQuery();
            baglanti.Close();
            if (ekl>0)
            {
                MessageBox.Show("Veri basariyla yuklendi");
                UserEkleme();
            }
            else
            {
                MessageBox.Show("Islem Basarisiz");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAdi.Text = dataGridView1.CurrentRow.Cells["Name"].Value.ToString();
            txtAdi.Tag = dataGridView1.CurrentRow.Cells["Id"].Value;
            txtSoyadi.Text = dataGridView1.CurrentRow.Cells["Surname"].Value.ToString();
            txtAdress.Text = dataGridView1.CurrentRow.Cells["Adress"].Value.ToString();
            mskdMailAdress.Text = dataGridView1.CurrentRow.Cells["MailAdress"].Value.ToString();
            mskdTelefon.Text = dataGridView1.CurrentRow.Cells["Phone"].Value.ToString();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format(@"update Username set Name='{0}',Surname='{1}',
            Phone='{2}',Adress='{3}',MailAdress='{4}' where Id={5}",txtAdi.Text,txtSoyadi.Text,
            mskdTelefon.Text,txtAdress.Text,mskdMailAdress.Text,txtAdi.Tag);
            cmd.Connection = baglanti;
            baglanti.Open();
            try
            {
                int ekl = cmd.ExecuteNonQuery();
                baglanti.Close();
                if (ekl > 0)
                {
                    MessageBox.Show("Kayit Guncellendi");
                    UserEkleme();

                }
                else
                {
                    MessageBox.Show("Islem basarisiz");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Islem basarisiz");
                baglanti.Close();
            }
           

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
           
            SqlCommand cmd = new SqlCommand(string.Format("select*from Username where Name like'%"+txtSearch.Text+"%'"));
            baglanti.Open();
            cmd.Connection = baglanti;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

            
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow!=null)
            {
               
                DialogResult dialog = MessageBox.Show(@"Are you sure you want to delete it?", "Exit"
                , MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {

                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                    SqlCommand cmd = new SqlCommand(string.Format("delete UserName where Id={0}", id), baglanti);
                    baglanti.Open();
                    int ekl = cmd.ExecuteNonQuery();
                    baglanti.Close();
                    if (ekl > 0)
                    {
                        MessageBox.Show("Veri silindi");
                        UserEkleme();
                    }
                    else
                    {
                        MessageBox.Show("Islem basarisiz");
                    }
                }
                else if (dialog == DialogResult.No)
                {
                    return;
                }



            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show(@"Are you sure you want to exit it?","Exit"
                ,MessageBoxButtons.YesNo);
            if (dialog==DialogResult.Yes)
            {
                
            }
            else if(dialog==DialogResult.No)
            {
                e.Cancel=true;
            }
            
        }

       
   
    }
}
