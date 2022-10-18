//            !!!"..?!!.' ......        !!!!!!! 
//           !!! e2$ .<!!!!!!!!!`~!~!!!!!!~! ""!`.` 
//           !!!!^:!!!!!!!!!!!!!!.:!!!!!!!!! *@ !4:'
//          . >! !!!!!!!!!!!!!!!!!:^:!!!!!!!!:  J!: 
//          .!! ,<!!!!!!!!!!!!!...`*."!!!!!!!!!!.~~
//          !!~!!!!!!!!!f !!!! #$$$$$$b`!!!!!L!!!(  
//         !!! ! !!!!! !>b"!!!!. ^$$$*"!!~!4!!!!!!`x 
//        .!!!! !`!! d "= "$N !!f u `!!!~' !!!!!!!!! 
//        !!!!!  !XH.=m" C..^*$!.  .~L:u@ !! !!!!~:` 
//       !!!!!   '`"*:$$P k  $$$$e$R""" mee"<!!!!!  
//      :!!!!"    $N $$$  * x$$$$$$$   <-m.` !!!!!'<! 
//     .!!!!f     "$ $$$.  u$$$$$$$e $ : ee `  !`:!!!`
//     !!!!!.        $$$$$$$$$$$ $$   u$$" r'    !!!!!             ~4
//    !!!!!          "$$$$$$&""%$$$ee$$$ @"      !!!!!h            $b`
//   !!!!!             $$$$     $$$$$$$           !!!!!           @$ 
//  !!!!! X             "&$c   $$$$$"              !!!!!       `e$$
// !!!!! !              $$."***""                   !!!!h     z$$$$$$$$$$$$$$eJ
//!!!!! !!     .....     ^"'$$$            $         !!!!    J$$$$$$$$$$$"
//!!!! !!  .d$$$$$$$$$$e( <  d            4$          ~!!! z$$F$$$$$$$$$$b
//!!! !!  J$$$$$$*****$$$$. "J<=    t'b  `)$b' ,C)`    `!~@$$$$$J'$$$$$$$
//!!~:!   $$$$"e$$$$$$$$c"$N". - ". :F$ ?P"$$$ #$$      .$$$$$$$FL$$$$$$$
//!`:!    $$"$$$$$$$$$$$$$$e $$$.   '>@ z$&$$$eC$"    .d$$$$$$$P      "*$$.
// !!     #$$$$$$$*"zddaaa""e^*F""*. "$ $$P.#$$$$E:: d$$$$$$$$           ^$ 
//!!~      ;$$$$"d$$$$$$$$$$$$$u       $c#d$$@$\$>`x$$$$$$$$"             "c
//!!        ;e?(."$$$$$$$$$$$$$$$$u     "$NJ$$$d"x$$$$$$$$$ 

// Written by Annabel Jocelyn Sandford (@annie_sandford)
// 14.10.2022
// Matt where content updates? :c

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using NAudio.Wave;
using Microsoft.VisualBasic;
using System.IO.Compression;
using YoutubeExplode;
using System.IO;
using YoutubeExplode.Videos.Streams;

namespace APBMLEditor
{
    public partial class Form1 : Form
    {
        public bool imported_yt = false;
        public string project_path = "";
        public string last_clicked = "";
        public int project_size;

