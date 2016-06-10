using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace AESImplementation
{
    public partial class Form1 : Form
    {
        public int Nk = 4;
        public int Nb = 4;
        public int Nr = 10;
        public char[] RoundKey = new char[176];
        public char[,] S = new char[4,4];
        public char[] data = new char[16];
        public char[,] state = new char[4,4];

        public char[,] S_d = new char[4,4];
        public char[] data_d = new char[16];

        public string encrypt_val = "";
        public string output = "";

        public char[] Sbox = new char[256] {
    '\x0063', '\x007c','\x0077','\x007b','\x00f2','\x006b','\x006f','\x00c5', '\x0030', '\x0001', 
	'\x0067', '\x002b', '\x00fe', '\x00d7', '\x00ab', '\x0076',	'\x00ca', '\x0082', '\x00c9', '\x007d', 
	'\x00fa', '\x0059', '\x0047', '\x00f0', '\x00ad', '\x00d4', '\x00a2', '\x00af', '\x009c', '\x00a4', 
	'\x0072', '\x00c0', '\x00b7', '\x00fd', '\x0093', '\x0026', '\x0036', '\x003f', '\x00f7', '\x00cc', 
	'\x0034', '\x00a5', '\x00e5', '\x00f1', '\x0071', '\x00d8', '\x0031', '\x0015', '\x0004', '\x00c7', 
	'\x0023', '\x00c3', '\x0018', '\x0096', '\x0005', '\x009a', '\x0007', '\x0012', '\x0080', '\x00e2', 
	'\x00eb', '\x0027', '\x00b2', '\x0075', '\x0009', '\x0083', '\x002c', '\x001a', '\x001b', '\x006e', 
	'\x005a', '\x00a0', '\x0052', '\x003b', '\x00d6', '\x00b3', '\x0029', '\x00e3', '\x002f', '\x0084', 
	'\x0053', '\x00d1', '\x0000', '\x00ed', '\x0020', '\x00fc', '\x00b1', '\x005b', '\x006a', '\x00cb',
	'\x00be', '\x0039', '\x004a', '\x004c', '\x0058', '\x00cf',	'\x00d0', '\x00ef', '\x00aa', '\x00fb', 
	'\x0043', '\x004d', '\x0033', '\x0085', '\x0045', '\x00f9', '\x0002', '\x007f', '\x0050', '\x003c', 
	'\x009f', '\x00a8', '\x0051', '\x00a3', '\x0040', '\x008f', '\x0092', '\x009d', '\x0038', '\x00f5', 
	'\x00bc', '\x00b6', '\x00da', '\x0021', '\x0010', '\x00ff', '\x00f3', '\x00d2', '\x00cd', '\x000c', 
	'\x0013', '\x00ec', '\x005f', '\x0097', '\x0044', '\x0017', '\x00c4', '\x00a7', '\x007e', '\x003d', 
	'\x0064', '\x005d', '\x0019', '\x0073', '\x0060', '\x0081', '\x004f', '\x00dc', '\x0022', '\x002a', 
	'\x0090', '\x0088', '\x0046', '\x00ee', '\x00b8', '\x0014', '\x00de', '\x005e', '\x000b', '\x00db', 
	'\x00e0', '\x0032', '\x003a', '\x000a', '\x0049', '\x0006', '\x0024', '\x005c', '\x00c2', '\x00d3',
	'\x00ac', '\x0062', '\x0091', '\x0095', '\x00e4', '\x0079', '\x00e7', '\x00c8', '\x0037', '\x006d', 
	'\x008d', '\x00d5', '\x004e', '\x00a9', '\x006c', '\x0056', '\x00f4', '\x00ea', '\x0065', '\x007a', 
	'\x00ae', '\x0008', '\x00ba', '\x0078', '\x0025', '\x002e', '\x001c', '\x00a6', '\x00b4', '\x00c6', 
	'\x00e8', '\x00dd', '\x0074', '\x001f', '\x004b', '\x00bd', '\x008b', '\x008a', '\x0070', '\x003e', 
	'\x00b5', '\x0066', '\x0048', '\x0003', '\x00f6', '\x000e', '\x0061', '\x0035', '\x0057', '\x00b9', 
	'\x0086', '\x00c1', '\x001d', '\x009e', '\x00e1', '\x00f8', '\x0098', '\x0011', '\x0069', '\x00d9', 
	'\x008e', '\x0094', '\x009b', '\x001e', '\x0087', '\x00e9', '\x00ce', '\x0055', '\x0028', '\x00df', 
	'\x008c', '\x00a1', '\x0089', '\x000d', '\x00bf', '\x00e6', '\x0042', '\x0068', '\x0041', '\x0099', 
	'\x002d', '\x000f', '\x00b0', '\x0054', '\x00bb', '\x0016'
    };

        public char[] SboxInv = new char[256] { 
    '\x0052', '\x0009', '\x006a', '\x00d5', '\x0030', '\x0036', '\x00a5', '\x0038', '\x00bf', '\x0040', 
    '\x00a3', '\x009e', '\x0081', '\x00f3', '\x00d7', '\x00fb', '\x007c', '\x00e3', '\x0039', '\x0082', 
    '\x009b', '\x002f', '\x00ff', '\x0087', '\x0034', '\x008e', '\x0043', '\x0044', '\x00c4', '\x00de', 
    '\x00e9', '\x00cb', '\x0054', '\x007b', '\x0094', '\x0032', '\x00a6', '\x00c2', '\x0023', '\x003d', 
    '\x00ee', '\x004c', '\x0095', '\x000b', '\x0042', '\x00fa', '\x00c3', '\x004e', '\x0008', '\x002e', 
    '\x00a1', '\x0066', '\x0028', '\x00d9', '\x0024', '\x00b2', '\x0076', '\x005b', '\x00a2', '\x0049',
    '\x006d', '\x008b', '\x00d1', '\x0025', '\x0072', '\x00f8', '\x00f6', '\x0064', '\x0086', '\x0068', 
    '\x0098', '\x0016', '\x00d4', '\x00a4', '\x005c', '\x00cc', '\x005d', '\x0065', '\x00b6', '\x0092', 
    '\x006c', '\x0070', '\x0048', '\x0050', '\x00fd', '\x00ed', '\x00b9', '\x00da', '\x005e', '\x0015', 
    '\x0046', '\x0057', '\x00a7', '\x008d', '\x009d', '\x0084', '\x0090', '\x00d8', '\x00ab', '\x0000', 
    '\x008c', '\x00bc', '\x00d3', '\x000a', '\x00f7', '\x00e4', '\x0058', '\x0005', '\x00b8', '\x00b3', 
    '\x0045', '\x0006', '\x00d0', '\x002c', '\x001e', '\x008f', '\x00ca', '\x003f', '\x000f', '\x0002',
    '\x00c1', '\x00af', '\x00bd', '\x0003', '\x0001', '\x0013', '\x008a', '\x006b', '\x003a', '\x0091', 
    '\x0011', '\x0041', '\x004f', '\x0067', '\x00dc', '\x00ea', '\x0097', '\x00f2', '\x00cf', '\x00ce', 
    '\x00f0', '\x00b4', '\x00e6', '\x0073', '\x0096', '\x00ac', '\x0074', '\x0022', '\x00e7', '\x00ad', 
    '\x0035', '\x0085', '\x00e2', '\x00f9', '\x0037', '\x00e8', '\x001c', '\x0075', '\x00df', '\x006e',
    '\x0047', '\x00f1', '\x001a', '\x0071', '\x001d', '\x0029', '\x00c5', '\x0089', '\x006f', '\x00b7', 
    '\x0062', '\x000e', '\x00aa', '\x0018', '\x00be', '\x001b', '\x00fc', '\x0056', '\x003e', '\x004b', 
    '\x00c6', '\x00d2', '\x0079', '\x0020', '\x009a', '\x00db', '\x00c0', '\x00fe', '\x0078', '\x00cd', 
    '\x005a', '\x00f4', '\x001f', '\x00dd', '\x00a8', '\x0033', '\x0088', '\x0007', '\x00c7', '\x0031',
    '\x00b1', '\x0012', '\x0010', '\x0059', '\x0027', '\x0080', '\x00ec', '\x005f', '\x0060', '\x0051', 
    '\x007f', '\x00a9', '\x0019', '\x00b5', '\x004a', '\x000d', '\x002d', '\x00e5', '\x007a', '\x009f', 
    '\x0093', '\x00c9', '\x009c', '\x00ef', '\x00a0', '\x00e0', '\x003b', '\x004d', '\x00ae', '\x002a',
    '\x00f5', '\x00b0', '\x00c8', '\x00eb', '\x00bb', '\x003c', '\x0083', '\x0053', '\x0099', '\x0061',
    '\x0017', '\x002b', '\x0004', '\x007e', '\x00ba', '\x0077', '\x00d6', '\x0026', '\x00e1', '\x0069',
    '\x0014', '\x0063', '\x0055', '\x0021', '\x000c', '\x007d'
    };

        public char[] Rcon = new char[255] { 
    '\x008d', '\x0001', '\x0002', '\x0004', '\x0008', '\x0010', '\x0020', '\x0040', '\x0080', '\x001b', 
	'\x0036', '\x006c', '\x00d8', '\x00ab', '\x004d', '\x009a', '\x002f', '\x005e', '\x00bc', '\x0063', 
	'\x00c6', '\x0097', '\x0035', '\x006a', '\x00d4', '\x00b3', '\x007d', '\x00fa', '\x00ef', '\x00c5', 
	'\x0091', '\x0039', '\x0072', '\x00e4', '\x00d3', '\x00bd', '\x0061', '\x00c2', '\x009f', '\x0025', 
	'\x004a', '\x0094', '\x0033', '\x0066', '\x00cc', '\x0083', '\x001d', '\x003a', '\x0074', '\x00e8', 
	'\x00cb', '\x008d', '\x0001', '\x0002', '\x0004', '\x0008', '\x0010', '\x0020', '\x0040', '\x0080', 
	'\x001b', '\x0036', '\x006c', '\x00d8', '\x00ab', '\x004d', '\x009a', '\x002f', '\x005e', '\x00bc', 
	'\x0063', '\x00c6', '\x0097', '\x0035', '\x006a', '\x00d4', '\x00b3', '\x007d', '\x00fa', '\x00ef', 
	'\x00c5', '\x0091', '\x0039', '\x0072', '\x00e4', '\x00d3', '\x00bd', '\x0061', '\x00c2', '\x009f', 
	'\x0025', '\x004a', '\x0094', '\x0033', '\x0066', '\x00cc', '\x0083', '\x001d', '\x003a', '\x0074', 
	'\x00e8', '\x00cb', '\x008d', '\x0001', '\x0002', '\x0004', '\x0008', '\x0010', '\x0020', '\x0040', 
	'\x0080', '\x001b', '\x0036', '\x006c', '\x00d8', '\x00ab', '\x004d', '\x009a', '\x002f', '\x005e', 
	'\x00bc', '\x0063', '\x00c6', '\x0097', '\x0035', '\x006a', '\x00d4', '\x00b3',	'\x007d', '\x00fa', 
	'\x00ef', '\x00c5', '\x0091', '\x0039', '\x0072', '\x00e4', '\x00d3', '\x00bd', '\x0061', '\x00c2', 
	'\x009f', '\x0025', '\x004a', '\x0094', '\x0033', '\x0066', '\x00cc', '\x0083', '\x001d', '\x003a', 
	'\x0074', '\x00e8', '\x00cb', '\x008d', '\x0001', '\x0002', '\x0004', '\x0008', '\x0010', '\x0020', 
	'\x0040', '\x0080', '\x001b', '\x0036', '\x006c', '\x00d8', '\x00ab', '\x004d', '\x009a', '\x002f', 
	'\x005e', '\x00bc', '\x0063', '\x00c6', '\x0097', '\x0035', '\x006a', '\x00d4', '\x00b3', '\x007d', 
	'\x00fa', '\x00ef', '\x00c5', '\x0091', '\x0039', '\x0072', '\x00e4', '\x00d3', '\x00bd', '\x0061', 
	'\x00c2', '\x009f', '\x0025', '\x004a', '\x0094', '\x0033', '\x0066', '\x00cc', '\x0083', '\x001d', 
	'\x003a', '\x0074', '\x00e8', '\x00cb', '\x008d', '\x0001', '\x0002', '\x0004', '\x0008', '\x0010', 
	'\x0020', '\x0040', '\x0080', '\x001b', '\x0036', '\x006c', '\x00d8', '\x00ab', '\x004d', '\x009a', 
	'\x002f', '\x005e', '\x00bc', '\x0063', '\x00c6', '\x0097', '\x0035', '\x006a', '\x00d4', '\x00b3', 
	'\x007d', '\x00fa', '\x00ef', '\x00c5', '\x0091', '\x0039', '\x0072', '\x00e4', '\x00d3', '\x00bd', 
	'\x0061', '\x00c2', '\x009f', '\x0025', '\x004a', '\x0094', '\x0033', '\x0066', '\x00cc', '\x0083', 
	'\x001d', '\x003a', '\x0074', '\x00e8', '\x00cb' 
    };

        BackgroundWorker bw = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "") {
                MessageBox.Show("Enter value to encrypt");
                return;
            }
            string input = textBox1.Text;

            //Web Service Call <SOAP>
          /*  localhost.Service1 ob = new localhost.Service1();
            char[] key = new char[16];
            key = ob.RandomKey();    */    

            //SSL Web Service <SOAP>
            //To create End to End Trust 
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((localhost_ssl, certificate, chain, sslPolicyErrors) => true);
            localhost_ssl.Service1 ob_ssl = new localhost_ssl.Service1();
            char[] key = new char[16];
            key = ob_ssl.RandomKey();

           

            int i = input.Length;

            int slot = i / 16;
            int mod = i % 16;
            //MessageBox.Show("value of string: " + mod);

            int pr = i / 16;
            if (pr == 0)
                pr = 1;
            int inc = 100 / pr;

            char[] en_data = new char[16];
            int count = 0;
            int ctr = 0;

            for (int x = 0; x < slot; x++)
            {
                for (int index = 0; index < 16; index++)
                {
                    en_data[index] = input[index + count];
                    ctr++;
                }
                AESEncrypt(en_data, key);
                AESDecrypt();
                count = count + 16;
                progressBar1.Value = progressBar1.Value + inc;
            }

            int diff = 100 - progressBar1.Value;

            char[] ch = new char[16];
           // MessageBox.Show("value of string: " + ctr);

            for (int index = 0; index < 1; index++)
            {
                for (int x = 0; x < mod; x++)
                {
                    ch[x] = input[ctr + x];
                    
                }
                AESEncrypt(ch, key);
                AESDecrypt();
                progressBar1.Value = progressBar1.Value + diff;
            }

            //This is FCS test case data.$#@!~

           // MessageBox.Show("value of string: " + en_data[2]);

            //Progress Bar
            /*bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();*/

            
           // MessageBox.Show("key : "+ch[15]);


           // char[] data = new char[16] { '\x0000', '\x0011', '\x0022', '\x0033', '\x0044', '\x0055', '\x0066', '\x0077', '\x0088', '\x0099', '\x00aa', '\x00bb', '\x00cc', '\x00dd', '\x00ee', '\x00ff' };
           // char[] key = new char[16] { '\x002b', '\x007e', '\x0015', '\x0016', '\x0028', '\x00ae', '\x00d2', '\x00a6', '\x00ab', '\x00f7', '\x0015', '\x0088', '\x0009', '\x00cf', '\x004f', '\x003c' };
           // char[] data = new char[16] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', '@', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

           // AESEncrypt(en_data, key);
          //  AESDecrypt();

            textBox2.Text = encrypt_val.ToString();
            //textBox3.Text = output.ToString();
          //  decFun();

          
        }

        public void populate() {
            textBox3.Text = output.ToString();
            progressBar1.Value = 100;
        }

        public void decFun() {
            int i = encrypt_val.Length;

            int slot = i / 16;
            int mod = i % 16;
            //MessageBox.Show("value of string: " + mod);

              

            for (int x = 0; x < slot; x++)
            {
                
 
                AESDecrypt();
            }

            for (int index = 0; index < 1; index++)
            {
                
                AESDecrypt();
            }
            MessageBox.Show("call: " + output.ToString());

            textBox3.Text = output.ToString();
        }

     /*   void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int total = 57; //some number (this is your variable to change)!!

            for (int i = 0; i <= total; i++) //some number (total)
            {
                System.Threading.Thread.Sleep(100);
                int percents = (i * 100) / total;
                bw.ReportProgress(percents, i);
                //2 arguments:
                //1. procenteges (from 0 t0 100) - i do a calcumation 
                //2. some current value!
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label2.Text = String.Format("Progress: {0} %", e.ProgressPercentage);
            label2.Text = String.Format("Rijndael-128 bit Encryption: {0}", e.UserState);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //do the code when bgv completes its work
        }
        */

        private int gfCal(char gf) {
            char temp = gf;
            gf = (char)(gf << 1);
            temp = (char)(temp >> 7);
            temp = (char)(temp & 1);
            temp = (char)(temp * (0x11b));
            gf = (char)(gf ^ temp);
            return gf;
        }

        private char getMul_GF(char a, char b)
        {
            char temp1, temp2, temp3, temp4, temp5, temp6;
            char i2, i3, i4, i5;
            temp1 = (char)(b & 1);
            temp1 = (char)(temp1 * a);
            i2 = (char)((b >> 1) & 1);
            temp2 = (char)(gfCal(a));
            temp2 = (char)(i2 * temp2);
            i3 = (char)((b >> 2) & 1);
            temp3 = (char)(gfCal((char)gfCal(a)));
            temp3 = (char)(i3 * temp3);
            i4 = (char)((b >> 3) & 1);
            temp4 = (char)(gfCal((char)gfCal((char)gfCal(a))));
            temp4 = (char)(i4 * temp4);
            i5 = (char)((b >> 4) & 1);
            temp5 = (char)(gfCal((char)gfCal((char)gfCal((char)gfCal(a)))));
            temp5 = (char)(i5 * temp5);
            temp6 = (char)(temp1 ^ temp2 ^ temp3 ^ temp4 ^ temp5);
            return temp6;
        }

        public void addKey(int r)
        {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    S[j, i] = (char)(S[j, i] ^ (RoundKey[r * Nb * 4 + i * Nb + j]));
        }

        private void SubBytes() {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    S[i,j] = Sbox[S[i,j]];
        }

        private void ShiftRows() {
            char shift;
            shift = S[1,0];  //shifting the rows
            S[1,0] = S[1,1];
            S[1,1] = S[1,2];
            S[1,2] = S[1,3];
            S[1,3] = shift;

            shift = S[2,0];
            S[2,0] = S[2,2];
            S[2,2] = shift;

            shift = S[2,1];
            S[2,1] = S[2,3];
            S[2,3] = shift;

            shift = S[3,0];
            S[3,0] = S[3,3];
            S[3,3] = S[3,2];
            S[3,2] = S[3,1];
            S[3,1] = shift;
        }

        private void MixColumn() {
            int i;
            char temp1, gfval, temp2;
            for (i = 0; i < 4; i++)
            {
                temp2 = S[0,i];
                temp1 = (char)(S[0,i] ^ S[1,i] ^ S[2,i] ^ S[3,i]);
                gfval = (char)(S[0,i] ^ S[1,i]);
                gfval = (char)(gfCal(gfval));
                S[0,i] = (char)(S[0,i] ^ gfval ^ temp1);
                //printf("\nValue = %x",S[0][0]);
                gfval = (char)(S[1,i] ^ S[2,i]);
                gfval = (char)(gfCal(gfval));
                S[1,i] = (char)(S[1,i] ^ gfval ^ temp1);
                gfval = (char)(S[2,i] ^ S[3,i]);
                gfval = (char)(gfCal(gfval));
                S[2,i] = (char)(S[2,i] ^ gfval ^ temp1);
                gfval = (char)(S[3,i] ^ temp2);
                //printf("\nValue = %x",S[0][0]);
                gfval = (char)(gfCal(gfval));
                //printf("\nValue = %x",S[0][0]);
                S[3,i] = (char)(S[3,i] ^ gfval ^ temp1);
            }
        }

        private void encrypt(char[] ddata)
        {
            //char[] ddata = new char[16] { '\x0000', '\x0011', '\x0022', '\x0033', '\x0044', '\x0055', '\x0066', '\x0077', '\x0088', '\x0099', '\x00aa', '\x00bb', '\x00cc', '\x00dd', '\x00ee', '\x00ff' };
            int i, j, r = 0;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    S[j,i] = ddata[i * 4 + j];
            addKey(r);
            for (r = 1; r < Nr; r++)
            {
                SubBytes();
                ShiftRows();
                MixColumn();
                addKey(r);
            }
            SubBytes();
            ShiftRows();
            addKey(Nr);
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    data[i * 4 + j] = S[j,i];

            for (int id = 0; id < 16; id++)
            {
                encrypt_val = encrypt_val + data[id];
            }
           
            int value = Convert.ToInt32(data[0]);
            string hexOutput = String.Format("{0:X}", value);
           // MessageBox.Show("encrypted value: " + hexOutput);
        }

        private void addKey_d(int r) {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    S_d[j,i] = (char)(S_d[j,i] ^ (RoundKey[r * Nb * 4 + i * Nb + j]));
        }

        private void InvSubBytes()
        {
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    S_d[i,j] = SboxInv[S_d[i,j]];

                }
            }
        }

        private void InvShiftRows() { 
            char temp;
	        temp=S_d[1,3];
	        S_d[1,3]=S_d[1,2];
	        S_d[1,2]=S_d[1,1];
	        S_d[1,1]=S_d[1,0];
	        S_d[1,0]=temp;

	        // Rotate second row 2 columns to right	
	        temp=S_d[2,0];
	        S_d[2,0]=S_d[2,2];
	        S_d[2,2]=temp;

	        temp=S_d[2,1];
	        S_d[2,1]=S_d[2,3];
	        S_d[2,3]=temp;

	        // Rotate third row 3 columns to right
	        temp=S_d[3,0];
	        S_d[3,0]=S_d[3,1];
	        S_d[3,1]=S_d[3,2];
	        S_d[3,2]=S_d[3,3];
	        S_d[3,3]=temp;
        }

        private void InvMixColumns(){
            int i;
	        char a,b,c,d;
            for (i = 0; i < 4; i++) {
                a = S_d[0,i];
                b = S_d[1,i];
                c = S_d[2,i];
                d = S_d[3,i];


                S_d[0, i] = (char)(getMul_GF(a, '\x000e') ^ getMul_GF(b, '\x000b') ^ getMul_GF(c, '\x000d') ^ getMul_GF(d, '\x0009'));
                S_d[1, i] = (char)(getMul_GF(a, '\x0009') ^ getMul_GF(b, '\x000e') ^ getMul_GF(c, '\x000b') ^ getMul_GF(d, '\x000d'));
                S_d[2, i] = (char)(getMul_GF(a, '\x000d') ^ getMul_GF(b, '\x0009') ^ getMul_GF(c, '\x000e') ^ getMul_GF(d, '\x000b'));
                S_d[3, i] = (char)(getMul_GF(a, '\x000b') ^ getMul_GF(b, '\x000d') ^ getMul_GF(c, '\x0009') ^ getMul_GF(d, '\x000e'));
            }
        }

        private void AESEncrypt(char[] data, char[] key) {
            expandKey(key);
            encrypt(data);
        }

        private void AESDecrypt() {
            int i, j, r = 0;
            // To Do
            for (i = 0; i < 4; i++) { 
                for (j = 0; j < 4; j++)
                    S_d[j,i] = data[i * 4 + j];
            }

            addKey_d(Nr);
            for (r = Nr - 1; r > 0; r--) {
                InvSubBytes();
                InvShiftRows();
                addKey_d(r);
                InvMixColumns(); 
            }

            InvSubBytes();
            InvShiftRows();
            addKey_d(0);

            
            for (i = 0; i < 4; i++) { 
                for(j=0;j<4;j++)
                    data_d[i * 4 + j] = S_d[j,i];
            }

            int value = Convert.ToInt32(data_d[8]);
            string hexOutput = String.Format("{0:X}", value);
            //MessageBox.Show("Decrypted value: " + hexOutput.ToString());
           // MessageBox.Show("Decrypted value: " + data_d[8]);
            
            for (int id = 0; id < 16; id++) {
                output = output + data_d[id];
            }
         
        }

        private void expandKey(char[] key) {
           
            char[] currValue = new char[4];
            int totalround;
            int totalroundkey;
            int i,j,k;
            for (i = 0; i < Nk; i++) {
                RoundKey[i * 4] = key[i * 4];
                RoundKey[i * 4 + 1] = key[i * 4 + 1];
                RoundKey[i * 4 + 2] = key[i * 4 + 2];
                RoundKey[i * 4 + 3] = key[i * 4 + 3];
            }
           // MessageBox.Show(RoundKey[0].ToString());
            totalround = Nr + 1;
            totalroundkey = Nb * totalround;
            while (i < totalroundkey) {
                for (j = 0; j < 4; j++) 
                    currValue[j] = RoundKey[(i - 1) * 4 + j];
                if (i % Nk == 0) {
                    k = currValue[0];
                    currValue[0] = currValue[1];
                    currValue[1] = currValue[2];
                    currValue[2] = currValue[3];
                    currValue[3] = (char)k; // int to char typecast

                    currValue[0] = Sbox[currValue[0]];
                    currValue[1] = Sbox[currValue[1]];
                    currValue[2] = Sbox[currValue[2]];
                    currValue[3] = Sbox[currValue[3]];

                    currValue[0] = (char)(currValue[0] ^ Rcon[i / Nk]);
                }
                else if (i % Nk == 4)
                {
                    currValue[0] = Sbox[currValue[0]];
                    currValue[1] = Sbox[currValue[1]];
                    currValue[2] = Sbox[currValue[2]];
                    currValue[3] = Sbox[currValue[3]];
                }

                RoundKey[i * 4 + 0] = (char)(RoundKey[(i - Nk) * 4 + 0] ^ currValue[0]);
                RoundKey[i * 4 + 1] = (char)(RoundKey[(i - Nk) * 4 + 1] ^ currValue[1]);
                RoundKey[i * 4 + 2] = (char)(RoundKey[(i - Nk) * 4 + 2] ^ currValue[2]);
                RoundKey[i * 4 + 3] = (char)(RoundKey[(i - Nk) * 4 + 3] ^ currValue[3]);

                i++;
            }
            
            //int value = Convert.ToInt32(c);
            int value = Convert.ToInt32(RoundKey[35]);
            string hexOutput = String.Format("{0:X}", value);
            //string hexOutput = String.Format("{0:X}", RoundKey[175]);
           // MessageBox.Show("Roun k: "+hexOutput);
            //MessageBox.Show(RoundKey[25].ToString());
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            encrypt_val = "";
            output = "";
            radioButton1.Checked = false;
            progressBar1.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            populate();
        }
    }
}
