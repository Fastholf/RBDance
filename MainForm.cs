using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Media;
using System.Text.RegularExpressions;

namespace DanceDance
{
    public partial class main_f : Form
    {
        #region fields

        private FileStream Log; // Trace stream

        private static DanceRB dance;
        private enum DanceState { Stopped, Running, Paused };
        private DanceState danceState = DanceState.Stopped;

        private List<RobotRB> robots = new List<RobotRB>();

        private List<Button> MinusRobot_buttons = new List<Button>();
        private List<CheckBox> isRobotUsing_checkBoxes = new List<CheckBox>();
        private List<ComboBox> COMPort_comboBoxes = new List<ComboBox>();
        private List<PictureBox> PortStatus_pictureBoxes = new List<PictureBox>();
        private List<PictureBox> DCModeStatus_pictureBoxes = new List<PictureBox>();
        private List<Label> RobotSerialNumber_labels = new List<Label>();
        private List<Label> RobotName_labels = new List<Label>();
        private List<Button> PortConnect_buttons = new List<Button>();
        private List<Button> DCModeEnable_buttons = new List<Button>();
        private List<Button> RobotBasicPosture_buttons = new List<Button>();
        private List<Button> DCModeDisable_buttons = new List<Button>();
        private List<Button> PortDisconnect_buttons = new List<Button>();
        private List<Panel> RobotControls_panels = new List<Panel>();
        private int RobotControlLinesCount = 0;

        private List<Button> MinusFile_buttons = new List<Button>();
        private List<CheckBox> isFileUsing_checkBoxes = new List<CheckBox>();
        private List<TextBox> FileFolder_textBoxes = new List<TextBox>();
        private List<Button> FileChoose_buttons = new List<Button>();
        private List<Label> FileName_labels = new List<Label>();
        private List<Panel> FileControls_panels = new List<Panel>();
        private int FileControlLinesCount = 0;

        private List<Panel> RobotUsingInFiles_panels = new List<Panel>();
        private List<List<RadioButton>> RobotToFile_radioButtons = new List<List<RadioButton>>();

        private const int spaceBetweenRobotControls = 6, spaceBetweenRobotControlLines = 4, RobotControlLineHeight = 30;
        private const int spaceBetweenRobotToFilePanels = 2, robotToFilePanelWidth = 33, spaceBetweenFileControls = 6, 
            fileControlLineHeight = 30,
            spaceBetweenFileControlLines = 4;
        private int RobotToFileStartXPos = 0, RobotToFileStartYPos = 0;
        private int DanceLength = 0;

        #endregion

        public static void textChanged_handler(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            cb.Text = Regex.Replace(cb.Text, "[^0-9]", "");
            cb.Select(cb.Text.Length, 0);
        }

