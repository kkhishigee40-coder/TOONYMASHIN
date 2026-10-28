using System.Globalization;

namespace TOONYMASHIN
{
    public partial class Form1 : Form
    {
        double accumulator = 0;    
        string pendingOp = "";      
        bool newEntry = true;      
        double memory = 0;          
        TextBox display;         

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            display = GetAll(this).OfType<TextBox>().FirstOrDefault();
            if (display != null)
            {
                display.Text = "0";
                display.ReadOnly = true;
                display.TextAlign = HorizontalAlignment.Right;
            }

            foreach (Button b in GetAll(this).OfType<Button>())
                b.Click += Calc_Click;
        }

        private IEnumerable<Control> GetAll(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (Control child in GetAll(c))
                    yield return child;
            }
        }

        private double Current()
        {
            double v;
            if (double.TryParse(display.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                return v;
            return 0;
        }

        private void Show(double v)
        {
            if (double.IsNaN(v) || double.IsInfinity(v))
            {
                display.Text = "Алдаа";
                newEntry = true;
                pendingOp = "";
                return;
            }
            display.Text = v.ToString("G15", CultureInfo.InvariantCulture);
        }

        private double Compute(double a, double b, string op)
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/": return b == 0 ? double.NaN : a / b;
                default: return b;
            }
        }

        private void Calc_Click(object sender, EventArgs e)
        {
            if (display == null) return;

            string t = ((Button)sender).Text.Trim();

     
            if (display.Text == "Алдаа" && t != "C")
            {
                display.Text = "0";
                newEntry = true;
            }

            if (t.Length == 1 && char.IsDigit(t[0]))
            {
                if (newEntry || display.Text == "0")
                    display.Text = t;
                else
                    display.Text += t;
                newEntry = false;
                return;
            }

            switch (t)
            {
                case ".":
                case ",":
                    if (newEntry)
                    {
                        display.Text = "0.";
                        newEntry = false;
                    }
                    else if (!display.Text.Contains("."))
                    {
                        display.Text += ".";
                    }
                    break;

                case "+":
                case "-":
                case "*":
                case "/":
                    if (pendingOp != "" && !newEntry)
                    {
                        accumulator = Compute(accumulator, Current(), pendingOp);
                        Show(accumulator);
                        if (display.Text == "Алдаа") return;
                    }
                    else
                    {
                        accumulator = Current();
                    }
                    pendingOp = t;
                    newEntry = true;
                    break;

                case "=":
                    if (pendingOp != "")
                    {
                        double result = Compute(accumulator, Current(), pendingOp);
                        Show(result);
                        accumulator = result;
                        pendingOp = "";
                        newEntry = true;
                    }
                    break;

                case "C":
                    accumulator = 0;
                    pendingOp = "";
                    display.Text = "0";
                    newEntry = true;
                    break;

                case "CE":
                    display.Text = "0";
                    newEntry = true;
                    break;

                case "Backspace":
                    if (!newEntry && display.Text.Length > 1)
                        display.Text = display.Text.Substring(0, display.Text.Length - 1);
                    else
                    {
                        display.Text = "0";
                        newEntry = true;
                    }
                    if (display.Text == "-") display.Text = "0";
                    break;
                case "sqrt":
                case "√":
                    Show(Math.Sqrt(Current()));
                    newEntry = true;
                    break;

                case "^2":
                case "x²":
                    Show(Current() * Current());
                    newEntry = true;
                    break;

                case "1/x":
                    Show(Current() == 0 ? double.NaN : 1 / Current());
                    newEntry = true;
                    break;

                case "+/-":
                case "±":
                case "()":
                    Show(-Current());
                    break;

                case "%":
                    if (pendingOp != "")
                        Show(accumulator * Current() / 100);
                    else
                        Show(Current() / 100);
                    newEntry = true;
                    break;

               
                case "MC":
                    memory = 0;
                    break;

                case "MR":
                case "MA":
                    Show(memory);
                    newEntry = true;
                    break;

                case "MS":
                    memory = Current();
                    newEntry = true;
                    break;

                case "M+":
                    memory += Current();
                    newEntry = true;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void button16_Click(object sender, EventArgs e) { }
        private void button15_Click(object sender, EventArgs e) { }
        private void button11_Click(object sender, EventArgs e) { }
        private void button24_Click(object sender, EventArgs e) { }
        private void button17_Click(object sender, EventArgs e) { }
        private void button9_Click(object sender, EventArgs e) { }
        private void button22_Click(object sender, EventArgs e) { }
        private void button6_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
    }
}