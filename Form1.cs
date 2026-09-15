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
            String nombres = tbNombre.Text;
            String apellidos = tbApellidos.Text;
            String tel = tbTelefono.Text;
            String edad = tbEdad.Text;
            String estatura = tbEstatura.Text;
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
