
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCodeRobotCSharpEdition.Tamplates;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
namespace GCodeRobotCSharpEdition.Robot
{
    public class RobotTamplate
    {
       
        SendTampalte sTmpl = new SendTampalte();
        public printParam PrParam;
        
        private bool _sendNextFile = false;
        public string extension=".tp";
        public bool SendNextFile 
        { 
            get 
            {
                return _sendNextFile;
            }
            set 
            { 
                if(value && isAwait/* && !isPrinting*/)
                {
                    stateTempl.color = Color.Orange;
                    stateTempl.CurrentState = "Await sending Next file";
                    form.CurState= stateTempl;
                    
                }
               

                _sendNextFile = value;
            } 
        } 
        public string Addres { get; set; }

        private bool lastFile = false;
        public StateTempl stateTempl;

        public bool isPrinting { private set; get; } = false;

        private bool isAwait = false;
        public Form2 form;
        public RobotTamplate(string ip)
        {
            Addres = ip;
            stateTempl.CurrentState = "Unconnect";
            stateTempl.color = Color.Red;
            ChechConnection();
        }

        public void PrintAsync(bool await_layer, string fname, int fid)
        {
            lastFile = false;
            isAwait = await_layer;


            if (PrParam.count == 0)
            {
                stateTempl.CurrentState = "Print Errore empty patch";
                LogList.Add(3, "Print Errore empty patch");
                stateTempl.color = Color.Orange;
                form.CurState = stateTempl;
                return;
            }
            form.CurState = stateTempl;
            ChechConnection();
            sTmpl.is_start = true.ToString();
            sTmpl.stop_after_layer = await_layer.ToString();

            if (await_layer)
            {
                sTmpl.current_file_path = fname;

            }
            else
                sTmpl.current_file_path = fname;
            FTPLoad(fname, PrParam.patch + "\\"  + fname);
            isPrinting = true;
            sendtoServ(sTmpl);
            printing = true;
            PrParam.curnumb = fid;
            stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}";
            LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
            stateTempl.color = Color.Green;

