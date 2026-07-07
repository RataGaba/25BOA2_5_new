namespace New_practica;

public partial class Form1 : Form
{
    private Panel pnlHeader = new();
    private Label lblTitle = new();
    private Label lblInput = new();
    private TextBox txtInput = new();
    private Button btnSort = new();
    private Button btnRandom = new();
    private Button btnClear = new();
    private Label lblCount = new();
    private NumericUpDown numCount = new();
    private Button btnLoad = new();
    private Button btnSave = new();
    private Label lblResult = new();
    private TextBox txtResult = new();
    private Label lblLog = new();
    private TextBox txtLog = new();

    public Form1()
    {
        InitializeComponent();
        BuildUI();
    }

    private void BuildUI()
    {
        Text = "Сортировка Шелла";
        ClientSize = new Size(680, 600);
        MinimumSize = new Size(500, 540);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(240, 244, 248);
        Font = new Font("Segoe UI", 9.5f);

        // Шапка
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 50;
        pnlHeader.BackColor = Color.FromArgb(37, 99, 235);

        lblTitle.Text = "Сортировка Шелла";
        lblTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(12, 12);
        pnlHeader.Controls.Add(lblTitle);

        // Ввод
        SetLabel(lblInput, "Введите числа через запятую или пробел:", 12, 64);
        txtInput.Location = new Point(12, 84);
        txtInput.Width = 652;
        txtInput.Font = new Font("Consolas", 10f);
        txtInput.BackColor = Color.White;
        txtInput.Text = "64, 34, 25, 12, 22, 11, 90, 55, 3, 78";

        // Ряд 1
        SetBtn(btnSort, "Сортировать", Color.FromArgb(37, 99, 235), 12, 118);
        SetBtn(btnRandom, "Случайный", Color.FromArgb(16, 185, 129), 166, 118);
        SetBtn(btnClear, "Очистить", Color.FromArgb(220, 53, 69), 320, 118);

        // Только очистить работает
        btnClear.Click += (s, e) =>
        {
            txtInput.Text = string.Empty;
            txtResult.Text = string.Empty;
            txtLog.Text = string.Empty;
        };

        SetLabel(lblCount, "Кол-во:", 476, 126);
        numCount.Location = new Point(532, 122);
        numCount.Width = 80;
        numCount.Minimum = 2;
        numCount.Maximum = 50000;
        numCount.Value = 20;

        // Ряд 2
        SetBtn(btnLoad, "Загрузить из файла", Color.FromArgb(108, 43, 217), 12, 162);
        SetBtn(btnSave, "Сохранить в файл", Color.FromArgb(13, 110, 253), 220, 162);
        btnLoad.Width = 196;
        btnSave.Width = 196;

        // Результат
        SetLabel(lblResult, "Отсортированный массив:", 12, 208);
        txtResult.Location = new Point(12, 228);
        txtResult.Width = 652;
        txtResult.Font = new Font("Consolas", 10f);
        txtResult.ReadOnly = true;
        txtResult.BackColor = Color.FromArgb(236, 253, 245);
        txtResult.ForeColor = Color.FromArgb(5, 150, 105);

        // Лог
        SetLabel(lblLog, "Лог / статистика:", 12, 264);
        txtLog.Location = new Point(12, 284);
        txtLog.Width = 652;
        txtLog.Height = 276;
        txtLog.Multiline = true;
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Font = new Font("Consolas", 9f);
        txtLog.BackColor = Color.FromArgb(248, 250, 252);
        txtLog.ForeColor = Color.FromArgb(30, 41, 59);

        Controls.AddRange(new Control[]
        {
            pnlHeader,
            lblInput,  txtInput,
            btnSort,   btnRandom, btnClear, lblCount, numCount,
            btnLoad,   btnSave,
            lblResult, txtResult,
            lblLog,    txtLog
        });
    }

    private static void SetLabel(Label l, string text, int x, int y)
    {
        l.Text = text;
        l.AutoSize = true;
        l.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        l.ForeColor = Color.FromArgb(51, 65, 85);
        l.Location = new Point(x, y);
    }

    private static void SetBtn(Button b, string text, Color color, int x, int y)
    {
        b.Text = text;
        b.Width = 142;
        b.Height = 32;
        b.Location = new Point(x, y);
        b.BackColor = color;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        b.Cursor = Cursors.Hand;
        b.FlatAppearance.BorderSize = 0;
    }
}
