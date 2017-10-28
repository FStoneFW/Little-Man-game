namespace WindowsFormsApplication1.MapMaker
{
    partial class MapMakerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.локацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.локацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.уровеньToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьУровеньToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.свойстваToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.локацииToolStripMenuItem,
            this.создатьToolStripMenuItem,
            this.выбратьУровеньToolStripMenuItem,
            this.toolStripTextBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1386, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // локацииToolStripMenuItem
            // 
            this.локацииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьToolStripMenuItem,
            this.сохранитьToolStripMenuItem});
            this.локацииToolStripMenuItem.Name = "локацииToolStripMenuItem";
            this.локацииToolStripMenuItem.Size = new System.Drawing.Size(67, 23);
            this.локацииToolStripMenuItem.Text = "Локации";
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить";
            this.загрузитьToolStripMenuItem.Click += new System.EventHandler(this.загрузитьToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // создатьToolStripMenuItem
            // 
            this.создатьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.локацияToolStripMenuItem,
            this.уровеньToolStripMenuItem});
            this.создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            this.создатьToolStripMenuItem.Size = new System.Drawing.Size(62, 23);
            this.создатьToolStripMenuItem.Text = "Создать";
            // 
            // локацияToolStripMenuItem
            // 
            this.локацияToolStripMenuItem.Name = "локацияToolStripMenuItem";
            this.локацияToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.локацияToolStripMenuItem.Text = "Локация";
            this.локацияToolStripMenuItem.Click += new System.EventHandler(this.локацияToolStripMenuItem_Click);
            // 
            // уровеньToolStripMenuItem
            // 
            this.уровеньToolStripMenuItem.Name = "уровеньToolStripMenuItem";
            this.уровеньToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.уровеньToolStripMenuItem.Text = "Уровень";
            this.уровеньToolStripMenuItem.Click += new System.EventHandler(this.уровеньToolStripMenuItem_Click);
            // 
            // выбратьУровеньToolStripMenuItem
            // 
            this.выбратьУровеньToolStripMenuItem.Name = "выбратьУровеньToolStripMenuItem";
            this.выбратьУровеньToolStripMenuItem.Size = new System.Drawing.Size(114, 23);
            this.выбратьУровеньToolStripMenuItem.Text = "Выбрать уровень";
            this.выбратьУровеньToolStripMenuItem.Click += new System.EventHandler(this.выбратьУровеньToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Enabled = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox1_KeyUp);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.свойстваToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 48);
            // 
            // свойстваToolStripMenuItem
            // 
            this.свойстваToolStripMenuItem.Name = "свойстваToolStripMenuItem";
            this.свойстваToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.свойстваToolStripMenuItem.Text = "Свойства";
            this.свойстваToolStripMenuItem.Click += new System.EventHandler(this.свойстваToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1386, 788);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseUp);
            // 
            // MapMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(751, 407);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MapMakerForm";
            this.Text = "MapMakerForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapMakerForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapMakerForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DeleteObject);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapMakerForm_MouseUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem локацииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem локацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem уровеньToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьУровеньToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem свойстваToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}