            form.CurState = stateTempl;
            if (form.ct.IsCancellationRequested)
                return;
            while (PrParam.curnumb < PrParam.count)
            {
                if (form.ct.IsCancellationRequested)
                    return;
                CheckNext();
                if (SendNextFile)
                {

                    if (await_layer)
                        return;

                    else
                    {
                        int delay = 0;
                        var a = Form1.sets.Params.Where(c => c.ParamName == "NextLayerDelay").ToList();
                        if (a.Count > 0)
                        {
                            delay = Convert.ToInt32(a[0].ParamValue);
                        }

                        SendNextFile = false;
                        var userResult = AutoClosingMessageBox.Show("Все хорошо? Продолжаем?", "Caption", 5 * 1000, MessageBoxButtons.YesNo);
                        if (userResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            

                            if (PrParam.curnumb  == PrParam.count)
                            {
                                sTmpl.is_start = true.ToString();
                                sTmpl.stop_after_layer = await_layer.ToString();
                                sTmpl.is_last_file = true.ToString();
                                PrParam.curnumb++;
                                sTmpl.current_file_path =PrParam.filename + $"_{ PrParam.curnumb }" + extension;
                                LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                                FTPLoad(PrParam.filename + $"_{ PrParam.curnumb }" + extension, PrParam.patch + "\\" + PrParam.filename + $"_{ PrParam.curnumb }" + extension);
                                sendtoServ(sTmpl);
                                printing = true;
                                lastFile = true;
                            }
                            else
                            {
                                sTmpl.is_start = true.ToString();
                                PrParam.curnumb++;
                                sTmpl.stop_after_layer = await_layer.ToString();
                                sTmpl.current_file_path =  PrParam.filename + $"_{ PrParam.curnumb }" + extension;
                                FTPLoad(PrParam.filename + $"_{ PrParam.curnumb }" + extension, PrParam.patch + "\\" + PrParam.filename + $"_{ PrParam.curnumb }" + extension);
                                printing = true;
                                sendtoServ(sTmpl);
                            }
                            PrParam.curnumb++;
                            Thread.Sleep(2000);
                            LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                            stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}";
                            stateTempl.color = Color.Green;

                            form.CurState = stateTempl;
                        }
                        
                    }
                    form.CurState = stateTempl;
                    Thread.Sleep(100);
                }
                
               
            }
            isPrinting = false;
        }

       
        public void printNext(string fname, int fid)
        {
            ChechConnection();
            SendNextFile = false;
            //isPrinting = true;
            if (fid+1 == PrParam.count)
            {
                sTmpl.is_start = true.ToString();
                sTmpl.is_last_file = true.ToString();
                sTmpl.current_file_path = fname;
                FTPLoad(fname, PrParam.patch + "\\" +  fname);
                sendtoServ(sTmpl);
                printing = true;
                lastFile = true;
                LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                form.CurState = stateTempl;
            }
            else
            {
                sTmpl.is_start = true.ToString();
                
                sTmpl.current_file_path = fname;
                FTPLoad(fname, PrParam.patch + "\\"+ fname);
                sendtoServ(sTmpl);
                printing = true;
                LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                form.CurState = stateTempl;
            }
            PrParam.curnumb =fid+1;
            while(SendNextFile == false)
            {
                if (form.ct.IsCancellationRequested)
                    return;
                CheckNext();
                form.CurState = stateTempl;
                Thread.Sleep(100);

            }
            if (lastFile)
            {
                isPrinting = false;
            }
        }
        private void sendtoServ(SendTampalte req)
        {
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://{Addres}:5000");
                client.SendFrame(req.convertToString());
                var msg = client.ReceiveFrameString();
                if (msg != "1")
                {
                    stateTempl.color = Color.Red;
                    stateTempl.CurrentState = "format errore ";
                    form.CurState = stateTempl;
                }
            }
        }

        private string lastState = "0";
        private bool printing = true;
        /// <summary>
        /// проверка необходимости отправки следующего файла работает пораллельно во время печати
        /// </summary>
            public void CheckNext()
            {
                using (var client = new RequestSocket())
                {
                Thread.Sleep(100);
                    client.Connect($"tcp://{Addres}:5000");

                    client.SendFrame("states");
                    string message = client.ReceiveFrameString();
                var state = message;
                string z = "0";
                    try
                    {
                        state = message.Substring(0, message.IndexOf(';'));
                        z = message.Substring(message.IndexOf(';') + 1);
                        z = z.Substring(z.IndexOf('$') + 1);
                        state = state.Substring(state.IndexOf('$') + 1);
                    }
                    catch (Exception)
                    {

                        state = state.Substring(state.IndexOf('$') + 1);
                    }
               
                switch (state)
                {
                    case "0":
                        stateTempl.color = Color.Green;
                        stateTempl.CurrentState = "Ready to print";
                        //lastState = state;
                        LogList.Add(1, "Ready to print");
                        form.CurState = stateTempl;
                        if (printing)
                        {   
                            SendNextFile = true;
                            printing = false;
                            
                        }
                        else
                            SendNextFile = false;
                        break;
                    case "1":
                        stateTempl.color = Color.Green;
                        lastState = state;
                        if (z == "0")
                        {
                            LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                            stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}";
                        }
                        else
                        {
                            LogList.Add(2, "");
                            LogList.AddZ(float.Parse(z));
                            stateTempl.CurrentState = $"current hieght z={z}";
                        }
                        //stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}, current z={z}";
                        form.CurState = stateTempl;
                        SendNextFile = false;
                        break;
                    case "2":
                        stateTempl.color = Color.Green;
                        lastState = state;
                        if (z == "0")
                        {
                            LogList.Add(1, $"Printing file { PrParam.curnumb}/{ PrParam.count}");
                            stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}";
                        }
                        else
                        {
                            LogList.Add(2, "");
                            LogList.AddZ(float.Parse(z));
                            stateTempl.CurrentState = $"current hieght z={z}";
                        }
                        //stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}, current z={z}";
                        form.CurState = stateTempl;
                        SendNextFile = false;
                        break;
                    default:
                        stateTempl.color = Color.Red;
                        stateTempl.CurrentState = "Connection errore";
                        form.CurState = stateTempl;
                        SendNextFile = false;
                        break;
                }
               

               

                }
            }
            
        /// <summary>
        /// проверяет связь с сервером работает в основном потоке
        /// </summary>
        public void ChechConnection()
        {
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://{Addres}:5000");
                client.SendFrame("states");
                string message = client.ReceiveFrameString();
                var state = message;
                //string z = "0";
                client.Connect($"tcp://{Addres}:5000");
                client.SendFrame("states");
                 message = client.ReceiveFrameString();
               
                string z = message;
                try
                {
                    state = message.Substring(0, message.IndexOf(';'));
                    z = message.Substring(message.IndexOf(';') + 1);
                    z = z.Substring(z.IndexOf('$') + 1);
                    state = state.Substring(state.IndexOf('$') + 1);
                }
                catch (Exception)
                {
                    
                    state = state.Substring(state.IndexOf('$') + 1);
                }
                    //state:1(0Готов  1Печать 2Необходим файл);z:0 
                    switch (state)
                    {
                        case "0":
                            stateTempl.color = Color.Green;
                            stateTempl.CurrentState = "Ready to print";
                           //form.ll.Add(1, "Ready to print");
                            //form.CurState = stateTempl;
                        break;
                        case "1":
                            stateTempl.color = Color.Green;
                            stateTempl.CurrentState = $"Printing file {PrParam.curnumb}/{PrParam.count}, current z={z}";
                            //form.ll.Add(1, $"Printing file {PrParam.curnumb}/{PrParam.count}");
                            break;
                        default:
                            stateTempl.color = Color.Red;
                            stateTempl.CurrentState = "Connection error";
                            LogList.Add(3, "Connection error");
                            form.CurState = stateTempl;
                            break;
                    }
            }
               
               //form.UpdateRobotTable();
            }
 public  void ChechTest()
        {
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://{Addres}:5000");
                client.SendFrame("states");
                string message = client.ReceiveFrameString();
                var state = message;
                //string z = "0";
                client.Connect($"tcp://{Addres}:5000");
                client.SendFrame("states");
                 message = client.ReceiveFrameString();
               
                string z = message;
                try
                {
                    state = message.Substring(0, message.IndexOf(';'));
                    z = message.Substring(message.IndexOf(';') + 1);
                    z = z.Substring(z.IndexOf('$') + 1);
                    state = state.Substring(state.IndexOf('$') + 1);
                }
                catch (Exception)
                {
                    
                    state = state.Substring(state.IndexOf('$') + 1);
                }
                    //state:1(0Готов  1Печать 2Необходим файл);z:0 
                    switch (state)
                    {
                        case "0":
                            stateTempl.CurrentState = $" current z={z}";
                            stateTempl.color = Color.Green;
                            stateTempl.CurrentState = "Ready to print";
                            //lastState = state;
                            form.CurState = stateTempl;
                           
                            break;
                        case "1":
                            stateTempl.color = Color.Green;
                            lastState = state;
                            stateTempl.CurrentState = $" current z={z}";
                            form.CurState = stateTempl;
                            SendNextFile = false;
                            break;
                    }
            }
               
               //form.UpdateRobotTable();
            }
        private void FTPLoad(string fileName,string pathWay)
        {
            //WebClient client = new WebClient();
            //client.Credentials = new NetworkCredential("pi", "8");
            //client.UploadFile(
            //    $"ftp://{Addres}/files/{fileName}", $"{pathWay}");

        }



    }
}