        private void AddComponentsForOneRobot()
        {
            int dy = RobotControlLineHeight + spaceBetweenRobotControlLines;
            int yp = dy * RobotControlLinesCount;
            int x = robots_gb.Location.X, y = 18 + yp;

            // сдвинуть кнопки для управления сразу всеми роботами вниз.
            RobotControlAllTogether_panel.Location = new Point(RobotControlAllTogether_panel.Location.X,
                RobotControlAllTogether_panel.Location.Y + dy);

            // MinusRobot_button
            Button newMinusRobot_button = new Button();
            newMinusRobot_button.Image = DanceDance.Properties.Resources.minus;
            newMinusRobot_button.Size = new Size(26, 26);
            newMinusRobot_button.Location = new Point(x, y);
            newMinusRobot_button.Name = RobotControlLinesCount.ToString();
            newMinusRobot_button.Click += new System.EventHandler(MinusRobot_button_Click);
            x += newMinusRobot_button.Size.Width + spaceBetweenRobotControls;
            MinusRobot_buttons.Add(newMinusRobot_button);

            // isRobotUsing_checkBox
            CheckBox newIsRobotUsing_chechkBox = new CheckBox();
            newIsRobotUsing_chechkBox.Text = "";
            newIsRobotUsing_chechkBox.Size = new Size(15, 15);
            newIsRobotUsing_chechkBox.Location = new Point(x, y + 3);
            newIsRobotUsing_chechkBox.Name = RobotControlLinesCount.ToString();
            newIsRobotUsing_chechkBox.CheckedChanged += new System.EventHandler(isRobotUsing_chechkBox_CheckedChanged);
            x += newIsRobotUsing_chechkBox.Size.Width + spaceBetweenRobotControls;
            isRobotUsing_checkBoxes.Add(newIsRobotUsing_chechkBox);

            int xp = x;
            x = 3;
            y = 3;

            // COMPort_comboBox
            ComboBox newCOMPort_comboBox = new ComboBox();
            //newCOMPort_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            newCOMPort_comboBox.Size = new Size(60, 21);
            newCOMPort_comboBox.Location = new Point(x, y);
            newCOMPort_comboBox.Name = RobotControlLinesCount.ToString();
            x += newCOMPort_comboBox.Size.Width + spaceBetweenRobotControls;
            newCOMPort_comboBox.TextChanged += new System.EventHandler(textChanged_handler);
            for (int i = 0; i < 80; ++i)
                newCOMPort_comboBox.Items.Add(i.ToString());
            int index = (RobotControlLinesCount + 4) % 21;
            newCOMPort_comboBox.SelectedIndex = index;
            COMPort_comboBoxes.Add(newCOMPort_comboBox);

            // PortStatus_pictureBox
            PictureBox newPortStatus_pictureBox = new PictureBox();
            newPortStatus_pictureBox.Image = DanceDance.Properties.Resources.question;
            newPortStatus_pictureBox.Size = new Size(20, 20);
            newPortStatus_pictureBox.Location = new Point(x, y);
            newPortStatus_pictureBox.Name = RobotControlLinesCount.ToString();
            x += newPortStatus_pictureBox.Size.Width + spaceBetweenRobotControls;
            PortStatus_pictureBoxes.Add(newPortStatus_pictureBox);

            // DCModeStatus_pictureBox
            PictureBox newDCModeStatus_pictureBox = new PictureBox();
            newDCModeStatus_pictureBox.Image = DanceDance.Properties.Resources.question;
            newDCModeStatus_pictureBox.Size = new Size(20, 20);
            newDCModeStatus_pictureBox.Location = new Point(x, y);
            newDCModeStatus_pictureBox.Name = RobotControlLinesCount.ToString();
            x += newDCModeStatus_pictureBox.Size.Width + spaceBetweenRobotControls;
            DCModeStatus_pictureBoxes.Add(newDCModeStatus_pictureBox);

            // RobotSerialNumber_label
            Label newRobotSerialNumber_label = new Label();
            newRobotSerialNumber_label.Text = "_____________";
            newRobotSerialNumber_label.Location = new Point(x, y + 2);
            newRobotSerialNumber_label.Size = new Size(85, 13);
            newRobotSerialNumber_label.Name = RobotControlLinesCount.ToString();
            x += newRobotSerialNumber_label.Size.Width + spaceBetweenRobotControls;
            RobotSerialNumber_labels.Add(newRobotSerialNumber_label);

            // RobotName_label
            Label newRobotName_label = new Label();
            newRobotName_label.Text = "N/A";
            newRobotName_label.Location = new Point(x, y + 2);
            newRobotName_label.Size = new Size(51, 13);
            newRobotName_label.Name = RobotControlLinesCount.ToString();
            x += newRobotName_label.Size.Width + spaceBetweenRobotControls;
            RobotName_labels.Add(newRobotName_label);

            // PortConnect_button
            Button newPortConnect_button = new Button();
            newPortConnect_button.Text = "Connect";
            newPortConnect_button.Location = new Point(x, y + 2);
            newPortConnect_button.Size = new Size(55, 23);
            newPortConnect_button.Name = RobotControlLinesCount.ToString();
            newPortConnect_button.Click += new System.EventHandler(PortConnect_button_Click);
            x += newPortConnect_button.Size.Width + spaceBetweenRobotControls;
            PortConnect_buttons.Add(newPortConnect_button);

            // DCModeEnable_button
            Button newDCModeEnable_button = new Button();
            newDCModeEnable_button.Text = "DC On";
            newDCModeEnable_button.Size = new Size(47, 23);
            newDCModeEnable_button.Location = new Point(x, y + 2);
            newDCModeEnable_button.Name = RobotControlLinesCount.ToString();
            newDCModeEnable_button.Click += new System.EventHandler(DCModeEnable_button_Click);
            x += newDCModeEnable_button.Size.Width + spaceBetweenRobotControls;
            DCModeEnable_buttons.Add(newDCModeEnable_button);

            // RobotBasicPosture_button
            Button newRobotBasicPosture_button = new Button();
            newRobotBasicPosture_button.Text = "Basic Posture";
            newRobotBasicPosture_button.Location = new Point(x, y + 2);
            newRobotBasicPosture_button.Size = new Size(82, 23);
            newRobotBasicPosture_button.Name = RobotControlLinesCount.ToString();
            newRobotBasicPosture_button.Click += new System.EventHandler(RobotBasicPosture_button_Click);
            x += newRobotBasicPosture_button.Size.Width + spaceBetweenRobotControls;
            RobotBasicPosture_buttons.Add(newRobotBasicPosture_button);

            // DCModeDisable_button
            Button newDCModeDisable_button = new Button();
            newDCModeDisable_button.Text = "DC Off";
            newDCModeDisable_button.Location = new Point(x, y + 2);
            newDCModeDisable_button.Size = new Size(47, 23);
            newDCModeDisable_button.Name = RobotControlLinesCount.ToString();
            newDCModeDisable_button.Click += new System.EventHandler(DCModeDisable_button_Click);
            x += newDCModeDisable_button.Size.Width + spaceBetweenRobotControls;
            DCModeDisable_buttons.Add(newDCModeDisable_button);

            // PortDisconnect_button
            Button newPortDisconnect_button = new Button();
            newPortDisconnect_button.Text = "Disconnect";
            newPortDisconnect_button.Location = new Point(x, y + 2);
            newPortDisconnect_button.Size = new Size(69, 23);
            newPortDisconnect_button.Name = RobotControlLinesCount.ToString();
            newPortDisconnect_button.Click += new System.EventHandler(PortDisconnect_button_Click);
            x += newPortDisconnect_button.Size.Width + spaceBetweenRobotControls;
            PortDisconnect_buttons.Add(newPortDisconnect_button);

            // RobotControls_panel
            Panel newRobotControls_panel = new Panel();
            newRobotControls_panel.Size = new Size(595, 30);
            newRobotControls_panel.Location = new Point(xp, yp + 15);
            newRobotControls_panel.Name = "RobotControls_panel_" + RobotControlLinesCount.ToString();
            newRobotControls_panel.Controls.Add(COMPort_comboBoxes.Last());
            newRobotControls_panel.Controls.Add(PortStatus_pictureBoxes.Last());
            newRobotControls_panel.Controls.Add(DCModeStatus_pictureBoxes.Last());
            newRobotControls_panel.Controls.Add(RobotSerialNumber_labels.Last());
            newRobotControls_panel.Controls.Add(RobotName_labels.Last());
            newRobotControls_panel.Controls.Add(PortConnect_buttons.Last());
            newRobotControls_panel.Controls.Add(DCModeEnable_buttons.Last());
            newRobotControls_panel.Controls.Add(RobotBasicPosture_buttons.Last());
            newRobotControls_panel.Controls.Add(DCModeDisable_buttons.Last());
            newRobotControls_panel.Controls.Add(PortDisconnect_buttons.Last());
            newRobotControls_panel.Enabled = false;
            //newRobotControls_panel.BackColor = Color.LightGray;
            RobotControls_panels.Add(newRobotControls_panel);

            robots_gb.Size = new Size(robots_gb.Size.Width, robots_gb.Size.Height + dy);

            Size tempSize = new Size(this.Size.Width, this.Size.Height + dy); 
            this.MinimumSize = tempSize;
            this.MaximumSize = tempSize;
            this.Size = tempSize;

            files_gb.Location = new Point(files_gb.Location.X, files_gb.Location.Y + dy);

            DanceControls_panel.Location = new Point(DanceControls_panel.Location.X, DanceControls_panel.Location.Y + dy);
            frames_panel.Location = new Point(frames_panel.Location.X, frames_panel.Location.Y + dy);

            funny_p.Location = new Point(funny_p.Location.X, funny_p.Location.Y + dy);

            robots_gb.Controls.Add(MinusRobot_buttons.Last());
            robots_gb.Controls.Add(isRobotUsing_checkBoxes.Last());
            robots_gb.Controls.Add(RobotControls_panels.Last());

            RobotRB tempRobot = new RobotRB();
            robots.Add(tempRobot);

            int xfp = RobotToFileStartXPos;

            int dx = robotToFilePanelWidth + spaceBetweenRobotToFilePanels;
            xfp += dx * RobotControlLinesCount;

            int dyrb = fileControlLineHeight + spaceBetweenFileControlLines;

            Panel newRobotUsingInFiles_panel = new Panel();
            int p_height = dyrb * FileControlLinesCount;
            newRobotUsingInFiles_panel.Size = new Size(robotToFilePanelWidth, p_height);
            newRobotUsingInFiles_panel.Location = new Point(xfp, RobotToFileStartYPos);
            newRobotUsingInFiles_panel.Enabled = false;
            newRobotUsingInFiles_panel.BackColor = Color.LightSkyBlue;
            RobotUsingInFiles_panels.Add(newRobotUsingInFiles_panel);

            List<RadioButton> tempListRadioButton = new List<RadioButton>();
            int yrb = spaceBetweenFileControlLines;
            for ( int i = 0; i < FileControlLinesCount; ++i )
            {
                RadioButton tempRadioButton = new RadioButton();
                if (i == 0)
                    tempRadioButton.Checked = true;
                tempRadioButton.Location = new Point(0, yrb);
                tempRadioButton.Size = new Size(31, 17);
                tempRadioButton.Text = RobotControlLinesCount.ToString();
                tempRadioButton.Enabled = isFileUsing_checkBoxes[i].Checked;
                tempListRadioButton.Add(tempRadioButton);
                RobotUsingInFiles_panels.Last().Controls.Add(tempRadioButton);
                yrb += dyrb;
            }
            RobotToFile_radioButtons.Add(tempListRadioButton);

            files_gb.Controls.Add(RobotUsingInFiles_panels.Last());

            ++RobotControlLinesCount;

            files_gb.Size = new Size(files_gb.Size.Width + dx, files_gb.Size.Height);

            funny_p.Location = new Point(funny_p.Location.X + dx, funny_p.Location.Y);

            if (this.Size.Width < funny_p.Location.X + funny_p.Size.Width + 20)
            {
                tempSize = new Size(this.Size.Width + dx, this.Size.Height);
                this.MinimumSize = tempSize;
                this.MaximumSize = tempSize;
                this.Size = tempSize;
            }

            MinusRobot_buttons.First().Enabled = true;
        }

