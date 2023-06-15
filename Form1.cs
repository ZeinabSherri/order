using CheckBox = System.Windows.Forms.CheckBox;

namespace order
{
    public partial class Form1 : Form
    {
        private readonly decimal tax = 0.05M;
        private readonly decimal shipping = 1.5M;
        private readonly List<Item> items = new();

        public Form1()
        {
            InitializeComponent();
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Price";
            for (int i = 1; i < 10; i++)
            {
                items.Add(new("Item " + i, i * 1.5M));
            }
            listBox1.Items.AddRange(items.ToArray());

            textBox1.Text = "Order Number";
            textBox2.Text = "Shipping Address";
            textBox3.Text = "Customer Name";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected item
            if (listBox1.SelectedItem is not Item selectedItem)
            {
                MessageBox.Show("Please select an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the selected colors
            List<string> colors = new();
            foreach (var checkBox in new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4 })
            {
                if (checkBox.Checked)
                    colors.Add(checkBox.Text);
            }

            // Get the quantity
            if (!int.TryParse(textBox4.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string line = $"> {selectedItem.Name}, " +
                          $"Color: {(colors.Count > 0 ? string.Join(", ", colors) : "None")}, " +
                          $"Quantity: {quantity}, Price: {selectedItem.Price:C}";

            textBox5.Text += line + Environment.NewLine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Calculate the order total
            var orderTotal = CalculateOrderTotal();

            // Update the order total label
            string orderNumber = textBox1.Text;
            string shippingAddress = textBox2.Text;
            string customerName = textBox3.Text;

            textBox6.Text = $"Order No: {orderNumber}\t\tCustomer Name: {customerName}{Environment.NewLine}Shipping Address: {shippingAddress}{Environment.NewLine}{Environment.NewLine}";
            textBox6.Text += textBox5.Text;
            textBox6.Text += $"{Environment.NewLine}Order Total: {orderTotal:C}";
        }

        private decimal CalculateOrderTotal()
        {
            // Calculate the total price for each item in the receipt
            decimal totalQuantity = 0;
            decimal orderTotal = 0;

            foreach (string line in textBox5.Lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    var isQuantity = int.TryParse(parts[2].Replace("Quantity: ", string.Empty).Trim(), out int quantity);
                    var isPrice = decimal.TryParse(parts[3].Replace("Price: ", string.Empty).Trim().Replace("£", string.Empty), out decimal price);

                    if (parts.Length >= 4 && isPrice && isQuantity)
                    {
                        totalQuantity += quantity;
                        orderTotal += (price * quantity);
                    }
                }
            }

            // order total with tax, and shipping if less than 20 items
            return orderTotal * (1 + tax) + (totalQuantity > 20 || totalQuantity < 1 ? 0 : shipping);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear everything?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            // Clear the input fields
            textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "Customer Name";
            textBox2.Text = "Shipping Address";
            textBox1.Text = "Order Number";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            listBox1.SelectedIndex = -1;
        }
    }
}



