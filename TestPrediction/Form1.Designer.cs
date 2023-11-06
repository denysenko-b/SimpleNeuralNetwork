namespace NeuralNetwork
{
    partial class Form1
    {
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
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            loadBtn = new Button();
            ImgBtn = new Button();
            textBox1 = new TextBox();
            predictBtn = new Button();
            loadNetworkFIleDialog = new OpenFileDialog();
            openImageDialog = new OpenFileDialog();
            widthTextBox = new TextBox();
            heightTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(542, 322);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // loadBtn
            // 
            loadBtn.Location = new Point(12, 12);
            loadBtn.Name = "loadBtn";
            loadBtn.Size = new Size(121, 29);
            loadBtn.TabIndex = 1;
            loadBtn.Text = "LoadNetwork";
            loadBtn.UseVisualStyleBackColor = true;
            loadBtn.Click += loadBtn_Click;
            // 
            // ImgBtn
            // 
            ImgBtn.Location = new Point(139, 12);
            ImgBtn.Name = "ImgBtn";
            ImgBtn.Size = new Size(139, 29);
            ImgBtn.TabIndex = 2;
            ImgBtn.Text = "Choose Image";
            ImgBtn.UseVisualStyleBackColor = true;
            ImgBtn.Click += ImgBtn_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(429, 14);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 3;
            // 
            // predictBtn
            // 
            predictBtn.Location = new Point(284, 12);
            predictBtn.Name = "predictBtn";
            predictBtn.Size = new Size(139, 29);
            predictBtn.TabIndex = 4;
            predictBtn.Text = "Predict";
            predictBtn.UseVisualStyleBackColor = true;
            predictBtn.Click += predictBtn_Click;
            // 
            // loadNetworkFIleDialog
            // 
            loadNetworkFIleDialog.FileName = "loadNetworkDialog";
            loadNetworkFIleDialog.Filter = "*.txt|*.json";
            loadNetworkFIleDialog.FilterIndex = 2;
            loadNetworkFIleDialog.RestoreDirectory = true;
            // 
            // openImageDialog
            // 
            openImageDialog.FileName = "openFileDialog1";
            openImageDialog.Filter = "Files|*.jpg;*.jpeg;*.png;";
            // 
            // widthTextBox
            // 
            widthTextBox.Location = new Point(66, 53);
            widthTextBox.Name = "widthTextBox";
            widthTextBox.Size = new Size(125, 27);
            widthTextBox.TabIndex = 5;
            widthTextBox.Text = "28";
            // 
            // heightTextBox
            // 
            heightTextBox.Location = new Point(257, 53);
            heightTextBox.Name = "heightTextBox";
            heightTextBox.Size = new Size(125, 27);
            heightTextBox.TabIndex = 6;
            heightTextBox.Text = "28";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 56);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 7;
            label1.Text = "Width";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(197, 56);
            label2.Name = "label2";
            label2.Size = new Size(54, 20);
            label2.TabIndex = 8;
            label2.Text = "Height";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 427);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(heightTextBox);
            Controls.Add(widthTextBox);
            Controls.Add(predictBtn);
            Controls.Add(textBox1);
            Controls.Add(ImgBtn);
            Controls.Add(loadBtn);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Network Tester";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button loadBtn;
        private Button ImgBtn;
        private TextBox textBox1;
        private Button predictBtn;
        private OpenFileDialog loadNetworkFIleDialog;
        private OpenFileDialog openImageDialog;
        private TextBox widthTextBox;
        private TextBox heightTextBox;
        private Label label1;
        private Label label2;
    }
}