        private int defaultRobotCount = 3, defaultFileCount = 3;

        public main_f()
        {
            if (File.Exists("Error_log.txt"))
                File.Delete("Error_log.txt");

            Log = new FileStream("Error_log.txt", FileMode.OpenOrCreate);
            Trace.Listeners.Add(new TextWriterTraceListener(Log));

            if (File.Exists("options.txt"))
            {
                StreamReader sr = new StreamReader("options.txt");
                string temp = " ";
                int optionNumber = 0;
                while ((temp = sr.ReadLine()) != null)
                {
                    temp.Trim();
                    if ((temp[0] == ';') || (temp[0] == '#')) // Комментарии
                        continue;
                    int n = 0;
                    try
                    {
                        n = Int32.Parse(temp);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                    }
                    if (optionNumber == 0)
                        defaultRobotCount = Math.Min(n, 20);
                    if (optionNumber == 1)
                        defaultFileCount = Math.Min(n, 20);
                    ++optionNumber;
                }
            }

            InitializeComponent();

            RobotToFileStartYPos = AddControlsForFile_button.Location.Y;

            AddControlsForOneFile();
            isFileUsing_checkBoxes.Last().Checked = true;
            for ( int i = 0; i < defaultFileCount - 1; ++i )
                AddControlsForOneFile();

            AddComponentsForOneRobot();
            isRobotUsing_checkBoxes.Last().Checked = true;
            for (int i = 0; i < defaultRobotCount - 1; ++i )
                AddComponentsForOneRobot();

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);

            axWindowsMediaPlayer1.Visible = false;
        }

