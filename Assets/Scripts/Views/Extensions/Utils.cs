/// <summary>
/// @author: Interativa
/// @date: 08/2013
/// 
/// Métodos diversos para ajudas em scripts e cenas
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Security.Cryptography;

public enum ConvertTimeType
{
    hmsc,
    hms,
    hm,
    msc,
    ms,
    sc,
    s
}

public class Utils
{
    // ############################################################################################ // Converte um float em formato de numeros reais
    public static string ConvertRealTime(float time, ConvertTimeType formato)
    {
        // @method author: Pedro de Souza
        int horas = Mathf.FloorToInt(time / 3600);
        int minutos = Mathf.FloorToInt((time - horas * 3600) / 60);
        int segundos = Mathf.FloorToInt((time - horas * 3600 - minutos * 60));
        float centesimos = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);
        string reth = horas.ToString();
        string retm = minutos.ToString();
        string rets = segundos.ToString();
        string retc = centesimos.ToString();

        //filtro para o zero
        if (time <= 0)
        {
            reth = "0";
            retm = "0";
            rets = "0";
            retc = "0";
        }

        if (horas < 10) reth = "0" + reth;
        if (minutos < 10) retm = "0" + retm;
        if (segundos < 10) rets = "0" + rets;
        if (centesimos < 10) retc = "0" + retc;

        string ret = "";

