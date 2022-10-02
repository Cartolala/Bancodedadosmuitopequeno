using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace MiniMuitoPequenoBancodeDados
{
    public partial class MainForm : Form
    {
        public MainForm()
        {

            InitializeComponent();
            
            // os comando comentado estao ai para me lembra

        }
        
        // Inicia variaveis para multiplas pesquisas
        string[][] bufferdados = new string[128][];
        int o = 0;
        int q = 0;
        
        void Limpar() 
        {
            // Reinicia a parte do programa para uma nova entrada de dado
            o = 0;
            button8.Visible = false;
            button6.Visible = false;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            label2.Text = richTextBox1.Lines.Length.ToString();
        }
        
        // Funcao para limpar o bufferdados caso um bug aconteça
        void NulllArray(Array[] arrayx)
        {
            // Muda todos os valores de um array para null
            // Para quando acha um valor null
            for (int i = 0; i < arrayx.Length; i++) {
                if(arrayx[i] != null){
                    arrayx[i] = null;
                } else {
                    return;
                }
            }
        }
        
        void Button1Click(object sender, EventArgs e)
        {
            // Botao Novo
            // Limpa os campos para uma nova entrada
            Limpar();
        }
        
        void Button2Click(object sender, EventArgs e)
        {
            // Botao Salvar
            // Salva para o 'banco de dado'
            // Caso ocorra uma pesquisa, mudara no banco
            
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                string linhaNova = textBox1.Text+"\t"+textBox2.Text+"\t"+textBox3.Text+"\t"+label2.Text;
                
                int numLinha = int.Parse(label2.Text);
                
                if (label2.Text == richTextBox1.Lines.Length.ToString())
                {
                    richTextBox1.Text = richTextBox1.Text+'\n'+linhaNova;
                    richTextBox1.SaveFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
                    MessageBox.Show("Salvou");
                    Limpar();
                } else {
                    string linhaVelha = richTextBox1.Lines[numLinha];
                    richTextBox1.Text = richTextBox1.Text.Replace(linhaVelha, linhaNova);
                    richTextBox1.SaveFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
                    MessageBox.Show("Mudou");

                }
            } else {
                MessageBox.Show("Campos Vazios");
            }
        }
        
        void Button3Click(object sender, EventArgs e)
        {
            // Botao Pesquisar
            // Pesquisa com o numero do textbox4 a linha do banco de dado 
            
            int numLinha;
            
            if (!int.TryParse(textBox4.Text, out numLinha))
            {
                MessageBox.Show("Não é um numero");
                return;
            }
            if (numLinha >= richTextBox1.Lines.Length || numLinha == 0)
            {
                MessageBox.Show("Não existe");
                return;
            }
            
            label2.Text = numLinha.ToString();
            
            string[] dado = richTextBox1.Lines[numLinha].Split('\t');
            textBox1.Text = dado[0];
            textBox2.Text = dado[1];
            textBox3.Text = dado[2];
            
        }
        
        void Button4Click(object sender, EventArgs e)
        {
            // Botao Deletar
            // Deleta a linha do banco de dados com o numero fornecido do textbox4
            
            int numLinha;
            if (!int.TryParse(textBox4.Text, out numLinha))
            {
                MessageBox.Show("Nao é um numero");
                return;
            }
            
            if (numLinha >= richTextBox1.Lines.Length)
            {
                MessageBox.Show("Nao existe");
                return;
            }
                            
            int maxlinha = richTextBox1.Lines.Length-1;
            
            if (numLinha > maxlinha || numLinha == 0){
                MessageBox.Show("Nao existe");
                return;
            }
            
            if (numLinha == maxlinha){
                string linhaVelha = richTextBox1.Lines[numLinha];
                richTextBox1.Text = richTextBox1.Text.Replace(linhaVelha, null);
                
                string linhaAnteriorOld = richTextBox1.Lines[numLinha-1];
                
                // Exclui o NewLine da penúltima linha
                string linhaAnteriorNew = linhaAnteriorOld.Replace('\n', ' ');
                richTextBox1.Text = richTextBox1.Text.Replace(linhaAnteriorOld + '\n', linhaAnteriorNew);
                richTextBox1.SaveFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
                MessageBox.Show("Apagou");
            } else {
                string linhaVelha = richTextBox1.Lines[numLinha];
                richTextBox1.Text = richTextBox1.Text.Replace(linhaVelha + '\n', null);
                richTextBox1.SaveFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
                MessageBox.Show("Apagou");
            }        
        }
        
//        void Button5Click(object sender, EventArgs e)
//        {
            // Ignorar, funçao de pesquisa, mas so retorna o primero resultado que encontrar
//            
//            int maxLinha = richTextBox1.Lines.Length;
//            
//            if (textBox1.Text == ""){
//                return;
//            }
//            
//            for (int i = 1; i < maxLinha; i++) {
//                string[] compare = richTextBox1.Lines[i].Split('\t');
//                
//                if (compare[0].ToLower() == textBox1.Text.ToLower()){
//                    textBox2.Text = compare[1];
//                    textBox3.Text = compare[2];
//                    return;
//                }
//            }
//        }
        
        void Button7Click(object sender, EventArgs e)
        {
            // Botao Pesquisar
            // Pesquisa por nomes com a string fornecida do textbox1
            // Se tiver mais que um resultado, habilita os botoes 6 e 8
            
            // Reinicia para uma nova pesquisa
            o=0; q=0;
            button8.Visible = false;
            button6.Visible = false;
            NulllArray(bufferdados);
            
            int maxLinha = richTextBox1.Lines.Length;
            
            if (textBox1.Text == ""){
                MessageBox.Show("Campo Vazio");
                return;
            }
            
            for (int i = 1; i < maxLinha; i++) {
                string[] compare = richTextBox1.Lines[i].Split('\t');
                
                // Usa Expressao Regular para tornar a pesquisa mais generica
                Regex regex = new Regex(textBox1.Text.ToLower());
                
                if (regex.IsMatch(compare[0].ToLower())){
                    bufferdados[o] = compare;
                    bufferdados[o][3] = i.ToString();
//                    MessageBox.Show(string.Concat(bufferdados[o])); Caso seja necessario o debug
                    o++;
                }
                
//                if (compare[0].ToLower() == textBox1.Text.ToLower()){
//                    
//                    bufferdados[o] = compare;
//                    bufferdados[o][3] = i.ToString();
////                    MessageBox.Show(string.Concat(bufferdados[o])); Caso seja necessario o debug
//                    o++;    
//                }
            }
            
            if(bufferdados[0] == null){
                MessageBox.Show("Nao encontrado");
                return;
            }
            
            if(o > 1){
                MessageBox.Show(o.ToString() + " Resultados Foram Encontrados");
                button8.Visible = true;
            } else {
                MessageBox.Show(o.ToString() + " Resultado Foi Encontrado");
            }
            
            textBox1.Text = bufferdados[0][0];
            textBox2.Text = bufferdados[0][1];
            textBox3.Text = bufferdados[0][2];
            label2.Text   = bufferdados[0][3];
        }
        
        void Button6Click(object sender, EventArgs e)
        {
            // Botao Voltar
            // retrocede o index, mostrando o resultado anterior
            
            button8.Visible = true;
            
            q--;
            textBox1.Text = bufferdados[q][0];
            textBox2.Text = bufferdados[q][1];
            textBox3.Text = bufferdados[q][2];
            label2.Text   = bufferdados[q][3];
            
            if (q == 0){
                button6.Visible = false;
                return;
            }
        }
        
        void Button8Click(object sender, EventArgs e)
        {
            // Botao avançar
            // avança o index, mostrando o proximo resultado
            
            button6.Visible = true;
            
            q++;
            textBox1.Text = bufferdados[q][0];
            textBox2.Text = bufferdados[q][1];
            textBox3.Text = bufferdados[q][2];
            label2.Text   = bufferdados[q][3];
            
            if (q == o - 1){
                button8.Visible = false;
                return;
            }
        }
        
        void MainFormLoad(object sender, EventArgs e)
        {
            try {
                richTextBox1.LoadFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
            } catch (IOException exc) {
                richTextBox1.SaveFile("funcionarios.txt", RichTextBoxStreamType.PlainText);
                MessageBox.Show("existe nao irmao, calma que eu crio");
            }
            
            int maxlinha = richTextBox1.Lines.Length;
            label2.Text = maxlinha.ToString();
        }
    }
}