        private void setEnableButtons_r( int num, int f1, int f2, int f3, int f4, int f5 )
        {
            if (f1 == 0)
                PortConnect_buttons[num].Enabled = false;
            if (f1 == 1)
                PortConnect_buttons[num].Enabled = true;

            if (f2 == 0)
                DCModeEnable_buttons[num].Enabled = false;
            if (f2 == 1)
                DCModeEnable_buttons[num].Enabled = true;

            if (f3 == 0)
                RobotBasicPosture_buttons[num].Enabled = false;
            if (f3 == 1)
                RobotBasicPosture_buttons[num].Enabled = true;

            if (f4 == 0)
                DCModeDisable_buttons[num].Enabled = false;
            if (f4 == 1)
                DCModeDisable_buttons[num].Enabled = true;

            if (f5 == 0)
                PortDisconnect_buttons[num].Enabled = false;
            if (f5 == 1)
                PortDisconnect_buttons[num].Enabled = true;
        }

        private void OpenPort(int num)
        {
            if (robots[num].openPort())
            {
                PortStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.ok;
                string serialNumber = robots[num].readSerialNumber();
                RobotSerialNumber_labels[num].Text = serialNumber;
                RobotName_labels[num].Text = robots[num].getName();
                setEnableButtons_r(num, 0, 1, 1, 0, 1);
            }
            else
            {
                PortStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.cross;
                setEnableButtons_r(num, 1, 0, 0, 0, 0);
            }
        }

        private void isFileUsing_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            int num = isFileUsing_checkBoxes.IndexOf((CheckBox)sender);
            bool _checked = isFileUsing_checkBoxes[num].Checked;
            FileControls_panels[num].Enabled = _checked;
            for (int i = 0; i < robots.Count; ++i)
                RobotToFile_radioButtons[i][num].Enabled = _checked;
        }

        private void FileChoose_button_Click(object sender, EventArgs e)
        {
            int num = FileChoose_buttons.IndexOf((Button)sender);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Заголовочные файлы Motion Builder (*.h)|*.h|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string tFileName = openFileDialog.FileName;
                FileFolder_textBoxes[num].Text = tFileName;
                string[] tstrings = tFileName.Split('\\');
                FileName_labels[num].Text = tstrings[tstrings.Length - 1];
            }
        }

        private void FileFolder_textBox_Enter(object sender, EventArgs e)
        {
            startDance_b.Focus();
        }

        private void connect(int num)
        {
            if (!robots[num].serialPort.IsOpen)
            {
                String port_num = COMPort_comboBoxes[num].Text.ToString();
                robots[num].setPort("COM" + port_num);
                OpenPort(num);
            }
        }

        private void connect_rAll_b_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < robots.Count; ++i)
                if (isRobotUsing_checkBoxes[i].Checked)
                    connect(i);
        }

        private void DCEnable(int num)
        {
            DCModeStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.hourglass;
            if (robots[num].setDCModeOn())
            {
                setEnableButtons_r(num, -1, 0, 0, 1, -1);
                DCModeStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.circle_green;
            }
            else
            {
                setEnableButtons_r(num, -1, 1, 1, 0, -1);
                DCModeStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.Not;
            }
        }

        private void DCOn_rAll_b_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < robots.Count; ++i)
                if (isRobotUsing_checkBoxes[i].Checked)
                    if (robots[i].serialPort.IsOpen)
                        DCEnable(i);
        }

        private void DCDisable(int num)
        {
            DCModeStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.Not;
            robots[num].setDCModeOff();
            setEnableButtons_r(num, -1, 1, 1, 0, -1);
        }

        private void DCOff_rAll_b_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < robots.Count; ++i)
                if (robots[i].serialPort.IsOpen)
                    DCDisable(i);
        }

        private void closePort(int num)
        {
            PortStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.cross;
            DCModeStatus_pictureBoxes[num].Image = DanceDance.Properties.Resources.Not;
            robots[num].setDCModeOff();
            robots[num].closePort();
            RobotName_labels[num].Text = "N/A";
            setEnableButtons_r(num, 1, 0, 0, 0, 0);
        }

        private void disconnect_rAll_b_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < robots.Count; ++i)
