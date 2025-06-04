using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using Npgsql;

namespace OOP2
{
    public partial class Form1 : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Username=Ivan;Password=14235687;Database=Flowers";
        private NpgsqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            connection = new NpgsqlConnection(connectionString);
            LoadFlowers();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                var columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                var row = dataGridView1.Rows[e.RowIndex];
                var id = Convert.ToInt32(row.Cells["ID"].Value);
                var newValue = row.Cells[e.ColumnIndex].Value;
                var editableColumns = new[] { "Владельцы", "Размер", "Лепестки", "Высота" };

                if (!editableColumns.Contains(columnName)) return;

                using (var connection = new NpgsqlConnection(connectionString))
                using (var cmd = new NpgsqlCommand())
                {
                    connection.Open();
                    cmd.Connection = connection;

                    switch (columnName)
                    {
                        case "Владельцы":
                            cmd.CommandText = "UPDATE Flowers SET owner_count = @value WHERE id = @id";
                            cmd.Parameters.AddWithValue("value", Convert.ToInt32(newValue));
                            break;

                        case "Размер":
                            cmd.CommandText = "UPDATE Flowers SET size = @value WHERE id = @id";
                            cmd.Parameters.AddWithValue("value", Convert.ToDecimal(newValue));
                            break;

                        case "Лепестки":
                            cmd.CommandText = "UPDATE Flowers SET petals = @value WHERE id = @id";
                            cmd.Parameters.AddWithValue("value", Convert.ToInt32(newValue));
                            break;

                        case "Высота":
                            cmd.CommandText = "UPDATE Flowers SET height = @value WHERE id = @id";
                            cmd.Parameters.AddWithValue("value", Convert.ToDecimal(newValue));
                            break;
                    }

                    cmd.Parameters.AddWithValue("id", id);
                    int affected = cmd.ExecuteNonQuery();
                }
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                label6.Text = $"Корни: {selectedRow.Cells["Корни"].Value}";
                label7.Text = $"Продолжительность жизни: {selectedRow.Cells["Продолжительность жизни"].Value} дней";

                if (selectedRow.Cells["Тип"].Value.ToString() == "Home")
                {
                    label8.Text = $"Тип: Домашний";
                    label9.Text = $"Владельцев: {selectedRow.Cells["Владельцы"].Value}";
                    label10.Text = $"Размер: {selectedRow.Cells["Размер"].Value} см";
                    label11.Text = "Хозяин цветка рассказывает о его истории";
                    label12.Text = "У домашнего цветка красивое кашпо";
                }
                else
                {
                    label8.Text = $"Тип: Полевой";
                    label9.Text = $"Лепестков: {selectedRow.Cells["Лепестки"].Value}";
                    label10.Text = $"Высота: {selectedRow.Cells["Высота"].Value} см";
                    label11.Text = "Полевой цветок опыляют шмели, бабочки, пчёлы";
                    label12.Text = "Полевой цветок растёт в поле";
                }
            }
        }

        private void LoadFlowers()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "SELECT * FROM Flowers ORDER BY id";
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dt.Columns["id"].ColumnName = "ID";
                dt.Columns["type"].ColumnName = "Тип";
                dt.Columns["roots"].ColumnName = "Корни";
                dt.Columns["life_duration"].ColumnName = "Продолжительность жизни";
                dt.Columns["owner_count"].ColumnName = "Владельцы";
                dt.Columns["size"].ColumnName = "Размер";
                dt.Columns["petals"].ColumnName = "Лепестки";
                dt.Columns["height"].ColumnName = "Высота";

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string type = radioButton1.Checked ? "Home" : "Field";
                string query;
                NpgsqlCommand cmd;

                if (radioButton1.Checked)
                {
                    if (!int.TryParse(TextBoxNumber1.Text, out int owners) ||
                        !float.TryParse(TextBoxNumber2.Text, out float size))
                    {
                        MessageBox.Show("Пожалуйста, введите корректные числовые значения", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    query = "INSERT INTO Flowers (type, roots, life_duration, owner_count, size) VALUES (@type, @roots, @life, @owners, @size)";
                    cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@roots", 1);
                    cmd.Parameters.AddWithValue("@life", 365);
                    cmd.Parameters.AddWithValue("@owners", owners);
                    cmd.Parameters.AddWithValue("@size", size);
                }
                else
                {
                    if (!int.TryParse(TextBoxNumber1.Text, out int petals) ||
                        !float.TryParse(TextBoxNumber2.Text, out float height))
                    {
                        MessageBox.Show("Пожалуйста, введите корректные числовые значения", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    query = "INSERT INTO Flowers (type, roots, life_duration, petals, height) VALUES (@type, @roots, @life, @petals, @height)";
                    cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@roots", 1);
                    cmd.Parameters.AddWithValue("@life", 90);
                    cmd.Parameters.AddWithValue("@petals", petals);
                    cmd.Parameters.AddWithValue("@height", height);
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Цветок успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TextBoxNumber1.Clear();
                    TextBoxNumber2.Clear();
                    LoadFlowers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении цветка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления", "Внимание",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                var idCell = selectedRow.Cells["ID"];

                if (idCell.Value == null || idCell.Value == DBNull.Value)
                {
                    MessageBox.Show("ID выбранного цветка не указан", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(idCell.Value.ToString(), out int id))
                {
                    MessageBox.Show($"Некорректный ID: '{idCell.Value}'. ID должен быть целым числом.", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить цветок с ID {id}?",
                                                  "Подтверждение удаления",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                using (var connection = new NpgsqlConnection(connectionString))
                using (var cmd = new NpgsqlCommand("DELETE FROM Flowers WHERE id = @id", connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Цветок успешно удален!", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFlowers();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить цветок. Возможно, он уже был удален.",
                                      "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Кол-во владельцев:";
            label4.Text = "Размер цветка (см):";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Кол-во лепестков:";
            label4.Text = "Высота цветка (см):";
        }
    }
}