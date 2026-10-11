using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            else
            {
                MessageBox.Show("Por favor, seleccione un género.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Interrumpe el guardado si no hay género
            }

            // Validar que no haya datos vacíos
            if (string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(tel) || string.IsNullOrWhiteSpace(edad) || string.IsNullOrWhiteSpace(estatura))
            {
                MessageBox.Show("Por favor, llene todos los campos correctamente antes de guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Uso de Environment.NewLine para correcta compatibilidad de saltos de línea en Windows
            String Mensaje = $"Nombre: {nombres}{Environment.NewLine}" +
                             $"Apellidos: {apellidos}{Environment.NewLine}" +
                             $"Teléfono: {tel}{Environment.NewLine}" +
                             $"Edad: {edad}{Environment.NewLine}" +
                             $"Estatura: {estatura}{Environment.NewLine}" +
                             $"Género: {genero}";

            String rutaArchivo = @"C:\Users\patat\source\repos\practica4-JGED_Septiembre\registro.txt";

            try
            {
                bool archivoExiste = File.Exists(rutaArchivo);

                // Instanciación del objeto escritor con codificación explícita
                using (StreamWriter escritor = new StreamWriter(rutaArchivo, true, Encoding.UTF8))
                {
                    if (archivoExiste)
                    {
                        escritor.WriteLine(); // Separador entre registros
                    }
                    escritor.WriteLine(Mensaje);
                }

                MessageBox.Show(Mensaje, "Confirmación de registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpia el formulario sin pedir confirmación tras un guardado exitoso
                LimpiarFormulario(pedirConfirmacion: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el archivo: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
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