//                if (isRobotUsing_checkBoxes[i].Checked)
                    closePort(i);
        }

        private bool DanceIsReady()
        {
            bool robotChecked = false;
            for (int i = 0; i < robots.Count; ++i)
                if (isRobotUsing_checkBoxes[i].Checked)
                    robotChecked = true;
            if (!robotChecked)
            {
                MessageBox.Show("Не выбрано ни одного робота!!");
                return false;
            }
            bool fileChecked = false;
            for (int i = 0; i < FileControlLinesCount; ++i)
                if (isFileUsing_checkBoxes[i].Checked)
                    fileChecked = true;
            if (!fileChecked)
            {
                MessageBox.Show("Не выбрано ни одного файла!!");
                return false;
            }
            for (int i = 0; i < robots.Count; ++i)
                if (isRobotUsing_checkBoxes[i].Checked)
                    if (!robots[i].serialPort.IsOpen)
                    {
                        MessageBox.Show("Робот №" + Convert.ToString(i + 1) + 
                            " выбран, но порт для него не открыт!!");
                        return false;
                    }
                    else
                        if (!robots[i].DCModeOn)
                        {
                            MessageBox.Show("Робот №" + Convert.ToString(i + 1) + " " +
                                RobotName_labels[i].Text + " выбран, для него открыт порт, но режим DC не включен!!");
                            return false;
                        }

            for (int i = 0; i < FileControlLinesCount; ++i)
                if (isFileUsing_checkBoxes[i].Checked)
                    if (FileName_labels[i].Text == "No File")
                    {
                        MessageBox.Show("Файл №" + Convert.ToString(i + 1) + 
                            " выбран, но не указан путь!!");
                        return false;
                    }
                    else
                    {
                        bool haveRobot = false;
                        for (int j = 0; j < robots.Count; ++j)
                            if (isRobotUsing_checkBoxes[j].Checked)
                                if (RobotToFile_radioButtons[j][i].Checked)
                                    haveRobot = true;
                        if (!haveRobot)
                        {
                            MessageBox.Show("Файл №" + Convert.ToString(i + 1) + 
                                " выбран, для него указан путь, но не выбрано ни одного активного робота!!");
                            return false;
                        }
                    }
            if (isUsingMusic_chb.Checked)
            {
                if (music_tb.Text == "")
                {
                    MessageBox.Show("Указано использование музыки, но не указан файл!");
                    return false;
                }
                if (!File.Exists(music_tb.Text))
                {
                    MessageBox.Show("Указанный файл музыки не существует!");
                    return false;
                }
            }

            return true;
        }

        private static void startDance()
        {
            dance.LetsDance();
        }

        private void startDance_b_Click(object sender, EventArgs e)
        {
            if (!DanceIsReady()) return;

            List<DanceRB.ControlFile> controlFiles = new List<DanceRB.ControlFile>();
            for (int i = 0; i < FileControlLinesCount; ++i)
                if (isFileUsing_checkBoxes[i].Checked)
                {
                    DanceRB.ControlFile cf = new DanceRB.ControlFile();
                    cf.fileName = FileFolder_textBoxes[i].Text;
                    for (int j = 0; j < robots.Count; ++j)
                        if (isRobotUsing_checkBoxes[j].Checked)
                            if (RobotToFile_radioButtons[j][i].Checked)
                                cf.robotNums.Add(j);
                    controlFiles.Add(cf);
                }

            List<SerialPort> ports = new List<SerialPort>();
            for (int i = 0; i < robots.Count; ++i)
                ports.Add(robots[i].getPort());

            if (isUsingMusic_chb.Checked)
            {
                axWindowsMediaPlayer1.URL = music_tb.Text;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }

            danceStatus_l.Text = "Dancing...";

            dance = new DanceRB(controlFiles, ports);
            dance.PrepareDance();
            DanceLength = dance.GetDanceLength();
            frames_trackBar.Maximum = DanceLength;
            frames_label.Text = "0/" + DanceLength.ToString();
            dance.setFramesLabel(frames_label);
            dance.setFramesTrackBar(frames_trackBar);
            Thread danceThread = new Thread(startDance);
            danceThread.Start();

            danceState = DanceState.Running;
            DanceTimer.Enabled = true;

            music_gb.Enabled = false;
            robots_gb.Enabled = false;
            files_gb.Enabled = false;
            startDance_b.Enabled = false;
            pauseResumeDance_b.Enabled = true;
            stopDance_b.Enabled = true;
            frames_panel.Enabled = false;
            funny_p_start();
        }

        private void main_f_FormClosed(object sender, FormClosedEventArgs e)
        {
            Trace.Flush();
            Log.Close();
        }

        private void main_f_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (danceState != DanceState.Stopped)
            {
                MessageBox.Show("Вы не можете закрыть программу, пока не завершен танец. Для завершения танца нажмите кнопку \"Stop\".");
                e.Cancel = true;
            }

            bool haveOpenedPort = false;
            for (int i = 0; i < robots.Count; ++i)
                if (robots[i].serialPort.IsOpen)
                    haveOpenedPort = true;
            if (haveOpenedPort)
            {
                DialogResult ask_Res = MessageBox.Show("Остались незакрытые порты. Все равно выйти?",
                    "Порты не закрыты!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask_Res == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void pauseResumeDance_b_Click(object sender, EventArgs e)
        {
            if (danceState == DanceState.Paused)
            {
                if (isUsingMusic_chb.Checked)
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                danceState = DanceState.Running;
                pauseResumeDance_b.Text = "Pause";
                danceStatus_l.Text = "Dancing...";
                dance.Resume();
                funny_p_start();
                return;
            }
            if (danceState == DanceState.Running)
            {
                if (isUsingMusic_chb.Checked)
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                danceState = DanceState.Paused;
                pauseResumeDance_b.Text = "Resume";
                danceStatus_l.Text = "Dance is paused.";
                dance.Pause();
                funny_p_pause();
                return;
            }
        }

        private bool danceIsRunning()
        {
            return dance.isDanceRunning();
        }

        private void DanceTimer_Tick(object sender, EventArgs e)
        {
            if (!danceIsRunning())
            {
                DanceTimer.Enabled = false;
                music_gb.Enabled = true;
                robots_gb.Enabled = true;
                files_gb.Enabled = true;
                startDance_b.Enabled = true;
                pauseResumeDance_b.Enabled = false;
                stopDance_b.Enabled = false;
                danceState = DanceState.Stopped;
                danceStatus_l.Text = "Dance is stopped.";
                if (isUsingMusic_chb.Checked)
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
//                Trace.Write("DanceTimer: Dance");
                frames_panel.Enabled = true;
            }
            funny_p.Invalidate();
        }

        private void stopDance_b_Click(object sender, EventArgs e)
        {
            dance.Abort();
            danceState = DanceState.Stopped;
            stopDance_b.Enabled = false;

            funny_p_stop();
        }

        private void basicPosture(int num)
        {
            if (robots[num].serialPort.IsOpen)
                robots[num].basicPosture();
        }

        private void basicPosture_rAll_b_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < robots.Count; ++i)
                if (isRobotUsing_checkBoxes[i].Checked)
                    basicPosture(i);
        }

        #region Funny panel
        /* Данный кусок кода используется для отображения движущихся во время танца кругов.
         * Помимо эстетической ценности данная панель позволяет видеть, что поток GUI не завис. */
        private const int FP_STOPPED = 0, FP_PAUSED = 1, FP_RUNNING = 2;
        private int funnyPanelState = 0;
        private int funnyTick = 0;

        private void funny_p_Paint(object sender, PaintEventArgs e)
        {
            if (funnyPanelState == FP_STOPPED)
                return;
            
            e.Graphics.Clear(Color.White);

            int circleCount = 5;
            float center = funny_p.ClientSize.Width / 2.0f;
            float step = center / (float)(circleCount);
            float shift = funnyTick % ((int)(step) * 2);
            for (int i = 0; i < circleCount; ++i)
            {
                float x = center - step * (i + 0) - shift / 2.0f;
                float y = center - step * (i + 0) - shift / 2.0f;
                float w = (step * 2) * (i + 0) + shift;
                float h = (step * 2) * (i + 0) + shift;
                e.Graphics.DrawEllipse(new Pen(Color.Blue, 1), new RectangleF(x, y, w, h));
            }
            
            if (funnyPanelState == FP_PAUSED)
                return;
            ++funnyTick;
        }

        private void funnyTimer_Tick(object sender, EventArgs e)
        {
            funny_p.Invalidate();
        }

        private void funny_p_start()
        {
            funnyPanelState = FP_RUNNING;
        }

        private void funny_p_pause()
        {
            funnyPanelState = FP_PAUSED;
        }

        private void funny_p_stop()
        {
            funnyPanelState = FP_STOPPED;
        }

        #endregion

        private void AddControlsForRobot_button_Click(object sender, EventArgs e)
        {
            AddComponentsForOneRobot();
        }

        private void DeleteComponentsForOneRobot(int num)
        {
            robots_gb.Controls.Remove(MinusRobot_buttons[num]);
            MinusRobot_buttons.RemoveAt(num);

            robots_gb.Controls.Remove(isRobotUsing_checkBoxes[num]);
            isRobotUsing_checkBoxes.RemoveAt(num);

            COMPort_comboBoxes.RemoveAt(num);

            PortStatus_pictureBoxes.RemoveAt(num);

            DCModeStatus_pictureBoxes.RemoveAt(num);

            RobotSerialNumber_labels.RemoveAt(num);

            RobotName_labels.RemoveAt(num);

            PortConnect_buttons.RemoveAt(num);

            DCModeEnable_buttons.RemoveAt(num);

            RobotBasicPosture_buttons.RemoveAt(num);

            DCModeDisable_buttons.RemoveAt(num);

            PortDisconnect_buttons.RemoveAt(num);

            robots_gb.Controls.Remove(RobotControls_panels[num]);
            RobotControls_panels.RemoveAt(num);

            robots.RemoveAt(num);

            --RobotControlLinesCount;

            int dy = RobotControlLineHeight + spaceBetweenRobotControlLines;
            for ( int i = num; i < RobotControlLinesCount; ++i )
            {
                MinusRobot_buttons[i].Location = new Point(MinusRobot_buttons[i].Location.X,
                    MinusRobot_buttons[i].Location.Y - dy);
                isRobotUsing_checkBoxes[i].Location = new Point(isRobotUsing_checkBoxes[i].Location.X,
                    isRobotUsing_checkBoxes[i].Location.Y - dy);
                RobotControls_panels[i].Location = new Point(RobotControls_panels[i].Location.X,
                    RobotControls_panels[i].Location.Y - dy);
            }

            RobotControlAllTogether_panel.Location = new Point(RobotControlAllTogether_panel.Location.X,
                RobotControlAllTogether_panel.Location.Y - dy);
            robots_gb.Size = new Size(robots_gb.Size.Width, robots_gb.Size.Height - dy);

            Size tempSize = new Size(this.Size.Width, this.Size.Height - dy); 
            this.MinimumSize = tempSize;
            this.MaximumSize = tempSize;
            this.Size = tempSize;

            files_gb.Location = new Point(files_gb.Location.X, files_gb.Location.Y - dy);
            DanceControls_panel.Location = new Point(DanceControls_panel.Location.X, DanceControls_panel.Location.Y - dy);
            funny_p.Location = new Point(funny_p.Location.X, funny_p.Location.Y - dy);

            files_gb.Controls.Remove(RobotUsingInFiles_panels[num]);
            RobotUsingInFiles_panels.RemoveAt(num);
            RobotToFile_radioButtons.RemoveAt(num);
            int dx = robotToFilePanelWidth + spaceBetweenRobotToFilePanels;
            for (int i = num; i < RobotUsingInFiles_panels.Count; ++i)
            {
                RobotUsingInFiles_panels[i].Location = new Point(RobotUsingInFiles_panels[i].Location.X - dx,
                    RobotUsingInFiles_panels[i].Location.Y);
                for (int j = 0; j < FileControlLinesCount; ++j)
                    RobotToFile_radioButtons[i][j].Text = i.ToString();
            }

            files_gb.Size = new Size(files_gb.Size.Width - dx, files_gb.Size.Height);

            funny_p.Location = new Point(funny_p.Location.X - dx, funny_p.Location.Y);

            if (this.Size.Width > robots_gb.Location.X + robots_gb.Width + 30)
            {
                tempSize = new Size(this.Size.Width - dx, this.Size.Height);
                this.MinimumSize = tempSize;
                this.MaximumSize = tempSize;
                this.Size = tempSize;
            }

            if (RobotControlLinesCount == 1)
                MinusRobot_buttons.First().Enabled = false;
        }

        private void MinusRobot_button_Click(object sender, EventArgs e)
        {
            int num = MinusRobot_buttons.IndexOf((Button)sender);
            DeleteComponentsForOneRobot(num);
        }

        private void isRobotUsing_chechkBox_CheckedChanged(object sender, EventArgs e)
        {
            int num = isRobotUsing_checkBoxes.IndexOf((CheckBox)sender);
            if (isRobotUsing_checkBoxes[num].Checked)
            {
                RobotControls_panels[num].Enabled = true;
                RobotUsingInFiles_panels[num].Enabled = true;

                if (robots[num].serialPort.IsOpen)
                {
                    setEnableButtons_r(num, 0, -1, -1, -1, 1);
                    if (robots[num].DCModeOn)
                        setEnableButtons_r(num, -1, 0, 0, 1, -1);
                    else
                        setEnableButtons_r(num, -1, 1, 0, 0, -1);
                }
                else
                    setEnableButtons_r(num, 1, 0, 0, 0, 0);

//                 if (!robots[num].serialPort.IsOpen)
//                     COMPort_cbs[num].Enabled = true;
//                 else
//                     COMPort_cbs[num].Enabled = false;
            }
            else
            {
                RobotControls_panels[num].Enabled = false;
                RobotUsingInFiles_panels[num].Enabled = false;
            }
        }

        private void PortConnect_button_Click(object sender, EventArgs e)
        {
            int num = PortConnect_buttons.IndexOf((Button)sender);
            connect(num);
        }

        private void DCModeEnable_button_Click(object sender, EventArgs e)
        {
            int num = DCModeEnable_buttons.IndexOf((Button)sender);
            DCEnable(num);
        }

        private void RobotBasicPosture_button_Click(object sender, EventArgs e)
        {
            int num = RobotBasicPosture_buttons.IndexOf((Button)sender);
            basicPosture(num);
        }

        private void DCModeDisable_button_Click(object sender, EventArgs e)
        {
            int num = DCModeDisable_buttons.IndexOf((Button)sender);
            DCDisable(num);
        }

        private void PortDisconnect_button_Click(object sender, EventArgs e)
        {
            int num = PortDisconnect_buttons.IndexOf((Button)sender);
            closePort(num);
        }

        private void AddControlsForOneFile()
        {
            int x = AddControlsForFile_button.Location.X, y = AddControlsForFile_button.Location.Y;
            int dy = spaceBetweenFileControlLines + fileControlLineHeight;

            // MinusFile_button
            Button newMinusFile_button = new Button();
            newMinusFile_button.Image = DanceDance.Properties.Resources.minus;
            newMinusFile_button.Size = new Size(26, 26);
            newMinusFile_button.Location = new Point(x, y);
            //newMinusFile_button.Name = RobotControlLinesCount.ToString();
            newMinusFile_button.Click += new System.EventHandler(MinusFile_button_Click);
            x += newMinusFile_button.Size.Width + spaceBetweenFileControls;
            MinusFile_buttons.Add(newMinusFile_button);

            // isFileUsing_checkBox
            CheckBox newIsFileUsing_checkBox = new CheckBox();
            newIsFileUsing_checkBox.Size = new Size(15, 15);
            newIsFileUsing_checkBox.Location = new Point(x, y);
            newIsFileUsing_checkBox.CheckedChanged += new System.EventHandler(isFileUsing_checkBox_CheckedChanged);
            x += newIsFileUsing_checkBox.Size.Width + spaceBetweenFileControls;
            isFileUsing_checkBoxes.Add(newIsFileUsing_checkBox);

            int xp = x, yp = y;
            RobotToFileStartXPos = x;
            y = 3;
            x = spaceBetweenFileControls;

            // FileFolder_textBox
            TextBox newFileFolder_textBox = new TextBox();
            newFileFolder_textBox.Size = new Size(245, 20);
            newFileFolder_textBox.Location = new Point(x, y);
            newFileFolder_textBox.Enter += new System.EventHandler(FileFolder_textBox_Enter);
            x += newFileFolder_textBox.Size.Width + spaceBetweenFileControls;
            FileFolder_textBoxes.Add(newFileFolder_textBox);

            // FileChoose_button
            Button newFileChoose_button = new Button();
            newFileChoose_button.Size = new Size(31, 23);
            newFileChoose_button.Location = new Point(x, y);
            newFileChoose_button.Text = "...";
            newFileChoose_button.Click += new System.EventHandler(FileChoose_button_Click);
            x += newFileChoose_button.Size.Width + spaceBetweenFileControls;
            FileChoose_buttons.Add(newFileChoose_button);

            // FileName_label
            Label newFileName_label = new Label();
            newFileName_label.Size = new Size(40, 13);
            newFileName_label.Location = new Point(x, y);
            x += newFileName_label.Size.Width + spaceBetweenFileControls;
            FileName_labels.Add(newFileName_label);

            // FileControls_panel
            Panel newFileControls_panel = new Panel();
            newFileControls_panel.Size = new Size(390, 29);
            newFileControls_panel.Location = new Point(xp, yp);
            newFileControls_panel.Enabled = false;
            newFileControls_panel.Controls.Add(FileFolder_textBoxes.Last());
            newFileControls_panel.Controls.Add(FileChoose_buttons.Last());
            newFileControls_panel.Controls.Add(FileName_labels.Last());
            FileControls_panels.Add(newFileControls_panel);

            RobotToFileStartXPos += FileControls_panels.Last().Size.Width;

            files_gb.Controls.Add(isFileUsing_checkBoxes.Last());
            files_gb.Controls.Add(MinusFile_buttons.Last());
            files_gb.Controls.Add(FileControls_panels.Last());

            AddControlsForFile_button.Location = new Point(AddControlsForFile_button.Location.X, AddControlsForFile_button.Location.Y + dy);

            dy = fileControlLineHeight + spaceBetweenFileControlLines;
            int yrb = dy * FileControlLinesCount + spaceBetweenFileControlLines;
            for ( int i = 0; i < robots.Count; ++i )
            {
                RadioButton tempRadioButton = new RadioButton();
                tempRadioButton.Location = new Point(0, yrb);
                tempRadioButton.Size = new Size(31, 17);
                tempRadioButton.Text = i.ToString();
                tempRadioButton.Enabled = isFileUsing_checkBoxes[FileControlLinesCount].Checked;
                RobotToFile_radioButtons[i].Add(tempRadioButton);
                RobotUsingInFiles_panels[i].Controls.Add(tempRadioButton);
                RobotUsingInFiles_panels[i].Size = new Size(RobotUsingInFiles_panels[i].Size.Width,
                    RobotUsingInFiles_panels[i].Size.Height + dy);
            }

            files_gb.Size = new Size(files_gb.Size.Width, files_gb.Size.Height + dy);

            Size tempSize = new Size(this.Size.Width, this.Size.Height + dy);
            this.Size = tempSize;
            this.MinimumSize = tempSize;
            this.MaximumSize = tempSize;

            DanceControls_panel.Location = new Point(DanceControls_panel.Location.X, DanceControls_panel.Location.Y + dy);
            frames_panel.Location = new Point(frames_panel.Location.X, frames_panel.Location.Y + dy);

            int cy = files_gb.Height / 2 + files_gb.Location.Y - funny_p.Height / 2;
            funny_p.Location = new Point(funny_p.Location.X, cy);

            ++FileControlLinesCount;

            MinusFile_buttons.First().Enabled = true;
        }

        private void AddControlsForFile_button_Click(object sender, EventArgs e)
        {
            AddControlsForOneFile();
        }

        private void DeleteComponentsForOneFile(int num)
        {
            files_gb.Controls.Remove(MinusFile_buttons[num]);
            MinusFile_buttons.RemoveAt(num);

            files_gb.Controls.Remove(isFileUsing_checkBoxes[num]);
            isFileUsing_checkBoxes.RemoveAt(num);

            FileFolder_textBoxes.RemoveAt(num);

            FileChoose_buttons.RemoveAt(num);

            FileName_labels.RemoveAt(num);

            files_gb.Controls.Remove(FileControls_panels[num]);
            FileControls_panels.RemoveAt(num);

            --FileControlLinesCount;

            int dy = fileControlLineHeight + spaceBetweenFileControlLines;
            for (int i = 0; i < robots.Count; ++i)
            {
                RobotUsingInFiles_panels[i].Controls.Remove(RobotToFile_radioButtons[i][num]);
                RobotToFile_radioButtons[i].RemoveAt(num);
                
                RobotUsingInFiles_panels[i].Size = new Size(RobotUsingInFiles_panels[i].Size.Width,
                    RobotUsingInFiles_panels[i].Size.Height - dy);
            }
            for ( int i = num; i < FileControlLinesCount; ++i )
            {
                MinusFile_buttons[i].Location = new Point(MinusFile_buttons[i].Location.X,
                    MinusFile_buttons[i].Location.Y - dy);

                isFileUsing_checkBoxes[i].Location = new Point(isFileUsing_checkBoxes[i].Location.X,
                    isFileUsing_checkBoxes[i].Location.Y - dy);

                FileControls_panels[i].Location = new Point(FileControls_panels[i].Location.X,
                    FileControls_panels[i].Location.Y - dy);

                for (int j = 0; j < robots.Count; ++j)
                    RobotToFile_radioButtons[j][i].Location = new Point(RobotToFile_radioButtons[j][i].Location.X,
                        RobotToFile_radioButtons[j][i].Location.Y - dy);
            }

            AddControlsForFile_button.Location = new Point(AddControlsForFile_button.Location.X, AddControlsForFile_button.Location.Y - dy);

            files_gb.Size = new Size(files_gb.Size.Width, files_gb.Size.Height - dy);

            Size tempSize = new Size(this.Size.Width, this.Size.Height - dy);
            this.Size = tempSize;
            this.MinimumSize = tempSize;
            this.MaximumSize = tempSize;

            DanceControls_panel.Location = new Point(DanceControls_panel.Location.X, DanceControls_panel.Location.Y - dy);

            int cy = files_gb.Height / 2 + files_gb.Location.Y - funny_p.Height / 2;
            funny_p.Location = new Point(funny_p.Location.X, cy);

            if (FileControlLinesCount == 1)
                MinusFile_buttons.First().Enabled = false;
        }

        private void MinusFile_button_Click(object sender, EventArgs e)
        {
            DeleteComponentsForOneFile( MinusFile_buttons.IndexOf( (Button)sender ) );
        }

        private void openMusic_b_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files(*.mp3;*.wav)" +
                "|*.mp3;*.wav|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = "C:\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                music_tb.Text = openFileDialog.FileName;
        }

        private void frames_trackBar_ValueChanged(object sender, EventArgs e)
        {
            frames_label.Text = frames_trackBar.Value.ToString() + "/" + DanceLength.ToString();
        }

        private void frames_trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            dance.SetToFrame(frames_trackBar.Value);
        }
    }
}
