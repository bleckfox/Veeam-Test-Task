
namespace CopyFile
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.errorLogListBox = new System.Windows.Forms.ListBox();
            this.copyErrorLogBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(82, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(439, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Существующие файлы в папке назначения будут перезаписаны!!!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Не удалось скопировать (в файле):";
            // 
            // openFileBtn
            // 
            this.openFileBtn.Location = new System.Drawing.Point(54, 109);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(136, 76);
            this.openFileBtn.TabIndex = 3;
            this.openFileBtn.Text = "Выбрать файл";
            this.openFileBtn.UseVisualStyleBackColor = true;
            // 
            // errorLogListBox
            // 
            this.errorLogListBox.FormattingEnabled = true;
            this.errorLogListBox.Location = new System.Drawing.Point(258, 109);
            this.errorLogListBox.Name = "errorLogListBox";
            this.errorLogListBox.Size = new System.Drawing.Size(330, 173);
            this.errorLogListBox.TabIndex = 4;
            // 
            // copyErrorLogBtn
            // 
            this.copyErrorLogBtn.Location = new System.Drawing.Point(54, 206);
            this.copyErrorLogBtn.Name = "copyErrorLogBtn";
            this.copyErrorLogBtn.Size = new System.Drawing.Size(136, 76);
            this.copyErrorLogBtn.TabIndex = 5;
            this.copyErrorLogBtn.Text = "Копировать в буфер";
            this.copyErrorLogBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 301);
            this.Controls.Add(this.copyErrorLogBtn);
            this.Controls.Add(this.errorLogListBox);
            this.Controls.Add(this.openFileBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.MaximumSize = new System.Drawing.Size(620, 340);
            this.MinimumSize = new System.Drawing.Size(620, 340);
            this.Name = "Form1";
            this.Text = "Копирование файлов из config.xml";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.ListBox errorLogListBox;
        private System.Windows.Forms.Button copyErrorLogBtn;
    }
}

