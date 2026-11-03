using Microsoft.Win32;
// Cambios
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practica4_JGED_Septiembre
{
    public partial class Form1 : Form
    {
        string SQLConection = "Server=127.0.0.1; Port=3306; Database=ap_v1; Uid=root; Pwd=26060799;";
        public Form1()
        {
            InitializeComponent();

            // Creación de los event handlers usando delegates
            tbNombre.TextChanged += ValidarNombre;
            tbApellidos.TextChanged += ValidarApellido;
            tbEdad.TextChanged += ValidarEdad;
            tbEstatura.TextChanged += ValidarEstatura;
            tbTelefono.Leave += ValidarTelefono;
        }

        private void insertarRegistros(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection conectar = new MySqlConnection(SQLConection))
            {
                conectar.Open();
                string insertQuery = "INSERT INTO registros (nombre, apellidos, telefono, estatura, edad, genero) " +
                                     "VALUES (@nombre, @apellidos, @telefono, @estatura, @edad, @genero)";

                using (MySqlCommand comando = new MySqlCommand(insertQuery, conectar))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellidos", apellidos);
                    comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@estatura", estatura);
                    comando.Parameters.AddWithValue("@edad", edad);
                    comando.Parameters.AddWithValue("@genero", genero);
                    comando.ExecuteNonQuery();
                }
                conectar.Close();
            }
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;

            // Ignorar validación si el campo está vacío para no marcar error al limpiar
            if (string.IsNullOrWhiteSpace(textboxLocal.Text))
            {
                textboxLocal.BackColor = System.Drawing.SystemColors.Window;
                return;
            }

            if (!TelefonoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un teléfono válido de 10 dígitos", "Error Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.BackColor = System.Drawing.Color.LightPink;
            }
            else
            {
                textboxLocal.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private bool TelefonoValido(string valor)
        {
            // Se utiliza discard '_' ya que no necesitamos almacenar el resultado del out
            return long.TryParse(valor, out _) && valor.Length == 10;
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!string.IsNullOrEmpty(textboxLocal.Text) && !EsDecimalValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba una estatura válida", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsDecimalValido(string valor)
        {
            return decimal.TryParse(valor, out _);
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textLocal = (TextBox)sender;
            if (!string.IsNullOrEmpty(textLocal.Text) && !EsEnteroValido(textLocal.Text))
            {
                MessageBox.Show("Escriba una edad válida", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textLocal.Clear();
            }
        }

        private bool EsEnteroValido(string valor)
        {
            return int.TryParse(valor, out _);
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!string.IsNullOrEmpty(textboxLocal.Text) && !EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un nombre válido", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!string.IsNullOrEmpty(textboxLocal.Text) && !EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un apellido válido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsTextoValido(string Valor)
        {
            return Regex.IsMatch(Valor, @"^[a-zA-Z\s]+$");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            String nombres = tbNombre.Text.Trim();
            String apellidos = tbApellidos.Text.Trim();
            String tel = tbTelefono.Text.Trim();
            String edad = tbEdad.Text.Trim();
            String estatura = tbEstatura.Text.Trim();
            String genero = "";

            if (rbFemenino.Checked)
            {
                genero = "Femenino";
            }
            else if (rbMasculino.Checked)
            {
                genero = "Masculino";
            }
            else if (rbOtros.Checked)
            {
                genero = "Otros";
            }

            // Validación global añadida por el profesor (adaptada a tus métodos)
            if (EsEnteroValido(edad) && EsDecimalValido(estatura) && TelefonoValido(tel) && EsTextoValido(nombres) && EsTextoValido(apellidos))
            {
                String Mensaje = $"Nombre: {nombres}\n\rApellidos: {apellidos}\n\rTeléfono: {tel}\n\rEdad: {edad}\n\rEstatura: {estatura}\n\rGénero: {genero}";
                String rutaArchivo = "C:\\Users\\patat\\source\\repos\\practica4-JGED_Septiembre\\registro.txt";

                bool archivoExiste = File.Exists(rutaArchivo);

                // 1. Guarda en el TXT (Tu lógica original)
                using (StreamWriter escritor = new StreamWriter(rutaArchivo, true))
                {
                    if (archivoExiste)
                    {
                        escritor.WriteLine();
                    }
                    escritor.WriteLine(Mensaje);
                }

                // 2. Inserta en la Base de Datos
                insertarRegistros(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), tel, genero);

                MessageBox.Show("Datos guardados en TXT y Base de Datos con éxito.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Mensaje de error si alguna validación falla
                MessageBox.Show("Por favor, ingrese datos válidos en los campos antes de guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario(pedirConfirmacion: true);
        }

        // Método refactorizado para centralizar la limpieza y manejar la confirmación
        private void LimpiarFormulario(bool pedirConfirmacion)
        {
            if (pedirConfirmacion)
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro que desea limpiar todos los campos?", "Confirmar Limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.No)
                {
                    return;
                }
            }

            tbNombre.Clear();
            tbApellidos.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
            tbEdad.Clear();
            rbFemenino.Checked = false;
            rbMasculino.Checked = false;
            rbOtros.Checked = false;
            tbTelefono.BackColor = System.Drawing.SystemColors.Window; // Restaura el color original
        }
    }
}