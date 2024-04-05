using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Net.Http;
using AESEncryption.Model;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace AESEncryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                //var data = Encrypt(textBoxInput.Text, textBoxEncryptPassword.Text);
                //textBoxEncryptedOutput.Text = data.StringEncryptOrDecrypt;
                //textTimeEncrypt.Text = data.DecrypEncryptTime.ToString("0.#############");

                //AES.Encrypt("plain.txt", textBoxEncryptPassword.Text);
                AES.Encrypt(textBoxInput.Text, textBoxEncryptPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                //var data = Decrypt(textBoxEncrypted.Text, textBoxDcryptPassword.Text);
                //textBoxDecryptOutput.Text = data.StringEncryptOrDecrypt;
                //textTimeDecrypt.Text = data.DecrypEncryptTime.ToString("0.#############");

                //AES.Decrypt("plain_encrypted.txt", textBoxDcryptPassword.Text);
                AES.Decrypt(textBoxEncrypted.Text, textBoxDcryptPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            textBoxEncryptPassword.Enabled = textBoxInput.Text.Length > 0;
            //buttonEncrypt.Enabled = textBoxInput.Text.Length > 0;
            buttonEncrypt.Enabled = true;
        }

        private void textBoxEncryptPassword_TextChanged(object sender, EventArgs e)
        {
            textBoxInput.Enabled = textBoxInput.Text.Length > 0;
            //buttonEncrypt.Enabled = textBoxInput.Text.Length > 0;
            buttonEncrypt.Enabled = true;

        }

        private void textBoxEncrypted_TextChanged(object sender, EventArgs e)
        {
            textBoxDcryptPassword.Enabled = textBoxEncrypted.Text.Length > 0;
            //buttonDecrypt.Enabled = textBoxEncrypted.Text.Length > 0;
            buttonDecrypt.Enabled = true;
        }

        private void textBoxDcryptPassword_TextChanged(object sender, EventArgs e)
        {
            textBoxEncrypted.Enabled = textBoxEncrypted.Text.Length > 0;
            //buttonDecrypt.Enabled = textBoxEncrypted.Text.Length > 0;
            buttonDecrypt.Enabled = true;
        }

        //private DataEncryptionProvider Encrypt(string plainText, string Password)
        //{
        //    Stopwatch stopwatch = new Stopwatch();

        //    stopwatch.Start();
        //    string encryptString = Convert.ToBase64String(AES.Encrypt(plainText, Password));
        //    stopwatch.Stop();
        //    double encryptionTime = stopwatch.Elapsed.TotalSeconds;
        //    return new DataEncryptionProvider(encryptString, encryptionTime);
        //}

        //private DataEncryptionProvider Decrypt(string plaintext, string Password)
        //{
        //    Stopwatch stopwatch = new Stopwatch();

        //    stopwatch.Start();
        //    string decryptString = AES.Decrypt(Convert.FromBase64String(plaintext), Password);
        //    stopwatch.Stop();
        //    double decryptionTime = stopwatch.Elapsed.TotalSeconds;
        //    return new DataEncryptionProvider(decryptString, decryptionTime);
        //}

        private void textBoxEncryptPassword_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length != 16)
            {
                MessageBox.Show("You need to write at least 16 characters.");
            }
        }

        private void textBoxInput_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (textBoxInput.Text.Length > 0)
            {
                byte[] InputBytes = Encoding.UTF8.GetBytes(textBoxInput.Text);
                textBoxDebug.Text = BitConverter.ToString(InputBytes).ToLower().Replace("-", " ");
            }
        }

        private void textBoxEncryptPassword_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (textBoxEncryptPassword.Text.Length > 0)
            {
                byte[] InputBytes = Encoding.UTF8.GetBytes(textBoxEncryptPassword.Text);
                textBoxDebug.Text = BitConverter.ToString(InputBytes).ToLower().Replace("-", " ");
            }
        }

        private void textBoxEncryptedOutput_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (textBoxEncryptedOutput.Text.Length > 0)
            {
                byte[] InputBytes = Convert.FromBase64String(textBoxEncryptedOutput.Text);
                textBoxDebug.Text = BitConverter.ToString(InputBytes).ToLower().Replace("-", " ");
            }
        }

        private void textBoxEncrypted_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            byte[] InputBytes = Convert.FromBase64String(textBoxEncrypted.Text);
            textBoxDebug.Text = BitConverter.ToString(InputBytes).ToLower().Replace("-", " ");
        }

        private void textBoxDcryptPassword_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (textBoxDcryptPassword.Text.Length > 0)
            {
                byte[] InputBytes = Encoding.UTF8.GetBytes(textBoxDcryptPassword.Text);
                textBoxDebug.Text = BitConverter.ToString(InputBytes).ToLower().Replace("-", "");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxInput.Text = openFile.FileName;
                    //using (StreamReader sr = new StreamReader(openFile.FileName))
                    //{
                    //    string content = sr.ReadToEnd();
                    //    textBoxInput.Text = content;

                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            string randomString = "";

            int length = 16;

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < length; i++)
            {
                randomString += chars[random.Next(chars.Length)];
            }
            textBoxEncryptPassword.Text = randomString;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBoxEncrypted.Text = openFile.FileName;
                    //using (StreamReader sr = new StreamReader(openFile.FileName))
                    //{
                    //    string content = sr.ReadToEnd();
                    //    textBoxEncrypted.Text = content;
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            };
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxEncryptPassword.Text = "";
            textBoxInput.Text = "";
            textBoxEncryptedOutput.Text = "";
            textBoxDcryptPassword.Text = "";
            textBoxDecryptOutput.Text = "";
            textBoxEncrypted.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFile.FileName))
                    {
                        string content = sr.ReadToEnd();
                        textBoxDcryptPassword.Text = content;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBoxEncryptedOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
