using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Practica;

[StructLayout(LayoutKind.Sequential)]
public struct SortStats
{
    public long Comparisons;
    public long Swaps;
    public double TimeMs;
}

public static class ShellSorter
{

    private const string DLL = "shell_sort";

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
    private static extern void shell_sort(
        int[] arr,
        int n,
        ref SortStats stats);

    public static (int[] sorted, SortStats stats) Sort(int[] input)
    {

        int[] arr = (int[])input.Clone();
        var stats = new SortStats();

        // Вызов C-функции
        shell_sort(arr, arr.Length, ref stats);

        return (arr, stats);
    }
}

public partial class Form1 : Form
{
    private Panel pnlHeader = new();
    private Label lblTitle = new();
    private Label lblDll = new();
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

    // Настюша
    public Form1()
    {
        InitializeComponent();
        BuildUI();
        CheckDll();   // проверяем наличие DLL при старте
    }

    //  Проверка наличия DLL
    private void CheckDll()
    {
        // DLL должна лежать рядом с .exe
        string dllPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "shell_sort.dll");

        if (File.Exists(dllPath))
        {
            lblDll.Text = "✔ shell_sort.dll найдена — алгоритм выполняется на C";
            lblDll.ForeColor = Color.FromArgb(5, 150, 105);
        }
        else
        {
            lblDll.Text = "✘ shell_sort.dll не найдена! Сначала скомпилируйте C-файл.";
            lblDll.ForeColor = Color.FromArgb(220, 53, 69);
            btnSort.Enabled = false;   // блокируем кнопку пока нет DLL
        }
    }

    private void BuildUI()
    {
        Text = "Сортировка Шелла — P/Invoke (C + C#)";
        ClientSize = new Size(680, 630);
        MinimumSize = new Size(500, 570);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(240, 244, 248);
        Font = new Font("Segoe UI", 9.5f);

        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 50;
        pnlHeader.BackColor = Color.FromArgb(37, 99, 235);

        lblTitle.Text = "Сортировка Шелла — C + C# (P/Invoke)";
        lblTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(12, 12);
        pnlHeader.Controls.Add(lblTitle);

        lblDll.AutoSize = true;
        lblDll.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblDll.Location = new Point(12, 60);

        SetLabel(lblInput, "Введите числа через запятую или пробел:", 12, 84);

        txtInput.Location = new Point(12, 104);
        txtInput.Width = 652;
        txtInput.Font = new Font("Consolas", 10f);
        txtInput.BackColor = Color.White;
        txtInput.Text = "64, 34, 25, 12, 22, 11, 90, 55, 3, 78";

        SetBtn(btnSort, "Сортировать (C)", Color.FromArgb(37, 99, 235), 12, 140);
        SetBtn(btnRandom, "Случайный", Color.FromArgb(16, 185, 129), 174, 140);
        SetBtn(btnClear, "Очистить", Color.FromArgb(220, 53, 69), 336, 140);

        btnSort.Width = 150;

        btnSort.Click += OnSort;
        btnRandom.Click += OnRandom;
        btnClear.Click += OnClear;

        SetLabel(lblCount, "Кол-во:", 480, 148);

        numCount.Location = new Point(546, 144);
        numCount.Width = 80;
        numCount.Minimum = 2;
        numCount.Maximum = 50000;
        numCount.Value = 20;

        SetBtn(btnLoad, "Загрузить из файла", Color.FromArgb(108, 43, 217), 12, 182);
        SetBtn(btnSave, "Сохранить в файл", Color.FromArgb(13, 110, 253), 220, 182);

        btnLoad.Width = 196;
        btnSave.Width = 196;

        btnLoad.Click += OnLoad;
        btnSave.Click += OnSave;

        // Результат
        SetLabel(lblResult, "Отсортированный массив:", 12, 228);

        txtResult.Location = new Point(12, 248);
        txtResult.Width = 652;
        txtResult.Font = new Font("Consolas", 10f);
        txtResult.ReadOnly = true;
        txtResult.BackColor = Color.FromArgb(236, 253, 245);
        txtResult.ForeColor = Color.FromArgb(5, 150, 105);

        SetLabel(lblLog, "Лог / статистика:", 12, 284);

        txtLog.Location = new Point(12, 304);
        txtLog.Width = 652;
        txtLog.Height = 282;
        txtLog.Multiline = true;
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Font = new Font("Consolas", 9f);
        txtLog.BackColor = Color.FromArgb(248, 250, 252);
        txtLog.ForeColor = Color.FromArgb(30, 41, 59);

        Controls.AddRange(new Control[]
        {
            pnlHeader,
            lblDll,
            lblInput,  txtInput,
            btnSort,   btnRandom, btnClear, lblCount, numCount,
            btnLoad,   btnSave,
            lblResult, txtResult,
            lblLog,    txtLog
        });
    }


    private void OnSort(object? sender, EventArgs e)
    {
        if (!TryParse(out int[] nums)) return;

        try
        {
            var (sorted, stats) = ShellSorter.Sort(nums);

            txtResult.Text = string.Join(", ", sorted);

            LogLine(
                $"АЛГОРИТМ   : C (shell_sort.dll)\r\n" +
                $"  Элементов  : {nums.Length}\r\n" +
                $"  Время      : {stats.TimeMs * 1000.0:F2} мкс  ({stats.TimeMs:F4} мс)\r\n" +
                $"  Сравнений  : {stats.Comparisons}\r\n" +
                $"  Перестан.  : {stats.Swaps}");
        }
        catch (DllNotFoundException)
        {
            MessageBox.Show(
                "Файл shell_sort.dll не найден!\n\n" +
                "Скомпилируйте C-файл командой:\n" +
                "gcc shell_sort.c -shared -o shell_sort.dll -std=c99 -O2\n\n" +
                "И положите shell_sort.dll в папку с .exe",
                "DLL не найдена", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка вызова C-кода:\n{ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnRandom(object? sender, EventArgs e)
    {
        var rng = new Random();
        int n = (int)numCount.Value;
        var arr = new int[n];
        for (int i = 0; i < n; i++)
            arr[i] = rng.Next(1, 10000);

        txtInput.Text = string.Join(", ", arr);
        txtResult.Text = string.Empty;
    }

    private void OnClear(object? sender, EventArgs e)
    {
        txtInput.Text = string.Empty;
        txtResult.Text = string.Empty;
        txtLog.Text = string.Empty;
    }

    // Настюша
    private void OnLoad(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Выберите файл с числами",
            Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            string raw = File.ReadAllText(dlg.FileName)
                .Replace(",", " ").Replace(";", " ")
                .Replace("\r", " ").Replace("\n", " ");

            string[] tokens = raw.Split(' ',
                StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
            {
                MessageBox.Show("Файл пустой.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (string t in tokens)
            {
                if (!int.TryParse(t, out _))
                {
                    MessageBox.Show($"Найдено не число: \"{t}\"",
                        "Ошибка формата",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            txtInput.Text = string.Join(", ", tokens);
            txtResult.Text = string.Empty;

            LogLine(
                $"Загружен файл : {Path.GetFileName(dlg.FileName)}\r\n" +
                $"  Чисел        : {tokens.Length}");

            MessageBox.Show($"Загружено {tokens.Length} чисел.", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось прочитать файл:\n{ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    // Настюша
    private void OnSave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtResult.Text))
        {
            MessageBox.Show("Сначала отсортируйте массив.",
                "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new SaveFileDialog
        {
            Title = "Сохранить результат",
            Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
            FileName = "sorted_result.txt"
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            string content =
                $"Сортировка Шелла — результат (C + C# P/Invoke)\r\n" +
                $"Дата: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\r\n" +
                $"--------------------------------------------------\r\n" +
                $"Отсортированный массив:\r\n{txtResult.Text}\r\n";

            File.WriteAllText(dlg.FileName, content,
                System.Text.Encoding.UTF8);

            LogLine(
                $"Сохранён файл : {Path.GetFileName(dlg.FileName)}\r\n" +
                $"  Путь         : {dlg.FileName}");

            MessageBox.Show($"Файл сохранён:\n{dlg.FileName}", "Готово",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось сохранить:\n{ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool TryParse(out int[] nums)
    {
        nums = Array.Empty<int>();

        string raw = txtInput.Text.Replace(",", " ").Trim();

        if (string.IsNullOrWhiteSpace(raw))
        {
            MessageBox.Show("Введите числа или загрузите из файла.",
                "Пустой ввод", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        string[] parts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var list = new List<int>();

        foreach (string p in parts)
        {
            if (!int.TryParse(p, out int v))
            {
                MessageBox.Show($"Не число: \"{p}\"",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            list.Add(v);
        }

        if (list.Count < 2)
        {
            MessageBox.Show("Нужно минимум 2 числа.",
                "Мало чисел", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        nums = list.ToArray();
        return true;
    }

    private void LogLine(string text)
    {
        string sep = new string('-', 48);
        txtLog.AppendText(
            $"{sep}\r\n" +
            $"[{DateTime.Now:HH:mm:ss.fff}]\r\n" +
            $"  {text.Replace("\r\n", "\r\n  ")}\r\n" +
            $"{sep}\r\n");
        txtLog.SelectionStart = txtLog.Text.Length;
        txtLog.ScrollToCaret();
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
