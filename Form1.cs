using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practica4_JGED_Septiembre
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            // Construcción del mensaje usando Environment.NewLine para mayor robustez
            String Mensaje = $"Nombre: {nombres}{Environment.NewLine}" +
                             $"Apellidos: {apellidos}{Environment.NewLine}" +
                             $"Teléfono: {tel}{Environment.NewLine}" +
                             $"Edad: {edad}{Environment.NewLine}" +
                             $"Estatura: {estatura}{Environment.NewLine}" +
                             $"Género: {genero}";

            String rutaArchivo = "C:\\Users\\patat\\source\\repos\\practica4-JGED_Septiembre\\registro.txt";

            try
            {
                // Verificamos si el archivo existe y contiene datos reales
                bool archivoExiste = File.Exists(rutaArchivo) && new FileInfo(rutaArchivo).Length > 0;

                // Uso de StreamWriter dentro de un bloque using para asegurar el cierre del recurso
                using (StreamWriter escritor = new StreamWriter(rutaArchivo, true, Encoding.UTF8))
                {
                    if (archivoExiste)
                    {
                        escritor.WriteLine(); // Separador entre registros
                    }
                    escritor.WriteLine(Mensaje);
                }

                MessageBox.Show(Mensaje, "Confirmación de registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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