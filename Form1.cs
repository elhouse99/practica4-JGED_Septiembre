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
            if (!TelefonoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un telefono valido", "Error Telefono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.BackColor = System.Drawing.Color.LightPink;
                //textboxLocal.Clear();
            }
            else
            {
                textboxLocal.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private bool TelefonoValido(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsDecimalValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba una estatura valida", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textLocal = (TextBox)sender;
            if (!EsEnteroValido(textLocal.Text))
            {
                MessageBox.Show("Escriba una edad valida", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textLocal.Clear();
            }
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un nombre valido", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un apellido valido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsTextoValido(string Valor)
        {
            return Regex.IsMatch(Valor, @"^[a-zA-Z\s]+$");
            //^[a-zA-Z\s]+$
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Limpiamos los espacios en blanco sobrantes de los TextBoxes
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

            String Mensaje = $"Nombre: {nombres}\n\rApellidos: {apellidos}\n\rTeléfono: {tel}\n\rEdad: {edad}\n\rEstatura: {estatura}\n\rGénero: {genero}";
            //Ruta de archivo using.System.IO
            String rutaArchivo = "C:\\Users\\patat\\source\\repos\\practica4-JGED_Septiembre\\registro.txt";
            //Alternativas
            bool archivoExiste = File.Exists(rutaArchivo);
            //Instanciación de objeto escritor y verificación para ver si ya existe.
            using (StreamWriter escritor = new StreamWriter(rutaArchivo, true))
            {
                //Si el archivo ya existe, añadimos un separador antes del nuevo registro
                if (archivoExiste)
                {
                    escritor.WriteLine();
                }
                escritor.WriteLine(Mensaje);
            }
            MessageBox.Show(Mensaje, "Confirmación de registro", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbNombre.Clear();
            tbApellidos.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
            tbEdad.Clear();
            rbFemenino.Checked = false;
            rbMasculino.Checked = false;
            rbOtros.Checked = false;
        }
    }
}