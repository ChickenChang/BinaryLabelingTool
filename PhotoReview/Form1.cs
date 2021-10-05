using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoReview
{
    public partial class Form1 : Form
    {
        string dirIn = "";
        string dirOK = "";
        string dirNG = "";
        int count = 0;
        IEnumerable<FileInfo> files = null;
        System.Windows.Forms.PictureBox pick_img;
        public Form1()
        {
            InitializeComponent();
            pb_big.BringToFront();
            pb_dirIn.Image = MyResource.GetImage("NG.png");
            pb_dirOK.Image = MyResource.GetImage("NG.png");
            pb_dirNG.Image = MyResource.GetImage("NG.png");
        }

        private void btn_dirIn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_dirIn.Text))
            {
                try
                {
                    var fb = new FolderBrowserDialog();
                    if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var dirInfo = new DirectoryInfo(fb.SelectedPath);
                        var files = dirInfo.GetFiles().Where(c => (c.Extension.ToLower().Equals(".jpg") || c.Extension.ToLower().Equals(".jpeg") || c.Extension.ToLower().Equals(".bmp") || c.Extension.ToLower().Equals(".png")));
                        if (files.Count() == 0)
                        {
                            tb_dirIn.Text = "";
                            dirIn = "";
                            pb_dirIn.Image = MyResource.GetImage("NG.png");
                            pb_dirIn.Tag = "NG";
                            MessageBox.Show("No image in this folder!!");
                        }
                        else
                        {
                            dirIn = fb.SelectedPath;
                            tb_dirIn.Text = dirIn;
                            pb_dirIn.Image = MyResource.GetImage("OK.png");
                            pb_dirIn.Tag = "OK";
                            refresh(0);
                            resetPicture();
                        }
                    }
                }
                catch (Exception ex)
                {
                    tb_dirIn.Text = "";
                    dirIn = "";
                    pb_dirIn.Image = MyResource.GetImage("NG.png");
                    pb_dirIn.Tag = "NG";
                    MessageBox.Show("There was an error: " + ex.Message + " " + ex.Source);
                }
            }
            else if (Directory.Exists(tb_dirIn.Text))
            {
                var dirInfo = new DirectoryInfo(tb_dirIn.Text);
                var files = dirInfo.GetFiles().Where(c => (c.Extension.ToLower().Equals(".jpg") || c.Extension.ToLower().Equals(".jpeg") || c.Extension.ToLower().Equals(".bmp") || c.Extension.ToLower().Equals(".png")));
                if (files.Count() == 0)
                {
                    tb_dirIn.Text = "";
                    dirIn = "";
                    pb_dirIn.Image = MyResource.GetImage("NG.png");
                    pb_dirIn.Tag = "NG";
                    MessageBox.Show("No image in this folder!!");
                }
                else
                {
                    dirIn = tb_dirIn.Text;
                    pb_dirIn.Image = MyResource.GetImage("OK.png");
                    pb_dirIn.Tag = "OK";
                    refresh(0);
                    resetPicture();
                }
            }
            else
            {
                tb_dirIn.Text = "";
                dirIn = "";
                pb_dirIn.Image = MyResource.GetImage("NG.png");
                pb_dirIn.Tag = "NG";
                MessageBox.Show("Directory is not exists");
            }
            count = 0;
        }

        private void btn_dirOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_dirOK.Text))
            {
                try
                {
                    var fb = new FolderBrowserDialog();
                    if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        dirOK = fb.SelectedPath;
                        tb_dirOK.Text = dirOK;
                        pb_dirOK.Image = MyResource.GetImage("OK.png");
                        pb_dirOK.Tag = "OK";
                    }
                }
                catch (Exception ex)
                {
                    tb_dirOK.Text = "";
                    dirOK = "";
                    pb_dirOK.Image = MyResource.GetImage("NG.png");
                    pb_dirOK.Tag = "NG";
                    MessageBox.Show("There was an error: " + ex.Message + " " + ex.Source);
                }
            }
            else if (Directory.Exists(tb_dirOK.Text))
            {
                dirOK = tb_dirOK.Text;
                pb_dirOK.Image = MyResource.GetImage("OK.png");
                pb_dirOK.Tag = "OK";
            }
            else
            {
                tb_dirOK.Text = "";
                dirOK = "";
                pb_dirOK.Image = MyResource.GetImage("NG.png");
                pb_dirOK.Tag = "NG";
                MessageBox.Show("Directory is not exists");
            }
        }

        private void btn_dirNG_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_dirNG.Text))
            {
                try
                {
                    var fb = new FolderBrowserDialog();
                    if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        dirNG = fb.SelectedPath;
                        tb_dirNG.Text = dirNG;
                        pb_dirNG.Image = MyResource.GetImage("OK.png");
                        pb_dirNG.Tag = "OK";
                    }
                }
                catch (Exception ex)
                {
                    tb_dirNG.Text = "";
                    dirNG = "";
                    pb_dirNG.Image = MyResource.GetImage("NG.png");
                    pb_dirNG.Tag = "NG";
                    MessageBox.Show("There was an error: " + ex.Message + " " + ex.Source);
                }
            }
            else if (Directory.Exists(tb_dirNG.Text))
            {
                dirNG = tb_dirNG.Text;
                pb_dirNG.Image = MyResource.GetImage("OK.png");
                pb_dirNG.Tag = "OK";
            }
            else
            {
                tb_dirNG.Text = "";
                dirNG = "";
                pb_dirNG.Image = MyResource.GetImage("NG.png");
                pb_dirNG.Tag = "NG";
                MessageBox.Show("Directory is not exists");
            }
        }

        private void pb_big_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tlp_big.Visible = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (pb_bigStatus.Tag.Equals("NO"))
                {
                    pb_bigStatus.Image = MyResource.GetImage("NG.png");
                    pb_bigStatus.Tag = "NG";
                    pick_img.Image = MyResource.GetImage("NG.png");
                    pick_img.Tag = "NG";
                }
                else if (pb_bigStatus.Tag.Equals("NG"))
                {
                    pb_bigStatus.Image = MyResource.GetImage("OK.png");
                    pb_bigStatus.Tag = "OK";
                    pick_img.Image = MyResource.GetImage("OK.png");
                    pick_img.Tag = "OK";
                }
                else if (pb_bigStatus.Tag.Equals("OK"))
                {
                    pb_bigStatus.Image = null;
                    pb_bigStatus.Tag = "NO";
                    pick_img.Image = null;
                    pick_img.Tag = "NO";
                }
            }
        }

        private void pb_bigStatus_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                tlp_big.Visible = false;
            }
        }

        private void pb_preview1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pb_preview1.ImageLocation != null)
                {
                    pb_big.Image = pb_preview1.Image;
                    tlp_big.Visible = true;
                    pb_bigStatus.Image = pb_status1.Image;
                    pb_bigStatus.Tag = pb_status1.Tag;
                    pick_img = pb_status1;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (pb_preview1.ImageLocation != null)
                {
                    if (pb_status1.Tag.Equals("NO"))
                    {
                        pb_status1.Image = MyResource.GetImage("NG.png");
                        pb_status1.Tag = "NG";
                    }
                    else if (pb_status1.Tag.Equals("NG"))
                    {
                        pb_status1.Image = MyResource.GetImage("OK.png");
                        pb_status1.Tag = "OK";
                    }
                    else if (pb_status1.Tag.Equals("OK"))
                    {
                        pb_status1.Image = null;
                        pb_status1.Tag = "NO";
                    }
                }

            }

        }

        private void pb_preview2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pb_preview2.ImageLocation != null)
                {
                    pb_big.Image = pb_preview2.Image;
                    tlp_big.Visible = true;
                    pb_bigStatus.Image = pb_status2.Image;
                    pb_bigStatus.Tag = pb_status2.Tag;
                    pick_img = pb_status2;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (pb_preview2.ImageLocation != null)
                {
                    if (pb_status2.Tag.Equals("NO"))
                    {
                        pb_status2.Image = MyResource.GetImage("NG.png");
                        pb_status2.Tag = "NG";
                    }
                    else if (pb_status2.Tag.Equals("NG"))
                    {
                        pb_status2.Image = MyResource.GetImage("OK.png");
                        pb_status2.Tag = "OK";
                    }
                    else if (pb_status2.Tag.Equals("OK"))
                    {
                        pb_status2.Image = null;
                        pb_status2.Tag = "NO";
                    }
                }
            }
        }

        private void pb_preview3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pb_preview3.ImageLocation != null)
                {
                    pb_big.Image = pb_preview3.Image;
                    tlp_big.Visible = true;
                    pb_bigStatus.Image = pb_status3.Image;
                    pb_bigStatus.Tag = pb_status3.Tag;
                    pick_img = pb_status3;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (pb_preview3.ImageLocation != null)
                {
                    if (pb_status3.Tag.Equals("NO"))
                    {
                        pb_status3.Image = MyResource.GetImage("NG.png");
                        pb_status3.Tag = "NG";
                    }
                    else if (pb_status3.Tag.Equals("NG"))
                    {
                        pb_status3.Image = MyResource.GetImage("OK.png");
                        pb_status3.Tag = "OK";
                    }
                    else if (pb_status3.Tag.Equals("OK"))
                    {
                        pb_status3.Image = null;
                        pb_status3.Tag = "NO";
                    }
                }
            }
        }

        private void pb_preview4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pb_preview4.ImageLocation != null)
                {
                    pb_big.Image = pb_preview4.Image;
                    tlp_big.Visible = true;
                    pb_bigStatus.Image = pb_status4.Image;
                    pb_bigStatus.Tag = pb_status4.Tag;
                    pick_img = pb_status4;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (pb_preview4.ImageLocation != null)
                {
                    if (pb_status4.Tag.Equals("NO"))
                    {
                        pb_status4.Image = MyResource.GetImage("NG.png");
                        pb_status4.Tag = "NG";
                    }
                    else if (pb_status4.Tag.Equals("NG"))
                    {
                        pb_status4.Image = MyResource.GetImage("OK.png");
                        pb_status4.Tag = "OK";
                    }
                    else if (pb_status4.Tag.Equals("OK"))
                    {
                        pb_status4.Image = null;
                        pb_status4.Tag = "NO";
                    }
                }
            }
        }

        static class MyResource
        {
            public static Bitmap GetImage(string name)
            {
                Bitmap bmp;
                Assembly _assembly = Assembly.GetExecutingAssembly();
                using (Stream _imageStream = _assembly.GetManifestResourceStream("PhotoReview.pictures." + name))
                {
                    bmp = new Bitmap(_imageStream);
                }
                return bmp;
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (!pb_dirIn.Tag.Equals("OK") || !pb_dirOK.Tag.Equals("OK") || !pb_dirNG.Tag.Equals("OK"))
            {
                MessageBox.Show("Directory path not found!!");
            }
            else
            {
                try
                {
                    if (pb_status1.Tag.Equals("OK"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo1.Text), Path.Combine(dirOK, lbl_photo1.Text));
                    }
                    else if (pb_status1.Tag.Equals("NG"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo1.Text), Path.Combine(dirNG, lbl_photo1.Text));
                    }
                    else
                    {
                        count ++;
                    }

                    if (pb_status2.Tag.Equals("OK"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo2.Text), Path.Combine(dirOK, lbl_photo2.Text));
                    }
                    else if (pb_status2.Tag.Equals("NG"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo2.Text), Path.Combine(dirNG, lbl_photo2.Text));
                    }
                    else
                    {
                        count ++;
                    }

                    if (pb_status3.Tag.Equals("OK"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo3.Text), Path.Combine(dirOK, lbl_photo3.Text));
                    }
                    else if (pb_status3.Tag.Equals("NG"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo3.Text), Path.Combine(dirNG, lbl_photo3.Text));
                    }
                    else
                    {
                        count ++;
                    }

                    if (pb_status4.Tag.Equals("OK"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo4.Text), Path.Combine(dirOK, lbl_photo4.Text));
                    }
                    else if (pb_status4.Tag.Equals("NG"))
                    {
                        System.IO.File.Move(Path.Combine(dirIn, lbl_photo4.Text), Path.Combine(dirNG, lbl_photo4.Text));
                    }
                    else
                    {
                        count ++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error: " + ex.Message + " " + ex.Source);
                }
                refresh(count);
            }

        }

        private void refresh(int index)
        {
            listBox1.Items.Clear();
            var dirInfo = new DirectoryInfo(dirIn);
            files = dirInfo.GetFiles().Where(c => (c.Extension.ToLower().Equals(".jpg") || c.Extension.ToLower().Equals(".jpeg") || c.Extension.ToLower().Equals(".bmp") || c.Extension.ToLower().Equals(".png")));
            foreach (var image in files)
            {
                listBox1.Items.Add(image.Name);
            }
            lbl_amount.Text = files.Count().ToString();
            if (files.Count() > index)
            {
                listBox1.SelectedIndex = index;
            }
            else
            {
                pb_status1.Tag = "NO";
                pb_status1.Image = null;
                pb_preview1.ImageLocation = null;
                lbl_photo1.Text = "";
                btn_OK1.Visible = false;
                btn_NG1.Visible = false;
                pb_status2.Tag = "NO";
                pb_status2.Image = null;
                pb_preview2.ImageLocation = null;
                lbl_photo2.Text = "";
                btn_OK2.Visible = false;
                btn_NG2.Visible = false;
                pb_status3.Tag = "NO";
                pb_status3.Image = null;
                pb_preview3.ImageLocation = null;
                lbl_photo3.Text = "";
                btn_OK3.Visible = false;
                btn_NG3.Visible = false;
                pb_status4.Tag = "NO";
                pb_status4.Image = null;
                pb_preview4.ImageLocation = null;
                lbl_photo4.Text = "";
                btn_OK4.Visible = false;
                btn_NG4.Visible = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetPicture();
        }

        private void resetPicture()
        {
            try
            {
                pb_status1.Tag = "NO";
                pb_status1.Image = null;
                pb_preview1.ImageLocation = null;
                lbl_photo1.Text = "";
                btn_OK1.Visible = false;
                btn_NG1.Visible = false;
                pb_status2.Tag = "NO";
                pb_status2.Image = null;
                pb_preview2.ImageLocation = null;
                lbl_photo2.Text = "";
                btn_OK2.Visible = false;
                btn_NG2.Visible = false;
                pb_status3.Tag = "NO";
                pb_status3.Image = null;
                pb_preview3.ImageLocation = null;
                lbl_photo3.Text = "";
                btn_OK3.Visible = false;
                btn_NG3.Visible = false;
                pb_status4.Tag = "NO";
                pb_status4.Image = null;
                pb_preview4.ImageLocation = null;
                lbl_photo4.Text = "";
                btn_OK4.Visible = false;
                btn_NG4.Visible = false;
                int count0 = 0;
                count = listBox1.SelectedIndex;
                foreach (var image in files)
                {
                    if (count0 >= count)
                    {
                        if (pb_preview1.Tag == null)
                        {
                            //var fullPath = Path.Combine(dirIn, image.Name);
                            string fullPath = dirIn + "\\" + image.Name;
                            //string reallyLongDirectory = @"\\?\D:\Test\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            //reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            //reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

                            //Directory.CreateDirectory(reallyLongDirectory);

                            pb_preview1.Load(fullPath);
                            MessageBox.Show(fullPath);
                            lbl_photo1.Text = image.Name;
                            pb_preview1.Tag = "Y";
                            btn_OK1.Visible = true;
                            btn_NG1.Visible = true;
                        }
                        else if (pb_preview2.Tag == null)
                        {
                            //var fullPath = Path.Combine(dirIn, image.Name);
                            string fullPath = dirIn + "\\" + image.Name;
                            pb_preview2.ImageLocation = fullPath;
                            lbl_photo2.Text = image.Name;
                            pb_preview2.Tag = "Y";
                            btn_OK2.Visible = true;
                            btn_NG2.Visible = true;
                        }
                        else if (pb_preview3.Tag == null)
                        {
                            //var fullPath = Path.Combine(dirIn, image.Name);
                            string fullPath = dirIn + "\\" + image.Name;
                            pb_preview3.ImageLocation = fullPath;
                            lbl_photo3.Text = image.Name;
                            pb_preview3.Tag = "Y";
                            btn_OK3.Visible = true;
                            btn_NG3.Visible = true;
                        }
                        else if (pb_preview4.Tag == null)
                        {
                            //var fullPath = Path.Combine(dirIn, image.Name);
                            string fullPath = dirIn + "\\" + image.Name;
                            pb_preview4.ImageLocation = fullPath;
                            lbl_photo4.Text = image.Name;
                            pb_preview4.Tag = "Y";
                            btn_OK4.Visible = true;
                            btn_NG4.Visible = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        count0++;
                    }
                }
                pb_preview1.Tag = null;
                pb_preview2.Tag = null;
                pb_preview3.Tag = null;
                pb_preview4.Tag = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message + " " + ex.Source);
            }
        }

        private void btn_csv_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(dirIn + "/Result.csv", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine("X,Y,Type,Result");

            IEnumerable<FileInfo> files_OK = null;
            var dirInfo_OK = new DirectoryInfo(dirOK);
            files_OK = dirInfo_OK.GetFiles().Where(c => (c.Extension.ToLower().Equals(".jpg") || c.Extension.ToLower().Equals(".jpeg") || c.Extension.ToLower().Equals(".bmp") || c.Extension.ToLower().Equals(".png")));
            foreach (var image in files_OK)
            {
                int x_index = image.Name.IndexOf("X");
                int y_index = image.Name.IndexOf("Y");
                int l_index = image.Name.IndexOf("L");
                int underLine_index = image.Name.IndexOf("_");
                int dot_index = image.Name.IndexOf(".");
                if (x_index == -1 || y_index == -1 || l_index == -1 || underLine_index == -1 || dot_index == -1)
                {
                    MessageBox.Show("File name is illegal");
                    return;
                }
                sw.WriteLine(image.Name.Substring(x_index + 1, y_index - x_index - 1) +"," + image.Name.Substring(y_index + 1, l_index - y_index - 1) + "," + image.Name.Substring(underLine_index + 1, dot_index - underLine_index - 1) + ",OK");
            }

            IEnumerable<FileInfo> files_NG = null;
            var dirInfo_NG = new DirectoryInfo(dirNG);
            files_NG = dirInfo_NG.GetFiles().Where(c => (c.Extension.ToLower().Equals(".jpg") || c.Extension.ToLower().Equals(".jpeg") || c.Extension.ToLower().Equals(".bmp") || c.Extension.ToLower().Equals(".png")));
            foreach (var image in files_NG)
            {
                int x_index = image.Name.IndexOf("X");
                int y_index = image.Name.IndexOf("Y");
                int l_index = image.Name.IndexOf("L");
                int underLine_index = image.Name.IndexOf("_");
                int dot_index = image.Name.IndexOf(".");
                if (x_index == -1 || y_index == -1 || l_index == -1 || underLine_index == -1 || dot_index == -1)
                {
                    MessageBox.Show("File name is illegal");
                    return;
                }
                sw.WriteLine(image.Name.Substring(x_index + 1, y_index - x_index - 1) + "," + image.Name.Substring(y_index + 1, l_index - y_index - 1) + "," + image.Name.Substring(underLine_index + 1, dot_index - underLine_index - 1) + ",NG");
            }
            sw.Close();
            fs.Close();

            MessageBox.Show("Write csv successfully");
        }

        private void btn_NG_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btn_NG1":
                    pb_status1.Image = MyResource.GetImage("NG.png");
                    pb_status1.Tag = "NG";
                    break;
                case "btn_NG2":
                    pb_status2.Image = MyResource.GetImage("NG.png");
                    pb_status2.Tag = "NG";
                    break;
                case "btn_NG3":
                    pb_status3.Image = MyResource.GetImage("NG.png");
                    pb_status3.Tag = "NG";
                    break;
                case "btn_NG4":
                    pb_status4.Image = MyResource.GetImage("NG.png");
                    pb_status4.Tag = "NG";
                    break;
                case "btn_bigNG":
                    pb_bigStatus.Image = MyResource.GetImage("NG.png");
                    pb_bigStatus.Tag = "NG";
                    pick_img.Image = MyResource.GetImage("NG.png");
                    pick_img.Tag = "NG";
                    break;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btn_OK1":
                    pb_status1.Image = MyResource.GetImage("OK.png");
                    pb_status1.Tag = "OK";
                    break;
                case "btn_OK2":
                    pb_status2.Image = MyResource.GetImage("OK.png");
                    pb_status2.Tag = "OK";
                    break;
                case "btn_OK3":
                    pb_status3.Image = MyResource.GetImage("OK.png");
                    pb_status3.Tag = "OK";
                    break;
                case "btn_OK4":
                    pb_status4.Image = MyResource.GetImage("OK.png");
                    pb_status4.Tag = "OK";
                    break;
                case "btn_bigOK":
                    pb_bigStatus.Image = MyResource.GetImage("OK.png");
                    pb_bigStatus.Tag = "OK";
                    pick_img.Image = MyResource.GetImage("OK.png");
                    pick_img.Tag = "OK";
                    break;
            }

        }
    }
}