        if (formato == ConvertTimeType.hmsc) ret = reth + ":" + retm + ":" + rets + ":" + retc;
        else if (formato == ConvertTimeType.hms) ret = reth + ":" + retm + ":" + rets;
        else if (formato == ConvertTimeType.hm) ret = reth + ":" + retm;
        else if (formato == ConvertTimeType.msc) ret = retm + ":" + rets + "," + retc;
        else if (formato == ConvertTimeType.ms) ret = retm + ":" + rets;
        else if (formato == ConvertTimeType.sc) ret = rets + ":" + retc;
        else if (formato == ConvertTimeType.s) ret = rets;
        return ret;
    }
    // ############################################################################################ // Transforma string em criptografia MD5
    public static string GetMD5HashString(string unhashed)
    {
        //Debug.Log("Generating MD5 hash value from: \"" + unhashed + "\"");
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(unhashed);
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(inputBytes);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    // ############################################################################################ // Retorna bool informando se CPF é verdadeiro
    public static bool CpfValid(string cpfTemp)
    {
        // @method author: Ulisses Dantas
        string cpf = Regex.Replace(cpfTemp, @"[^0-9]", ""); //Faz a checagem do tamanho da string só usando os numeros
        bool tudoOk = true;

        //Checagem inicial
        try
        {
            //float temp = float.Parse(cpfTemp);
            //Debug.Log("Sucesso");
        }
        catch
        {
            Debug.Log("Tem letras no meio: " + cpfTemp);
            return false;
        }

        //Calculo de tamanho do CPF
        if (cpf.Length != 11)
            tudoOk = false;

        //So tento checar se nao estiver errado
        if (tudoOk)
        {
            //Calculo do primeiro digito verificador
            float calcNove = 0;
            float digito1 = 0;

            for (int x = 0; x < 9; x++)
            {
                calcNove += (int.Parse(cpf[x].ToString()) * (10 - x));
            }

            if (calcNove % 11 < 2)
                digito1 = 0;
            else
                digito1 = 11 - (calcNove % 11);

            if (cpf[9].ToString() != digito1.ToString())
                tudoOk = false;
        }

        //So tento checar se nao estiver errado
        if (tudoOk)
        {
            //Calculo do segundo digito verificador
            float calcDez = 0;
            float digito2 = 0;

            for (int x = 0; x < 10; x++)
            {
                calcDez += (int.Parse(cpf[x].ToString()) * (11 - x));
            }

            if (calcDez % 11 < 2)
                digito2 = 0;
            else
                digito2 = 11 - (calcDez % 11);

            if (cpf[10].ToString() != digito2.ToString())
                tudoOk = false;
        }

        return tudoOk;
    }
    // ############################################################################################ // Transforma string em formato pronto de CPF
    public static string CpfFormat(string cpf)
    {
        // @method author: Ulisses Dantas
        string tempLength = cpf;
        //Checa em um string limpo de traços e pontos para caso o jogador apague deposi de preencher, nao aconteca o auto preenchimento de 0's
        tempLength = Regex.Replace(tempLength, @"[^0-9]", "");

        if (tempLength.Length >= 11)
        {
            cpf = Regex.Replace(cpf, @"[^0-9]", "");
            long num;
            bool ok = long.TryParse(cpf, out num);
            if (ok)
            {
                string temp2 = String.Format(@"{0:000\.000\.000\-00}", num);
                cpf = temp2;
            }
        }
        else
        {
            cpf = Regex.Replace(cpf, @"[^0-9]", "");
        }

        return cpf;
    }
    // ############################################################################################ // Transforma string em forma de data
    public static string DateFormat(string data)
    {
        // @method author: Ulisses Dantas
        string tempLength = data;
        //Checa em um string limpo de traços e pontos para caso o jogador apague deposi de preencher, nao aconteca o auto preenchimento
        tempLength = Regex.Replace(tempLength, @"[^0-9]", "");

        if (tempLength.Length == 6)
        {
            data = Regex.Replace(data, @"[^0-9]", "");
            long num;
            bool ok = long.TryParse(data, out num);
            if (ok)
            {
                string temp2 = String.Format(@"{0:00\/00\/00}", num);
                data = temp2;
            }
        }
        else if (tempLength.Length >= 8)
        {
            data = Regex.Replace(data, @"[^0-9]", "");
            long num;
            bool ok = long.TryParse(data, out num);
            if (ok)
            {
                string temp2 = String.Format(@"{0:00\/00\/0000}", num);
                data = temp2;
            }
        }
        else
        {
            data = Regex.Replace(data, @"[^0-9]", "");
        }

        return data;
    }
    // ############################################################################################ // Checa se string é um campo de email válido
    public static bool DateValid(string data) //Checa se os valores inicias equivale a dias, e os segundos a mes (considera-se o padrao dd/mm/-----)
    {
        bool dataOk = true;

        data = Regex.Replace(data, @"[^0-9]", "");

        if (data.Length >= 4)
        {
            string dias = data.Substring(0, 2);
            string mes = data.Substring(2, 2); Debug.Log(dias + "/" + mes);

            int diasNum = int.Parse(dias);
            if (diasNum <= 0 || diasNum >= 32)
                dataOk = false;

            int mesNum = int.Parse(mes);
            if (mesNum <= 0 || mesNum >= 13)
                dataOk = false;
        }
        else
        {
            dataOk = false;
        }

        return dataOk;
    }
    // ####################################################################################################
    public static bool EmailIsValid(string email)
    {
        // @method author: Ulisses Dantas

        bool emailOk = true;

        //Limpa espacos
        email = email.Replace(" ", "");

        string[] camposSeparadosArroba = email.Split('@');
        if (camposSeparadosArroba.Length == 2) //Só pode haver um arroba em um email. Se houver menos ou mais, retorna como errado
        {
            if (camposSeparadosArroba[0].Length < 2) //Se houver menos de 2 digitos antes da separacao
            {
                emailOk = false;
                //Debug.Log("Dei erro no lenght do campo 0 de arroba");
            }

            string[] camposSeparadosPonto = camposSeparadosArroba[1].Split('.');
            if (camposSeparadosPonto.Length > 1) //Deve haver pelo menos dois campos depois de separar no ponto
            {
                if (camposSeparadosPonto[0].Length < 2 || camposSeparadosPonto[1].Length < 2) //Deve haver pelo menos xx.xx
                {
                    emailOk = false;
                    //Debug.Log("Dei erro no lenght dos campos");
                }
            }
            else
            {
                emailOk = false;
            }
        }
        else
        {
            emailOk = false;
        }

        return emailOk;
    }
    // ############################################################################################ // Adapta a fonte desejada para o tamanho da tela
    public static int AdjustNumberToScreen(float wantedFont, float screenSize)
    {
        int resultFont = (int) Mathf.Round((Screen.width * wantedFont) / screenSize);
        return resultFont;
    }
    // ############################################################################################ // Destroi renderer
    public static void DestroyMyRender(Renderer render)
    {
        DestroyMyRender(render);
    }
    // ############################################################################################ // Salva dados em arquivo
    public static void SaveToFile(string value, string url)
    {
        TextWriter tw = new StreamWriter(url);
        tw.WriteLine(value);
        tw.Close();
    }
    // ############################################################################################
    public static bool CheckRectBounds(Rect rect, Vector3 pos)
    {
        bool bounds = false;

        if (pos.x > rect.x && pos.x < rect.x + rect.width)
        {
            if (pos.y < Screen.height - rect.y && pos.y > (Screen.height - (rect.y + rect.height)))
                bounds = true;
        }

        return bounds;

    }
    // ####################################################################################################
    public static bool CheckIfHasLetters(string tempString) //checa se tem pelo menos uma letra essa string
    {
        bool haveLetters = false;

        foreach (char s in tempString)
        {
            if (char.IsLetter(s))
            {
                haveLetters = true;
                break;
            }
        }

        return haveLetters;
    }
    // ####################################################################################################
    public static int[] RetornaRespostasPorCampo(List<string> todasRespostas, string[] campos)
    {
        //Adiciona campos que poderiam estar de fora. É corrigido depois retirando 1 de cada na soma
        for (int x = 0; x < campos.Length; x++)
        {
            todasRespostas.Add(campos[x]);
        }

        //Cria lista temporaria
        List<int> respostasCampoTemp = new List<int>();

        //Sort na lista que recebeu
        todasRespostas.Sort();

        string lastEntry = todasRespostas[0];
        int lastCount = 0;

        for (int x = 0; x < todasRespostas.Count; x++)
        {
            if (todasRespostas[x] == lastEntry)
            {
                lastCount++;
            }

            if (todasRespostas[x] != lastEntry)
            {
                //Debug.Log("Adicionou " + lastCount + " no index " + x);
                respostasCampoTemp.Add(lastCount);
                lastCount = 0;

                lastEntry = todasRespostas[x];
                lastCount++;
            }

            if (x == (todasRespostas.Count - 1))
            {
                //Debug.Log("Adicionou " + lastCount + " no index " + x);
                respostasCampoTemp.Add(lastCount);
            }
        }

        //Retira o 1 de todos os campos
        for (int x = 0; x < respostasCampoTemp.Count; x++)
        {
            respostasCampoTemp[x]--;
        }

        //Retorna em array
        int[] respostasPorCampo = new int[respostasCampoTemp.Count];
        for (int x = 0; x < respostasPorCampo.Length; x++)
        {
            respostasPorCampo[x] = respostasCampoTemp[x];
        }

        return respostasPorCampo;
    }
    // ####################################################################################################
    public static float[] RetornaPorcetagens(int[] respostasPorCampo)
    {
        //Calcula o total de respostas
        int totalRespostas = 0;
        for (int x = 0; x < respostasPorCampo.Length; x++)
        {
            totalRespostas += respostasPorCampo[x];
        }

        float[] porcentagensPorCampo = new float[respostasPorCampo.Length];
        for (int x = 0; x < porcentagensPorCampo.Length; x++)
        {
            float valTemp = (float)(respostasPorCampo[x] * 100) / totalRespostas;
            //Debug.Log(valTemp + " " + respostasPorCampo[x] + " " + totalRespostas);
            porcentagensPorCampo[x] = valTemp;
        }

        return porcentagensPorCampo;
    }
    // ####################################################################################################
    public static Matrix4x4 GuiReziseMatriz(float originalWidth, float originalHeight)
    {
        Matrix4x4 originalMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / originalWidth, Screen.height / originalHeight, 1));
        return originalMatrix;
    }
    // ####################################################################################################
    public static string QuebraNome(string input, int limiteCaracteres)
    {
        //Filtra tamanho do nome
        string temp = input;
        if (temp.Length > limiteCaracteres)
        {
            string[] broke = temp.Split(' ');
            for (int x = broke.Length; x >= 0; x--)
            {
                string checkTemp = "";
                for (int y = 0; y < x; y++)
                {
                    if(y == (x-1))
                        checkTemp += broke[y];
                    else
                        checkTemp += (broke[y] + " ");
                }

                if (checkTemp.Length <= limiteCaracteres) 
                {
                    temp = checkTemp;
                    break;
                }
            }
        }

        return temp;
    }
    // ####################################################################################################
    public static string ReturnFilenameNow()
    {
        System.DateTime data = System.DateTime.Now;
        string file = data.Day.ToString() + "_" + data.Hour.ToString() + "_" + data.Minute.ToString() + "_" + data.Second.ToString() + ".png";
        return file;
    }
    // ################################################################################################################################################################
}
