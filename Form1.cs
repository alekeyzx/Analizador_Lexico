using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        static string conexionstring = "Data Source=ALEXPC;Initial Catalog=MatrizTrans;Integrated Security=True";
        SqlConnection conexion = new SqlConnection(conexionstring);

        List<Error> ListaErrores = new List<Error>();
        List<Simbolo> ListaSimbolo = new List<Simbolo>();

        string GuardarTOKENS;
        int lineaError = 1;
        int ContadorLinea = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public int CalcularEstado(int Simbolo)
        {
            switch (Simbolo)
            {
                case 97: return 1;
                case 98: return 2;
                case 99: return 3;
                case 100: return 4;
                case 101: return 5;
                case 102: return 6;
                case 103: return 7;
                case 104: return 8;
                case 105: return 9;
                case 106: return 10;
                case 107: return 11;
                case 108: return 12;
                case 109: return 13;
                case 110: return 14;
                case 164: return 15;
                case 111: return 16;
                case 112: return 17;
                case 113: return 18;
                case 114: return 19;
                case 115: return 20;
                case 116: return 21;
                case 117: return 22;
                case 118: return 23;
                case 119: return 24;
                case 120: return 25;
                case 121: return 26;
                case 122: return 27;
                case 65: return 28;
                case 66: return 29;
                case 67: return 30;
                case 68: return 31;
                case 69: return 32;
                case 70: return 33;
                case 71: return 34;
                case 72: return 35;
                case 73: return 36;
                case 74: return 37;
                case 75: return 38;
                case 76: return 39;
                case 77: return 40;
                case 78: return 41;
                case 165: return 42;
                case 79: return 43;
                case 80: return 44;
                case 81: return 45;
                case 82: return 46;
                case 83: return 47;
                case 84: return 48;
                case 85: return 49;
                case 86: return 50;
                case 87: return 51;
                case 88: return 52;
                case 89: return 53;
                case 90: return 54;
                case 48: return 55;
                case 49: return 56;
                case 50: return 57;
                case 51: return 58;
                case 52: return 59;
                case 53: return 60;
                case 54: return 61;
                case 55: return 62;
                case 56: return 63;
                case 57: return 64;
                case 60: return 65;
                case 62: return 66;
                case 61: return 67;
                case 33: return 68;
                case 43: return 69;
                case 45: return 70;
                case 47: return 71;
                case 42: return 72;
                case 94: return 73;
                case 95: return 74;
                case 64: return 75;
                case 35: return 76;
                case 39: return 77;
                case 32: return 78;
                case 46: return 79;
                case 34: return 80;
                case 36: return 81;
                case 59: return 82;
                case 40: return 83;
                case 41: return 84;
                case 123: return 85;
                case 125: return 86;
                case 38: return 87;
            }

            return 100;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conexion.Open();

            string Consulta = "Select * From matriz";
            SqlCommand comando = new SqlCommand(Consulta, conexion);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable tabla = new DataTable();
            data.Fill(tabla);
            dgvTabla.DataSource = tabla;

        }

        private void btnAnalizador_Click(object sender, EventArgs e)
        {
            lineaError = 1;
            ContadorLinea = 0;
            txtLinea.Text = "";
            //txtLinea.Text = "";
            if (txtFuente.Text == "")
            {
                MessageBox.Show("Ingrese el codigo fuente.");
                return;
            }

            txtToken.Text = null;
            ListaErrores.Clear();
            ListaSimbolo.Clear();

            string Cadena = txtFuente.Text;
            char[] AlmacenaSimbolo = new char[Cadena.Length+1];
            AlmacenaSimbolo[Cadena.Length] = '?';

            //ALMACENA LA CANDENA
            for (int i = 0; i < Cadena.Length; i++)
            {
                AlmacenaSimbolo[i] = char.Parse(Cadena.Substring(i, 1));
            }

            int Estado = 0;
            int EstadoAnt = 0;
            int Codigo = 0;
            int ContadorError = 0;
            int ContadorIdentificador = 0;
            string nomSim = "";
            char Anterior=' ';
            double VARaceptada = 0;
            string GuardarValor = "";
            int Bandera = 0;


            for (int i = 0; i < Cadena.Length+1; i++)
            {
                //GUARDAS DATOS
                if (AlmacenaSimbolo[i] == '=')
                {
                    VARaceptada = 1;
                }
                if(VARaceptada == 1)
                {
                    if ((AlmacenaSimbolo[i] > 47 && AlmacenaSimbolo[i] < 58) || (AlmacenaSimbolo[i] == 46 || AlmacenaSimbolo[i] == 45))
                    {
                        GuardarValor = GuardarValor + AlmacenaSimbolo[i];
                    }
                }
                if (i != 0)
                {
                    Anterior = AlmacenaSimbolo[i - 1];
                    EstadoAnt = Estado;
                }
                if (Anterior == ' ' && AlmacenaSimbolo[i] == ' ' || AlmacenaSimbolo[i] == '\r')
                {
                    //Estado = 0;
                    continue;
                }
                if((AlmacenaSimbolo[i] != '?') && (VARaceptada == 0) &&((AlmacenaSimbolo[i] > 64  && AlmacenaSimbolo[i] < 92) || (AlmacenaSimbolo[i] > 96 && AlmacenaSimbolo[i] < 123) || (AlmacenaSimbolo[i]>47 && AlmacenaSimbolo[i] < 58)))
                {
                    nomSim = nomSim + AlmacenaSimbolo[i];
                }
                

                //ESTADOS DE ERROR
                if (Estado > 199 && (AlmacenaSimbolo[i] == ' ' || AlmacenaSimbolo[i] == '\n' || AlmacenaSimbolo[i] == '?'))
                {
                    ContadorError = ContadorError + 1;
                    Error miError = new Error();
                    miError.Numero = ContadorError;
                    miError.Tipo_de_Error = dgvTabla.Rows[Estado - 73].Cells[90].Value.ToString();

                    miError.Linea_Error = lineaError;

                    ListaErrores.Add(miError);

                    dgvError.DataSource = null;
                    dgvError.DataSource = ListaErrores;


                    string Error = "ERROR" + ContadorError;

                    txtToken.SelectionStart = txtToken.TextLength;
                    txtToken.SelectionLength = 0;
                    txtToken.SelectionColor = Color.Red;
                    txtToken.AppendText(Error);
                    txtToken.SelectionColor = txtToken.ForeColor;


                    if (AlmacenaSimbolo[i] == ' ')
                    {
                        
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText(" ");
                        
                    }
                    
                    if(AlmacenaSimbolo[i] == '\n')
                    {
                        lineaError += 1;
                        //txtLinea.AppendText((lineaError-1).ToString());
                        //txtLinea.AppendText("\r" + "\n");
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText("\r" + "\n");
                    }

                    VARaceptada = 0;
                    GuardarValor = "";
                    nomSim = "";
                    Estado = 0;

                    continue;
                }

                //SIGUIENTE ESTADO DE ERROR
                if (Estado > 199)
                {
                    continue;
                }

                //FIN DE CADENA
                if (AlmacenaSimbolo[i] == '?')
                {
                    lineaError += 1;
                    //txtLinea.AppendText((lineaError-1).ToString());
                    //txtLinea.AppendText("\r" + "\n");
                    if (Estado == 0)
                    {
                        break;
                    }
                    if (Estado > 199)
                    {
                        break;
                    }
                    else
                    {
                        if (dgvTabla.Rows[Estado].Cells[90].Value.ToString() == "")
                        {
                            Estado = int.Parse(dgvTabla.Rows[Estado].Cells[89].Value.ToString());
                            i = i - 1;
                            continue;
                        }
                        else
                        {

                            if ((Estado == 53 || Estado == 56) && VARaceptada == 1)
                            {
                                //ContadorIdentificador = ContadorIdentificador + 1;
                                Simbolo miSimbolo = new Simbolo();
                                miSimbolo.INDENTIFICADOR = ContadorIdentificador;
                                miSimbolo.NOMBRE = nomSim;
                                miSimbolo.VALOR = double.Parse(GuardarValor);

                                ListaSimbolo.Add(miSimbolo);

                                dgvSimbolos.DataSource = null;
                                dgvSimbolos.DataSource = ListaSimbolo;
                                VARaceptada = 0;
                                GuardarValor = "";
                                nomSim = "";

                            }

                            if (Estado == 49)
                            {
                                string sim = nomSim;
                                foreach (Simbolo simbolo in ListaSimbolo)
                                {
                                    if(nomSim == simbolo.NOMBRE)
                                    {
                                        sim = simbolo.NOMBRE;
                                        ContadorIdentificador--;
                                        Bandera = 1;
                                        break;
                                    }
                                    else
                                    {
                                        sim = nomSim;
                                    }
                                } 

                                if(Bandera == 0)
                                {
                                    //ContadorIdentificador = ContadorIdentificador + 1;
                                    Simbolo miSimbolo = new Simbolo();
                                    miSimbolo.INDENTIFICADOR = ContadorIdentificador + 1;
                                    miSimbolo.NOMBRE = sim;


                                    ListaSimbolo.Add(miSimbolo);

                                    dgvSimbolos.DataSource = null;
                                    dgvSimbolos.DataSource = ListaSimbolo;
                                    VARaceptada = 0;
                                    GuardarValor = "";
                                    nomSim = "";
                                    
                                }
                                Bandera = 0;

                            }


                            if(Estado == 49)
                            {
                                ContadorIdentificador++;
                                txtToken.SelectionStart = txtToken.TextLength;
                                txtToken.SelectionLength = 0;
                                txtToken.AppendText(dgvTabla.Rows[Estado].Cells[90].Value.ToString() + ContadorIdentificador);
                            }
                            else
                            {
                                txtToken.SelectionStart = txtToken.TextLength;
                                txtToken.SelectionLength = 0;
                                txtToken.AppendText(dgvTabla.Rows[Estado].Cells[90].Value.ToString());
                            }
                            
                            break;
                        }
                    }

                }

                //FIN DE CADENA Y SIGUIENTE
                if (AlmacenaSimbolo[i] == ' ' || AlmacenaSimbolo[i] == '\n' )
                {
                    ContadorLinea++;
                    txtLinea.AppendText((ContadorLinea).ToString());
                    txtLinea.AppendText("\r" + "\n");

                    if (Estado == 0)
                    {
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText("\r" + "\n");
                        continue;
                    }

                    Estado = int.Parse(dgvTabla.Rows[Estado].Cells[89].Value.ToString());

                    if ((Estado == 53 || Estado == 56) && VARaceptada == 1 )
                    {
                        //Bandera = 1;
                        //ContadorIdentificador = ContadorIdentificador + 1;
                        Simbolo miSimbolo = new Simbolo();
                        miSimbolo.INDENTIFICADOR = ContadorIdentificador;
                        miSimbolo.NOMBRE = nomSim;
                        miSimbolo.VALOR = double.Parse(GuardarValor);

                        ListaSimbolo.Add(miSimbolo);

                        dgvSimbolos.DataSource = null;
                        dgvSimbolos.DataSource = ListaSimbolo;
                        VARaceptada = 0;
                        GuardarValor = "";
                        

                    }


                    if (Estado == 49 && AlmacenaSimbolo[i + 1] != '=')
                    {
                        string sim = nomSim;
                        foreach (Simbolo simbolo in ListaSimbolo)
                        {
                            if (nomSim == simbolo.NOMBRE)
                            {
                                sim = simbolo.NOMBRE;
                                ContadorIdentificador--;
                                Bandera = 1;
                                break;
                            }
                            else
                            {
                                sim = nomSim;
                            }
                        }

                        if(Bandera == 0)
                        {
                            //ContadorIdentificador = ContadorIdentificador + 1;
                            Simbolo miSimbolo = new Simbolo();
                            miSimbolo.INDENTIFICADOR = ContadorIdentificador + 1;
                            miSimbolo.NOMBRE = sim;
                            miSimbolo.VALOR = null;


                            ListaSimbolo.Add(miSimbolo);

                            dgvSimbolos.DataSource = null;
                            dgvSimbolos.DataSource = ListaSimbolo;
                            VARaceptada = 0;
                            GuardarValor = "";
                            nomSim = "";
                        }
                        Bandera = 0;

                    }

                    //andera = 0;
                    

                    if (Estado == 49)
                    {
                        ContadorIdentificador++;
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText(dgvTabla.Rows[Estado].Cells[90].Value.ToString() + ContadorIdentificador);
                   
                    }
                    else
                    {
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText(dgvTabla.Rows[Estado].Cells[90].Value.ToString());
                    }




                    if (AlmacenaSimbolo[i] == ' ')
                    {
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText(" ");
                    }
                    else
                    {
                        lineaError += 1;
                        //txtLinea.AppendText((lineaError-1).ToString());
                        //txtLinea.AppendText("\r" + "\n");
                        txtToken.SelectionStart = txtToken.TextLength;
                        txtToken.SelectionLength = 0;
                        txtToken.AppendText("\r" + "\n");
                    }


                    if (AlmacenaSimbolo[i + 1] == '=' || AlmacenaSimbolo[i-1] == '=' )
                    {
                        
                    }
                    else
                    {
                        nomSim = "";
                    }


                    Estado = 0;

                    continue;
                }

               
                //CAMBIAR ESTADO
                Codigo = (int)AlmacenaSimbolo[i];

                Estado = int.Parse(dgvTabla.Rows[Estado].Cells[CalcularEstado(Codigo)].Value.ToString());

                //MessageBox.Show(Estado.ToString());

            }


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarTOKENS = txtToken.Text;
            MessageBox.Show("Se guardo correctamente el archivo de tokens: \n"+ GuardarTOKENS);
        }
    }
}