        public Form1()
        {
            InitializeComponent();
            // add drag & drop to form1
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            // hide panel1
            panel1.Visible = false;
            button5.Visible = false;
            loadLastUsedToolStripMenuItem.Enabled = false;
            importMP3FolderToolStripMenuItem.Enabled = false;

            if (System.IO.File.Exists("cache.txt"))
            {
                // get last used project
                string[] lines = System.IO.File.ReadAllLines("cache.txt");
                string cached_project = lines[0];

                // check if project exists
                if (System.IO.Directory.Exists(cached_project))
                {
                    if (System.IO.File.Exists(cached_project + "\\mlproject.ann"))
                    {
                        // load project
                        project_path = cached_project;
                        loadLastUsedToolStripMenuItem.Enabled = true;
                    } else {
                        // delete cache
                        System.IO.File.Delete("cache.txt");
                    }
                } else {
                    // delete cache
                    System.IO.File.Delete("cache.txt");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void unloadProject()
        {
            // unload project
            project_path = "";
            imported_yt = false;
            last_clicked = "";
            project_size = 0;
            treeView1.Nodes.Clear();
            // hide panel1
            panel1.Visible = false;
            button5.Visible = false;
            loadLastUsedToolStripMenuItem.Enabled = false;
            importMP3FolderToolStripMenuItem.Enabled = false;

            toolStripStatusLabel1.Text = "No project loaded";
        }

        private void changeMetadata(string metadata) {
            if (metadata == "title") {
                // open dialog with textbox to change title
                string title = Microsoft.VisualBasic.Interaction.InputBox("Enter new title", "Change title", "Title");
                if (title != "") {
                    string annie_file = project_path + "\\" + last_clicked + ".annie";
                    string[] annie_lines = System.IO.File.ReadAllLines(annie_file);
                    annie_lines[0] = title;
                    annie_lines[4] = "true";
                    System.IO.File.WriteAllLines(annie_file, annie_lines);
                    loadFile();
                } else {
                    // try again
                    changeMetadata("title");
                }
            }
            if (metadata == "artist") {
                // open dialog with textbox to change artist
                string artist = Microsoft.VisualBasic.Interaction.InputBox("Enter new artist", "Change artist", "Artist");
                if (artist != "") {
                    string annie_file = project_path + "\\" + last_clicked + ".annie";
                    string[] annie_lines = System.IO.File.ReadAllLines(annie_file);
                    annie_lines[1] = artist;
                    annie_lines[5] = "true";
                    System.IO.File.WriteAllLines(annie_file, annie_lines);
                    loadFile();
                } else {
                    // try again
                    changeMetadata("artist");
                }
            }
            if (metadata == "album") {
                // open dialog with textbox to change album
                string album = Microsoft.VisualBasic.Interaction.InputBox("Enter new album", "Change album", "Album");
                if (album != "") {
                    string annie_file = project_path + "\\" + last_clicked + ".annie";
                    string[] annie_lines = System.IO.File.ReadAllLines(annie_file);
                    annie_lines[2] = album;
                    annie_lines[6] = "true";
                    System.IO.File.WriteAllLines(annie_file, annie_lines);
                    loadFile();
                } else {
                    // try again
                    changeMetadata("album");
                }
            }
            if (metadata == "filename") {
                // open dialog with textbox to change filename
                string filename = Microsoft.VisualBasic.Interaction.InputBox("Enter new filename without extension", "Change filename", "Filename");
                if (filename != "") {
                    // check if filename includes spaces, if yes remove them
                    if (filename.Contains(" ")) {
                        filename = filename.Replace(" ", "");
                    }
                    // make filename lowercase
                    filename = filename.ToLower();
                    // check if filename includes ".mp3" if yes remove
                    if (filename.Contains(".mp3")) {
                        filename = filename.Replace(".mp3", "");
                    }
                    // remove all special characters
                    filename = new string(filename.Where(c => !char.IsPunctuation(c)).ToArray());
                    // check if filename is longer than 16 characters
                    if (filename.Length > 16) {
                        MessageBox.Show("Filename is too long, maximum length is 16 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try {
                        string new_filename = filename + ".mp3";
                        string new_annie = new_filename + ".annie";
                        string[] annie_lines = System.IO.File.ReadAllLines(project_path + "\\" + last_clicked + ".annie");
                        annie_lines[7] = "true";
                        System.IO.File.WriteAllLines(project_path + "\\" + last_clicked + ".annie", annie_lines);
                        GC.Collect();
                        // rename mp3 file
                        string mp3_file = project_path + "\\" + last_clicked;
                        System.IO.File.Move(mp3_file, project_path + "\\" + new_filename);
                        // rename annie file
                        string annie_file = project_path + "\\" + last_clicked + ".annie";
                        System.IO.File.Move(annie_file, project_path + "\\" + new_annie);
                        // update last_clicked
                        last_clicked = new_filename;
                        loadProject();
                        loadFile();
                    } catch (Exception ex) {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                } else {
                    // try again
                    changeMetadata("filename");
                }
            }
        }

        private void clearScreen() {
            panel1.Visible = false;
        }

        private void loadProject() {
            progressBar1.Maximum = 100;
            progressBar1.Value = 5;
            clearScreen();
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(project_path);
            string[] files = System.IO.Directory.GetFiles(project_path, "*.mp3", System.IO.SearchOption.AllDirectories);
            bool export_okay = true;
            foreach (string file in files)
            {
                treeView1.Nodes[0].Nodes.Add(file);
                string corres_annie = file + ".annie";
                // if file exists
                if (System.IO.File.Exists(corres_annie))
                {
                    string[] annie = System.IO.File.ReadAllLines(corres_annie);
                    string status_title = annie[4];
                    string status_artist = annie[5];
                    string status_album = annie[6];
                    string status_filename = annie[7];
                    // if any of them are false, change color
                    if (status_title == "false" || status_artist == "false" || status_album == "false" || status_filename == "false") {
                        treeView1.Nodes[0].Nodes[treeView1.Nodes[0].Nodes.Count - 1].ForeColor = System.Drawing.Color.Red;
                        export_okay = false;
                    } else {
                        treeView1.Nodes[0].Nodes[treeView1.Nodes[0].Nodes.Count - 1].ForeColor = System.Drawing.Color.Green;
                    }
                } else {
                    treeView1.Nodes[0].Nodes[treeView1.Nodes[0].Nodes.Count - 1].ForeColor = Color.Orange;
                    export_okay = false;
                }
            }
            toolStripStatusLabel1.Text = "Loaded project: " + project_path;
            toolStripStatusLabel2.Text = "Cloud Synced!";
            progressBar1.Value = 30;

            // count files
            int fileCount = 0;
            foreach (TreeNode node in treeView1.Nodes[0].Nodes)
            {
                fileCount++;
            }
            label2.Text = "Amount Files: " + fileCount;
            progressBar1.Value = 80;

            // get size of folder project_path in MB
            long size = 0;
            foreach (string file in files)
            {
                size += new System.IO.FileInfo(file).Length;
            }
            project_size = (int)size / 1024 / 1024;
            label3.Text = "Project Size: " + (size / 1024 / 1024) + " MB";
            importMP3FolderToolStripMenuItem.Enabled = true;
            button5.Visible = true;

            // open treeview
            treeView1.ExpandAll();
            progressBar1.Value = 100;

            if (export_okay) {
                button5.BackColor = SystemColors.ActiveCaption;
            } else {
                button5.BackColor = SystemColors.ButtonShadow;
            }

            if (imported_yt == true) {
                imported_yt = false;
                // show success message box
                MessageBox.Show("Imported YouTube Video successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            progressBar1.Value = 0;
        }

        private void loadFile() {
            string fileToEdit = project_path + "\\" + last_clicked;
            string annieFile = fileToEdit + ".annie";

            // get length of mp3 file in seconds
            Mp3FileReader reader = new Mp3FileReader(fileToEdit);
            TimeSpan duration = reader.TotalTime;
            // stop reader
            reader.Dispose();

            // convert to seconds
            int seconds = (int)duration.TotalSeconds;

            // get size of mp3 file in MB
            long size = new System.IO.FileInfo(fileToEdit).Length;

            // calculate how many percent size is of project size
            int percent = (int)(size / 1024 / 1024) * 100 / project_size;

            // check if annie doesnt exist, create it
            if (!System.IO.File.Exists(annieFile))
            {
                // create annie file, line 1 is "0" and line 2 is length of mp3 file
                System.IO.File.WriteAllText(annieFile, "New Title" + Environment.NewLine + "New Artist" + Environment.NewLine + "New Album" + Environment.NewLine + seconds + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false");
            }

            // load annie file. string 1 = title, string 2 = artist, string 3 = album, string 4 = year, string 5 = genre
            string[] annie = System.IO.File.ReadAllLines(annieFile);
            string file_title = annie[0];
            string file_artist = annie[1];
            string file_album = annie[2];

            string status_title = annie[4];
            string status_artist = annie[5];
            string status_album = annie[6];
            string status_filename = annie[7];

            bool status_ok = true;

            if (status_title == "true") {
                title_status.ForeColor = Color.Green;
            } else {
                title_status.ForeColor = Color.Red;
                status_ok = false;
            }
            if (status_artist == "true") {
                artist_status.ForeColor = Color.Green;
            } else {
                artist_status.ForeColor = Color.Red;
                status_ok = false;
            }
            if (status_album == "true") {
                album_status.ForeColor = Color.Green;
            } else {
                album_status.ForeColor = Color.Red;
                status_ok = false;
            }
            if (status_filename == "true") {
                filename_status.ForeColor = Color.Green;
            } else {
                filename_status.ForeColor = Color.Red;
                status_ok = false;
            }

            if (status_ok) {
                panel1.BackColor = Color.Honeydew;
            } else {
                panel1.BackColor = SystemColors.Control;
            }


            // show panel1
            panel1.Visible = true;
            label_title.Text = file_title;
            label_artist.Text = file_artist;
            label_album.Text = file_album;
            label_length.Text = seconds + " seconds";
            label_filename.Text = last_clicked;
            label_size.Text = (size / 1024 / 1024) + " MB";
            label_ratio.Text = percent + "%";

        }

        // new project
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create makle folder dialog
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder where you want to create the project";
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                project_path = fbd.SelectedPath;

                // create "mlproject.ann" file
                System.IO.File.WriteAllText(project_path + "\\mlproject.ann", "APBMLEditor Project File");

                // get file https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/tree.ini and save it to project folder
                //System.Net.WebClient wc = new System.Net.WebClient();
                //wc.DownloadFile("https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/tree.ini", project_path + "\\tree.ini");
                //wc.DownloadFile("https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/defaultmusiclibrary.xml", project_path + "\\defaultmusiclibrary.xml");

                // load project
                loadProject();
            }
        }

        // open project
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open folder dialog
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder containing the project files";
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                project_path = folderBrowserDialog.SelectedPath;
                
                // check if folder contains "mlproject.ann"
                if (System.IO.File.Exists(project_path + "\\mlproject.ann"))
                {
                    // save project_path in cache
                    System.IO.File.WriteAllText("cache.txt", project_path);
                    loadProject();
                }
                else
                {
                    // messagebox
                    MessageBox.Show("The selected folder does not contain a valid project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // try again
                    openProjectToolStripMenuItem_Click(sender, e);
                }
            }

        }

        // save project
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // drag enter
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // dragdrop file
            if (project_path == "")
            {
                MessageBox.Show("Please create or open a project first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    // copy file to project folder
                    System.IO.File.Copy(file, project_path + "\\" + System.IO.Path.GetFileName(file));
                }
                loadProject();
            }
        }

        private void importMp3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open file chooser, select mp3 file and copy it to project folder
            if (project_path != "")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        try
                        {
                            System.IO.File.Copy(file, project_path + "\\" + System.IO.Path.GetFileName(file));
                        } catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    loadProject();
                }
            }
            else
            {
                MessageBox.Show("Please create or open a project first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importMP3FolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project_path != "")
            {
                treeView1.Nodes.Clear();
                clearScreen();
                System.Threading.Thread.Sleep(500);
                loadProject();
            } else {
                MessageBox.Show("Please create or open a project first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // clicking on treeview node
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // messagebox path of node
            MessageBox.Show(e.Node.FullPath);
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            // if right click
            if (e.Button == MouseButtons.Right && treeView1.GetNodeAt(e.X, e.Y).FullPath != project_path)
            {
                // select the node under the mouse pointer
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);

                if (treeView1.SelectedNode != null)
                {
                    // show context menu
                    contextMenuStrip1.Show(treeView1, e.Location);
                }
            }
            if (treeView1.GetNodeAt(e.X, e.Y).FullPath != project_path) {
                string fileClicked = treeView1.GetNodeAt(e.X, e.Y).FullPath;
                // clean path
                last_clicked = fileClicked.Replace(project_path + "\\", "");
                // messagebox path of node
                //MessageBox.Show(fileClicked);
                loadFile();
            } else {
                clearScreen();
            }
        }

        // private void treeView arrow key up
        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            try {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) {
                    if (treeView1.SelectedNode.FullPath != project_path && treeView1.SelectedNode.FullPath != null) {
                        // get selected node
                        string fileClicked = treeView1.SelectedNode.FullPath;
                        // clean path
                        last_clicked = fileClicked.Replace(project_path + "\\", "");
                        loadFile();
                    } else {
                        clearScreen();
                    }
                }
            } catch (Exception ex)
            {
                // stay quiet.
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // open github.com in browser
            System.Diagnostics.Process.Start("https://github.com/annabelsandford/APBMediaLauncher");
        }

        // EXPORT BUTTON
        private void button5_Click(object sender, EventArgs e)
        {
            // progressbar max to 100 and set 10 
            progressBar1.Maximum = 100;
            progressBar1.Value = 10;

            // check if there is already a library folder in the project folder
            if (System.IO.Directory.Exists(project_path + "\\Library"))
            {
                // delete it
                System.IO.Directory.Delete(project_path + "\\Library", true);
            }

            // get all mp3 files from project folder
            string[] files = System.IO.Directory.GetFiles(project_path, "*.mp3", System.IO.SearchOption.AllDirectories);
            // for each, check if their annie file is true in line 5,6,7,8
            foreach (string file in files)
            {
                // if file+annie exists
                if (!System.IO.File.Exists(file + ".annie"))
                {
                    // error out
                    MessageBox.Show("The file " + file + " does not have an annie file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    return;
                }
                string[] lines = System.IO.File.ReadAllLines(file + ".annie");
                if (lines[4] == "true" && lines[5] == "true" && lines[6] == "true" && lines[7] == "true")
                {
                    continue;
                } else {
                    // error out
                    MessageBox.Show("Before exporting, please make sure all your files have the necessary metadata. The file " + file + " is not ready to be launched. Please check the file and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    return;
                }
            }
            // all files have metadata. Let's create library
            string server_mlver = "https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/mlver.ini";
            string server_mlver_num = new WebClient().DownloadString(server_mlver);

            string library_version = Microsoft.VisualBasic.Interaction.InputBox("Enter a new version for this Library (higher than -> "+server_mlver_num+"", "Enter Library", server_mlver_num);
            if (library_version != "" && library_version != server_mlver_num) {
                //continue;
            } else {
                // try again
                MessageBox.Show("Please enter a valid version number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar1.Value = 0;
                return;
            }

            // create library folder
            try {
                System.IO.Directory.CreateDirectory(project_path + "\\Library");
                //continue;
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar1.Value = 0;
                return;
            }

            // download server tree
            string server_tree = "https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/tree.ini";
            string server_defaultlib = "https://raw.githubusercontent.com/annabelsandford/APBMediaLauncher/main/Media/defaultmusiclibrary.xml";
            string server_tree_text = new WebClient().DownloadString(server_tree);

            // get all mp3's from folder
            string[] files_2 = System.IO.Directory.GetFiles(project_path, "*.mp3", System.IO.SearchOption.AllDirectories);
            // foreach file, change .mp3 to .anna and check if it's in server tree
            foreach (string file in files_2)
            {
                // remove path from file
                string file_name = file.Replace(project_path + "\\", "");
                string anna_file = file_name.Replace(".mp3", ".anna");
                // check if anna_file is in server_tree_text
                if (!server_tree_text.Contains(anna_file))
                {
                    System.IO.File.Copy(project_path + "\\" + file_name, project_path + "\\Library\\" + anna_file);
                } else {
                    // error out
                    MessageBox.Show("The file " + file + " is already in the server tree. Please rename it and try again.", "Compatibility Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Value = 0;
                    return;
                }
            }

            // all mp3's are in library folder.

            // get all .anna files from library folder
            string[] anna_files = System.IO.Directory.GetFiles(project_path + "\\Library", "*.anna", System.IO.SearchOption.AllDirectories);

            string server_lib_text = new WebClient().DownloadString(server_defaultlib);
            string lib_header = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine + "<MusicPlayer Version=\"1\">" + Environment.NewLine + "  <Library>" + Environment.NewLine;

            // remove first 3 lines from server_lib_text
            string[] server_lib_text_lines = server_lib_text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string server_lib_text_lines_2 = "";
            for (int i = 3; i < server_lib_text_lines.Length; i++)
            {
                server_lib_text_lines_2 += server_lib_text_lines[i] + Environment.NewLine;
            }

            // add them to server_tree_text
            foreach (string anna_file in anna_files)
            {
                string anna_file_name = anna_file.Replace(project_path + "\\Library\\", "");
                server_tree_text = anna_file_name + Environment.NewLine + server_tree_text;
                string anna_to_mp3 = anna_file_name.Replace(".anna", ".mp3");

                // get annie file of anna file in project folder
                string annie_file = project_path + "\\" + anna_file_name.Replace(".anna", ".mp3.annie");
                // read annie file
                string[] annie_file_lines = System.IO.File.ReadAllLines(annie_file);
                string annie_title = annie_file_lines[0];
                string annie_artist = annie_file_lines[1];
                string annie_album = annie_file_lines[2];
                string annie_seconds = annie_file_lines[3] + ".0";
                string add_xml = "    <Track Default=\"true\" Name=\"" + annie_title + "\" Artist=\"" + annie_artist + "\" Album=\"" + annie_album + "\" Genre=\"Electronica/Dance\" Duration=\"" + annie_seconds + "\" Removed=\"false\">" + anna_to_mp3 + "</Track>";

                // add add_xml to beginning of server_lib_text_lines_2
                server_lib_text_lines_2 = add_xml + Environment.NewLine + server_lib_text_lines_2;
            }
            // save server_tree_text to library as "tree.ini"
            System.IO.File.WriteAllText(project_path + "\\Library\\tree.ini", server_tree_text);

            server_lib_text_lines_2 = lib_header + server_lib_text_lines_2;

            System.IO.File.WriteAllText(project_path + "\\Library\\defaultmusiclibrary.xml", server_lib_text_lines_2);

            // save library_version to mlver.ini
            System.IO.File.WriteAllText(project_path + "\\Library\\mlver.ini", library_version);

            // create zip of library folder
            string zip_path = project_path + "\\Library.zip";
            ZipFile.CreateFromDirectory(project_path + "\\Library", zip_path);
            // move zip to library folder
            System.IO.File.Move(zip_path, project_path + "\\Library\\Library.zip");

            // progress bar 100
            progressBar1.Value = 100;
            // message box
            MessageBox.Show("Library created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            progressBar1.Value = 0;

            // open Library folder in explorer
            System.Diagnostics.Process.Start(project_path + "\\Library");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changeMetadata("title");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeMetadata("artist");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changeMetadata("album");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeMetadata("filename");
        }

        private void loadLastUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadProject();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // delete file context strip
            // show messagebox confirmation
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this file?", "Delete file", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // delete file
                System.IO.File.Delete(project_path + "\\" + last_clicked);
                string corr_annie = project_path + "\\" + last_clicked + ".annie";
                if (System.IO.File.Exists(corr_annie))
                {
                    System.IO.File.Delete(corr_annie);
                }
                clearScreen();
                loadProject();
            }
            else if (dialogResult == DialogResult.No)
            {
                // do nothing
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // cock check
            System.Diagnostics.Process.Start("https://raw.githubusercontent.com/annabelsandford/APBMLEditor/main/cc.jpg");
        }

        private async void importYTLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project_path == "") {
                MessageBox.Show("Please create or open a project first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else {
                // show inputbox
                string yt_link = Microsoft.VisualBasic.Interaction.InputBox("Enter YouTube link:", "Import YouTube link", "https://www.youtube.com/watch?v=", -1, -1);
                if (yt_link == "") {
                    return;
                }
                // check if link is valid
                if (yt_link.Contains("youtube.com/watch?v=")) {
                    // check if yt_link contains https:// or http://, if not add it
                    if (!yt_link.Contains("https://")) {
                        if (!yt_link.Contains("http://")) {
                            yt_link = "https://" + yt_link;
                        }
                    }
                    // set progressbar max to 100
                    progressBar1.Maximum = 100;
                    // set progressbar value to 0
                    progressBar1.Value = 10;

                    // generate random filename
                    string random_filename = Path.GetRandomFileName();
                    random_filename = random_filename.Replace(".", "");

                    try {
                        // get windows download path
                    string download_path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

                    var youtube = new YoutubeClient();

                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(yt_link);
                    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    progressBar1.Value = 50;
                    // download video
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, download_path + "\\"+random_filename+".wav");
                    cs_ffmpeg_mp3_converter.FFMpeg.Convert2Mp3(download_path + "\\"+random_filename+".wav", download_path + "\\"+random_filename+".mp3");

                    // run function "downloadComplete" when download is complete

                    // move file to project_path
                    System.IO.File.Move(download_path + "\\"+random_filename+".mp3", project_path + "\\"+random_filename+".mp3");
                    // delete temp.wav
                    System.IO.File.Delete(download_path + "\\"+random_filename+".wav");

                    progressBar1.Value = 100;

                    //  reload project
                    imported_yt = true;
                    loadProject();
                    progressBar1.Value = 0;
                    } catch (Exception ex) {
                        progressBar1.Value = 0;
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                } else {
                    MessageBox.Show("Invalid YouTube link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open last clicked file in explorer
            System.Diagnostics.Process.Start("explorer.exe", "/select," + project_path + "\\" + last_clicked);
        }

        private void deleteProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (project_path != "" || project_path != null) {
                // yes or cancel messagebox
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this project?", "Delete project", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // delete project
                    System.IO.Directory.Delete(project_path, true);
                    unloadProject();
                }
                else if (dialogResult == DialogResult.No)
                {
                    // do nothing
                }
            }
        }
    }
}
